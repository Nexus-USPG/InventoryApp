namespace InventoryApp.WinForms
{
    partial class CustomerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label_clientes = new Label();
            label_productos = new Label();
            dataGridCostumers = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridCostumers).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label_clientes);
            panel1.Controls.Add(label_productos);
            panel1.Controls.Add(dataGridCostumers);
            panel1.Location = new Point(-168, -73);
            panel1.Name = "panel1";
            panel1.Size = new Size(1136, 596);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // label_clientes
            // 
            label_clientes.AutoSize = true;
            label_clientes.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_clientes.Location = new Point(200, 100);
            label_clientes.Name = "label_clientes";
            label_clientes.Size = new Size(151, 50);
            label_clientes.TabIndex = 5;
            label_clientes.Text = "Clientes";
            // 
            // label_productos
            // 
            label_productos.AutoSize = true;
            label_productos.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_productos.Location = new Point(22, 25);
            label_productos.Name = "label_productos";
            label_productos.Size = new Size(189, 50);
            label_productos.TabIndex = 4;
            label_productos.Text = "Productos";
            // 
            // dataGridCostumers
            // 
            dataGridCostumers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridCostumers.Location = new Point(170, 204);
            dataGridCostumers.Name = "dataGridCostumers";
            dataGridCostumers.Size = new Size(801, 253);
            dataGridCostumers.TabIndex = 0;
            // 
            // CustomerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(844, 471);
            Controls.Add(panel1);
            Name = "CustomerForm";
            Text = "CustomerForm";
            Load += CustomerForm_Load_1;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridCostumers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label_productos;
        private DataGridView dataGridCostumers;
        private Label label_clientes;
    }
}