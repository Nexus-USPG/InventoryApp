using InventoryApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Repositories
{
    public interface ISalesRepository
    {
        Task<List<SaleView>> GetSalesAsync(DateTime? from = null, DateTime? to = null, int? clienteId = null);

        Task<List<SaleDetailView>> GetSaleDetailsAsync(int ventaId);
    }
}
