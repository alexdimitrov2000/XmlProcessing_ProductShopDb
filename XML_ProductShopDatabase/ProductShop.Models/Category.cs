using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<CategoryProduct>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CategoryProduct> Products { get; set; }
    }
}
