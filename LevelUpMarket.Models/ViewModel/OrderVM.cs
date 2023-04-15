using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models.ViewModel
{
    public class OrderVM
    {
        public Orderheader Orderheader { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
