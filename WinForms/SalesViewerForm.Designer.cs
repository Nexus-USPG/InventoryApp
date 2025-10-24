namespace InventoryApp.WinForms
{
    partial class SalesViewerForm
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
            dgvSales = new DataGridView();
            btnFilter = new Button();
            btnClear = new Button();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            dgvDetails = new DataGridView();
            cmbClienteFilter = new ComboBox();
            lblCount = new Label();
            label_detalleVentas = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            SuspendLayout();
            // 
            // dgvSales
            // 
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new Point(12, 228);
            dgvSales.Name = "dgvSales";
            dgvSales.Size = new Size(674, 506);
            dgvSales.TabIndex = 1;
            // 
            // btnFilter
            // 
            btnFilter.Location = new Point(227, 183);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(75, 23);
            btnFilter.TabIndex = 2;
            btnFilter.Text = "Filtrar";
            btnFilter.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(330, 183);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 3;
            btnClear.Text = "Limpiar";
            btnClear.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(350, 140);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(200, 23);
            dtpTo.TabIndex = 4;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(91, 140);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 23);
            dtpFrom.TabIndex = 5;
            // 
            // dgvDetails
            // 
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Location = new Point(727, 228);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.Size = new Size(695, 506);
            dgvDetails.TabIndex = 6;
            // 
            // cmbClienteFilter
            // 
            cmbClienteFilter.FormattingEnabled = true;
            cmbClienteFilter.Location = new Point(91, 96);
            cmbClienteFilter.Name = "cmbClienteFilter";
            cmbClienteFilter.Size = new Size(121, 23);
            cmbClienteFilter.TabIndex = 7;
            // 
            // lblCount
            // 
            lblCount.AutoSize = true;
            lblCount.Location = new Point(12, 737);
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(0, 15);
            lblCount.TabIndex = 8;
            // 
            // label_detalleVentas
            // 
            label_detalleVentas.AutoSize = true;
            label_detalleVentas.Font = new Font("Segoe UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label_detalleVentas.Location = new Point(31, 23);
            label_detalleVentas.Name = "label_detalleVentas";
            label_detalleVentas.Size = new Size(303, 50);
            label_detalleVentas.TabIndex = 9;
            label_detalleVentas.Text = "Detalle de ventas";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 104);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 10;
            label1.Text = "Cliente";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(61, 146);
            label2.Name = "label2";
            label2.Size = new Size(24, 15);
            label2.TabIndex = 11;
            label2.Text = "De:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(304, 146);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 12;
            label3.Text = "Hasta:";
            // 
            // SalesViewerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1463, 769);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(label_detalleVentas);
            Controls.Add(lblCount);
            Controls.Add(cmbClienteFilter);
            Controls.Add(dgvDetails);
            Controls.Add(dtpFrom);
            Controls.Add(dtpTo);
            Controls.Add(btnClear);
            Controls.Add(btnFilter);
            Controls.Add(dgvSales);
            Name = "SalesViewerForm";
            Text = "SalesViewerForm";
            Load += SalesViewerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvSales;
        private Button btnFilter;
        private Button btnClear;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private DataGridView dgvDetails;
        private ComboBox cmbClienteFilter;
        private Label lblCount;
        private Label label_detalleVentas;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}