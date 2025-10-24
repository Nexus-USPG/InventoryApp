using InventoryApp.Repositories;
using InventoryApp.Services;

namespace InventoryApp.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IProductRepository _productRepo;
        private readonly IClientRepository _clientRepo;
        private readonly SalesService _salesService;
        private readonly ISalesRepository _salesRepo;
        
        public MainForm(IProductRepository productRepo, IClientRepository clientRepo, SalesService salesService, ISalesRepository saleRepository)
        {
            InitializeComponent();
            _productRepo = productRepo;
            _clientRepo = clientRepo;
            _salesService = salesService;
            _salesRepo = saleRepository;
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new ProductsInlineForm(_productRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new NewSaleInlineForm(_productRepo, _clientRepo, _salesService);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new CustomerForm(_clientRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void btnVerVentas_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                using var f = new SalesViewerForm(_clientRepo, _salesRepo);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog(this);
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }
    }
}
