using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public class BezierManager
    {
        private readonly CanvasManager canvasManager;
        private List<PointF> controlPoints;
        private int selectedPointIndex = -1;
        private bool isDragging = false;
        
        // Colores para dibujar
        private readonly Pen curvePen = new Pen(Color.Blue, 3);
        private readonly Pen controlLinePen = new Pen(Color.Gray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
        private readonly Brush controlPointBrush = new SolidBrush(Color.Red);
        private readonly Brush selectedPointBrush = new SolidBrush(Color.Orange);
        private readonly Pen controlPointPen = new Pen(Color.DarkRed, 2);

        public BezierManager(CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
            this.controlPoints = new List<PointF>();
        }

        public void AddControlPoint(PointF point)
        {
            controlPoints.Add(point);
            RedrawCanvas();
        }

        public void ClearPoints()
        {
            controlPoints.Clear();
            selectedPointIndex = -1;
            isDragging = false;
            RedrawCanvas();
        }

        public bool HandleMouseDown(PointF mousePoint)
        {
            selectedPointIndex = GetPointAt(mousePoint);
            if (selectedPointIndex >= 0)
            {
                isDragging = true;
                RedrawCanvas();
                return true;
            }
            return false;
        }

        public bool HandleMouseMove(PointF mousePoint)
        {
            if (isDragging && selectedPointIndex >= 0)
            {
                controlPoints[selectedPointIndex] = mousePoint;
                RedrawCanvas();
                return true;
            }
            return false;
        }

        public void HandleMouseUp()
        {
            isDragging = false;
        }

        private int GetPointAt(PointF mousePoint, float tolerance = 10f)
        {
            for (int i = 0; i < controlPoints.Count; i++)
            {
                float distance = (float)Math.Sqrt(
                    Math.Pow(mousePoint.X - controlPoints[i].X, 2) + 
                    Math.Pow(mousePoint.Y - controlPoints[i].Y, 2));
                
                if (distance <= tolerance)
                {
                    return i;
                }
            }
            return -1;
        }

        public void DrawCanvas()
        {
            RedrawCanvas();
        }

        private void RedrawCanvas()
        {
            using (Graphics g = Graphics.FromImage(canvasManager.GetCanvasImage()))
            {
                g.Clear(Color.White);

                if (controlPoints.Count >= 4)
                {
                    try
                    {
                        // Dibujar curva de Bézier
                        List<PointF> curvePoints = BezierCurve.CalculateMultipleBezier(controlPoints, 100);
                        if (curvePoints.Count > 1)
                        {
                            g.DrawLines(curvePen, curvePoints.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        using (Font font = new Font("Arial", 9))
                        using (SolidBrush textBrush = new SolidBrush(Color.Red))
                        {
                            g.DrawString($"Error: {ex.Message}", font, textBrush, 10, 10);
                        }
                    }
                }

                // Dibujar líneas de control
                if (controlPoints.Count > 1)
                {
                    for (int i = 0; i < controlPoints.Count - 1; i++)
                    {
                        g.DrawLine(controlLinePen, controlPoints[i], controlPoints[i + 1]);
                    }
                }

                // Dibujar puntos de control
                for (int i = 0; i < controlPoints.Count; i++)
                {
                    PointF point = controlPoints[i];
                    Brush brush = (i == selectedPointIndex) ? selectedPointBrush : controlPointBrush;
                    
                    g.FillEllipse(brush, point.X - 5, point.Y - 5, 10, 10);
                    g.DrawEllipse(controlPointPen, point.X - 5, point.Y - 5, 10, 10);
                    
                    // Dibujar número del punto
                    using (Font font = new Font("Arial", 8, FontStyle.Bold))
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        string pointNumber = i.ToString();
                        SizeF textSize = g.MeasureString(pointNumber, font);
                        g.DrawString(pointNumber, font, textBrush, 
                                   point.X - textSize.Width / 2, point.Y - textSize.Height / 2);
                    }
                }

                // Información en pantalla
                using (Font font = new Font("Arial", 9))
                using (SolidBrush textBrush = new SolidBrush(Color.Black))
                {
                    string info = $"Puntos de control: {controlPoints.Count}";
                    if (controlPoints.Count >= 4)
                    {
                        int segments = (controlPoints.Count - 1) / 3;
                        info += $" | Segmentos Bézier: {segments}";
                    }
                    else if (controlPoints.Count > 0)
                    {
                        info += $" | Necesita {4 - controlPoints.Count} puntos más para Bézier cúbica";
                    }
                    
                    g.DrawString(info, font, textBrush, 10, 10);
                    g.DrawString("Rojo: Puntos de control | Azul: Curva Bézier", font, new SolidBrush(Color.Blue), 10, 30);
                    g.DrawString("Arrastra los puntos rojos para modificar la curva", font, new SolidBrush(Color.Green), 10, 50);
                    g.DrawString("ESC: Limpiar puntos", font, new SolidBrush(Color.Gray), 10, 70);
                }
            }
            
            canvasManager.RefreshCanvas();
        }

        public int GetControlPointsCount()
        {
            return controlPoints.Count;
        }

        public void Dispose()
        {
            curvePen?.Dispose();
            controlLinePen?.Dispose();
            controlPointBrush?.Dispose();
            selectedPointBrush?.Dispose();
            controlPointPen?.Dispose();
        }
    }
}
