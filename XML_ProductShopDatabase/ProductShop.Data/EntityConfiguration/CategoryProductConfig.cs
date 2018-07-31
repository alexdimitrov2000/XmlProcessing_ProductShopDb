using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductShop.Models;

namespace ProductShop.Data.EntityConfiguration
{
    public class CategoryProductConfig : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            builder
                .HasOne(cp => cp.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(cp => cp.CategoryId);

            builder
                .HasOne(cp => cp.Product)
                .WithMany(p => p.Categories)
                .HasForeignKey(cp => cp.ProductId);
        }
    }
}
