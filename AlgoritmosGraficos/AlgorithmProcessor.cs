using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlgoritmosGraficos
{
    public static class AlgorithmProcessor
    {
        public static List<Point> ProcessDDALine(int x1, int y1, int x2, int y2)
        {
            var points = new List<Point>();
            
            // Implementar algoritmo DDA aquí o usar clase existente
            var ddaPoints = DDALine.GetDDAPoints(x1, y1, x2, y2);
            foreach (var point in ddaPoints)
            {
                points.Add(new Point((int)Math.Round(point.X), (int)Math.Round(point.Y)));
            }
            
            return points;
        }

        public static List<Point> ProcessBresenhamLine(int x1, int y1, int x2, int y2)
        {
            var points = new List<Point>();
            
            // Implementar algoritmo Bresenham línea aquí o usar clase existente
            var bresenhamPoints = BresenhamLine.GetBresenhamPoints(x1, y1, x2, y2);
            foreach (var point in bresenhamPoints)
            {
                points.Add(new Point((int)point.X, (int)point.Y));
            }
            
            return points;
        }

        public static List<Point> ProcessBresenhamCircle(int centerX, int centerY, int radius)
        {
            var points = new List<Point>();
            
            // Implementar algoritmo Bresenham círculo aquí o usar clase existente
            var circlePoints = BresenhamCircle.GetCirclePoints(centerX, centerY, radius);
            foreach (var point in circlePoints)
            {
                points.Add(new Point(point.X, point.Y));
            }
            
            return points;
        }
    }
}
