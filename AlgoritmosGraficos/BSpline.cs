using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlgoritmosGraficos
{
    public class BSpline
    {
        public static List<PointF> CalculateBSpline(List<PointF> controlPoints, int degree = 3, int resolution = 100)
        {
            List<PointF> curvePoints = new List<PointF>();

            if (controlPoints.Count < degree + 1)
            {
                return curvePoints; // Necesitamos al menos degree + 1 puntos
            }

            // Crear vector de nodos (knot vector) uniforme
            List<float> knots = CreateUniformKnotVector(controlPoints.Count, degree);

            // Calcular puntos de la curva - CORREGIDO
            float tStart = knots[degree];
            float tEnd = knots[controlPoints.Count];
            float step = (tEnd - tStart) / (float)resolution;

            for (int i = 0; i <= resolution; i++)
            {
                float t = tStart + i * step;

                // Asegurar que t esté dentro del rango válido - CORREGIDO
                if (t >= tEnd) t = tEnd - 0.0001f; // Evitar el valor exacto del límite
                if (t < tStart) t = tStart;

                PointF point = EvaluateBSpline(controlPoints, knots, degree, t);

                // Verificar que el punto sea válido - AÑADIDO
                if (!float.IsNaN(point.X) && !float.IsNaN(point.Y) &&
                    !float.IsInfinity(point.X) && !float.IsInfinity(point.Y))
                {
                    curvePoints.Add(point);
                }
            }

            return curvePoints;
        }

        private static List<float> CreateUniformKnotVector(int numControlPoints, int degree)
        {
            List<float> knots = new List<float>();
            int numKnots = numControlPoints + degree + 1;

            // CORREGIDO: Implementación más robusta del vector de nodos
            for (int i = 0; i < numKnots; i++)
            {
                if (i <= degree)
                {
                    knots.Add(0.0f);
                }
                else if (i < numControlPoints)
                {
                    knots.Add((float)(i - degree));
                }
                else
                {
                    knots.Add((float)(numControlPoints - degree));
                }
            }

            return knots;
        }

        private static PointF EvaluateBSpline(List<PointF> controlPoints, List<float> knots, int degree, float t)
        {
            float x = 0, y = 0;

            for (int i = 0; i < controlPoints.Count; i++)
            {
                float basis = BasisFunction(i, degree, t, knots);

                // Verificar que la función base sea válida - AÑADIDO
                if (!float.IsNaN(basis) && !float.IsInfinity(basis))
                {
                    x += basis * controlPoints[i].X;
                    y += basis * controlPoints[i].Y;
                }
            }

            return new PointF(x, y);
        }

        private static float BasisFunction(int i, int degree, float t, List<float> knots)
        {
            // Verificar límites del array - AÑADIDO
            if (i < 0 || i + degree + 1 >= knots.Count)
                return 0.0f;

            if (degree == 0)
            {
                // CORREGIDO: Manejo mejorado de los límites
                if (i == knots.Count - degree - 2) // Último intervalo
                {
                    return (knots[i] <= t && t <= knots[i + 1]) ? 1.0f : 0.0f;
                }
                else
                {
                    return (knots[i] <= t && t < knots[i + 1]) ? 1.0f : 0.0f;
                }
            }

            float left = 0, right = 0;

            // Término izquierdo - CORREGIDO
            float denomLeft = knots[i + degree] - knots[i];
            if (Math.Abs(denomLeft) > 1e-10) // Evitar división por cero
            {
                left = (t - knots[i]) / denomLeft * BasisFunction(i, degree - 1, t, knots);
            }

            // Término derecho - CORREGIDO
            float denomRight = knots[i + degree + 1] - knots[i + 1];
            if (Math.Abs(denomRight) > 1e-10) // Evitar división por cero
            {
                right = (knots[i + degree + 1] - t) / denomRight * BasisFunction(i + 1, degree - 1, t, knots);
            }

            return left + right;
        }
    }
}
