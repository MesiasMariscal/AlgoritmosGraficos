using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmosGraficos
{
    internal class Rombo
    {
        private Point[] vertices;
        private int centerX;
        private int centerY;
        private int size;

        public Rombo(int centerX, int centerY, int size)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.size = size;
            CalcularVertices();
        }

        private void CalcularVertices()
        {
            vertices = new Point[]
            {
                new Point(centerX, centerY - size),         // Arriba
                new Point(centerX + size, centerY),         // Derecha
                new Point(centerX, centerY + size),         // Abajo
                new Point(centerX - size, centerY)          // Izquierda
            };
        }

        public void Dibujar(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                // Dibujar las cuatro líneas del rombo
                g.DrawLine(pen, vertices[0], vertices[1]); // Arriba-Derecha
                g.DrawLine(pen, vertices[1], vertices[2]); // Derecha-Abajo
                g.DrawLine(pen, vertices[2], vertices[3]); // Abajo-Izquierda
                g.DrawLine(pen, vertices[3], vertices[0]); // Izquierda-Arriba
            }
        }

        public void DibujarEnBitmap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Dibujar(g);
            }
        }

        public Point GetCentro()
        {
            return new Point(centerX, centerY);
        }

        public Point[] GetVertices()
        {
            return (Point[])vertices.Clone();
        }

        public bool ContieneElPunto(int x, int y)
        {
            // Verificar si un punto está dentro del rombo usando el método de productos cruzados
            return PuntoEnTriangulo(x, y, vertices[0], vertices[1], vertices[2]) ||
                   PuntoEnTriangulo(x, y, vertices[0], vertices[2], vertices[3]);
        }

        private bool PuntoEnTriangulo(int px, int py, Point p1, Point p2, Point p3)
        {
            float denom = (p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y);
            if (Math.Abs(denom) < 0.001f) return false;

            float a = ((p2.Y - p3.Y) * (px - p3.X) + (p3.X - p2.X) * (py - p3.Y)) / denom;
            float b = ((p3.Y - p1.Y) * (px - p3.X) + (p1.X - p3.X) * (py - p3.Y)) / denom;
            float c = 1 - a - b;

            return a >= 0 && b >= 0 && c >= 0;
        }

        public int GetSize()
        {
            return size;
        }

        public void ActualizarPosicion(int newCenterX, int newCenterY)
        {
            this.centerX = newCenterX;
            this.centerY = newCenterY;
            CalcularVertices();
        }

        public void ActualizarTamaño(int newSize)
        {
            this.size = newSize;
            CalcularVertices();
        }
    }
}
