using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AlgoritmosGraficos
{
    internal class floodFill
    {
        private Bitmap imagen;

        public floodFill(Bitmap bitmap)
        {
            this.imagen = bitmap;
        }

        public void Rellenar(int x, int y, Color nuevoColor)
        {
            if (x < 0 || x >= imagen.Width || y < 0 || y >= imagen.Height)
                return;

            Color colorOriginal = imagen.GetPixel(x, y);

            if (colorOriginal.ToArgb() == nuevoColor.ToArgb())
                return;

            // Usar una estructura de datos (Stack) para simular la recursión
            Stack<Tuple<int, int>> pixeles = new Stack<Tuple<int, int>>();
            pixeles.Push(new Tuple<int, int>(x, y));

            while (pixeles.Count > 0)
            {
                var punto = pixeles.Pop();
                int pX = punto.Item1;
                int pY = punto.Item2;

                // Validar límites
                if (pX < 0 || pX >= imagen.Width || pY < 0 || pY >= imagen.Height)
                    continue;

                // Validar si el píxel es del color original
                if (imagen.GetPixel(pX, pY).ToArgb() != colorOriginal.ToArgb())
                    continue;

                // Pintar el píxel actual
                imagen.SetPixel(pX, pY, nuevoColor);

                // Agregar los cuatro vecinos a la pila, simulando el mismo orden de la versión recursiva
                pixeles.Push(new Tuple<int, int>(pX - 1, pY));    /* Oeste */
                pixeles.Push(new Tuple<int, int>(pX, pY - 1));    /* Sur */
                pixeles.Push(new Tuple<int, int>(pX + 1, pY));    /* Este */
                pixeles.Push(new Tuple<int, int>(pX, pY + 1));    /* Norte */
            }
        }

        private void RellenarRecursivo(int x, int y, Color colorOriginal, Color nuevoColor)
        {
            // Validar límites
            if (x < 0 || x >= imagen.Width || y < 0 || y >= imagen.Height)
                return;

            // Validar si el píxel es del color original
            if (imagen.GetPixel(x, y).ToArgb() != colorOriginal.ToArgb())
                return;

            // Pintar el píxel actual
            imagen.SetPixel(x, y, nuevoColor);

            // Llamadas recursivas optimizadas (una por vez para reducir profundidad)
            RellenarRecursivo(x + 1, y, colorOriginal, nuevoColor);     /* Este */
            RellenarRecursivo(x - 1, y, colorOriginal, nuevoColor);     /* Oeste */
            RellenarRecursivo(x, y + 1, colorOriginal, nuevoColor);     /* Norte */
            RellenarRecursivo(x, y - 1, colorOriginal, nuevoColor);     /* Sur */
        }

        public void Dispose()
        {
        }
    }
}