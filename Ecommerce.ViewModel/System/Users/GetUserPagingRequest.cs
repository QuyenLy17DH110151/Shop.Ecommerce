using Ecommerce.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.ViewModel.System.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public string NumberPhone { get; set; }
    }
}
