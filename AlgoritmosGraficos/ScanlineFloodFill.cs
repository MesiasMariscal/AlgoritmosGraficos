using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlgoritmosGraficos
{
    internal class ScanlineFloodFill
    {
        private Bitmap imagen;

        public ScanlineFloodFill(Bitmap bitmap)
        {
            this.imagen = bitmap;
        }

        // Algoritmo Scanline Flood Fill - Más eficiente
        public void Rellenar(int x, int y, Color nuevoColor)
        {
            if (x < 0 || x >= imagen.Width || y < 0 || y >= imagen.Height)
                return;

            Color colorOriginal = imagen.GetPixel(x, y);

            if (colorOriginal.ToArgb() == nuevoColor.ToArgb())
                return;

            // Usar una cola para manejar las líneas de scanline
            Queue<Point> lineas = new Queue<Point>();
            lineas.Enqueue(new Point(x, y));

            while (lineas.Count > 0)
            {
                Point punto = lineas.Dequeue();
                int pX = punto.X;
                int pY = punto.Y;

                // Validar límites verticales
                if (pY < 0 || pY >= imagen.Height)
                    continue;

                // Encontrar el extremo izquierdo de la línea
                int izquierda = pX;
                while (izquierda > 0 && imagen.GetPixel(izquierda - 1, pY).ToArgb() == colorOriginal.ToArgb())
                {
                    izquierda--;
                }

                // Encontrar el extremo derecho de la línea
                int derecha = pX;
                while (derecha < imagen.Width - 1 && imagen.GetPixel(derecha + 1, pY).ToArgb() == colorOriginal.ToArgb())
                {
                    derecha++;
                }

                // Rellenar toda la línea horizontal de una vez
                for (int i = izquierda; i <= derecha; i++)
                {
                    if (imagen.GetPixel(i, pY).ToArgb() == colorOriginal.ToArgb())
                    {
                        imagen.SetPixel(i, pY, nuevoColor);
                    }
                }

                // Buscar semillas en las líneas superior e inferior
                BuscarSemillasEnLinea(lineas, izquierda, derecha, pY - 1, colorOriginal); // Línea superior
                BuscarSemillasEnLinea(lineas, izquierda, derecha, pY + 1, colorOriginal); // Línea inferior
            }
        }

        private void BuscarSemillasEnLinea(Queue<Point> cola, int izquierda, int derecha, int y, Color colorOriginal)
        {
            if (y < 0 || y >= imagen.Height)
                return;

            bool dentroDeSegmento = false;

            for (int x = izquierda; x <= derecha; x++)
            {
                bool esColorOriginal = imagen.GetPixel(x, y).ToArgb() == colorOriginal.ToArgb();

                if (!dentroDeSegmento && esColorOriginal)
                {
                    // Inicio de un nuevo segmento
                    cola.Enqueue(new Point(x, y));
                    dentroDeSegmento = true;
                }
                else if (dentroDeSegmento && !esColorOriginal)
                {
                    // Fin del segmento actual
                    dentroDeSegmento = false;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}

