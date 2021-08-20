using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Models
{
    public class HomeIndexViewModel
    {
        public IList<CarViewModel> Cars { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }
        public SearchCarIndexModel Search { get; set; }

    }
}
