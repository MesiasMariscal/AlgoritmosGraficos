namespace AlgoritmosGraficos
{
    partial class Menu
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
            this.leftPanel = new System.Windows.Forms.Panel();
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.lblPoints = new System.Windows.Forms.Label();
            this.lblVertices = new System.Windows.Forms.Label();
            this.lblAnimated = new System.Windows.Forms.Label();
            this.lblPixels = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.drawingGroupBox = new System.Windows.Forms.GroupBox();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.controlsGroupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.speedValueLabel = new System.Windows.Forms.Label();
            this.speedTrackBar = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.algorithmComboBox = new System.Windows.Forms.ComboBox();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.pixelsGroupBox = new System.Windows.Forms.GroupBox();
            this.pixelListBox = new System.Windows.Forms.ListBox();
            this.instructionsGroupBox = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.leftPanel.SuspendLayout();
            this.optionsTabControl.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            this.drawingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.controlsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).BeginInit();
            this.rightPanel.SuspendLayout();
            this.pixelsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.White;
            this.leftPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.leftPanel.Controls.Add(this.optionsTabControl);
            this.leftPanel.Controls.Add(this.statusGroupBox);
            this.leftPanel.Controls.Add(this.ProgressBar);
            this.leftPanel.Controls.Add(this.drawingGroupBox);
            this.leftPanel.Controls.Add(this.controlsGroupBox);
            this.leftPanel.Location = new System.Drawing.Point(15, 15);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(900, 724);
            this.leftPanel.TabIndex = 0;
            this.leftPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.leftPanel_Paint);
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Controls.Add(this.tabPage1);
            this.optionsTabControl.Controls.Add(this.tabPage2);
            this.optionsTabControl.Controls.Add(this.tabPage3);
            this.optionsTabControl.Controls.Add(this.tabPage4);
            this.optionsTabControl.Location = new System.Drawing.Point(10, 119);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(874, 100);
            this.optionsTabControl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(866, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(866, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(866, 74);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(866, 74);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Controls.Add(this.lblPoints);
            this.statusGroupBox.Controls.Add(this.lblVertices);
            this.statusGroupBox.Controls.Add(this.lblAnimated);
            this.statusGroupBox.Controls.Add(this.lblPixels);
            this.statusGroupBox.Location = new System.Drawing.Point(10, 678);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(800, 35);
            this.statusGroupBox.TabIndex = 3;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "📊 Estado del Sistema";
            this.statusGroupBox.Enter += new System.EventHandler(this.statusGroupBox_Enter);
            // 
            // lblPoints
            // 
            this.lblPoints.AutoSize = true;
            this.lblPoints.Location = new System.Drawing.Point(15, 15);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(43, 13);
            this.lblPoints.TabIndex = 4;
            this.lblPoints.Text = "Puntos:";
            // 
            // lblVertices
            // 
            this.lblVertices.AutoSize = true;
            this.lblVertices.Location = new System.Drawing.Point(360, 15);
            this.lblVertices.Name = "lblVertices";
            this.lblVertices.Size = new System.Drawing.Size(48, 13);
            this.lblVertices.TabIndex = 3;
            this.lblVertices.Text = "Vértices:";
            this.lblVertices.Click += new System.EventHandler(this.lblVertices_Click);
            // 
            // lblAnimated
            // 
            this.lblAnimated.AutoSize = true;
            this.lblAnimated.Location = new System.Drawing.Point(245, 15);
            this.lblAnimated.Name = "lblAnimated";
            this.lblAnimated.Size = new System.Drawing.Size(56, 13);
            this.lblAnimated.TabIndex = 2;
            this.lblAnimated.Text = "Animados:";
            this.lblAnimated.Click += new System.EventHandler(this.lblAnimated_Click);
            // 
            // lblPixels
            // 
            this.lblPixels.AutoSize = true;
            this.lblPixels.Location = new System.Drawing.Point(130, 15);
            this.lblPixels.Name = "lblPixels";
            this.lblPixels.Size = new System.Drawing.Size(45, 13);
            this.lblPixels.TabIndex = 1;
            this.lblPixels.Text = "Píxeles:";
            this.lblPixels.Click += new System.EventHandler(this.lblPixels_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(10, 639);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(800, 25);
            this.ProgressBar.TabIndex = 2;
            this.ProgressBar.Click += new System.EventHandler(this.ProgressBar_Click);
            // 
            // drawingGroupBox
            // 
            this.drawingGroupBox.Controls.Add(this.picCanvas);
            this.drawingGroupBox.Location = new System.Drawing.Point(10, 225);
            this.drawingGroupBox.Name = "drawingGroupBox";
            this.drawingGroupBox.Size = new System.Drawing.Size(880, 408);
            this.drawingGroupBox.TabIndex = 1;
            this.drawingGroupBox.TabStop = false;
            this.drawingGroupBox.Text = "🎨 Área de Dibujo";
            // 
            // picCanvas
            // 
            this.picCanvas.Location = new System.Drawing.Point(6, 19);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(868, 383);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            this.picCanvas.Click += new System.EventHandler(this.picCanvas_Click);
            // 
            // controlsGroupBox
            // 
            this.controlsGroupBox.Controls.Add(this.button1);
            this.controlsGroupBox.Controls.Add(this.btnPause);
            this.controlsGroupBox.Controls.Add(this.btnStart);
            this.controlsGroupBox.Controls.Add(this.speedValueLabel);
            this.controlsGroupBox.Controls.Add(this.speedTrackBar);
            this.controlsGroupBox.Controls.Add(this.lblSpeed);
            this.controlsGroupBox.Controls.Add(this.algorithmComboBox);
            this.controlsGroupBox.Controls.Add(this.lblAlgorithm);
            this.controlsGroupBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.controlsGroupBox.Location = new System.Drawing.Point(10, 10);
            this.controlsGroupBox.Name = "controlsGroupBox";
            this.controlsGroupBox.Size = new System.Drawing.Size(880, 100);
            this.controlsGroupBox.TabIndex = 0;
            this.controlsGroupBox.TabStop = false;
            this.controlsGroupBox.Text = "🔧 Controles";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.button1.Enabled = false;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(680, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "🗑️ Limpiar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnPause.Enabled = false;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.ForeColor = System.Drawing.Color.White;
            this.btnPause.Location = new System.Drawing.Point(570, 45);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 30);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "⏸️ Pausa";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(460, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "▶️ Animar";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // speedValueLabel
            // 
            this.speedValueLabel.AutoSize = true;
            this.speedValueLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(118)))), ((int)(((byte)(210)))));
            this.speedValueLabel.Location = new System.Drawing.Point(385, 50);
            this.speedValueLabel.Name = "speedValueLabel";
            this.speedValueLabel.Size = new System.Drawing.Size(49, 15);
            this.speedValueLabel.TabIndex = 5;
            this.speedValueLabel.Text = "Nivel: 5";
            this.speedValueLabel.Click += new System.EventHandler(this.speedValueLabel_Click);
            // 
            // speedTrackBar
            // 
            this.speedTrackBar.Location = new System.Drawing.Point(230, 45);
            this.speedTrackBar.Minimum = 1;
            this.speedTrackBar.Name = "speedTrackBar";
            this.speedTrackBar.Size = new System.Drawing.Size(150, 45);
            this.speedTrackBar.TabIndex = 4;
            this.speedTrackBar.Value = 5;
            this.speedTrackBar.Scroll += new System.EventHandler(this.speedTrackBar_Scroll);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(230, 25);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(60, 15);
            this.lblSpeed.TabIndex = 3;
            this.lblSpeed.Text = "Velocidad";
            // 
            // algorithmComboBox
            // 
            this.algorithmComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.algorithmComboBox.FormattingEnabled = true;
            this.algorithmComboBox.Location = new System.Drawing.Point(15, 45);
            this.algorithmComboBox.Name = "algorithmComboBox";
            this.algorithmComboBox.Size = new System.Drawing.Size(200, 23);
            this.algorithmComboBox.TabIndex = 2;
            this.algorithmComboBox.SelectedIndexChanged += new System.EventHandler(this.algorithmComboBox_SelectedIndexChanged);
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.Location = new System.Drawing.Point(15, 25);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(80, 23);
            this.lblAlgorithm.TabIndex = 1;
            this.lblAlgorithm.Text = "Algoritmo";
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.Color.White;
            this.rightPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rightPanel.Controls.Add(this.pixelsGroupBox);
            this.rightPanel.Controls.Add(this.instructionsGroupBox);
            this.rightPanel.Location = new System.Drawing.Point(930, 15);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(300, 650);
            this.rightPanel.TabIndex = 1;
            // 
            // pixelsGroupBox
            // 
            this.pixelsGroupBox.Controls.Add(this.pixelListBox);
            this.pixelsGroupBox.Location = new System.Drawing.Point(10, 140);
            this.pixelsGroupBox.Name = "pixelsGroupBox";
            this.pixelsGroupBox.Size = new System.Drawing.Size(280, 493);
            this.pixelsGroupBox.TabIndex = 1;
            this.pixelsGroupBox.TabStop = false;
            this.pixelsGroupBox.Text = "📍 Lista de Píxeles";
            // 
            // pixelListBox
            // 
            this.pixelListBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pixelListBox.FormattingEnabled = true;
            this.pixelListBox.ItemHeight = 14;
            this.pixelListBox.Location = new System.Drawing.Point(10, 20);
            this.pixelListBox.Name = "pixelListBox";
            this.pixelListBox.Size = new System.Drawing.Size(260, 452);
            this.pixelListBox.TabIndex = 0;
            this.pixelListBox.SelectedIndexChanged += new System.EventHandler(this.pixelListBox_SelectedIndexChanged);
            // 
            // instructionsGroupBox
            // 
            this.instructionsGroupBox.Location = new System.Drawing.Point(10, 10);
            this.instructionsGroupBox.Name = "instructionsGroupBox";
            this.instructionsGroupBox.Size = new System.Drawing.Size(280, 120);
            this.instructionsGroupBox.TabIndex = 0;
            this.instructionsGroupBox.TabStop = false;
            this.instructionsGroupBox.Text = "📋 Instrucciones";
            this.instructionsGroupBox.Enter += new System.EventHandler(this.instructionsGroupBox_Enter);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1240, 751);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.leftPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🎨 Algoritmos Gráficos";
            this.Load += new System.EventHandler(this.Menu_Load);
            this.leftPanel.ResumeLayout(false);
            this.optionsTabControl.ResumeLayout(false);
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.drawingGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.controlsGroupBox.ResumeLayout(false);
            this.controlsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedTrackBar)).EndInit();
            this.rightPanel.ResumeLayout(false);
            this.pixelsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.GroupBox controlsGroupBox;
        private System.Windows.Forms.Label lblAlgorithm;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.ComboBox algorithmComboBox;
        private System.Windows.Forms.TrackBar speedTrackBar;
        private System.Windows.Forms.Label speedValueLabel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.GroupBox drawingGroupBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.Label lblVertices;
        private System.Windows.Forms.Label lblAnimated;
        private System.Windows.Forms.Label lblPixels;
        private System.Windows.Forms.GroupBox pixelsGroupBox;
        private System.Windows.Forms.GroupBox instructionsGroupBox;
        private System.Windows.Forms.ListBox pixelListBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl optionsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.PictureBox picCanvas;
    }
}