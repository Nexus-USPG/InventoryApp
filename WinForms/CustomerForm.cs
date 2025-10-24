using InventoryApp.Domain;
using InventoryApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryApp.WinForms
{
    public partial class CustomerForm : Form
    {
        private readonly IClientRepository _clientRepo;

        private DataTable _table = new();
        private readonly BindingSource _bs = new();
        private bool _persisting = false;

        public CustomerForm(IClientRepository clientRepo)
        {
            InitializeComponent();
            _clientRepo = clientRepo;
        }

        private async void CustomerForm_Load_1(object sender, EventArgs e)
        {
            await LoadTableAsync();
            SetupGrid();
            SetupContextMenu();

            // Validaciones y errores
            dataGridCostumers.CellValidating += dataGridCostumers_CellValidating;
            dataGridCostumers.DataError += (s, ev) => { ev.ThrowException = false; };

            // Persistencia inmediata por celda (más estable que RowValidated)
            dataGridCostumers.CellValidated += dataGridCostumers_CellValidated;



            // DELETE con tecla Supr
            dataGridCostumers.UserDeletingRow += dataGridCostumers_UserDeletingRow;
        }

        // ================================
        // Carga de datos
        // ================================
        private async System.Threading.Tasks.Task LoadTableAsync()
        {
            _table = BuildSchema();

            var clientes = await _clientRepo.GetAllAsync();
            foreach (var c in clientes)
            {
                var r = _table.NewRow();
                r["id"] = c.Id;
                r["nombre"] = c.Nombre;
                r["nit"] = c.Nit;
                _table.Rows.Add(r);
            }

            _table.AcceptChanges();
            _bs.DataSource = _table;
            dataGridCostumers.DataSource = _bs;
        }

        private static DataTable BuildSchema()
        {
            var t = new DataTable("cliente");

            var cId = t.Columns.Add("id", typeof(int));
            cId.AllowDBNull = true;   // filas nuevas sin id
            cId.Unique = false;       // no PK aquí (evita problemas con nulos)

            t.Columns.Add("nombre", typeof(string));
            t.Columns.Add("nit", typeof(string));
            return t;
        }

        // ================================
        // Configuración visual
        // ================================
        private void SetupGrid()
        {
            dataGridCostumers.AutoGenerateColumns = true;
            dataGridCostumers.AllowUserToAddRows = true;
            dataGridCostumers.AllowUserToDeleteRows = true;
            dataGridCostumers.MultiSelect = false;
            dataGridCostumers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridCostumers.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            dataGridCostumers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dataGridCostumers.Columns["id"] is DataGridViewColumn idCol)
            {
                idCol.HeaderText = "ID";
                idCol.ReadOnly = true;  // lo pone la BD
                idCol.Width = 70;
            }
            if (dataGridCostumers.Columns["nombre"] is DataGridViewColumn nomCol)
            {
                nomCol.HeaderText = "Nombre";
                nomCol.ReadOnly = false;
            }
            if (dataGridCostumers.Columns["nit"] is DataGridViewColumn preCol)
            {
                preCol.HeaderText = "Nit";
                preCol.DefaultCellStyle.Format = "N2";
                preCol.ReadOnly = false;
            }
        }

        // ================================
        // Menú contextual (Eliminar)
        // ================================
        private void SetupContextMenu()
        {
            var ctx = new ContextMenuStrip();
            var miEliminar = new ToolStripMenuItem("Eliminar");

            miEliminar.Click += async (s, ev) =>
            {
                if (dataGridCostumers.CurrentRow?.DataBoundItem is not DataRowView drv) return;
                await DeleteRowAsync(drv, confirm: true);
            };

            ctx.Items.Add(miEliminar);
            dataGridCostumers.ContextMenuStrip = ctx;
        }

        // ================================
        // Validaciones
        // ================================
        private void dataGridCostumers_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var colName = dataGridCostumers.Columns[e.ColumnIndex].Name;
            var value = e.FormattedValue?.ToString() ?? "";

            if (colName == "nombre")
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    e.Cancel = true;
                    dataGridCostumers.Rows[e.RowIndex].ErrorText = "El nombre es requerido.";
                }
                else dataGridCostumers.Rows[e.RowIndex].ErrorText = string.Empty;
            }
            else if (colName == "nit")
            {
                if (!decimal.TryParse(value, out var d) || d < 0)
                {
                    e.Cancel = true;
                    dataGridCostumers.Rows[e.RowIndex].ErrorText = "Precio inválido (>= 0).";
                }
                else dataGridCostumers.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        // ================================
        // Persistencia inmediata por celda
        // ================================
        private async void dataGridCostumers_CellValidated(object? sender, DataGridViewCellEventArgs e)
        {
            if (_persisting) return;
            if (e.RowIndex < 0 || e.RowIndex >= dataGridCostumers.Rows.Count) return;

            var gridRow = dataGridCostumers.Rows[e.RowIndex];
            if (gridRow.IsNewRow) return;

            // Asegura que lo editado pasó al DataTable
            dataGridCostumers.EndEdit();
            _bs.EndEdit();

            if (gridRow.DataBoundItem is not DataRowView drv) return;
            var row = drv.Row;

            // Si la fila está "vacía", no persistimos
            if (IsNullOrEmpty(row, "nombre") &&
                IsNullOrZero(row, "nit"))
                return;

            try
            {
                _persisting = true;

                // INSERT (id nulo o 0 y fila Added)
                if ((row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                    && IsValidRow(row))
                {
                    var p = new Client
                    {
                        Nombre = row["nombre"]?.ToString() ?? "",
                        Nit = row["nit"]?.ToString() ?? ""
                    };

                    int newId = await _clientRepo.InsertAsync(p);

                    row["id"] = newId;
                    row.AcceptChanges(); // sincroniza estados
                    MessageBox.Show("Se agregó correctamente el cliente.",
                               "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // UPDATE (id válido y fila Modified)
                if (row.RowState == DataRowState.Modified && IsValidRow(row))
                {
                    int id = ToInt(row["id"]);
                    if (id > 0)
                    {
                        var p = new Client
                        {
                            Id = id,
                            Nombre = row["nombre"]?.ToString() ?? "",
                            Nit = row["nit"]?.ToString() ?? ""
                        };

                        var ok = await _clientRepo.UpdateAsync(p);
                        if (ok)
                        {
                            row.AcceptChanges();
                            MessageBox.Show("Se actualizó correctamente",
                                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else row.RowError = "No se pudo actualizar en BD.";
                    }
                }
            }
            catch (Exception ex)
            {
                row.RowError = ex.Message;
                MessageBox.Show("Error al persistir: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _persisting = false;
            }
        }

        // ================================
        // DELETE (tecla Supr)
        // ================================
        private async void dataGridCostumers_UserDeletingRow(object? sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row?.DataBoundItem is not DataRowView drv) return;

            // Confirmación
            var resp = MessageBox.Show("¿Eliminar este cliente?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) { e.Cancel = true; return; }

            // Ejecuta el borrado real (maneja Added/Existente adentro)
            var ok = await DeleteRowAsync(drv, confirm: false);
            if (!ok) e.Cancel = true;
        }

        // Elimina fila (desde menú o tecla) con protecciones
        private async System.Threading.Tasks.Task<bool> DeleteRowAsync(DataRowView drv, bool confirm)
        {
            if (_persisting) return false;

            var row = drv.Row;

            try
            {
                _persisting = true;

                // Si nunca se insertó en BD
                if (row.RowState == DataRowState.Added || row["id"] == DBNull.Value || ToInt(row["id"]) == 0)
                {
                    row.Delete(); // solo quita del DataTable
                    return true;
                }

                int id = ToInt(row["id"]);
                if (id <= 0) return false;

                if (confirm)
                {
                    var okConf = MessageBox.Show($"¿Eliminar el cliente #{id}?", "Confirmar",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                    if (!okConf) return false;
                }

                var ok = await _clientRepo.DeleteAsync(id);
                if (ok)
                {
                    row.Delete(); // elimina del DataTable
                    MessageBox.Show("cliente eliminado.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }

                MessageBox.Show("No se pudo eliminar en BD (¿referenciado en ventas?).",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                _persisting = false;
            }
        }

        // ================================
        // Helpers
        // ================================
        private static bool IsValidRow(DataRow r)
            => !IsNullOrEmpty(r, "nombre") && !IsNullOrEmpty(r, "nit");

        private static bool IsNullOrEmpty(DataRow r, string col)
            => !r.Table.Columns.Contains(col) || r[col] == DBNull.Value || string.IsNullOrWhiteSpace(r[col]?.ToString());

        private static bool IsNullOrZero(DataRow r, string col)
        {
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return true;
            var s = r[col].ToString();
            if (decimal.TryParse(s, out var d)) return d == 0m;
            if (int.TryParse(s, out var i)) return i == 0;
            return true;
        }

        private static bool TryGetDecimal(DataRow r, string col, out decimal value)
        {
            value = 0m;
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return false;
            return decimal.TryParse(r[col].ToString(), out value);
        }

        private static bool TryGetInt(DataRow r, string col, out int value)
        {
            value = 0;
            if (!r.Table.Columns.Contains(col) || r[col] == DBNull.Value) return false;
            return int.TryParse(r[col].ToString(), out value);
        }

        private static int ToInt(object? o)
            => o == null || o == DBNull.Value ? 0 : int.TryParse(o.ToString(), out var i) ? i : 0;

        private static decimal ToDecimal(object? o)
            => o == null || o == DBNull.Value ? 0m : decimal.TryParse(o.ToString(), out var d) ? d : 0m;

    }

}


