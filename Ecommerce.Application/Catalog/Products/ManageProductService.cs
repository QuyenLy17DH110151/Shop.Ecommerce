using Ecommerce.Application.Catalog.Products.Dtos;
using Ecommerce.Application.Catalog.Products.Dtos.Manage;
using Ecommerce.Application.CommonDtos;
using Ecommerce.Data.EF;
using Ecommerce.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EcommerceDBContext _ecommerceDbContext;

        public ManageProductService(EcommerceDBContext ecommerceDbContext)
        {
            _ecommerceDbContext = ecommerceDbContext;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _ecommerceDbContext.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Data.Entities.Product
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.UtcNow,
                SeoAlias = request.SeoAlias,
                ProductTranslations = new List<Data.Entities.ProductTranslation>
                {
                    new Data.Entities.ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias =request.SeoAlias,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _ecommerceDbContext.Add(product);
            await _ecommerceDbContext.SaveChangesAsync();
            return 0;
        }

        public async Task<int> Delete(Guid productId)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            if (product==null)
            {
                throw new EcommerceException($"Cannot find a product : {productId}");
            }
            else
            {
                _ecommerceDbContext.Products.Remove(product);
                return await _ecommerceDbContext.SaveChangesAsync();
            }
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            // Select - join
            var query = from p in _ecommerceDbContext.Products
                        join pt in _ecommerceDbContext.ProductTranslations
                        on p.Id equals pt.ProductId
                        join pic in _ecommerceDbContext.ProductInCategories
                        on p.Id equals pic.ProductId
                        join c in _ecommerceDbContext.Categories
                        on pic.CategoryId equals c.Id
                        select new { p, pt ,pic };
            //filter
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId.Count>0)
            {
                query = query.Where(p => request.CategoryId.Contains(p.pic.CategoryId));
            }
            //Paging
            int totalRow = await query.CountAsync();
            var data = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x=>new ProductViewModel() { 
                  Id=x.p.Id, 
                  Name=x.pt.Name,
                  DateCreated=x.p.DateCreated,
                  Description = x.pt.Description,
                  Details=x.pt.Details,
                  LanguageId=x.pt.LanguageId,
                  OriginalPrice=x.p.OriginalPrice,
                  Price=x.p.Price,
                  SeoDescription=x.pt.SeoDescription,
                  SeoTitle=x.pt.SeoTitle,
                  Stock = x.p.Stock,
                  ViewCount=x.p.ViewCount
                }).ToListAsync();
            //Select and projection
            var pagingResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagingResult;
        }

        public Task<int> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }
    }
}
