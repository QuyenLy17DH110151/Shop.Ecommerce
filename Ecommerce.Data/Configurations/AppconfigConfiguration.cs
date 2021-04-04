using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Data.Configurations
{
    class AppconfigConfiguration : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            // Đặt tên bảng
            builder.ToTable("AppConfig");
            //chọn propery là Khóa chính
            builder.HasKey(x => x.Key);
            //Required 
            builder.Property(x => x.Value).IsRequired();
        }
    }
}
