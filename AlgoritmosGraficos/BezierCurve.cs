using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlgoritmosGraficos
{
    public class BezierCurve
    {
        public static List<PointF> CalculateCubicBezier(PointF p0, PointF p1, PointF p2, PointF p3, int resolution = 100)
        {
            List<PointF> points = new List<PointF>();

            for (int i = 0; i <= resolution; i++)
            {
                float t = (float)i / resolution;
                PointF point = CalculateBezierPoint(t, p0, p1, p2, p3);
                points.Add(point);
            }

            return points;
        }

        private static PointF CalculateBezierPoint(float t, PointF p0, PointF p1, PointF p2, PointF p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            float x = uuu * p0.X + 3 * uu * t * p1.X + 3 * u * tt * p2.X + ttt * p3.X;
            float y = uuu * p0.Y + 3 * uu * t * p1.Y + 3 * u * tt * p2.Y + ttt * p3.Y;

            return new PointF(x, y);
        }

        public static List<PointF> CalculateMultipleBezier(List<PointF> controlPoints, int resolution = 50)
        {
            List<PointF> allPoints = new List<PointF>();

            if (controlPoints.Count < 4 || (controlPoints.Count - 1) % 3 != 0)
            {
                return allPoints; // Necesitamos 4, 7, 10, 13... puntos
            }

            for (int i = 0; i <= controlPoints.Count - 4; i += 3)
            {
                var segmentPoints = CalculateCubicBezier(
                    controlPoints[i], 
                    controlPoints[i + 1], 
                    controlPoints[i + 2], 
                    controlPoints[i + 3], 
                    resolution);

                if (i == 0)
                {
                    allPoints.AddRange(segmentPoints);
                }
                else
                {
                    // Evitar duplicar el primer punto del siguiente segmento
                    segmentPoints.RemoveAt(0);
                    allPoints.AddRange(segmentPoints);
                }
            }

            return allPoints;
        }
    }
}
