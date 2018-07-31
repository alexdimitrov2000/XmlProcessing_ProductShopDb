using AutoMapper;
using ProductShop.App.DTOs;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using DataValidation = System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ProductShop.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Xml;

namespace ProductShop.App
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = config.CreateMapper();

            SeedUsers(context, mapper);

            SeedProducts(context, mapper);

            SeedCategories(context, mapper);

            GenerateProductsCategories(context);

            ProductsInRange(context, config);

            UsersSoldProducts(context, config);

            CategoriesByProducts(context, config);

            UsersAndProducts(context, config);
        }

        private static void UsersAndProducts(ProductShopContext context, MapperConfiguration config)
        {
            var users = new UsersDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Count > 0),
                Users = context.Users.Where(u => u.ProductsSold.Count > 0).Select(u => new UserProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age.ToString(),
                    SoldProducts = new SoldProductsDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.AsQueryable().ProjectTo<ProductDto>(config).ToArray()
                    }
                }).ToArray()
            };

            var serializer = new XmlSerializer(typeof(UsersDto), new XmlRootAttribute("users"));
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            using (var writer = new StreamWriter("../SerializedQueries/users-and-products.xml"))
            {
                serializer.Serialize(writer, users, namespaces);
            }
        }

        private static void CategoriesByProducts(ProductShopContext context, MapperConfiguration config)
        {
            var categories = context.Categories
                            .Include(c => c.Products)
                            .OrderByDescending(c => c.Products.Count)
                            .ProjectTo<CategoryDto>(config)
                            .ToArray();

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            using (var writer = new StreamWriter("../SerializedQueries/categories-by-products.xml"))
            {
                serializer.Serialize(writer, categories, namespaces);
            }
        }

        private static void UsersSoldProducts(ProductShopContext context, MapperConfiguration config)
        {
            var users = context.Users
                            .Where(u => u.ProductsSold.Count > 0)
                            .OrderBy(u => u.LastName)
                            .ThenBy(u => u.FirstName)
                            .ProjectTo<UserSellerDto>(config)
                            .ToArray();

            var serializer = new XmlSerializer(typeof(UserSellerDto[]), new XmlRootAttribute("users"));
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            using (var writer = new StreamWriter("../SerializedQueries/users-sold-products.xml"))
            {
                serializer.Serialize(writer, users, namespaces);
            }
        }

        private static void ProductsInRange(ProductShopContext context, MapperConfiguration config)
        {
            var productsInRange = context.Products
                            .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                            .OrderBy(p => p.Price)
                            .ProjectTo<ProductsInRangeDto>(config)
                            .ToArray();

            var serializer = new XmlSerializer(typeof(ProductsInRangeDto[]), new XmlRootAttribute("products"));
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            using (var writer = new StreamWriter("../SerializedQueries/products-in-range.xml"))
            {
                serializer.Serialize(writer, productsInRange, namespaces);
            }
        }

        private static void GenerateProductsCategories(ProductShopContext context)
        {
            var numberOfCategories = context.Categories.Count();
            var halfOfCategories = numberOfCategories / 2;

            var numberOfProducts = context.Products.Count();

            var categoryProducts = new List<CategoryProduct>();

            for (int i = 1; i <= numberOfProducts; i++)
            {
                categoryProducts.Add(new CategoryProduct
                {
                    ProductId = i,
                    CategoryId = new Random().Next(1, halfOfCategories)
                });

                categoryProducts.Add(new CategoryProduct
                {
                    ProductId = i,
                    CategoryId = new Random().Next(halfOfCategories + 1, numberOfCategories)
                });
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
        }

        private static void SeedCategories(ProductShopContext context, IMapper mapper)
        {
            var stringCategories = File.ReadAllText("XMLs/categories.xml");

            var serializer = new XmlSerializer(typeof(CategoryNameDto[]), new XmlRootAttribute("categories"));

            var categoriesDeseriazlized = (CategoryNameDto[])serializer.Deserialize(new StringReader(stringCategories));

            var categories = new List<Category>();

            foreach (var categoryDto in categoriesDeseriazlized)
            {
                if (!IsValid(categoryDto))
                    continue;

                var category = mapper.Map<Category>(categoryDto);

                categories.Add(category);
            }

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void SeedProducts(ProductShopContext context, IMapper mapper)
        {
            var stringProducts = File.ReadAllText("XMLs/products.xml");

            var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));

            var productsDeserialized = (ProductDto[])serializer.Deserialize(new StringReader(stringProducts));

            var products = new List<Product>();

            var usersLength = context.Users.Count();
            var usersHalf = usersLength / 2;
            var counter = 0;

            var random = new Random();
            foreach (var productDto in productsDeserialized)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                var product = mapper.Map<Product>(productDto);
                product.SellerId = random.Next(1, usersHalf);

                if (counter % 3 != 0)
                    product.BuyerId = random.Next(usersHalf + 1, usersLength);

                products.Add(product);
                counter++;
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        private static void SeedUsers(ProductShopContext context, IMapper mapper)
        {
            var stringUsers = File.ReadAllText("XMLs/users.xml");

            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));

            var usersDeserialized = (UserDto[])serializer.Deserialize(new StringReader(stringUsers));

            var users = new List<User>();

            foreach (var userDto in usersDeserialized)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }

                var user = mapper.Map<User>(userDto);

                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new DataValidation.ValidationContext(obj);
            var validationResults = new List<DataValidation.ValidationResult>();

            return DataValidation.Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
