using Ecommerce.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.Catalog.Product
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}
