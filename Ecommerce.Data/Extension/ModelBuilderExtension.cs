using Ecommerce.Data.Entities;
using Ecommerce.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Extension
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppConfig>().HasData(
              new AppConfig() { Key = "HomeTitle", Value = "This is home page of eShopSolution" },
              new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of eShopSolution" },
              new AppConfig() { Key = "HomeDescription", Value = "This is description of eShopSolution" }
              );
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi-VN", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en-US", Name = "English", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    ParentId = null,
                    SortOrder = 1,
                    Status = Status.Active,
                },
                 new Category()
                 {
                     Id = 2,
                     IsShowOnHome = true,
                     ParentId = null,
                     SortOrder = 2,
                     Status = Status.Active
                 });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                  new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Áo nam", LanguageId = "vi-VN", SeoAlias = "ao-nam", SeoDescription = "Sản phẩm áo thời trang nam", SeoTitle = "Sản phẩm áo thời trang nam" },
                  new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Men Shirt", LanguageId = "en-US", SeoAlias = "men-shirt", SeoDescription = "The shirt products for men", SeoTitle = "The shirt products for men" },
                  new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Áo nữ", LanguageId = "vi-VN", SeoAlias = "ao-nu", SeoDescription = "Sản phẩm áo thời trang nữ", SeoTitle = "Sản phẩm áo thời trang women" },
                  new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Women Shirt", LanguageId = "en-US", SeoAlias = "women-shirt", SeoDescription = "The shirt products for women", SeoTitle = "The shirt products for women" }
                    );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    DateCreated = DateTime.Now,
                    OriginalPrice = 100000,
                    Price = 200000,
                    Stock = 0,
                    ViewCount = 0,
                });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "Áo sơ mi nam trắng Việt Tiến",
                     LanguageId = "vi-VN",
                     SeoAlias = "ao-so-mi-nam-trang-viet-tien",
                     SeoDescription = "Áo sơ mi nam trắng Việt Tiến",
                     SeoTitle = "Áo sơ mi nam trắng Việt Tiến",
                     Details = "Áo sơ mi nam trắng Việt Tiến",
                     Description = "Áo sơ mi nam trắng Việt Tiến"
                 },
                 new ProductTranslation()
                 {
                     Id = 2,
                     ProductId = 1,
                     Name = "Viet Tien Men T-Shirt",
                     LanguageId = "en-US",
                     SeoAlias = "viet-tien-men-t-shirt",
                     SeoDescription = "Viet Tien Men T-Shirt",
                     SeoTitle = "Viet Tien Men T-Shirt",
                     Details = "Viet Tien Men T-Shirt",
                     Description = "Viet Tien Men T-Shirt"
                 });
            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 }
                );

            // any guid
            //[Guid("3F7DE0AE-BE1B-4E92-9CA7-32A11F8C5EE1")]
            //[Guid("FADF5757-D5AA-476E-B7DF-F1BFCCA313D0")]
            var roleId = new Guid("3F7DE0AE-BE1B-4E92-9CA7-32A11F8C5EE1");
            var adminId = new Guid("FADF5757-D5AA-476E-B7DF-F1BFCCA313D0");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "lqboss1020304050@gmail.com",
                NormalizedEmail = "lqboss1020304050@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
                SecurityStamp = string.Empty,
                FristName = "Quyen",
                LastName = "Ly",
                Dob = new DateTime(2020, 01, 31)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

        }
    }
}
