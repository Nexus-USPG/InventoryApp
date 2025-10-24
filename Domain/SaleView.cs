using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Domain
{
    public class SaleView
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = "";
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
