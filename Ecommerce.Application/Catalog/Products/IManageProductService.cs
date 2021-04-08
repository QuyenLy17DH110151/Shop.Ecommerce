using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest request);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);
        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addedQuantity);
        Task AddViewCount(int productId);
        Task<List<ProductViewModel>> GetAll();
        Task<ProductViewModel> GetById(int productId,string languageId);
        Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> AddImage(int productId, List<IFormFile> files);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, string caption,bool isDefault);
        Task<ProductImageViewModel> GetListImage(int productId);
    }
}
