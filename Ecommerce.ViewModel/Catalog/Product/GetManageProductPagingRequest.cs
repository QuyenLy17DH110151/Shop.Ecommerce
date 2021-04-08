using Ecommerce.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.Catalog.Product
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public List<int> CategoryId { get; set; }
    }
}
