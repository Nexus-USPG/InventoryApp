using InventoryApp.Domain;
using InventoryApp.Repositories;
using Org.BouncyCastle.Asn1.Cmp;
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
    public partial class SalesViewerForm : Form
    {
        private readonly IClientRepository _clientRepo;
        private readonly ISalesRepository _salesRepo;

        private readonly BindingSource _bsSales = new();
        private readonly BindingSource _bsDetails = new();

        public SalesViewerForm(IClientRepository clientRepo, ISalesRepository salesRepo)
        {
            _clientRepo = clientRepo;
            _salesRepo = salesRepo;
            InitializeComponent();

            this.Load += SalesViewerForm_Load;
            btnFilter.Click += async (_, __) => await LoadSalesAsync();
            btnClear.Click += async (_, __) => { ClearFilters(); await LoadSalesAsync(); };

            dgvSales.SelectionChanged += DgvSales_SelectionChanged;
        }

        private async void SalesViewerForm_Load(object? sender, EventArgs e)
        {
            ConfigureGrids();
            await LoadClientsAsync();
            
            dtpTo.Value = DateTime.Now;
            dtpFrom.Value = DateTime.Now.AddDays(-30);
            await LoadSalesAsync();
        }

        private void ConfigureGrids()
        {
            // Datagrid Ventas
            dgvSales.AutoGenerateColumns = false;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.MultiSelect = false;
            dgvSales.Columns.Clear();

            dgvSales.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", Name = "colId", HeaderText = "ID", Width = 60 });
            dgvSales.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cliente", Name = "colCliente", HeaderText = "Cliente", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvSales.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha",
                Name = "colFecha",
                HeaderText = "Fecha",
                Width = 160,
                DefaultCellStyle = { Format = "g" } // fecha y hora corta
            });
            dgvSales.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Total",
                Name = "colTotal",
                HeaderText = "Total",
                Width = 120,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvSales.DataSource = _bsSales;

            // Detalle de ventas
            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.MultiSelect = false;
            dgvDetails.Columns.Clear();

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Producto", Name = "d_colProducto", HeaderText = "Producto", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cantidad", Name = "d_colCantidad", HeaderText = "Cantidad", Width = 90 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnit",
                Name = "d_colPrecio",
                HeaderText = "Precio Unit.",
                Width = 110,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Subtotal",
                Name = "d_colSub",
                HeaderText = "Subtotal",
                Width = 120,
                DefaultCellStyle = { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvDetails.DataSource = _bsDetails;
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                var clients = await _clientRepo.GetAllAsync();
                
                var list = new List<Client> { new Client { Id = 0, Nombre = "-- Todos --" } };
                list.AddRange(clients);
                cmbClienteFilter.DataSource = list;
                cmbClienteFilter.DisplayMember = "Nombre";
                cmbClienteFilter.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadSalesAsync()
        {
            try
            {
                int? clienteId = null;
                if (cmbClienteFilter.SelectedValue is int id && id > 0) clienteId = id;

                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddTicks(-1);

                var results = await _salesRepo.GetSalesAsync(from, to, clienteId);
                _bsSales.DataSource = new BindingList<SaleView>(results);
                lblCount.Text = $"Ventas: {results.Count}";
                _bsDetails.DataSource = new BindingList<SaleDetailView>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando ventas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvSales_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvSales.CurrentRow?.DataBoundItem is not SaleView sv)
            {
                _bsDetails.DataSource = new BindingList<SaleDetailView>();
                return;
            }

            try
            {
                var details = await _salesRepo.GetSaleDetailsAsync(sv.Id);
                _bsDetails.DataSource = new BindingList<SaleDetailView>(details);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFilters()
        {
            cmbClienteFilter.SelectedIndex = 0;
            dtpFrom.Value = DateTime.Now.AddDays(-30);
            dtpTo.Value = DateTime.Now;
        }
    }
}