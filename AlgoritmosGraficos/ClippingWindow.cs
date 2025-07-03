using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohen_Sutherland
{
    public abstract class ClippingWindow
    {
        protected float xMin, yMin, xMax, yMax;

        public enum EdgeType { Left, Right, Bottom, Top }

        public struct WindowEdge
        {
            public PointF Start;
            public PointF End;
            public EdgeType Type;

            public WindowEdge(PointF start, PointF end, EdgeType type)
            {
                Start = start;
                End = end;
                Type = type;
            }
        }

        protected ClippingWindow(float xMin, float yMin, float xMax, float yMax)
        {
            if (xMax <= xMin || yMax <= yMin)
                throw new ArgumentException("Los valores máximos deben ser mayores que los mínimos");

            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;

            OnWindowCreated();
        }

        public (float xMin, float yMin, float xMax, float yMax) GetWindowBounds()
        {
            return (xMin, yMin, xMax, yMax);
        }

        public void UpdateWindow(float xMin, float yMin, float xMax, float yMax)
        {
            if (xMax <= xMin || yMax <= yMin)
                throw new ArgumentException("Los valores máximos deben ser mayores que los mínimos");

            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;

            OnWindowUpdated();
        }

        public void DrawClippingWindow(Graphics g, Pen pen)
        {
            if (g == null || pen == null)
                throw new ArgumentNullException("Los parámetros g y pen no pueden ser nulos");

            g.DrawRectangle(pen, xMin, yMin, xMax - xMin, yMax - yMin);
        }

        protected List<WindowEdge> CreateWindowEdges()
        {
            return new List<WindowEdge>
            {
                new WindowEdge(new PointF(xMin, yMin), new PointF(xMin, yMax), EdgeType.Left),
                new WindowEdge(new PointF(xMax, yMin), new PointF(xMax, yMax), EdgeType.Right),
                new WindowEdge(new PointF(xMin, yMin), new PointF(xMax, yMin), EdgeType.Bottom),
                new WindowEdge(new PointF(xMin, yMax), new PointF(xMax, yMax), EdgeType.Top)
            };
        }

        protected bool IsPointInside(float x, float y)
        {
            return x >= xMin && x <= xMax && y >= yMin && y <= yMax;
        }

        protected bool IsInsideEdge(PointF point, EdgeType edgeType)
        {
            switch (edgeType)
            {
                case EdgeType.Left:
                    return point.X >= xMin;
                case EdgeType.Right:
                    return point.X <= xMax;
                case EdgeType.Bottom:
                    return point.Y >= yMin;
                case EdgeType.Top:
                    return point.Y <= yMax;
                default:
                    return false;
            }
        }

        protected PointF CalculateIntersection(PointF p1, PointF p2, EdgeType edgeType)
        {
            float x1 = p1.X, y1 = p1.Y;
            float x2 = p2.X, y2 = p2.Y;
            float epsilon = 1e-10f;

            switch (edgeType)
            {
                case EdgeType.Left:
                    {
                        if (Math.Abs(x2 - x1) < epsilon) return new PointF(float.NaN, float.NaN);
                        float t = (xMin - x1) / (x2 - x1);
                        return new PointF(xMin, y1 + t * (y2 - y1));
                    }
                case EdgeType.Right:
                    {
                        if (Math.Abs(x2 - x1) < epsilon) return new PointF(float.NaN, float.NaN);
                        float t = (xMax - x1) / (x2 - x1);
                        return new PointF(xMax, y1 + t * (y2 - y1));
                    }
                case EdgeType.Bottom:
                    {
                        if (Math.Abs(y2 - y1) < epsilon) return new PointF(float.NaN, float.NaN);
                        float t = (yMin - y1) / (y2 - y1);
                        return new PointF(x1 + t * (x2 - x1), yMin);
                    }
                case EdgeType.Top:
                    {
                        if (Math.Abs(y2 - y1) < epsilon) return new PointF(float.NaN, float.NaN);
                        float t = (yMax - y1) / (y2 - y1);
                        return new PointF(x1 + t * (x2 - x1), yMax);
                    }
                default:
                    return new PointF(float.NaN, float.NaN);
            }
        }

        protected virtual void OnWindowCreated() { }

        protected virtual void OnWindowUpdated() { }
    }
}
