using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohen_Sutherland
{
    internal class Cohen_Sutherland
    {
        // Definición de códigos de región (outcodes)
        private const int INSIDE = 0;  //Dentro
        private const int LEFT = 1;   //Izquierda
        private const int RIGHT = 2;  //Derecha
        private const int BOTTOM = 4; //Abajo
        private const int TOP = 8;   //Arriba

        // Límites de la ventana de recorte
        private float xMin, yMin, xMax, yMax;

        public Cohen_Sutherland(float xMin, float yMin, float xMax, float yMax)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
        }

        // Método para actualizar los límites de la ventana
        public void UpdateWindow(float xMin, float yMin, float xMax, float yMax)
        {
            this.xMin = xMin;
            this.yMin = yMin;
            this.xMax = xMax;
            this.yMax = yMax;
        }

        private int ComputeOutCode(float x, float y)
        {
            int code = INSIDE;

            if (x < xMin)
                code |= LEFT;
            else if (x > xMax)
                code |= RIGHT;
            if (y < yMin)
                code |= BOTTOM;
            else if (y > yMax)
                code |= TOP;

            return code;
        }

        public bool ClipLine(ref float x0, ref float y0, ref float x1, ref float y1)
        {
            int outcode0 = ComputeOutCode(x0, y0);
            int outcode1 = ComputeOutCode(x1, y1);
            bool accept = false;

            while (true)
            {
                //Caso trivial de aceptación
                // Si el OR de los outcodes es 0, ambos puntos tienen outcode 0 (INSIDE)

                if ((outcode0 | outcode1) == 0)
                {
                    accept = true;// La línea es completamente visible
                    break;
                }
                //Caso trivial de rechazo
                // Si el AND de los outcodes no es 0, hay al menos un bit en común
                // lo que significa que ambos puntos están fuera por el mismo lado
                else if ((outcode0 & outcode1) != 0)
                {
                    break;// La línea es completamente invisible
                }
                //Cálculo de intersecciones cuando la línea es parcialmente visible
                // En este caso, al menos uno de los puntos está fuera de la ventana

                else
                {
                    int outcodeOut = outcode0 != 0 ? outcode0 : outcode1;

                    float x = 0, y = 0;
                    // Calculamos el punto de intersección con el borde correspondiente
                    // usando las ecuaciones de la recta

                    if ((outcodeOut & TOP) != 0)
                    {
                        // Intersección con el borde superior (y = yMax)

                        x = x0 + (x1 - x0) * (yMax - y0) / (y1 - y0);
                        y = yMax;
                    }
                    else if ((outcodeOut & BOTTOM) != 0)
                    {
                        // Intersección con el borde inferior (y = yMin)

                        x = x0 + (x1 - x0) * (yMin - y0) / (y1 - y0);
                        y = yMin;
                    }
                    else if ((outcodeOut & RIGHT) != 0)
                    {
                        // Intersección con el borde derecho (x = xMax)

                        y = y0 + (y1 - y0) * (xMax - x0) / (x1 - x0);
                        x = xMax;
                    }
                    else if ((outcodeOut & LEFT) != 0)
                    {
                        // Intersección con el borde izquierdo (x = xMin)

                        y = y0 + (y1 - y0) * (xMin - x0) / (x1 - x0);
                        x = xMin;
                    }
                    // Reemplazamos el punto que está fuera con el punto de intersección

                    if (outcodeOut == outcode0)
                    {
                        // Si el primer punto está fuera, lo reemplazamos

                        x0 = x;
                        y0 = y;
                        outcode0 = ComputeOutCode(x0, y0);
                    }
                    else
                    {
                        // Si el segundo punto está fuera, lo reemplazamos

                        x1 = x;
                        y1 = y;
                        outcode1 = ComputeOutCode(x1, y1);

                    }
                    // Continuamos el ciclo hasta encontrar un caso trivial
                    // o hasta que ambos puntos estén dentro de la ventana
                }
            }
            return accept;
        }

        // Método para dibujar la ventana de recorte
        public void DrawClippingWindow(Graphics g, Pen pen)
        {
            g.DrawRectangle(pen, xMin, yMin, xMax - xMin, yMax - yMin);
        }
    }
}
