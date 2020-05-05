namespace DataPlotter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lstPath = new System.Windows.Forms.ListBox();
            this.btnAddInput = new System.Windows.Forms.Button();
            this.btnDeleteInput = new System.Windows.Forms.Button();
            this.btnEditColor = new System.Windows.Forms.Button();
            this.cmbProtocol = new System.Windows.Forms.ComboBox();
            this.btnAddProtocol = new System.Windows.Forms.Button();
            this.lstColor = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtToolPath = new System.Windows.Forms.TextBox();
            this.btnToolOpen = new System.Windows.Forms.Button();
            this.txtToolArguments = new System.Windows.Forms.TextBox();
            this.btnToolSave = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtLoad = new System.Windows.Forms.TextBox();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.cmbPorts = new System.Windows.Forms.ComboBox();
            this.btnClosePort = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.btnSaveBrowse = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBuffer = new System.Windows.Forms.TextBox();
            this.btnClearBuffer = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.22642F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new System.Drawing.Size(120, 25);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1192, 419);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1184, 386);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Setup";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 386);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstPath, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnAddInput, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteInput, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnEditColor, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbProtocol, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddProtocol, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstColor, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtToolPath, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnToolOpen, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtToolArguments, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnToolSave, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnProcess, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1184, 386);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Protocol";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 118);
            this.label3.TabIndex = 2;
            this.label3.Text = "Series line color";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 265);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 59);
            this.label4.TabIndex = 3;
            this.label4.Text = "Input files";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 32);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
            this.panel2.Size = new System.Drawing.Size(94, 112);
            this.panel2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 112);
            this.label2.TabIndex = 1;
            this.label2.Text = "Parser tool settings";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // lstPath
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstPath, 2);
            this.lstPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstPath.FormattingEnabled = true;
            this.lstPath.ItemHeight = 15;
            this.lstPath.Location = new System.Drawing.Point(103, 268);
            this.lstPath.Name = "lstPath";
            this.tableLayoutPanel1.SetRowSpan(this.lstPath, 2);
            this.lstPath.ScrollAlwaysVisible = true;
            this.lstPath.Size = new System.Drawing.Size(978, 115);
            this.lstPath.TabIndex = 5;
            // 
            // btnAddInput
            // 
            this.btnAddInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddInput.Location = new System.Drawing.Point(1087, 268);
            this.btnAddInput.Name = "btnAddInput";
            this.btnAddInput.Size = new System.Drawing.Size(94, 53);
            this.btnAddInput.TabIndex = 6;
            this.btnAddInput.Text = "Add";
            this.btnAddInput.UseVisualStyleBackColor = true;
            this.btnAddInput.Click += new System.EventHandler(this.btnAddInput_Click);
            // 
            // btnDeleteInput
            // 
            this.btnDeleteInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteInput.Location = new System.Drawing.Point(1087, 327);
            this.btnDeleteInput.Name = "btnDeleteInput";
            this.btnDeleteInput.Size = new System.Drawing.Size(94, 56);
            this.btnDeleteInput.TabIndex = 7;
            this.btnDeleteInput.Text = "Remove";
            this.btnDeleteInput.UseVisualStyleBackColor = true;
            this.btnDeleteInput.Click += new System.EventHandler(this.btnDeleteInput_Click);
            // 
            // btnEditColor
            // 
            this.btnEditColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEditColor.Location = new System.Drawing.Point(1087, 150);
            this.btnEditColor.Name = "btnEditColor";
            this.btnEditColor.Size = new System.Drawing.Size(94, 112);
            this.btnEditColor.TabIndex = 9;
            this.btnEditColor.Text = "Edit";
            this.btnEditColor.UseVisualStyleBackColor = true;
            this.btnEditColor.Click += new System.EventHandler(this.btnEditColor_Click);
            // 
            // cmbProtocol
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cmbProtocol, 2);
            this.cmbProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProtocol.FormattingEnabled = true;
            this.cmbProtocol.Location = new System.Drawing.Point(103, 3);
            this.cmbProtocol.Name = "cmbProtocol";
            this.cmbProtocol.Size = new System.Drawing.Size(978, 23);
            this.cmbProtocol.TabIndex = 10;
            this.cmbProtocol.SelectedIndexChanged += new System.EventHandler(this.cmbProtocol_SelectedIndexChanged);
            // 
            // btnAddProtocol
            // 
            this.btnAddProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddProtocol.Enabled = false;
            this.btnAddProtocol.Location = new System.Drawing.Point(1087, 3);
            this.btnAddProtocol.Name = "btnAddProtocol";
            this.btnAddProtocol.Size = new System.Drawing.Size(94, 23);
            this.btnAddProtocol.TabIndex = 11;
            this.btnAddProtocol.Text = "New";
            this.btnAddProtocol.UseVisualStyleBackColor = true;
            // 
            // lstColor
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstColor, 2);
            this.lstColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstColor.FormattingEnabled = true;
            this.lstColor.Location = new System.Drawing.Point(103, 150);
            this.lstColor.Name = "lstColor";
            this.lstColor.ScrollAlwaysVisible = true;
            this.lstColor.Size = new System.Drawing.Size(978, 112);
            this.lstColor.TabIndex = 12;
            this.lstColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstColor_DrawItem);
            this.lstColor.SelectedIndexChanged += new System.EventHandler(this.lstColor_SelectedIndexChanged);
            this.lstColor.DoubleClick += new System.EventHandler(this.lstColor_DoubleClick);
            this.lstColor.Leave += new System.EventHandler(this.lstColor_Leave);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(103, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 59);
            this.label5.TabIndex = 13;
            this.label5.Text = "Path";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(103, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 59);
            this.label6.TabIndex = 14;
            this.label6.Text = "Arguments";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtToolPath
            // 
            this.txtToolPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtToolPath.Location = new System.Drawing.Point(203, 32);
            this.txtToolPath.Multiline = true;
            this.txtToolPath.Name = "txtToolPath";
            this.txtToolPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtToolPath.Size = new System.Drawing.Size(878, 53);
            this.txtToolPath.TabIndex = 16;
            this.txtToolPath.TextChanged += new System.EventHandler(this.txtToolPath_TextChanged);
            // 
            // btnToolOpen
            // 
            this.btnToolOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToolOpen.Location = new System.Drawing.Point(1087, 32);
            this.btnToolOpen.Name = "btnToolOpen";
            this.btnToolOpen.Size = new System.Drawing.Size(94, 53);
            this.btnToolOpen.TabIndex = 17;
            this.btnToolOpen.Text = "Browse";
            this.btnToolOpen.UseVisualStyleBackColor = true;
            this.btnToolOpen.Click += new System.EventHandler(this.btnToolOpen_Click);
            // 
            // txtToolArguments
            // 
            this.txtToolArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtToolArguments.Location = new System.Drawing.Point(203, 91);
            this.txtToolArguments.Multiline = true;
            this.txtToolArguments.Name = "txtToolArguments";
            this.txtToolArguments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtToolArguments.Size = new System.Drawing.Size(878, 53);
            this.txtToolArguments.TabIndex = 18;
            this.txtToolArguments.TextChanged += new System.EventHandler(this.txtToolArguments_TextChanged);
            // 
            // btnToolSave
            // 
            this.btnToolSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnToolSave.Location = new System.Drawing.Point(1087, 91);
            this.btnToolSave.Name = "btnToolSave";
            this.btnToolSave.Size = new System.Drawing.Size(94, 53);
            this.btnToolSave.TabIndex = 19;
            this.btnToolSave.Text = "Save Tool";
            this.btnToolSave.UseVisualStyleBackColor = true;
            this.btnToolSave.Click += new System.EventHandler(this.btnToolSave_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnProcess.Location = new System.Drawing.Point(3, 327);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(94, 56);
            this.btnProcess.TabIndex = 20;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1184, 386);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Plot";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1184, 386);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tableLayoutPanel2);
            this.tabPage5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.150944F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1184, 386);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Device";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnUpdate, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLoad, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtLoad, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnOpenPort, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbPorts, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClosePort, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtSavePath, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnSaveBrowse, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtBuffer, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.btnClearBuffer, 2, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.49527F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.49527F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.49527F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56.51419F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1184, 386);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 30);
            this.label7.TabIndex = 0;
            this.label7.Text = "COM Port";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(3, 30);
            this.label8.Name = "label8";
            this.tableLayoutPanel2.SetRowSpan(this.label8, 3);
            this.label8.Size = new System.Drawing.Size(94, 141);
            this.label8.TabIndex = 1;
            this.label8.Text = "Command";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdate.Location = new System.Drawing.Point(1087, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(94, 24);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Save";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLoad.Location = new System.Drawing.Point(1087, 80);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(94, 41);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Start";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtLoad
            // 
            this.txtLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLoad.Location = new System.Drawing.Point(103, 33);
            this.txtLoad.Multiline = true;
            this.txtLoad.Name = "txtLoad";
            this.tableLayoutPanel2.SetRowSpan(this.txtLoad, 3);
            this.txtLoad.Size = new System.Drawing.Size(978, 135);
            this.txtLoad.TabIndex = 4;
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenPort.Location = new System.Drawing.Point(1087, 33);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(94, 41);
            this.btnOpenPort.TabIndex = 5;
            this.btnOpenPort.Text = "Open port";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnPort_Click);
            // 
            // cmbPorts
            // 
            this.cmbPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPorts.FormattingEnabled = true;
            this.cmbPorts.Location = new System.Drawing.Point(103, 3);
            this.cmbPorts.Name = "cmbPorts";
            this.cmbPorts.Size = new System.Drawing.Size(978, 23);
            this.cmbPorts.TabIndex = 6;
            this.cmbPorts.DropDown += new System.EventHandler(this.cmbPorts_DropDown);
            this.cmbPorts.SelectedIndexChanged += new System.EventHandler(this.cmbPorts_SelectedIndexChanged);
            this.cmbPorts.Click += new System.EventHandler(this.cmbPorts_Click);
            // 
            // btnClosePort
            // 
            this.btnClosePort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClosePort.Enabled = false;
            this.btnClosePort.Location = new System.Drawing.Point(1087, 127);
            this.btnClosePort.Name = "btnClosePort";
            this.btnClosePort.Size = new System.Drawing.Size(94, 41);
            this.btnClosePort.TabIndex = 7;
            this.btnClosePort.Text = "Close port";
            this.btnClosePort.UseVisualStyleBackColor = true;
            this.btnClosePort.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 171);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 184);
            this.label9.TabIndex = 8;
            this.label9.Text = "File path";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSavePath
            // 
            this.txtSavePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSavePath.Location = new System.Drawing.Point(103, 174);
            this.txtSavePath.Multiline = true;
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSavePath.Size = new System.Drawing.Size(978, 178);
            this.txtSavePath.TabIndex = 9;
            // 
            // btnSaveBrowse
            // 
            this.btnSaveBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveBrowse.Location = new System.Drawing.Point(1087, 174);
            this.btnSaveBrowse.Name = "btnSaveBrowse";
            this.btnSaveBrowse.Size = new System.Drawing.Size(94, 178);
            this.btnSaveBrowse.TabIndex = 10;
            this.btnSaveBrowse.Text = "Browse";
            this.btnSaveBrowse.UseVisualStyleBackColor = true;
            this.btnSaveBrowse.Click += new System.EventHandler(this.btnSaveBrowse_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 355);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 31);
            this.label10.TabIndex = 11;
            this.label10.Text = "Buffer size";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBuffer
            // 
            this.txtBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBuffer.Location = new System.Drawing.Point(103, 358);
            this.txtBuffer.Name = "txtBuffer";
            this.txtBuffer.Size = new System.Drawing.Size(978, 21);
            this.txtBuffer.TabIndex = 12;
            this.txtBuffer.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnClearBuffer
            // 
            this.btnClearBuffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearBuffer.Location = new System.Drawing.Point(1087, 358);
            this.btnClearBuffer.Name = "btnClearBuffer";
            this.btnClearBuffer.Size = new System.Drawing.Size(94, 25);
            this.btnClearBuffer.TabIndex = 13;
            this.btnClearBuffer.Text = "Clear";
            this.btnClearBuffer.UseVisualStyleBackColor = true;
            this.btnClearBuffer.Click += new System.EventHandler(this.btnClearBuffer_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBox1);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.830189F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1184, 386);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "About";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1184, 386);
            this.textBox1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // serialPort1
            // 
            this.serialPort1.DtrEnable = true;
            this.serialPort1.ReadTimeout = 1000;
            this.serialPort1.RtsEnable = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 419);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "LogicAnalyzerSTM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lstPath;
        private System.Windows.Forms.Button btnAddInput;
        private System.Windows.Forms.Button btnDeleteInput;
        private System.Windows.Forms.Button btnEditColor;
        private System.Windows.Forms.ComboBox cmbProtocol;
        private System.Windows.Forms.Button btnAddProtocol;
        private System.Windows.Forms.ListBox lstColor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtToolPath;
        private System.Windows.Forms.Button btnToolOpen;
        private System.Windows.Forms.TextBox txtToolArguments;
        private System.Windows.Forms.Button btnToolSave;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtLoad;
        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.ComboBox cmbPorts;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btnClosePort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Button btnSaveBrowse;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBuffer;
        private System.Windows.Forms.Button btnClearBuffer;
    }
}

