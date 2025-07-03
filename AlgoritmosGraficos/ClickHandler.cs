using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public class ClickHandler
    {
        private readonly UITabManager uiManager;
        private readonly CanvasManager canvasManager;
        private readonly AnimationManager animationManager;

        private bool primerPuntoLinea = true;
        private Point puntoInicialLinea;

        // Referencias a los managers de recorte
        private CohenSutherlandManager cohenSutherlandManager;
        private SutherlandHodgmanManager sutherlandHodgmanManager;
        private BezierManager bezierManager; // Agregar
        private BSplineManager bSplineManager; // Agregar
        public ClickHandler(UITabManager ui, CanvasManager canvas, AnimationManager animation)
        {
            uiManager = ui;
            canvasManager = canvas;
            animationManager = animation;
        }

        // Métodos para inyectar los managers
        public void SetCohenSutherlandManager(CohenSutherlandManager manager)
        {
            cohenSutherlandManager = manager;
        }

        public void SetSutherlandHodgmanManager(SutherlandHodgmanManager manager)
        {
            sutherlandHodgmanManager = manager;
        }
        public void SetBezierManager(BezierManager manager) // Agregar
        {
            bezierManager = manager;
        }

        public void SetBSplineManager(BSplineManager manager) // Agregar
        {
            bSplineManager = manager;
        }
        public void ManejarClick(Point clickPoint, int algoritmoIndex)
        {
            switch (algoritmoIndex)
            {
                case 1: // DDA
                case 2: // Bresenham Línea
                    ManejarClickLinea(clickPoint);
                    break;

                case 3: // Bresenham Círculo
                    ManejarClickCirculo();
                    break;

                case 4: // Relleno Stack
                case 5: // Relleno Scanline
                    ManejarClickRelleno(clickPoint);
                    break;

                case 6: // Cohen-Sutherland
                    ManejarClickCohenSutherland(clickPoint);
                    break;

                case 7: // Sutherland-Hodgman
                    ManejarClickSutherlandHodgman(clickPoint);
                    break;

                case 8: // Bézier
                    ManejarClickBezier(clickPoint);
                    break;

                case 9: // B-splines
                    ManejarClickBSpline(clickPoint);
                    break;


            }
        }
        private void ManejarClickBezier(Point clickPoint)
        {
            if (bezierManager != null)
            {
                bezierManager.AddControlPoint(new PointF(clickPoint.X, clickPoint.Y));
            }
        }

        private void ManejarClickBSpline(Point clickPoint)
        {
            if (bSplineManager != null)
            {
                bSplineManager.AddControlPoint(new PointF(clickPoint.X, clickPoint.Y));
            }
        }

        // Métodos para limpiar puntos de las curvas
        public void LimpiarPuntosBezier()
        {
            bezierManager?.ClearPoints();
        }

        public void LimpiarPuntosBSpline()
        {
            bSplineManager?.ClearPoints();
        }

        // Resto de métodos existentes...
        private void ManejarClickLinea(Point clickPoint)
        {
            if (primerPuntoLinea)
            {
                puntoInicialLinea = clickPoint;
                int canvasHeight = canvasManager.GetCanvasHeight();

                uiManager.SetLineCoordinates(
                    clickPoint.X,
                    canvasHeight - clickPoint.Y,
                    uiManager.GetLineCoordinates().x2,
                    uiManager.GetLineCoordinates().y2
                );

                canvasManager.DibujarPuntoTemporal(clickPoint, Color.Blue);
                primerPuntoLinea = false;
                uiManager.ActualizarInstruccionesLinea("Haga clic en el punto final de la línea");
            }
            else
            {
                int canvasHeight = canvasManager.GetCanvasHeight();
                var lineCoords = uiManager.GetLineCoordinates();

                uiManager.SetLineCoordinates(
                    lineCoords.x1,
                    lineCoords.y1,
                    clickPoint.X,
                    canvasHeight - clickPoint.Y
                );

                canvasManager.DibujarPuntoTemporal(clickPoint, Color.Red);
                primerPuntoLinea = true;
                uiManager.ActualizarInstruccionesLinea("Puntos establecidos. Use 'Animar' para ver el proceso");
            }
        }

        private void ManejarClickCohenSutherland(Point clickPoint)
        {
            if (primerPuntoLinea)
            {
                puntoInicialLinea = clickPoint;

                uiManager.SetLineCoordinates(
                    clickPoint.X,
                    clickPoint.Y,
                    uiManager.GetLineCoordinates().x2,
                    uiManager.GetLineCoordinates().y2
                );

                canvasManager.DibujarPuntoTemporal(clickPoint, Color.Blue);
                primerPuntoLinea = false;
                uiManager.ActualizarInstruccionesLinea("Haga clic en el punto final de la línea");
            }
            else
            {
                var lineCoords = uiManager.GetLineCoordinates();

                uiManager.SetLineCoordinates(
                    lineCoords.x1,
                    lineCoords.y1,
                    clickPoint.X,
                    clickPoint.Y
                );

                // Procesar inmediatamente el algoritmo Cohen-Sutherland
                if (cohenSutherlandManager != null)
                {
                    var coords = uiManager.GetLineCoordinates();
                    cohenSutherlandManager.ProcessCohenSutherlandClipping(
                        coords.x1, coords.y1, coords.x2, coords.y2);
                }

                primerPuntoLinea = true;
                uiManager.ActualizarInstruccionesLinea("Línea recortada. Haga clic para otra línea");
            }
        }

        // Nuevo método para manejar clics en Sutherland-Hodgman
        private void ManejarClickSutherlandHodgman(Point clickPoint)
        {
            if (sutherlandHodgmanManager != null)
            {
                sutherlandHodgmanManager.AddPolygonPoint(new PointF(clickPoint.X, clickPoint.Y));

                int pointCount = sutherlandHodgmanManager.GetPolygonPointsCount();
                if (pointCount < 3)
                {
                    uiManager.ActualizarInstruccionesLinea($"Punto {pointCount} agregado. Mínimo 3 puntos para polígono");
                }
                else
                {
                    uiManager.ActualizarInstruccionesLinea($"Polígono con {pointCount} puntos. Presione ESC para limpiar");
                }
            }
        }

        // Resto de métodos existentes...
        private void ManejarClickCirculo()
        {
            MessageBox.Show("El círculo se dibuja automáticamente en el centro del canvas.\n" +
                           "Use 'Animar' para ver el proceso paso a paso",
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManejarClickRelleno(Point clickPoint)
        {
            if (!canvasManager.TieneRombo())
            {
                canvasManager.DibujarRomboPorDefecto();
            }

            if (canvasManager.PuntoEnRombo(clickPoint.X, clickPoint.Y))
            {
                uiManager.SetSeedCoordinates(clickPoint.X, clickPoint.Y);

                try
                {
                    if (!animationManager.IsAnimating)
                    {
                        var pixelsToFill = canvasManager.PrepararRelleno(clickPoint.X, clickPoint.Y, uiManager.GetSelectedFillColor());

                        if (pixelsToFill != null && pixelsToFill.Count > 0)
                        {
                            animationManager.PrepareFillAnimation(pixelsToFill, uiManager.GetSelectedFillColor());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al iniciar el relleno: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Haga clic dentro del rombo para rellenarlo.",
                               "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ResetearEstadoLinea()
        {
            primerPuntoLinea = true;
        }

        // Nuevo método para limpiar polígono (puede ser llamado desde Menu.cs)
        public void LimpiarPoligonoSutherlandHodgman()
        {
            sutherlandHodgmanManager?.ClearPolygon();
        }
    }
}
