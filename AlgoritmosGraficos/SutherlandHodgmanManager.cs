using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Cohen_Sutherland;

namespace AlgoritmosGraficos
{
    public class SutherlandHodgmanManager
    {
        private readonly CanvasManager canvasManager;
        private SutherlandHodgman clipper;
        private List<PointF> polygonPoints;

        // Colores para dibujar
        private readonly Pen originalPolygonPen = new Pen(Color.Blue, 2);
        private readonly Brush originalPolygonBrush = new SolidBrush(Color.FromArgb(50, Color.Blue));
        private readonly Pen clippedPolygonPen = new Pen(Color.Red, 3);
        private readonly Brush clippedPolygonBrush = new SolidBrush(Color.FromArgb(100, Color.Red));
        private readonly Pen windowPen = new Pen(Color.Green, 2);
        private readonly Brush pointBrush = new SolidBrush(Color.Black);

        public SutherlandHodgmanManager(CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
            this.polygonPoints = new List<PointF>();
            InitializeClippingWindow();
        }

        private void InitializeClippingWindow()
        {
            // Definir la ventana de recorte en el centro del canvas
            int canvasWidth = canvasManager.GetCanvasWidth();
            int canvasHeight = canvasManager.GetCanvasHeight();

            float margin = 80; // Margen desde los bordes
            float xMin = margin;
            float yMin = margin;
            float xMax = canvasWidth - margin;
            float yMax = canvasHeight - margin;

            clipper = new SutherlandHodgman(xMin, yMin, xMax, yMax);
        }

        public void AddPolygonPoint(PointF point)
        {
            polygonPoints.Add(point);
            RedrawCanvas();
        }

        public void ClearPolygon()
        {
            polygonPoints.Clear();
            RedrawCanvas();
        }

        public void DrawClippingWindowOnly()
        {
            // Limpiar el canvas y dibujar solo la ventana de recorte
            canvasManager.LimpiarCanvas();

            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                // Dibujar la ventana de recorte
                clipper.DrawClippingWindow(g, windowPen);

                // Agregar texto explicativo
                using (Font font = new Font("Arial", 10))
                using (SolidBrush brush = new SolidBrush(Color.Green))
                {
                    var bounds = clipper.GetWindowBounds();
                    g.DrawString("Ventana de Recorte", font, brush,
                               bounds.xMin, bounds.yMin - 20);
                    g.DrawString("Haga clic para crear polígono", font, brush,
                               bounds.xMin, bounds.yMax + 5);
                }
            }

            // ¡IMPORTANTE! Refrescar el canvas para mostrar los cambios
            canvasManager.RefreshCanvas();
        }

        private void RedrawCanvas()
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                // Limpiar el canvas
                g.Clear(Color.White);

                // Dibujar la ventana de recorte
                clipper.DrawClippingWindow(g, windowPen);

                // Agregar texto explicativo
                using (Font font = new Font("Arial", 9))
                using (SolidBrush textBrush = new SolidBrush(Color.Green))
                {
                    var bounds = clipper.GetWindowBounds();
                    g.DrawString("Ventana de Recorte", font, textBrush,
                               bounds.xMin, bounds.yMin - 20);
                }

                // Si tenemos al menos 3 puntos, dibujar el polígono
                if (polygonPoints.Count >= 3)
                {
                    try
                    {
                        // Dibujar polígono original
                        g.FillPolygon(originalPolygonBrush, polygonPoints.ToArray());
                        g.DrawPolygon(originalPolygonPen, polygonPoints.ToArray());

                        // Aplicar algoritmo Sutherland-Hodgman
                        List<PointF> clippedPolygon = clipper.ClipPolygon(polygonPoints);

                        // Dibujar polígono recortado si tiene al menos 3 puntos
                        if (clippedPolygon != null && clippedPolygon.Count >= 3)
                        {
                            g.FillPolygon(clippedPolygonBrush, clippedPolygon.ToArray());
                            g.DrawPolygon(clippedPolygonPen, clippedPolygon.ToArray());
                        }

                        // Mostrar información
                        using (Font font = new Font("Arial", 9))
                        using (SolidBrush textBrush = new SolidBrush(Color.Black))
                        {
                            string info = $"Puntos originales: {polygonPoints.Count}";
                            if (clippedPolygon != null && clippedPolygon.Count >= 3)
                            {
                                info += $" | Puntos recortados: {clippedPolygon.Count}";
                            }
                            else if (clippedPolygon != null && clippedPolygon.Count > 0)
                            {
                                info += $" | Puntos recortados: {clippedPolygon.Count} (insuficientes para polígono)";
                            }
                            else
                            {
                                info += " | Polígono completamente fuera de la ventana";
                            }
                            g.DrawString(info, font, textBrush, 10, 10);

                            // Leyenda de colores
                            g.DrawString("Azul: Polígono original", font, new SolidBrush(Color.Blue), 10, 30);
                            g.DrawString("Rojo: Polígono recortado", font, new SolidBrush(Color.Red), 10, 50);
                            g.DrawString("Verde: Ventana de recorte", font, new SolidBrush(Color.Green), 10, 70);
                            g.DrawString("ESC: Limpiar polígono", font, new SolidBrush(Color.Gray), 10, 90);
                        }
                    }
                    catch (Exception ex)
                    {
                        // En caso de error, mostrar mensaje
                        using (Font font = new Font("Arial", 9))
                        using (SolidBrush textBrush = new SolidBrush(Color.Red))
                        {
                            g.DrawString($"Error: {ex.Message}", font, textBrush, 10, 10);
                        }
                    }
                }
                else if (polygonPoints.Count > 0)
                {
                    // Mostrar información sobre puntos insuficientes
                    using (Font font = new Font("Arial", 9))
                    using (SolidBrush textBrush = new SolidBrush(Color.Orange))
                    {
                        g.DrawString($"Puntos: {polygonPoints.Count}/3 mínimo para polígono",
                                   font, textBrush, 10, 10);
                        g.DrawString("Continúe haciendo clic para agregar más puntos",
                                   font, textBrush, 10, 30);
                        g.DrawString("ESC: Limpiar polígono", font, new SolidBrush(Color.Gray), 10, 50);
                    }
                }
                else
                {
                    // Sin puntos
                    using (Font font = new Font("Arial", 9))
                    using (SolidBrush textBrush = new SolidBrush(Color.Gray))
                    {
                        g.DrawString("Haga clic para agregar puntos al polígono",
                                   font, textBrush, 10, 10);
                        g.DrawString("Mínimo 3 puntos requeridos",
                                   font, textBrush, 10, 30);
                    }
                }

                // Dibujar todos los puntos del polígono
                for (int i = 0; i < polygonPoints.Count; i++)
                {
                    PointF point = polygonPoints[i];

                    // Dibujar el punto
                    g.FillEllipse(pointBrush, point.X - 4, point.Y - 4, 8, 8);

                    // Dibujar un borde blanco alrededor del punto para mejor visibilidad
                    using (Pen whitePen = new Pen(Color.White, 1))
                    {
                        g.DrawEllipse(whitePen, point.X - 4, point.Y - 4, 8, 8);
                    }

                    // Dibujar número del punto
                    using (Font font = new Font("Arial", 8, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        string pointNumber = (i + 1).ToString();
                        SizeF textSize = g.MeasureString(pointNumber, font);
                        g.DrawString(pointNumber, font, textBrush,
                                   point.X - textSize.Width / 2, point.Y - textSize.Height / 2);
                    }
                }

                // Si tenemos al menos 2 puntos, dibujar líneas de conexión temporales
                if (polygonPoints.Count >= 2)
                {
                    using (Pen dashedPen = new Pen(Color.Gray, 1))
                    {
                        dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                        for (int i = 0; i < polygonPoints.Count - 1; i++)
                        {
                            g.DrawLine(dashedPen, polygonPoints[i], polygonPoints[i + 1]);
                        }

                        // Si tenemos más de 2 puntos, dibujar línea de cierre temporal
                        if (polygonPoints.Count > 2)
                        {
                            g.DrawLine(dashedPen, polygonPoints[polygonPoints.Count - 1], polygonPoints[0]);
                        }
                    }
                }
            }

            // ¡IMPORTANTE! Refrescar el canvas para mostrar los cambios
            canvasManager.RefreshCanvas();
        }

        public void UpdateClippingWindow(float xMin, float yMin, float xMax, float yMax)
        {
            clipper.UpdateWindow(xMin, yMin, xMax, yMax);
            RedrawCanvas();
        }

        public int GetPolygonPointsCount()
        {
            return polygonPoints.Count;
        }

        public void RefreshDisplay()
        {
            RedrawCanvas();
        }

        // Método para limpiar recursos
        public void Dispose()
        {
            originalPolygonPen?.Dispose();
            originalPolygonBrush?.Dispose();
            clippedPolygonPen?.Dispose();
            clippedPolygonBrush?.Dispose();
            windowPen?.Dispose();
            pointBrush?.Dispose();
        }
    }
}
