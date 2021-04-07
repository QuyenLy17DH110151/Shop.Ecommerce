using Ecommerce.Application.Catalog.Products.Dtos;
using Ecommerce.Application.Catalog.Products.Dtos.Public;
using Ecommerce.Application.CommonDtos;
using System.Threading.Tasks;

namespace Ecommerce.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
