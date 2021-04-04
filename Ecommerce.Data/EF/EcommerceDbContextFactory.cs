using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ecommerce.Data.EF
{
    public class EShopContextFactory : IDesignTimeDbContextFactory<EcommerceDBContext>
    {
        public EcommerceDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("EcommerceDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<EcommerceDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EcommerceDBContext(optionsBuilder.Options);
        }
    }

}
