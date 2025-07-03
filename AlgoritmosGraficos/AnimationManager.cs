using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public class AnimationManager
    {
        private readonly Timer animationTimer;
        private readonly PictureBox picCanvas;
        private readonly ListBox pixelListBox;
        private readonly ProgressBar progressBar;
        private readonly Label lblAnimated;
        private readonly TrackBar speedTrackBar;

        private Queue<Point> pixelsToAnimate;
        private Queue<Point> pixelsToFill;
        private Color targetFillColor;
        private bool isAnimating = false;
        private int pixelsAnimatedCount = 0;
        private int pixelsFilledCount = 0;
        private string currentAlgorithm = "";

        public event EventHandler AnimationCompleted;
        public event EventHandler<(bool enabled, string text)> StartButtonStateChanged;
        public event EventHandler<bool> PauseButtonStateChanged;
        public event EventHandler<bool> AlgorithmComboBoxStateChanged;

        public bool IsAnimating => isAnimating;

        public AnimationManager(PictureBox canvas, ListBox pixelList, ProgressBar progress, 
                               Label lblAnim, TrackBar speed)
        {
            picCanvas = canvas;
            pixelListBox = pixelList;
            progressBar = progress;
            lblAnimated = lblAnim;
            speedTrackBar = speed;

            animationTimer = new Timer();
            animationTimer.Interval = 50;
            animationTimer.Tick += AnimationTimer_Tick;
        }

        public void PrepareLineAnimation(List<Point> points, string algorithm, int canvasHeight)
        {
            pixelsToAnimate = new Queue<Point>();
            
            foreach (var point in points)
            {
                pixelsToAnimate.Enqueue(new Point(point.X, canvasHeight - point.Y));
            }
            
            currentAlgorithm = algorithm;
            IniciarAnimacionGenerica("línea");
        }

        public void PrepareCircleAnimation(List<Point> points, string algorithm)
        {
            pixelsToAnimate = new Queue<Point>();
            
            foreach (var point in points)
            {
                pixelsToAnimate.Enqueue(point);
            }
            
            currentAlgorithm = algorithm;
            IniciarAnimacionGenerica("círculo");
        }

        public void PrepareFillAnimation(Queue<Point> fillPixels, Color fillColor)
        {
            pixelsToFill = fillPixels;
            targetFillColor = fillColor;
            pixelsFilledCount = 0;

            if (pixelsToFill == null || pixelsToFill.Count == 0)
            {
                MessageBox.Show("No hay píxeles para rellenar.", "Información", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IniciarAnimacionRelleno();
        }

        private void IniciarAnimacionGenerica(string tipo)
        {
            if (pixelsToAnimate == null || pixelsToAnimate.Count == 0)
            {
                MessageBox.Show($"No hay píxeles para animar en el {tipo}.", "Información", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            pixelsAnimatedCount = 0;
            ConfigurarAnimacion(pixelsToAnimate.Count);

            pixelListBox.Items.Clear();
            pixelListBox.Items.Add($"=== Animación de {currentAlgorithm} ===");
            pixelListBox.Items.Add($"Total de píxeles a dibujar: {pixelsToAnimate.Count}");

            animationTimer.Start();
        }

        private void IniciarAnimacionRelleno()
        {
            ConfigurarAnimacion(pixelsToFill.Count);

            pixelListBox.Items.Clear();
            pixelListBox.Items.Add("=== Animación de Relleno ===");
            pixelListBox.Items.Add($"Total de píxeles a rellenar: {pixelsToFill.Count}");

            animationTimer.Start();
        }

        private void ConfigurarAnimacion(int totalPixels)
        {
            int animationSpeed = Math.Max(1, 101 - (speedTrackBar.Value * 10));
            animationTimer.Interval = animationSpeed;

            isAnimating = true;
            StartButtonStateChanged?.Invoke(this, (false, "Animar"));
            PauseButtonStateChanged?.Invoke(this, true);
            AlgorithmComboBoxStateChanged?.Invoke(this, false);

            progressBar.Minimum = 0;
            progressBar.Maximum = totalPixels;
            progressBar.Value = 0;

            lblAnimated.Text = "Animados: 0";
        }

        public void PauseAnimation()
        {
            if (isAnimating)
            {
                animationTimer.Stop();
                PauseButtonStateChanged?.Invoke(this, false);
                StartButtonStateChanged?.Invoke(this, (true, "Continuar"));
            }
        }

        public void ContinueAnimation()
        {
            if (isAnimating)
            {
                animationTimer.Start();
                StartButtonStateChanged?.Invoke(this, (false, "Animar"));
                PauseButtonStateChanged?.Invoke(this, true);
            }
        }

        public void UpdateSpeed()
        {
            if (isAnimating)
            {
                int animationSpeed = Math.Max(1, 101 - (speedTrackBar.Value * 10));
                animationTimer.Interval = animationSpeed;
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (pixelsToFill != null && pixelsToFill.Count > 0)
            {
                AnimarRelleno();
                return;
            }

            if (pixelsToAnimate == null || pixelsToAnimate.Count == 0)
            {
                DetenerAnimacionGenerica();
                return;
            }

            int pixelsPerTick = Math.Max(1, (speedTrackBar.Value / 3));
            
            for (int i = 0; i < pixelsPerTick && pixelsToAnimate.Count > 0; i++)
            {
                Point pixel = pixelsToAnimate.Dequeue();
                
                ((Bitmap)picCanvas.Image).SetPixel(pixel.X, pixel.Y, Color.Black);
                pixelsAnimatedCount++;
                
                if (pixelsAnimatedCount % 5 == 0 || pixelsToAnimate.Count == 0)
                {
                    ActualizarInterfaz(pixelsAnimatedCount, pixel);
                }
            }
            
            if (pixelsToAnimate.Count == 0)
            {
                DetenerAnimacionGenerica();
            }
        }

        private void AnimarRelleno()
        {
            if (pixelsToFill == null || pixelsToFill.Count == 0)
            {
                DetenerAnimacion();
                return;
            }

            // Cambiar de (speedTrackBar.Value / 2) a un valor fijo más alto para acelerar el relleno
            int pixelsPerTick = Math.Max(5, (speedTrackBar.Value * 2)); // Ahora procesa más píxeles por tic

            for (int i = 0; i < pixelsPerTick && pixelsToFill.Count > 0; i++)
            {
                Point pixel = pixelsToFill.Dequeue();

                ((Bitmap)picCanvas.Image).SetPixel(pixel.X, pixel.Y, targetFillColor);
                pixelsFilledCount++;

                if (pixelsFilledCount % 10 == 0 || pixelsToFill.Count == 0)
                {
                    ActualizarInterfaz(pixelsFilledCount, pixel);
                }
            }

            if (pixelsToFill.Count == 0)
            {
                DetenerAnimacion();
            }
        }

        private void ActualizarInterfaz(int count, Point pixel)
        {
            if (pixelListBox.Items.Count > 30)
            {
                pixelListBox.Items.RemoveAt(2);
            }
            pixelListBox.Items.Add($"Pixel {count}: ({pixel.X}, {pixel.Y})");
            
            lblAnimated.Text = $"Animados: {count}";
            progressBar.Value = count;
            
            picCanvas.Refresh();
            
            pixelListBox.TopIndex = Math.Max(0, pixelListBox.Items.Count - pixelListBox.Height / pixelListBox.ItemHeight);
        }

        private void DetenerAnimacionGenerica()
        {
            animationTimer.Stop();
            FinalizarAnimacion();
            
            MessageBox.Show($"Animación de {currentAlgorithm} completada. Se dibujaron {pixelsAnimatedCount} píxeles.", 
                            "Animación Finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DetenerAnimacion()
        {
            animationTimer.Stop();
            FinalizarAnimacion();
            
            MessageBox.Show($"Relleno completado. Se rellenaron {pixelsFilledCount} píxeles.", 
                            "Animación Finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FinalizarAnimacion()
        {
            isAnimating = false;
            
            StartButtonStateChanged?.Invoke(this, (true, "Animar"));
            PauseButtonStateChanged?.Invoke(this, false);
            AlgorithmComboBoxStateChanged?.Invoke(this, true);
            
            picCanvas.Refresh();
            progressBar.Value = progressBar.Maximum;
            
            pixelsToAnimate = null;
            pixelsToFill = null;
            
            AnimationCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
