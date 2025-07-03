using System;
using System.Collections.Generic;
using System.Drawing;

namespace Cohen_Sutherland
{
    public class SutherlandHodgman : ClippingWindow
    {
        private List<WindowEdge> windowEdges;

        public SutherlandHodgman(float xMin, float yMin, float xMax, float yMax)
            : base(xMin, yMin, xMax, yMax)
        {
            // La inicialización de la ventana se hace en el constructor base
        }

        // Este método se llama automáticamente cuando se crea la ventana
        protected override void OnWindowCreated()
        {
            // Crear los bordes de la ventana
            windowEdges = CreateWindowEdges();
        }

        // Este método se llama automáticamente cuando se actualiza la ventana
        protected override void OnWindowUpdated()
        {
            // Actualizar los bordes de la ventana
            windowEdges = CreateWindowEdges();
        }

        // Implementación exacta del algoritmo Sutherland-Hodgman según el pseudocódigo
        public List<PointF> ClipPolygon(List<PointF> polygon)
        {
            if (polygon.Count < 3) return new List<PointF>();

            // entrada ← lista de vértices del polígono
            List<PointF> entrada = new List<PointF>(polygon);

            // para cada arco window.edge hacer
            foreach (WindowEdge edge in windowEdges)
            {
                // L ← lista vacía de puntos
                List<PointF> L = new List<PointF>();

                if (entrada.Count == 0) break;

                // Procesar cada par de vértices consecutivos
                for (int i = 0; i < entrada.Count; i++)
                {
                    PointF P_i = entrada[i];
                    PointF P_j = entrada[(i + 1) % entrada.Count]; // Vértice siguiente (circular)

                    bool P_i_inside = IsInsideEdge(P_i, edge.Type);
                    bool P_j_inside = IsInsideEdge(P_j, edge.Type);

                    // si P_i y P_j están fuera del borde
                    if (!P_i_inside && !P_j_inside)
                    {
                        // continuar al siguiente arco (no agregar nada a L)
                        continue;
                    }
                    // si P_i y P_j están dentro del borde
                    else if (P_i_inside && P_j_inside)
                    {
                        // L ← L + P_j
                        L.Add(P_j);
                    }
                    // si P_i está dentro y P_j está fuera
                    else if (P_i_inside && !P_j_inside)
                    {
                        // calcular punto de intersección p_k entre (p_i, p_j) y el borde
                        PointF p_k = CalculateIntersection(P_i, P_j, edge.Type);
                        // L ← L + p_k
                        if (!float.IsNaN(p_k.X) && !float.IsNaN(p_k.Y))
                        {
                            L.Add(p_k);
                        }
                    }
                    // si P_i está fuera y P_j está dentro
                    else if (!P_i_inside && P_j_inside)
                    {
                        // calcular punto de intersección p_k entre (p_i, p_j) y el borde
                        PointF p_k = CalculateIntersection(P_i, P_j, edge.Type);
                        // L ← L + p_k + p_j
                        if (!float.IsNaN(p_k.X) && !float.IsNaN(p_k.Y))
                        {
                            L.Add(p_k);
                        }
                        L.Add(P_j);
                    }
                }

                // entrada ← L
                entrada = L;
            }

            // devolver entrada como polígono recortado
            return entrada;
        }
    }
}
