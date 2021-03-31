using Ecommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Configurations
{
    class AppUserConfiguration:IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // Đặt tên bảng
            builder.ToTable("AppUsers");
            //chọn propery là Khóa chính
            //builder.HasKey(x => x.Id);
            //Required 
            builder.Property(x => x.Dob).IsRequired();
        }
    }
}
