using Ecommerce.Application.CommonDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Application.Catalog.Products.Dtos.Public
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public int CategoryId { get; set; }
    }
}
