using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosGraficos
{
    internal class DDALine
    {
        public class Point
        {
            public float X { get; set; }
            public float Y { get; set; }

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        public static void DrawDDA(Graphics g, int xi, int yi, int xf, int yf, Color color, int height)
        {
            // Cálculo de diferencias
            float dx = xf - xi;
            float dy = yf - yi;

            // Calcular pendiente
            float m = dx != 0 ? dy / dx : 1000f; // Pendiente muy grande si dx es 0

            // Determinar número de pasos
            float steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            if (steps == 0) steps = 1;

            // Inicializar puntos
            float x = xi;
            float y = yi;

            // Dibujar punto inicial
            using (SolidBrush brush = new SolidBrush(color))
            {
                // Invertir el eje Y: height - 1 - y
                g.FillRectangle(brush, (int)Math.Round(x), height - 1 - (int)Math.Round(y), 1, 1);

                // Calcular y dibujar los puntos intermedios
                if (Math.Abs(m) <= 1)
                {
                    // Caso: pendiente <= 1
                    int stepX = dx >= 0 ? 1 : -1;
                    while (Math.Abs(x - xf) >= 0.5f)
                    {
                        x += stepX;
                        y += m * stepX;
                        // Invertir el eje Y: height - 1 - y
                        g.FillRectangle(brush, (int)Math.Round(x), height - 1 - (int)Math.Round(y), 1, 1);
                    }
                }
                else
                {
                    // Caso: pendiente > 1
                    int stepY = dy >= 0 ? 1 : -1;
                    float mInv = dx / dy;
                    while (Math.Abs(y - yf) >= 0.5f)
                    {
                        y += stepY;
                        x += mInv * stepY;
                        // Invertir el eje Y: height - 1 - y
                        g.FillRectangle(brush, (int)Math.Round(x), height - 1 - (int)Math.Round(y), 1, 1);
                    }
                }
            }
        }

        public static void DrawBresenham(Graphics g, int x1, int y1, int x2, int y2, Color color, int height)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                // Paso 1: Calcular las constantes
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);
                int sx = x1 < x2 ? 1 : -1;
                int sy = y1 < y2 ? 1 : -1;

                // Paso 2: Evaluar la pendiente y definir caso
                bool isSteep = dy > dx;
                int p;

                if (isSteep)
                {
                    // Pendiente > 1
                    p = 2 * dx - dy;
                }
                else
                {
                    // Pendiente <= 1
                    p = 2 * dy - dx;
                }

                // Paso 3: Dibujar los puntos
                while (true)
                {
                    // Invertir el eje Y: height - 1 - y1
                    g.FillRectangle(brush, x1, height - 1 - y1, 1, 1);

                    if (x1 == x2 && y1 == y2)
                        break;

                    if (p < 0)
                    {
                        if (isSteep)
                        {
                            y1 += sy;
                        }
                        else
                        {
                            x1 += sx;
                        }
                        p += 2 * (isSteep ? dx : dy);
                    }
                    else
                    {
                        x1 += sx;
                        y1 += sy;
                        p += 2 * (isSteep ? dx : dy) - 2 * (isSteep ? dy : dx);
                    }
                }
            }
        }

        public static List<Point> GetDDAPoints(int xi, int yi, int xf, int yf)
        {
            List<Point> points = new List<Point>();

            // Cálculo de la pendiente
            float dx = xf - xi;
            float dy = yf - yi;

            if (dx == 0) dx = 0.001f; // Evitar división por cero

            float m = dy / dx;

            // Cálculo de pasos
            float steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            if (steps == 0) steps = 1;

            // Inicializar punto
            float x = xi;
            float y = yi;

            // Añadir punto inicial
            points.Add(new Point(x, y));

            // Calcular puntos intermedios
            if (Math.Abs(m) <= 1)
            {
                int stepX = dx >= 0 ? 1 : -1;
                while (Math.Abs(x - xf) >= 0.5f)
                {
                    x += stepX;
                    y += m * stepX;
                    points.Add(new Point((float)Math.Round(x), (float)Math.Round(y)));
                }
            }
            else
            {
                int stepY = dy >= 0 ? 1 : -1;
                float mInv = dx / dy;
                while (Math.Abs(y - yf) >= 0.5f)
                {
                    y += stepY;
                    x += mInv * stepY;
                    points.Add(new Point((float)Math.Round(x), (float)Math.Round(y)));
                }
            }

            return points;
        }
    }
}
