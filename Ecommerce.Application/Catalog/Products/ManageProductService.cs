using Ecommerce.Application.Common;
using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Ecommerce.Utilities.Exceptions;
using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Catalog.Product.Manage;
using Ecommerce.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ecommerce.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EcommerceDBContext _ecommerceDbContext;
        private readonly IStorageService _storageService;

        public ManageProductService(EcommerceDBContext ecommerceDbContext, IStorageService storageService)
        {
            _ecommerceDbContext = ecommerceDbContext;
            _storageService = storageService;
        }

        public Task<int> AddImage(int productId, List<IFormFile> files)
        {
            throw new NotImplementedException();
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
                ProductTranslations = new List<ProductTranslation>
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

            //Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }

            _ecommerceDbContext.Add(product);
            await _ecommerceDbContext.SaveChangesAsync();
            return 0;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EcommerceException($"Cannot find a product : {productId}");
            }
            else
            {
                _ecommerceDbContext.Products.Remove(product);
                return await _ecommerceDbContext.SaveChangesAsync();
            }

            var images = _ecommerceDbContext.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
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
                        select new { p, pt, pic };
            //filter
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId.Count > 0)
            {
                query = query.Where(p => request.CategoryId.Contains(p.pic.CategoryId));
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

        public Task<int> RemoveImage(int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(request.Id);
            var productTranlastion = await _ecommerceDbContext.ProductTranslations.FirstOrDefaultAsync(
            x => x.Product.Id == request.Id 
            && x.LanguageId == request.LanguageId);
            if (product == null || productTranlastion == null)
            {
                throw new EcommerceException($"Cannot find a product with id : {request.Id}");
            }

            productTranlastion.Name = request.Name;
            productTranlastion.SeoAlias = request.SeoAlias;
            productTranlastion.SeoDescription = request.SeoDescription;
            productTranlastion.SeoTitle = request.SeoTitle;
            productTranlastion.Description = request.Description;
            productTranlastion.Details = request.Details;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _ecommerceDbContext.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _ecommerceDbContext.ProductImages.Update(thumbnailImage);
                }
            }

            return await _ecommerceDbContext.SaveChangesAsync();
        }

        public Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EcommerceException($"Cannot find a product with id : {productId}");
            }
            product.Price = newPrice;
            return await _ecommerceDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EcommerceException($"Cannot find a product with id : {productId}");
            }
            product.Stock += addedQuantity;
            return await _ecommerceDbContext.SaveChangesAsync() > 0;
        }
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
