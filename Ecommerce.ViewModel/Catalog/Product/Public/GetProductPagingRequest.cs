using Ecommerce.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.Catalog.Product.Public
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
