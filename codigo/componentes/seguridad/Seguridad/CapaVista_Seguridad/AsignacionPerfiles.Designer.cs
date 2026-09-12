namespace AplicacionPerfiles
{
    partial class AsignacionPerfiles
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxMascota = new System.Windows.Forms.PictureBox();
            this.labelSubtitulo = new System.Windows.Forms.Label();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.pictureBoxBanner = new System.Windows.Forms.PictureBox();
            this.panelContenedorListas = new System.Windows.Forms.Panel();
            this.tableLayoutPanelContenido = new System.Windows.Forms.TableLayoutPanel();
            this.panelConsulta = new System.Windows.Forms.Panel();
            this.buttonCancelarConsulta = new System.Windows.Forms.Button();
            this.panelListaConsulta = new System.Windows.Forms.Panel();
            this.comboBoxUsuariosConsulta = new System.Windows.Forms.ComboBox();
            this.labelUsuariosConsulta = new System.Windows.Forms.Label();
            this.labelTituloConsulta = new System.Windows.Forms.Label();
            this.panelIconConsulta = new System.Windows.Forms.Panel();
            this.labelIconConsulta = new System.Windows.Forms.Label();
            this.panelAsignacion = new System.Windows.Forms.Panel();
            this.buttonAsignar = new System.Windows.Forms.Button();
            this.buttonCancelarAsignacion = new System.Windows.Forms.Button();
            this.panelListaAsignacion = new System.Windows.Forms.Panel();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.comboBoxPerfilesAsignacion = new System.Windows.Forms.ComboBox();
            this.comboBoxUsuariosAsignacion = new System.Windows.Forms.ComboBox();
            this.labelPerfilesAsignacion = new System.Windows.Forms.Label();
            this.labelUsuariosAsignacion = new System.Windows.Forms.Label();
            this.labelTituloAsignacion = new System.Windows.Forms.Label();
            this.panelIconAsignacion = new System.Windows.Forms.Panel();
            this.labelIconAsignacion = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMascota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).BeginInit();
            this.panelContenedorListas.SuspendLayout();
            this.tableLayoutPanelContenido.SuspendLayout();
            this.panelConsulta.SuspendLayout();
            this.panelIconConsulta.SuspendLayout();
            this.panelAsignacion.SuspendLayout();
            this.panelIconAsignacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.panelHeader.Controls.Add(this.pictureBox1);
            this.panelHeader.Controls.Add(this.pictureBoxMascota);
            this.panelHeader.Controls.Add(this.labelSubtitulo);
            this.panelHeader.Controls.Add(this.labelTitulo);
            this.panelHeader.Controls.Add(this.pictureBoxBanner);
            this.panelHeader.Location = new System.Drawing.Point(24, 24);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1490, 224);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CapaVista_Seguridad.Properties.Resources.iconAsignacionPerfiles;
            this.pictureBox1.Location = new System.Drawing.Point(39, 47);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(89, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxMascota
            // 
            this.pictureBoxMascota.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxMascota.BackColor = System.Drawing.Color.Linen;
            this.pictureBoxMascota.Image = global::CapaVista_Seguridad.Properties.Resources._8;
            this.pictureBoxMascota.Location = new System.Drawing.Point(905, 3);
            this.pictureBoxMascota.Name = "pictureBoxMascota";
            this.pictureBoxMascota.Size = new System.Drawing.Size(193, 216);
            this.pictureBoxMascota.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMascota.TabIndex = 4;
            this.pictureBoxMascota.TabStop = false;
            // 
            // labelSubtitulo
            // 
            this.labelSubtitulo.AutoSize = true;
            this.labelSubtitulo.BackColor = System.Drawing.Color.Linen;
            this.labelSubtitulo.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.labelSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.labelSubtitulo.Location = new System.Drawing.Point(158, 92);
            this.labelSubtitulo.Name = "labelSubtitulo";
            this.labelSubtitulo.Size = new System.Drawing.Size(497, 25);
            this.labelSubtitulo.TabIndex = 3;
            this.labelSubtitulo.Text = "Administra la consulta y asignación de perfiles a usuarios.";
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.BackColor = System.Drawing.Color.Linen;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.labelTitulo.Location = new System.Drawing.Point(156, 38);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(407, 50);
            this.labelTitulo.TabIndex = 2;
            this.labelTitulo.Text = "Asignación de Perfiles";
            // 
            // pictureBoxBanner
            // 
            this.pictureBoxBanner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxBanner.BackColor = System.Drawing.Color.Linen;
            this.pictureBoxBanner.Image = global::CapaVista_Seguridad.Properties.Resources.banner;
            this.pictureBoxBanner.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxBanner.Name = "pictureBoxBanner";
            this.pictureBoxBanner.Size = new System.Drawing.Size(1490, 224);
            this.pictureBoxBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBanner.TabIndex = 0;
            this.pictureBoxBanner.TabStop = false;
            // 
            // panelContenedorListas
            // 
            this.panelContenedorListas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContenedorListas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(238)))), ((int)(((byte)(230)))));
            this.panelContenedorListas.Controls.Add(this.tableLayoutPanelContenido);
            this.panelContenedorListas.Location = new System.Drawing.Point(24, 264);
            this.panelContenedorListas.Name = "panelContenedorListas";
            this.panelContenedorListas.Size = new System.Drawing.Size(1490, 604);
            this.panelContenedorListas.TabIndex = 1;
            // 
            // tableLayoutPanelContenido
            // 
            this.tableLayoutPanelContenido.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelContenido.ColumnCount = 2;
            this.tableLayoutPanelContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelContenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelContenido.Controls.Add(this.panelConsulta, 0, 0);
            this.tableLayoutPanelContenido.Controls.Add(this.panelAsignacion, 1, 0);
            this.tableLayoutPanelContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelContenido.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelContenido.Name = "tableLayoutPanelContenido";
            this.tableLayoutPanelContenido.RowCount = 1;
            this.tableLayoutPanelContenido.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContenido.Size = new System.Drawing.Size(1490, 604);
            this.tableLayoutPanelContenido.TabIndex = 0;
            // 
            // panelConsulta
            // 
            this.panelConsulta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.panelConsulta.Controls.Add(this.buttonCancelarConsulta);
            this.panelConsulta.Controls.Add(this.panelListaConsulta);
            this.panelConsulta.Controls.Add(this.comboBoxUsuariosConsulta);
            this.panelConsulta.Controls.Add(this.labelUsuariosConsulta);
            this.panelConsulta.Controls.Add(this.labelTituloConsulta);
            this.panelConsulta.Controls.Add(this.panelIconConsulta);
            this.panelConsulta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConsulta.Location = new System.Drawing.Point(3, 3);
            this.panelConsulta.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.panelConsulta.Name = "panelConsulta";
            this.panelConsulta.Size = new System.Drawing.Size(730, 598);
            this.panelConsulta.TabIndex = 0;
            this.panelConsulta.Paint += new System.Windows.Forms.PaintEventHandler(this.panelConsulta_Paint);
            // 
            // buttonCancelarConsulta
            // 
            this.buttonCancelarConsulta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelarConsulta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.buttonCancelarConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancelarConsulta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonCancelarConsulta.ForeColor = System.Drawing.Color.White;
            this.buttonCancelarConsulta.Location = new System.Drawing.Point(517, 210);
            this.buttonCancelarConsulta.Name = "buttonCancelarConsulta";
            this.buttonCancelarConsulta.Size = new System.Drawing.Size(174, 46);
            this.buttonCancelarConsulta.TabIndex = 2;
            this.buttonCancelarConsulta.Text = "Cancelar   ❌";
            this.buttonCancelarConsulta.UseVisualStyleBackColor = false;
            this.buttonCancelarConsulta.Click += new System.EventHandler(this.buttonCancelarConsulta_Click);
            // 
            // panelListaConsulta
            // 
            this.panelListaConsulta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelListaConsulta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(241)))), ((int)(((byte)(234)))));
            this.panelListaConsulta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListaConsulta.Location = new System.Drawing.Point(32, 180);
            this.panelListaConsulta.Name = "panelListaConsulta";
            this.panelListaConsulta.Size = new System.Drawing.Size(462, 370);
            this.panelListaConsulta.TabIndex = 1;
            // 
            // comboBoxUsuariosConsulta
            // 
            this.comboBoxUsuariosConsulta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxUsuariosConsulta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxUsuariosConsulta.FormattingEnabled = true;
            this.comboBoxUsuariosConsulta.Location = new System.Drawing.Point(32, 130);
            this.comboBoxUsuariosConsulta.Name = "comboBoxUsuariosConsulta";
            this.comboBoxUsuariosConsulta.Size = new System.Drawing.Size(631, 29);
            this.comboBoxUsuariosConsulta.TabIndex = 0;
            // 
            // labelUsuariosConsulta
            // 
            this.labelUsuariosConsulta.AutoSize = true;
            this.labelUsuariosConsulta.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelUsuariosConsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.labelUsuariosConsulta.Location = new System.Drawing.Point(32, 104);
            this.labelUsuariosConsulta.Name = "labelUsuariosConsulta";
            this.labelUsuariosConsulta.Size = new System.Drawing.Size(75, 23);
            this.labelUsuariosConsulta.TabIndex = 4;
            this.labelUsuariosConsulta.Text = "Usuarios";
            // 
            // labelTituloConsulta
            // 
            this.labelTituloConsulta.AutoSize = true;
            this.labelTituloConsulta.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelTituloConsulta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.labelTituloConsulta.Location = new System.Drawing.Point(90, 36);
            this.labelTituloConsulta.Name = "labelTituloConsulta";
            this.labelTituloConsulta.Size = new System.Drawing.Size(331, 30);
            this.labelTituloConsulta.TabIndex = 3;
            this.labelTituloConsulta.Text = "Consulta de Perfiles a Usuarios";
            // 
            // panelIconConsulta
            // 
            this.panelIconConsulta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.panelIconConsulta.Controls.Add(this.labelIconConsulta);
            this.panelIconConsulta.Location = new System.Drawing.Point(32, 28);
            this.panelIconConsulta.Name = "panelIconConsulta";
            this.panelIconConsulta.Size = new System.Drawing.Size(44, 44);
            this.panelIconConsulta.TabIndex = 5;
            this.panelIconConsulta.Paint += new System.Windows.Forms.PaintEventHandler(this.panelIconConsulta_Paint);
            // 
            // labelIconConsulta
            // 
            this.labelIconConsulta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIconConsulta.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelIconConsulta.ForeColor = System.Drawing.Color.White;
            this.labelIconConsulta.Location = new System.Drawing.Point(0, 0);
            this.labelIconConsulta.Name = "labelIconConsulta";
            this.labelIconConsulta.Size = new System.Drawing.Size(44, 44);
            this.labelIconConsulta.TabIndex = 0;
            this.labelIconConsulta.Text = "🔍";
            this.labelIconConsulta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelAsignacion
            // 
            this.panelAsignacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.panelAsignacion.Controls.Add(this.buttonAsignar);
            this.panelAsignacion.Controls.Add(this.buttonCancelarAsignacion);
            this.panelAsignacion.Controls.Add(this.panelListaAsignacion);
            this.panelAsignacion.Controls.Add(this.buttonAgregar);
            this.panelAsignacion.Controls.Add(this.comboBoxPerfilesAsignacion);
            this.panelAsignacion.Controls.Add(this.comboBoxUsuariosAsignacion);
            this.panelAsignacion.Controls.Add(this.labelPerfilesAsignacion);
            this.panelAsignacion.Controls.Add(this.labelUsuariosAsignacion);
            this.panelAsignacion.Controls.Add(this.labelTituloAsignacion);
            this.panelAsignacion.Controls.Add(this.panelIconAsignacion);
            this.panelAsignacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAsignacion.Location = new System.Drawing.Point(757, 3);
            this.panelAsignacion.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            this.panelAsignacion.Name = "panelAsignacion";
            this.panelAsignacion.Size = new System.Drawing.Size(730, 598);
            this.panelAsignacion.TabIndex = 1;
            this.panelAsignacion.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAsignacion_Paint);
            // 
            // buttonAsignar
            // 
            this.buttonAsignar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAsignar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.buttonAsignar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonAsignar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonAsignar.ForeColor = System.Drawing.Color.White;
            this.buttonAsignar.Location = new System.Drawing.Point(522, 352);
            this.buttonAsignar.Name = "buttonAsignar";
            this.buttonAsignar.Size = new System.Drawing.Size(174, 46);
            this.buttonAsignar.TabIndex = 4;
            this.buttonAsignar.Text = "Asignar   👤";
            this.buttonAsignar.UseVisualStyleBackColor = false;
            this.buttonAsignar.Click += new System.EventHandler(this.buttonAsignar_Click);
            // 
            // buttonCancelarAsignacion
            // 
            this.buttonCancelarAsignacion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelarAsignacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.buttonCancelarAsignacion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancelarAsignacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonCancelarAsignacion.ForeColor = System.Drawing.Color.White;
            this.buttonCancelarAsignacion.Location = new System.Drawing.Point(522, 260);
            this.buttonCancelarAsignacion.Name = "buttonCancelarAsignacion";
            this.buttonCancelarAsignacion.Size = new System.Drawing.Size(174, 46);
            this.buttonCancelarAsignacion.TabIndex = 3;
            this.buttonCancelarAsignacion.Text = "Cancelar   ❌";
            this.buttonCancelarAsignacion.UseVisualStyleBackColor = false;
            this.buttonCancelarAsignacion.Click += new System.EventHandler(this.buttonCancelarAsignacion_Click);
            // 
            // panelListaAsignacion
            // 
            this.panelListaAsignacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelListaAsignacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(241)))), ((int)(((byte)(234)))));
            this.panelListaAsignacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelListaAsignacion.Location = new System.Drawing.Point(32, 240);
            this.panelListaAsignacion.Name = "panelListaAsignacion";
            this.panelListaAsignacion.Size = new System.Drawing.Size(462, 310);
            this.panelListaAsignacion.TabIndex = 2;
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.buttonAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonAgregar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.buttonAgregar.ForeColor = System.Drawing.Color.White;
            this.buttonAgregar.Location = new System.Drawing.Point(247, 180);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(210, 46);
            this.buttonAgregar.TabIndex = 2;
            this.buttonAgregar.Text = "Agregar   ➕";
            this.buttonAgregar.UseVisualStyleBackColor = false;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);
            // 
            // comboBoxPerfilesAsignacion
            // 
            this.comboBoxPerfilesAsignacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPerfilesAsignacion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxPerfilesAsignacion.FormattingEnabled = true;
            this.comboBoxPerfilesAsignacion.Location = new System.Drawing.Point(392, 130);
            this.comboBoxPerfilesAsignacion.Name = "comboBoxPerfilesAsignacion";
            this.comboBoxPerfilesAsignacion.Size = new System.Drawing.Size(262, 29);
            this.comboBoxPerfilesAsignacion.TabIndex = 1;
            // 
            // comboBoxUsuariosAsignacion
            // 
            this.comboBoxUsuariosAsignacion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxUsuariosAsignacion.FormattingEnabled = true;
            this.comboBoxUsuariosAsignacion.Location = new System.Drawing.Point(32, 130);
            this.comboBoxUsuariosAsignacion.Name = "comboBoxUsuariosAsignacion";
            this.comboBoxUsuariosAsignacion.Size = new System.Drawing.Size(340, 29);
            this.comboBoxUsuariosAsignacion.TabIndex = 0;
            // 
            // labelPerfilesAsignacion
            // 
            this.labelPerfilesAsignacion.AutoSize = true;
            this.labelPerfilesAsignacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelPerfilesAsignacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.labelPerfilesAsignacion.Location = new System.Drawing.Point(392, 104);
            this.labelPerfilesAsignacion.Name = "labelPerfilesAsignacion";
            this.labelPerfilesAsignacion.Size = new System.Drawing.Size(63, 23);
            this.labelPerfilesAsignacion.TabIndex = 7;
            this.labelPerfilesAsignacion.Text = "Perfiles";
            // 
            // labelUsuariosAsignacion
            // 
            this.labelUsuariosAsignacion.AutoSize = true;
            this.labelUsuariosAsignacion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.labelUsuariosAsignacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.labelUsuariosAsignacion.Location = new System.Drawing.Point(32, 104);
            this.labelUsuariosAsignacion.Name = "labelUsuariosAsignacion";
            this.labelUsuariosAsignacion.Size = new System.Drawing.Size(75, 23);
            this.labelUsuariosAsignacion.TabIndex = 6;
            this.labelUsuariosAsignacion.Text = "Usuarios";
            // 
            // labelTituloAsignacion
            // 
            this.labelTituloAsignacion.AutoSize = true;
            this.labelTituloAsignacion.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.labelTituloAsignacion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.labelTituloAsignacion.Location = new System.Drawing.Point(90, 36);
            this.labelTituloAsignacion.Name = "labelTituloAsignacion";
            this.labelTituloAsignacion.Size = new System.Drawing.Size(354, 30);
            this.labelTituloAsignacion.TabIndex = 5;
            this.labelTituloAsignacion.Text = "Asignacion de Perfiles a Usuarios";
            // 
            // panelIconAsignacion
            // 
            this.panelIconAsignacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(92)))));
            this.panelIconAsignacion.Controls.Add(this.labelIconAsignacion);
            this.panelIconAsignacion.Location = new System.Drawing.Point(32, 28);
            this.panelIconAsignacion.Name = "panelIconAsignacion";
            this.panelIconAsignacion.Size = new System.Drawing.Size(44, 44);
            this.panelIconAsignacion.TabIndex = 8;
            this.panelIconAsignacion.Paint += new System.Windows.Forms.PaintEventHandler(this.panelIconAsignacion_Paint);
            // 
            // labelIconAsignacion
            // 
            this.labelIconAsignacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIconAsignacion.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelIconAsignacion.ForeColor = System.Drawing.Color.White;
            this.labelIconAsignacion.Location = new System.Drawing.Point(0, 0);
            this.labelIconAsignacion.Name = "labelIconAsignacion";
            this.labelIconAsignacion.Size = new System.Drawing.Size(44, 44);
            this.labelIconAsignacion.TabIndex = 0;
            this.labelIconAsignacion.Text = "👥";
            this.labelIconAsignacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AsignacionPerfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(238)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(1538, 844);
            this.Controls.Add(this.panelContenedorListas);
            this.Controls.Add(this.panelHeader);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1199, 749);
            this.Name = "AsignacionPerfiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asignación de Perfiles";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMascota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBanner)).EndInit();
            this.panelContenedorListas.ResumeLayout(false);
            this.tableLayoutPanelContenido.ResumeLayout(false);
            this.panelConsulta.ResumeLayout(false);
            this.panelConsulta.PerformLayout();
            this.panelIconConsulta.ResumeLayout(false);
            this.panelAsignacion.ResumeLayout(false);
            this.panelAsignacion.PerformLayout();
            this.panelIconAsignacion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox pictureBoxBanner;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelSubtitulo;
        private System.Windows.Forms.PictureBox pictureBoxMascota;
        private System.Windows.Forms.Panel panelContenedorListas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContenido;
        private System.Windows.Forms.Panel panelConsulta;
        private System.Windows.Forms.Panel panelIconConsulta;
        private System.Windows.Forms.Label labelIconConsulta;
        private System.Windows.Forms.Label labelTituloConsulta;
        private System.Windows.Forms.Label labelUsuariosConsulta;
        private System.Windows.Forms.ComboBox comboBoxUsuariosConsulta;
        private System.Windows.Forms.Panel panelListaConsulta;
        private System.Windows.Forms.Button buttonCancelarConsulta;
        private System.Windows.Forms.Panel panelAsignacion;
        private System.Windows.Forms.Panel panelIconAsignacion;
        private System.Windows.Forms.Label labelIconAsignacion;
        private System.Windows.Forms.Label labelTituloAsignacion;
        private System.Windows.Forms.Label labelUsuariosAsignacion;
        private System.Windows.Forms.Label labelPerfilesAsignacion;
        private System.Windows.Forms.ComboBox comboBoxUsuariosAsignacion;
        private System.Windows.Forms.ComboBox comboBoxPerfilesAsignacion;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Panel panelListaAsignacion;
        private System.Windows.Forms.Button buttonCancelarAsignacion;
        private System.Windows.Forms.Button buttonAsignar;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
