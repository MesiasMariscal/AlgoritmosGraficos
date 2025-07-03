using System;
using System.Drawing;
using System.Windows.Forms;
using Cohen_Sutherland;

namespace AlgoritmosGraficos
{
    public class CohenSutherlandManager
    {
        private readonly CanvasManager canvasManager;
        private Cohen_Sutherland.Cohen_Sutherland clipper;
        private RectangleF clippingWindow;

        public CohenSutherlandManager(CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
            InitializeClippingWindow();
        }

        private void InitializeClippingWindow()
        {
            // Definir la ventana de recorte en el centro del canvas
            int canvasWidth = canvasManager.GetCanvasWidth();
            int canvasHeight = canvasManager.GetCanvasHeight();

            float margin = 100; // Margen desde los bordes
            float xMin = margin;
            float yMin = margin;
            float xMax = canvasWidth - margin;
            float yMax = canvasHeight - margin;

            clippingWindow = new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
            clipper = new Cohen_Sutherland.Cohen_Sutherland(xMin, yMin, xMax, yMax);
        }

        public void ProcessCohenSutherlandClipping(float x1, float y1, float x2, float y2)
        {
            // Limpiar el canvas
            canvasManager.LimpiarCanvas();

            // Dibujar la ventana de recorte
            DrawClippingWindow();

            // Dibujar la línea original en gris
            DrawOriginalLine(x1, y1, x2, y2);

            // Aplicar el algoritmo Cohen-Sutherland
            float clippedX1 = x1, clippedY1 = y1;
            float clippedX2 = x2, clippedY2 = y2;

            bool isVisible = clipper.ClipLine(ref clippedX1, ref clippedY1, ref clippedX2, ref clippedY2);

            if (isVisible)
            {
                // Dibujar la parte visible en verde
                DrawClippedLine(clippedX1, clippedY1, clippedX2, clippedY2);

                // Dibujar las partes no visibles en rojo
                DrawInvisibleParts(x1, y1, x2, y2, clippedX1, clippedY1, clippedX2, clippedY2);
            }
            else
            {
                // Si la línea es completamente invisible, dibujarla en rojo
                DrawInvisibleLine(x1, y1, x2, y2);
            }

            // Dibujar puntos de inicio y fin
            DrawEndPoints(x1, y1, x2, y2);
        }

        private void DrawClippingWindow()
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    g.DrawRectangle(pen, clippingWindow.X, clippingWindow.Y, 
                                   clippingWindow.Width, clippingWindow.Height);
                }

                // Agregar texto explicativo
                using (Font font = new Font("Arial", 10))
                using (SolidBrush brush = new SolidBrush(Color.Blue))
                {
                    g.DrawString("Ventana de Recorte", font, brush, 
                               clippingWindow.X, clippingWindow.Y - 20);
                }
            }
        }

        private void DrawOriginalLine(float x1, float y1, float x2, float y2)
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                using (Pen pen = new Pen(Color.LightGray, 1))
                {
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void DrawClippedLine(float x1, float y1, float x2, float y2)
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                using (Pen pen = new Pen(Color.Green, 3))
                {
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void DrawInvisibleLine(float x1, float y1, float x2, float y2)
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void DrawInvisibleParts(float origX1, float origY1, float origX2, float origY2,
                                       float clippedX1, float clippedY1, float clippedX2, float clippedY2)
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    // Dibujar la parte antes del punto recortado inicial
                    if (Math.Abs(origX1 - clippedX1) > 1 || Math.Abs(origY1 - clippedY1) > 1)
                    {
                        g.DrawLine(pen, origX1, origY1, clippedX1, clippedY1);
                    }

                    // Dibujar la parte después del punto recortado final
                    if (Math.Abs(origX2 - clippedX2) > 1 || Math.Abs(origY2 - clippedY2) > 1)
                    {
                        g.DrawLine(pen, clippedX2, clippedY2, origX2, origY2);
                    }
                }
            }
        }

        private void DrawEndPoints(float x1, float y1, float x2, float y2)
        {
            canvasManager.DibujarPuntoTemporal(new Point((int)x1, (int)y1), Color.Blue);
            canvasManager.DibujarPuntoTemporal(new Point((int)x2, (int)y2), Color.Red);
        }

        public void UpdateClippingWindow(float xMin, float yMin, float xMax, float yMax)
        {
            clippingWindow = new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
            clipper.UpdateWindow(xMin, yMin, xMax, yMax);
        }
        // Agregar este método a la clase CohenSutherlandManager
        public void DrawClippingWindowOnly()
        {
            // Limpiar el canvas y dibujar solo la ventana de recorte
            canvasManager.LimpiarCanvas();
            DrawClippingWindow();

            // Refrescar el canvas para mostrar el rectángulo inmediatamente
            // (Asumiendo que CanvasManager tiene un método para refrescar)
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                // El rectángulo ya se dibujó en DrawClippingWindow()
            }
        }
    }
}
