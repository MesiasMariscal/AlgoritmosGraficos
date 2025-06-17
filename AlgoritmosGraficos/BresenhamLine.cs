using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosGraficos
{
    internal class BresenhamLine
    {
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public static void DrawBresenham(Graphics g, int x1, int y1, int x2, int y2, Color color, int height)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                // Calcular deltas
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);

                // Determinar dirección de incremento
                int sx = x1 < x2 ? 1 : -1;
                int sy = y1 < y2 ? 1 : -1;

                // Calcular la pendiente
                double m = (dx == 0) ? double.PositiveInfinity : (double)dy / dx;
                double absM = Math.Abs(m);

                // Parámetro de decisión inicial
                int p;
                int x = x1;
                int y = y1;

                // Determinar número de pasos k
                int k = (dx > dy) ? dx : dy;

                // Calcular el valor inicial de p según la pendiente
                if (absM < 1)
                {
                    p = 2 * dy - dx;

                    for (int i = 0; i <= k; i++)
                    {
                        // Invertir el eje Y: height - 1 - y
                        g.FillRectangle(brush, x, height - 1 - y, 1, 1);

                        if (p < 0)
                        {
                            p += 2 * dy;
                        }
                        else
                        {
                            y += sy;
                            p += 2 * (dy - dx);
                        }

                        x += sx;

                        // Si llegamos al punto final, terminamos
                        if (x == x2 && y == y2)
                            break;
                    }
                }
                else // |m| > 1
                {
                    // Según la fórmula, para pendientes mayores que 1
                    p = 2 * dx - dy;

                    for (int i = 0; i <= k; i++)
                    {
                        // Invertir el eje Y: height - 1 - y
                        g.FillRectangle(brush, x, height - 1 - y, 1, 1);

                        if (p < 0)
                        {
                            p += 2 * dx;
                        }
                        else
                        {
                            x += sx;
                            p += 2 * (dx - dy);
                        }

                        y += sy;

                        // Si llegamos al punto final, terminamos
                        if (x == x2 && y == y2)
                            break;
                    }
                }
            }
        }

        public static List<Point> GetBresenhamPoints(int x1, int y1, int x2, int y2)
        {
            List<Point> points = new List<Point>();

            // Calcular deltas
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            // Determinar dirección de incremento
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;

            // Calcular la pendiente
            double m = (dx == 0) ? double.PositiveInfinity : (double)dy / dx;
            double absM = Math.Abs(m);

            // Parámetro de decisión inicial
            int p;
            int x = x1;
            int y = y1;

            // Determinar número de pasos k
            int k = (dx > dy) ? dx : dy;

            // Añadir punto inicial
            points.Add(new Point(x, y));

            // Calcular el valor inicial de p según la pendiente
            if (absM < 1)
            {
                p = 2 * dy - dx;

                for (int i = 0; i < k; i++)
                {
                    if (p < 0)
                    {
                        p += 2 * dy;
                    }
                    else
                    {
                        y += sy;
                        p += 2 * (dy - dx);
                    }

                    x += sx;
                    points.Add(new Point(x, y));

                    // Si llegamos al punto final, terminamos
                    if (x == x2 && y == y2)
                        break;
                }
            }
            else // |m| > 1
            {
                // Según la fórmula, para pendientes mayores que 1
                p = 2 * dx - dy;

                for (int i = 0; i < k; i++)
                {
                    if (p < 0)
                    {
                        p += 2 * dx;
                    }
                    else
                    {
                        x += sx;
                        p += 2 * (dx - dy);
                    }

                    y += sy;
                    points.Add(new Point(x, y));

                    // Si llegamos al punto final, terminamos
                    if (x == x2 && y == y2)
                        break;
                }
            }

            return points;
        }
    }
}
