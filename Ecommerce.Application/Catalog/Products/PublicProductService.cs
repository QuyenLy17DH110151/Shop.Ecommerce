using Ecommerce.Data.EF;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Common;

namespace Ecommerce.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly EcommerceDBContext _ecommerceDbContext;

        public PublicProductService(EcommerceDBContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var query = from p in _ecommerceDbContext.Products
                        join pt in _ecommerceDbContext.ProductTranslations
                        on p.Id equals pt.ProductId
                        join pic in _ecommerceDbContext.ProductInCategories
                        on p.Id equals pic.ProductId
                        join c in _ecommerceDbContext.Categories
                        on pic.CategoryId equals c.Id
                        select new { p, pt, pic };

            var data = await query
                 .Select(x => new ProductViewModel()
                 {
                     Id = x.p.Id,
                     Name = x.pt.Name,
                     DateCreated = x.p.DateCreated,
                     Description = x.pt.Description,
                     Details = x.pt.Details,
                     LanguageId = x.pt.LanguageId,
                     OriginalPrice = x.p.OriginalPrice,
                     Price = x.p.Price,
                     SeoDescription = x.pt.SeoDescription,
                     SeoTitle = x.pt.SeoTitle,
                     Stock = x.p.Stock,
                     ViewCount = x.p.ViewCount
                 }).ToListAsync();

            return data;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            // Select - join
            var query = from p in _ecommerceDbContext.Products
                        join pt in _ecommerceDbContext.ProductTranslations
                        on p.Id equals pt.ProductId
                        join pic in _ecommerceDbContext.ProductInCategories
                        on p.Id equals pic.ProductId
                        join c in _ecommerceDbContext.Categories
                        on pic.CategoryId equals c.Id
                        select new { p, pt, pic };
            //filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //Paging
            int totalRow = await query.CountAsync();
            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreated,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();
            //Select and projection
            var pagingResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagingResult;
        }
    }
}
