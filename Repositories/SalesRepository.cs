using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace InventoryApp.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        public async Task<List<SaleView>> GetSalesAsync(DateTime? from = null, DateTime? to = null, int? clienteId = null)
        {
            var list = new List<SaleView>();

            var sql = "select v.id, c.nombre, v.creado_en, sum(dv.subtotal) total from venta v join cliente c on c.id = v.cliente_id join detalle_venta dv on dv.venta_id = v.id {WHERE} group by v.id order by v.creado_en desc;";

            var whereClauses = new List<string>();
            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand { Connection = con };

            if (from.HasValue)
            {
                whereClauses.Add("v.creado_en >= @from");
                cmd.Parameters.AddWithValue("@from", from.Value);
            }
            if (to.HasValue)
            {
                whereClauses.Add("v.creado_en <= @to");
                cmd.Parameters.AddWithValue("@to", to.Value);
            }
            if (clienteId.HasValue && clienteId.Value > 0)
            {
                whereClauses.Add("v.cliente_id = @clienteId");
                cmd.Parameters.AddWithValue("@clienteId", clienteId.Value);
            }

            var where = whereClauses.Count == 0 ? "" : "WHERE " + string.Join(" AND ", whereClauses);
            cmd.CommandText = sql.Replace("{WHERE}", where);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new SaleView
                {
                    Id = rd.GetInt32("id"),
                    Cliente = rd.GetString("nombre"),
                    Fecha = rd.GetDateTime("creado_en"),
                    Total = rd.IsDBNull(rd.GetOrdinal("total")) ? 0m : rd.GetDecimal("total")
                });
            }

            return list;
        }

        public async Task<List<SaleDetailView>> GetSaleDetailsAsync(int ventaId)
        {
            var list = new List<SaleDetailView>();
            var sql = @"
                    select p.nombre as producto, dv.cantidad, dv.precio_unit, dv.subtotal
                    from producto p
                    join detalle_venta dv on dv.producto_id = p.id
                    join venta v on dv.venta_id = v.id 
                    where v.id = @id;";

            using var con = DbConnectionFactory.Instance.CreateOpen();
            using var cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", ventaId);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                list.Add(new SaleDetailView
                {
                    Producto = rd.GetString("producto"),
                    Cantidad = rd.GetInt32("cantidad"),
                    PrecioUnit = rd.GetDecimal("precio_unit"),
                    Subtotal = rd.GetDecimal("subtotal")
                });
            }

            return list;
        }
    }
}
