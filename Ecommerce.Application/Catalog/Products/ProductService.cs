using Ecommerce.Application.Common;
using Ecommerce.Data.EF;
using Ecommerce.Data.Entities;
using Ecommerce.Utilities.Exceptions;
using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Catalog.ProductImage;
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
    public class ProductService : IProductService
    {
        private readonly EcommerceDBContext _ecommerceDbContext;
        private readonly IStorageService _storageService;

        public ProductService(EcommerceDBContext ecommerceDbContext, IStorageService storageService)
        {
            _ecommerceDbContext = ecommerceDbContext;
            _storageService = storageService;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.UtcNow,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.FileImage != null)
            {
                productImage.ImagePath = await this.SaveFile(request.FileImage);
                productImage.FileSize = request.FileImage.Length;
            }
            _ecommerceDbContext.ProductImages.Add(productImage);
            await _ecommerceDbContext.SaveChangesAsync();
            return productImage.Id;
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
            return product.Id;
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
                var images = _ecommerceDbContext.ProductImages.Where(i => i.ProductId == productId);
                foreach (var image in images)
                {
                    await _storageService.DeleteFileAsync(image.ImagePath);
                }
                return await _ecommerceDbContext.SaveChangesAsync();
            }



        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //1. Select join
            var query = from p in _ecommerceDbContext.Products
                        join pt in _ecommerceDbContext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _ecommerceDbContext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _ecommerceDbContext.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };
            //2. filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pic.CategoryId == request.CategoryId);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
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
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
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

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _ecommerceDbContext.Products.FindAsync(productId);
            var productTranlastion = await _ecommerceDbContext.ProductTranslations.FirstOrDefaultAsync(
            x => x.Product.Id == productId
            && x.LanguageId == languageId);
            var result = new ProductViewModel()
            {
                Id = product.Id,
                Name = productTranlastion.Name != null ? productTranlastion.Name : null,
                DateCreated = product.DateCreated,
                Description = productTranlastion.Description != null ? productTranlastion.Description : null,
                Details = productTranlastion.Details != null ? productTranlastion.Details : null,
                LanguageId = productTranlastion.LanguageId != null ? productTranlastion.LanguageId : null,
                OriginalPrice = product.OriginalPrice,
                Price = product.Price,
                SeoDescription = productTranlastion.SeoDescription != null ? productTranlastion.SeoDescription : null,
                SeoTitle = productTranlastion.SeoTitle != null ? productTranlastion.SeoTitle : null,
                Stock = product.Stock,
                ViewCount = product.ViewCount
            };
            return result;
        }

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var imageProduct = await _ecommerceDbContext.ProductImages.FindAsync(imageId);
            if (imageProduct == null)
            {
                throw new EcommerceException($"Cannot found Image_Product with id : {imageId}");
            }
            var productImage = await _ecommerceDbContext.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            var result = new ProductImageViewModel()
            {
                Caption = productImage.Caption,
                DateCreated = productImage.DateCreated,
                FileSize = productImage.FileSize,
                Id = productImage.Id,
                IsDefault = productImage.IsDefault,
                SortOrder = productImage.SortOrder
            };
            return result;
        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            return await _ecommerceDbContext.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    IsDefault = i.IsDefault,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var imageProduct = await _ecommerceDbContext.ProductImages.FindAsync(imageId);
            if (imageProduct == null)
            {
                throw new EcommerceException($"Cannot found Image_Product with id : {imageId}");
            }
            _ecommerceDbContext.Remove(imageProduct);
            return await _ecommerceDbContext.SaveChangesAsync();

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

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {

            var productImage = await _ecommerceDbContext.ProductImages.FindAsync(imageId);
            if (productImage == null)
            {
                throw new EcommerceException($"Cannot found a productImage with id:  {imageId}");
            }
            if (request.FileImage != null)
            {
                productImage.ImagePath = await this.SaveFile(request.FileImage);
                productImage.FileSize = request.FileImage.Length;
            }
            _ecommerceDbContext.ProductImages.Update(productImage);
            return await _ecommerceDbContext.SaveChangesAsync();
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
