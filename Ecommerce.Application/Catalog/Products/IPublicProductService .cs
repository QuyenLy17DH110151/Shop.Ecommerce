using Ecommerce.ViewModel.Catalog.Product;
using Ecommerce.ViewModel.Catalog.Product.Public;
using Ecommerce.ViewModel.Common;
using System.Threading.Tasks;

namespace Ecommerce.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
