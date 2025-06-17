using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public partial class Form1 : Form
    {
        private ListBox listBoxPixels;
        private Button btnAnimateDDA;
        private Button btnAnimateBresenham;
        private TrackBar trackBarSpeed;
        private Label labelSpeed;
        private bool isAnimating = false;

        public Form1()
        {
            InitializeComponent();
            InitializeAnimationControls();
        }

        private void InitializeAnimationControls()
        {
            // Crear ListBox para mostrar píxeles
            listBoxPixels = new ListBox();
            listBoxPixels.Location = new Point(10, 10);
            listBoxPixels.Size = new Size(200, 300);
            listBoxPixels.Font = new Font("Consolas", 8);
            this.Controls.Add(listBoxPixels);

            // Crear TrackBar para velocidad
            trackBarSpeed = new TrackBar();
            trackBarSpeed.Location = new Point(220, 10);
            trackBarSpeed.Size = new Size(200, 45);
            trackBarSpeed.Minimum = 1;    // Muy rápido (100ms)
            trackBarSpeed.Maximum = 20;   // Muy lento (2000ms)
            trackBarSpeed.Value = 5;      // Valor inicial (500ms)
            trackBarSpeed.TickFrequency = 2;
            trackBarSpeed.ValueChanged += TrackBarSpeed_ValueChanged;
            this.Controls.Add(trackBarSpeed);

            // Crear Label para mostrar velocidad actual
            labelSpeed = new Label();
            labelSpeed.Location = new Point(220, 60);
            labelSpeed.Size = new Size(200, 20);
            labelSpeed.Text = "Velocidad: 500ms por píxel";
            this.Controls.Add(labelSpeed);

            // Crear botón de animación DDA
            btnAnimateDDA = new Button();
            btnAnimateDDA.Text = "Animar Línea DDA";
            btnAnimateDDA.Location = new Point(220, 90);
            btnAnimateDDA.Size = new Size(120, 30);
            btnAnimateDDA.Click += BtnAnimateDDA_Click;
            this.Controls.Add(btnAnimateDDA);

            // Crear botón para Bresenham
            btnAnimateBresenham = new Button();
            btnAnimateBresenham.Text = "Animar Bresenham";
            btnAnimateBresenham.Location = new Point(220, 130);
            btnAnimateBresenham.Size = new Size(120, 30);
            btnAnimateBresenham.Click += BtnAnimateBresenham_Click;
            this.Controls.Add(btnAnimateBresenham);
        }

        private void TrackBarSpeed_ValueChanged(object sender, EventArgs e)
        {
            int delayMs = trackBarSpeed.Value * 100; // 100ms a 2000ms
            labelSpeed.Text = $"Velocidad: {delayMs}ms por píxel";
        }

        private async void BtnAnimateDDA_Click(object sender, EventArgs e)
        {
            if (isAnimating) return;

            isAnimating = true;
            btnAnimateDDA.Text = "Animando DDA...";
            btnAnimateDDA.Enabled = false;
            btnAnimateBresenham.Enabled = false;
            
            // Limpiar la lista
            listBoxPixels.Items.Clear();
            
            // Limpiar el área de dibujo
            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.White);
            }

            // Coordenadas de ejemplo
            int x1 = 50, y1 = 50, x2 = 200, y2 = 150;
            
            // Obtener velocidad del TrackBar
            int delayMs = trackBarSpeed.Value * 100;
            
            using (Graphics g = this.CreateGraphics())
            {
                await DDALine.AnimateDDA(g, x1, y1, x2, y2, Color.Red, this.Height, delayMs, UpdatePixelList);
            }

            btnAnimateDDA.Text = "Animar Línea DDA";
            btnAnimateDDA.Enabled = true;
            btnAnimateBresenham.Enabled = true;
            isAnimating = false;
        }

        private async void BtnAnimateBresenham_Click(object sender, EventArgs e)
        {
            if (isAnimating) return;

            isAnimating = true;
            btnAnimateBresenham.Text = "Animando Bresenham...";
            btnAnimateDDA.Enabled = false;
            btnAnimateBresenham.Enabled = false;
            
            // Limpiar la lista
            listBoxPixels.Items.Clear();
            
            // Limpiar el área de dibujo
            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.White);
            }

            // Coordenadas de ejemplo
            int x1 = 50, y1 = 50, x2 = 200, y2 = 150;
            
            // Obtener velocidad del TrackBar
            int delayMs = trackBarSpeed.Value * 100;
            
            using (Graphics g = this.CreateGraphics())
            {
                await DDALine.AnimateBresenham(g, x1, y1, x2, y2, Color.Blue, this.Height, delayMs, UpdatePixelList);
            }

            btnAnimateBresenham.Text = "Animar Bresenham";
            btnAnimateDDA.Enabled = true;
            btnAnimateBresenham.Enabled = true;
            isAnimating = false;
        }

        private void UpdatePixelList(List<DDALine.Point> points)
        {
            // Actualizar la lista en el hilo de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<DDALine.Point>>(UpdatePixelList), points);
                return;
            }

            listBoxPixels.Items.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                listBoxPixels.Items.Add($"Pixel {i + 1}: ({points[i].X:F0}, {points[i].Y:F0})");
            }
            
            // Hacer scroll al último elemento
            if (listBoxPixels.Items.Count > 0)
            {
                listBoxPixels.SelectedIndex = listBoxPixels.Items.Count - 1;
            }
        }
    }
}