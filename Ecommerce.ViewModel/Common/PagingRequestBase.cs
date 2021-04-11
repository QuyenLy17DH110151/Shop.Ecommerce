using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.Common
{
    public class PagingRequestBase : RequestBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
