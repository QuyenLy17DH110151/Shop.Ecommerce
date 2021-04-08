using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);
        Task<List<ProductViewModel>> GetAll();
    }
}
