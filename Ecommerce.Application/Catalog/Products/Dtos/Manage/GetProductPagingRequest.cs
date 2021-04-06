using Ecommerce.Application.CommonDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Application.Catalog.Products.Dtos.Manage
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
