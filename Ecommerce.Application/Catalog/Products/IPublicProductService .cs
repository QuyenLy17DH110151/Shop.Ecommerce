using Ecommerce.Application.Catalog.Products.Dtos;
using Ecommerce.Application.Catalog.Products.Dtos.Manage;
using Ecommerce.Application.CommonDtos;

namespace Ecommerce.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        PageResult<ProductViewModel> GetAllByCategoryId(GetProductPagingRequest request);
    }
}
