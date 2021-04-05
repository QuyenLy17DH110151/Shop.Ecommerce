using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Data.Configurations
{
    class ProductInCategoryConfiguration : IEntityTypeConfiguration<ProductInCategory>
    {
        public void Configure(EntityTypeBuilder<ProductInCategory> builder)
        {
            builder.ToTable("ProductInCategories");
            builder.HasKey(pc => new { pc.CategoryId, pc.ProductId });
            builder.HasOne(pc => pc.Product).WithMany(p => p.ProductInCategories)
                .HasForeignKey(pc => pc.ProductId);
            builder.HasOne(pc => pc.Category).WithMany(p => p.ProductInCategories)
               .HasForeignKey(pc => pc.CategoryId);
        }
    }
}


