using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosGraficos
{
    internal class BresenhamCircle
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

        public static void DrawCircle(Graphics g, int xc, int yc, int r, Color color, int height)
        {
            int x = 0, y = r;
            int p = 1 - r;

            // Dibujar los primeros puntos de los 8 octantes
            PlotCirclePoints(g, xc, yc, x, y, color, height);

            while (x < y)
            {
                x++;
                if (p < 0)
                {
                    p += 2 * x + 1;
                }
                else
                {
                    y--;
                    p += 2 * (x - y) + 1;
                }
                PlotCirclePoints(g, xc, yc, x, y, color, height);
            }
        }

        private static void PlotCirclePoints(Graphics g, int xc, int yc, int x, int y, Color color, int height)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                // Dibujar puntos en los 8 octantes
                // Invertir el eje Y: height - 1 - y
                g.FillRectangle(brush, xc + x, height - 1 - (yc + y), 1, 1);
                g.FillRectangle(brush, xc - x, height - 1 - (yc + y), 1, 1);
                g.FillRectangle(brush, xc + x, height - 1 - (yc - y), 1, 1);
                g.FillRectangle(brush, xc - x, height - 1 - (yc - y), 1, 1);
                g.FillRectangle(brush, xc + y, height - 1 - (yc + x), 1, 1);
                g.FillRectangle(brush, xc - y, height - 1 - (yc + x), 1, 1);
                g.FillRectangle(brush, xc + y, height - 1 - (yc - x), 1, 1);
                g.FillRectangle(brush, xc - y, height - 1 - (yc - x), 1, 1);
            }
        }

        public static List<Point> GetCirclePoints(int xc, int yc, int r)
        {
            List<Point> points = new List<Point>();
            int x = 0, y = r;
            int p = 1 - r;

            // Añadir los primeros puntos de los 8 octantes
            AddCirclePoints(points, xc, yc, x, y);

            while (x < y)
            {
                x++;
                if (p < 0)
                {
                    p += 2 * x + 1;
                }
                else
                {
                    y--;
                    p += 2 * (x - y) + 1;
                }
                AddCirclePoints(points, xc, yc, x, y);
            }

            return points;
        }

        private static void AddCirclePoints(List<Point> points, int xc, int yc, int x, int y)
        {
            // Añadir puntos en los 8 octantes
            points.Add(new Point(xc + x, yc + y));
            points.Add(new Point(xc - x, yc + y));
            points.Add(new Point(xc + x, yc - y));
            points.Add(new Point(xc - x, yc - y));
            points.Add(new Point(xc + y, yc + x));
            points.Add(new Point(xc - y, yc + x));
            points.Add(new Point(xc + y, yc - x));
            points.Add(new Point(xc - y, yc - x));
        }
    }
}
