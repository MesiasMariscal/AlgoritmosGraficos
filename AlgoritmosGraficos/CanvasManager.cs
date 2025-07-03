using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public class CanvasManager
    {
        private readonly PictureBox picCanvas;
        private readonly ListBox pixelListBox;
        private readonly Label lblPoints, lblPixels, lblAnimated, lblVertices;

        private Rombo romboActual;
        private Bitmap originalImage;

        public CanvasManager(PictureBox canvas, ListBox pixelList,
                           Label points, Label pixels, Label animated, Label vertices)
        {
            picCanvas = canvas;
            pixelListBox = pixelList;
            lblPoints = points;
            lblPixels = pixels;
            lblAnimated = animated;
            lblVertices = vertices;
        }

        public void InicializarAreaDibujo()
        {
            Bitmap bmp = new Bitmap(picCanvas.Width, picCanvas.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            picCanvas.Image = bmp;

            pixelListBox.Items.Clear();
            pixelListBox.Items.Add("Los píxeles aparecerán aquí durante la animación...");

            lblPoints.Text = "Puntos: 0";
            lblPixels.Text = "Píxeles: 0";
            lblAnimated.Text = "Animados: 0";
            lblVertices.Text = "Vértices: 0";
        }

        public void DibujarPuntoTemporal(Point punto, Color color)
        {
            if (picCanvas.Image == null)
                InicializarAreaDibujo();

            using (Graphics g = Graphics.FromImage(picCanvas.Image))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, punto.X - 3, punto.Y - 3, 6, 6);
                }

                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawEllipse(pen, punto.X - 3, punto.Y - 3, 6, 6);
                }
            }
            picCanvas.Refresh();
        }

        public void LimpiarCanvas()
        {
            if (picCanvas.Image != null)
            {
                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    g.Clear(Color.White);
                }
            }
            originalImage = new Bitmap((Bitmap)picCanvas.Image);
        }

        public void DibujarRomboPorDefecto()
        {
            if (picCanvas.Image == null)
                InicializarAreaDibujo();

            LimpiarCanvas();

            int centerX = picCanvas.Width / 2;
            int centerY = picCanvas.Height / 2;
            int size = 100;

            romboActual = new Rombo(centerX, centerY, size);
            romboActual.DibujarEnBitmap((Bitmap)picCanvas.Image);

            picCanvas.Refresh();
        }

        // Método original (Stack-based)
        public Queue<Point> PrepararRelleno(int seedX, int seedY, Color fillColor)
        {
            var pixelsToFill = new Queue<Point>();

            // Crear una copia del bitmap para no modificar el original
            Bitmap bitmapCopia = new Bitmap((Bitmap)picCanvas.Image);

            // Usar floodFill para obtener los píxeles modificados
            floodFill flood = new floodFill(bitmapCopia);
            flood.Rellenar(seedX, seedY, fillColor);

            // Comparar el bitmap original con el modificado para obtener los píxeles cambiados
            Bitmap original = (Bitmap)picCanvas.Image;
            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    if (original.GetPixel(x, y).ToArgb() != bitmapCopia.GetPixel(x, y).ToArgb())
                    {
                        pixelsToFill.Enqueue(new Point(x, y));
                    }
                }
            }

            return pixelsToFill;
        }

        // NUEVO: Método para algoritmo Scanline
        public Queue<Point> PrepararRellenoScanline(int seedX, int seedY, Color fillColor)
        {
            var pixelsToFill = new Queue<Point>();

            // Crear una copia del bitmap para no modificar el original
            Bitmap bitmapCopia = new Bitmap((Bitmap)picCanvas.Image);

            // Usar ScanlineFloodFill para obtener los píxeles modificados
            ScanlineFloodFill scanlineFill = new ScanlineFloodFill(bitmapCopia);
            scanlineFill.Rellenar(seedX, seedY, fillColor);

            // Comparar el bitmap original con el modificado para obtener los píxeles cambiados
            Bitmap original = (Bitmap)picCanvas.Image;
            for (int x = 0; x < original.Width; x++)
            {
                for (int y = 0; y < original.Height; y++)
                {
                    if (original.GetPixel(x, y).ToArgb() != bitmapCopia.GetPixel(x, y).ToArgb())
                    {
                        pixelsToFill.Enqueue(new Point(x, y));
                    }
                }
            }

            return pixelsToFill;
        }

        public void ActualizarEstadisticas(string tipo, string info = "")
        {
            switch (tipo)
            {
                case "línea":
                    lblPoints.Text = "Puntos: 2";
                    break;
                case "círculo":
                    lblPoints.Text = "Puntos: 1 (centro)";
                    lblVertices.Text = "Radio: " + info;
                    break;
                case "relleno":
                    lblPoints.Text = "Puntos: 1 (semilla)";
                    break;
            }
        }
        public Bitmap GetCanvasImage()
        {
            return (Bitmap)picCanvas.Image;
        }
        public void RefreshCanvas()
        {
            picCanvas.Refresh();
        }

        public bool TieneRombo() => romboActual != null;
        public bool PuntoEnRombo(int x, int y) => romboActual?.ContieneElPunto(x, y) ?? false;
        public int GetCanvasHeight() => picCanvas.Height;
        public int GetCanvasWidth() => picCanvas.Width;
    }
}

