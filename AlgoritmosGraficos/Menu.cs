using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public partial class Menu : Form
    {
        private UITabManager uiManager;
        private AnimationManager animationManager;
        private CanvasManager canvasManager;
        private ClickHandler clickHandler;
        private CohenSutherlandManager cohenSutherlandManager;
        private SutherlandHodgmanManager sutherlandHodgmanManager; // Agregar esta línea
        private BezierManager bezierManager; 
        private BSplineManager bSplineManager; 

        private Color selectedFillColor = Color.Red;
        private bool isInitialized = false;

        public Menu()
        {
            InitializeComponent();
            InicializarManagers();
        }

        private void InicializarManagers()
        {
            // Inicializar managers
            uiManager = new UITabManager(optionsTabControl, instructionsGroupBox, selectedFillColor);
            canvasManager = new CanvasManager(picCanvas, pixelListBox, lblPoints, lblPixels, lblAnimated, lblVertices);
            animationManager = new AnimationManager(picCanvas, pixelListBox, ProgressBar, lblAnimated, speedTrackBar);
            clickHandler = new ClickHandler(uiManager, canvasManager, animationManager);

            // Configurar eventos
            cohenSutherlandManager = new CohenSutherlandManager(canvasManager);
            sutherlandHodgmanManager = new SutherlandHodgmanManager(canvasManager); // Agregar esta línea
            bezierManager = new BezierManager(canvasManager); // Agregar
            bSplineManager = new BSplineManager(canvasManager); // Agregar

            // Inyectar el CohenSutherlandManager en el ClickHandler
            clickHandler.SetCohenSutherlandManager(cohenSutherlandManager);
            clickHandler.SetSutherlandHodgmanManager(sutherlandHodgmanManager); // Agregar esta línea
            clickHandler.SetBezierManager(bezierManager); // Agregar
            clickHandler.SetBSplineManager(bSplineManager); // Agregar

            // Resto del código existente...
            uiManager.ColorSelected += (s, color) => selectedFillColor = color;

            animationManager.StartButtonStateChanged += (s, state) =>
            {
                btnStart.Enabled = state.enabled;
                btnStart.Text = state.text;
            };

            animationManager.PauseButtonStateChanged += (s, enabled) => btnPause.Enabled = enabled;
            animationManager.AlgorithmComboBoxStateChanged += (s, enabled) => algorithmComboBox.Enabled = enabled;

            // Inicializar timer para animación
            animationManager.AnimationCompleted += (s, e) => { /* Lógica adicional si es necesaria */ };
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            // Inicializar el ComboBox con algoritmos
            algorithmComboBox.Items.AddRange(new string[]
            {
                "Seleccionar Algoritmo",
                "DDA - Línea Recta",
                "Bresenham - Línea Recta",
                "Bresenham - Circunferencia",
                "Relleno por Inundación (Stack)",
                "Relleno por Inundación (Scanline)",
                "Cohen–Sutherland",
                "Sutherland–Hodgman",
                "Curvas de Bézier",
                "B-splines"

            });
            algorithmComboBox.SelectedIndex = 0;

            // Configurar TabControl
            optionsTabControl.Appearance = TabAppearance.FlatButtons;
            optionsTabControl.ItemSize = new Size(0, 1);
            optionsTabControl.SizeMode = TabSizeMode.Fixed;

            // Inicializar componentes
            uiManager.InicializarControlesEnPestanas();
            optionsTabControl.Visible = false;
            canvasManager.InicializarAreaDibujo();

            isInitialized = true;
        }

        private void algorithmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialized) return;

            // Limpiar el canvas al cambiar de algoritmo
            canvasManager.LimpiarCanvas();
            canvasManager.InicializarAreaDibujo();

            switch (algorithmComboBox.SelectedIndex)
            {
                case 0:
                    optionsTabControl.Visible = false;
                    btnStart.Enabled = false;
                    // Mostrar botones de animación para algoritmos que no sean Cohen-Sutherland
                    btnStart.Visible = true;
                    btnPause.Visible = true;
                    speedTrackBar.Visible = true;
                    speedValueLabel.Visible = true;
                    break;

                case 1:
                case 2:
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 0;
                    btnStart.Enabled = true;
                    uiManager.ActualizarInstrucciones("Línea");
                    clickHandler.ResetearEstadoLinea();
                    uiManager.ActualizarInstruccionesLinea("Haga clic para el punto inicial de la línea");
                    // Mostrar botones de animación
                    btnStart.Visible = true;
                    btnPause.Visible = true;
                    speedTrackBar.Visible = true;
                    speedValueLabel.Visible = true;
                    break;

                case 3:
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 1;
                    btnStart.Enabled = true;
                    uiManager.ActualizarInstrucciones("Círculo");
                    // Mostrar botones de animación
                    btnStart.Visible = true;
                    btnPause.Visible = true;
                    speedTrackBar.Visible = true;
                    speedValueLabel.Visible = true;
                    break;

                case 4: // Relleno Stack
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 2;
                    btnStart.Enabled = true;
                    uiManager.ActualizarInstrucciones("Relleno por Inundación (Stack)");
                    canvasManager.DibujarRomboPorDefecto();
                    btnStart.Visible = true;
                    btnPause.Visible = true;
                    speedTrackBar.Visible = true;
                    speedValueLabel.Visible = true;
                    break;
                case 5: // Relleno Scanline
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 2;
                    btnStart.Enabled = true;
                    uiManager.ActualizarInstrucciones("Relleno por Inundación (Scanline)");
                    canvasManager.DibujarRomboPorDefecto();
                    btnStart.Visible = true;
                    btnPause.Visible = true;
                    speedTrackBar.Visible = true;
                    speedValueLabel.Visible = true;
                    break;


                case 6: // Cohen-Sutherland
                    optionsTabControl.Visible = false; // No necesitamos el panel de opciones
                    btnStart.Enabled = false;
                    uiManager.ActualizarInstrucciones("Recorte de líneas Cohen-Sutherland");
                    clickHandler.ResetearEstadoLinea();
                    uiManager.ActualizarInstruccionesLinea("Haga clic para el punto inicial de la línea a recortar");

                    // Ocultar botones de animación ya que no son necesarios
                    btnStart.Visible = false;
                    btnPause.Visible = false;
                    speedTrackBar.Visible = false;
                    speedValueLabel.Visible = false;

                    // Mostrar automáticamente el rectángulo de recorte
                    cohenSutherlandManager.DrawClippingWindowOnly();
                    break;
                case 7: // Sutherland-Hodgman - Agregar este caso
                    optionsTabControl.Visible = false;
                    btnStart.Enabled = false;
                    uiManager.ActualizarInstrucciones("Recorte de polígonos Sutherland-Hodgman");
                    uiManager.ActualizarInstruccionesLinea("Haga clic para agregar puntos al polígono (mínimo 3)");
                    btnStart.Visible = false;
                    btnPause.Visible = false;
                    speedTrackBar.Visible = false;
                    speedValueLabel.Visible = false;
                    sutherlandHodgmanManager.DrawClippingWindowOnly();
                    break;
                case 8: // Curvas de Bézier
                    optionsTabControl.Visible = false;
                    btnStart.Enabled = false;
                    uiManager.ActualizarInstrucciones("Curvas de Bézier");
                    btnStart.Visible = false;
                    btnPause.Visible = false;
                    speedTrackBar.Visible = false;
                    speedValueLabel.Visible = false;
                    bezierManager.DrawCanvas();
                    break;

                case 9: // B-splines
                    optionsTabControl.Visible = false;
                    btnStart.Enabled = false;
                    uiManager.ActualizarInstrucciones("B-splines");
                    btnStart.Visible = false;
                    btnPause.Visible = false;
                    speedTrackBar.Visible = false;
                    speedValueLabel.Visible = false;
                    bSplineManager.DrawCanvas();
                    break;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                switch (algorithmComboBox.SelectedIndex)
                {
                    case 7: // Sutherland-Hodgman
                        clickHandler.LimpiarPoligonoSutherlandHodgman();
                        uiManager.ActualizarInstruccionesLinea("Polígono limpiado. Haga clic para agregar puntos");
                        return true;

                    case 8: // Bézier
                        clickHandler.LimpiarPuntosBezier();
                        return true;

                    case 9: // B-splines
                        clickHandler.LimpiarPuntosBSpline();
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void speedTrackBar_Scroll(object sender, EventArgs e)
        {
            speedValueLabel.Text = $"Nivel: {speedTrackBar.Value}";
            animationManager.UpdateSpeed();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            if (animationManager.IsAnimating && btnStart.Text == "Continuar")
            {
                animationManager.ContinueAnimation();
                return;
            }

            try
            {
                switch (algorithmComboBox.SelectedIndex)
                {
                    case 1: // DDA
                        ProcesarLineaAlgoritmo("DDA");
                        break;

                    case 2: // Bresenham Línea
                        ProcesarLineaAlgoritmo("Bresenham");
                        break;

                    case 3: // Bresenham Círculo
                        ProcesarCirculoAlgoritmo();
                        break;

                    case 4: // Relleno Stack
                        ProcesarRellenoAlgoritmo(false);
                        break;

                    case 5: // Relleno Scanline
                        ProcesarRellenoAlgoritmo(true);
                        break;

                    case 6: // Cohen-Sutherland
                        ProcesarCohenSutherlandAlgoritmo();
                        break;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos.", "Error de formato",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcesarLineaAlgoritmo(string algoritmo)
        {
            var (x1, y1, x2, y2) = uiManager.GetLineCoordinates();

            canvasManager.LimpiarCanvas();
            canvasManager.DibujarPuntoTemporal(new Point(x1, canvasManager.GetCanvasHeight() - y1), Color.Blue);
            canvasManager.DibujarPuntoTemporal(new Point(x2, canvasManager.GetCanvasHeight() - y2), Color.Red);

            var points = algoritmo == "DDA"
                ? AlgorithmProcessor.ProcessDDALine(x1, y1, x2, y2)
                : AlgorithmProcessor.ProcessBresenhamLine(x1, y1, x2, y2);

            animationManager.PrepareLineAnimation(points, algoritmo, canvasManager.GetCanvasHeight());
        }

        private void ProcesarCirculoAlgoritmo()
        {
            int centerX = canvasManager.GetCanvasWidth() / 2;
            int centerY = canvasManager.GetCanvasHeight() / 2;
            int radius = uiManager.GetRadius();

            canvasManager.LimpiarCanvas();
            canvasManager.DibujarPuntoTemporal(new Point(centerX, centerY), Color.Green);

            var points = AlgorithmProcessor.ProcessBresenhamCircle(centerX, centerY, radius);
            animationManager.PrepareCircleAnimation(points, "Bresenham Círculo");
        }

        private void ProcesarRellenoAlgoritmo(bool usarScanline = false)
        {
            var (seedX, seedY) = uiManager.GetSeedCoordinates();

            if (seedX < 0 || seedX >= canvasManager.GetCanvasWidth() ||
                seedY < 0 || seedY >= canvasManager.GetCanvasHeight())
            {
                MessageBox.Show("El punto semilla está fuera del área de dibujo.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!canvasManager.TieneRombo())
            {
                canvasManager.DibujarRomboPorDefecto();
            }

            if (!canvasManager.PuntoEnRombo(seedX, seedY))
            {
                MessageBox.Show("El punto semilla debe estar dentro del rombo.", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Aquí necesitas modificar el CanvasManager para que use el algoritmo correspondiente
            var pixelsToFill = usarScanline
                ? canvasManager.PrepararRellenoScanline(seedX, seedY, selectedFillColor)
                : canvasManager.PrepararRelleno(seedX, seedY, selectedFillColor);

            if (pixelsToFill != null && pixelsToFill.Count > 0)
            {
                string algoritmoNombre = usarScanline ? "Scanline" : "Stack";
                animationManager.PrepareFillAnimation(pixelsToFill, selectedFillColor);
            }
        }



        private void btnPause_Click(object sender, EventArgs e)
        {
            animationManager.PauseAnimation();
        }

        private void picCanvas_Click(object sender, EventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            MouseEventArgs me = (MouseEventArgs)e;
            clickHandler.ManejarClick(me.Location, algorithmComboBox.SelectedIndex);
        }
        private void ProcesarCohenSutherlandAlgoritmo()
        {
            var (x1, y1, x2, y2) = uiManager.GetLineCoordinates();

            // Convertir coordenadas del sistema de la UI al sistema del canvas
            float canvasY1 = canvasManager.GetCanvasHeight() - y1;
            float canvasY2 = canvasManager.GetCanvasHeight() - y2;

            cohenSutherlandManager.ProcessCohenSutherlandClipping(x1, canvasY1, x2, canvasY2);

            // Actualizar estadísticas
            canvasManager.ActualizarEstadisticas("línea");
        }
        private void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            PointF mousePoint = new PointF(e.X, e.Y);

            switch (algorithmComboBox.SelectedIndex)
            {
                case 8: // Bézier (antes era 7)
                    if (!bezierManager.HandleMouseDown(mousePoint))
                    {
                        clickHandler.ManejarClick(e.Location, algorithmComboBox.SelectedIndex);
                    }
                    break;

                case 9: // B-splines (antes era 8)
                    if (!bSplineManager.HandleMouseDown(mousePoint))
                    {
                        clickHandler.ManejarClick(e.Location, algorithmComboBox.SelectedIndex);
                    }
                    break;



                default:
                    clickHandler.ManejarClick(e.Location, algorithmComboBox.SelectedIndex);
                    break;
            }
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            PointF mousePoint = new PointF(e.X, e.Y);

            switch (algorithmComboBox.SelectedIndex)
            {
                case 7: // Bézier
                    bezierManager.HandleMouseMove(mousePoint);
                    break;

                case 8: // B-splines
                    bSplineManager.HandleMouseMove(mousePoint);
                    break;
            }
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            switch (algorithmComboBox.SelectedIndex)
            {
                case 7: // Bézier
                    bezierManager.HandleMouseUp();
                    break;

                case 8: // B-splines
                    bSplineManager.HandleMouseUp();
                    break;
            }
        }
        // Eventos vacíos
        private void leftPanel_Paint(object sender, PaintEventArgs e) { }
        private void speedValueLabel_Click(object sender, EventArgs e) { }
        private void tabPage3_Click(object sender, EventArgs e) { }
        private void ProgressBar_Click(object sender, EventArgs e) { }
        private void statusGroupBox_Enter(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void lblPixels_Click(object sender, EventArgs e) { }
        private void lblAnimated_Click(object sender, EventArgs e) { }
        private void lblVertices_Click(object sender, EventArgs e) { }
        private void pixelListBox_SelectedIndexChanged(object sender, EventArgs e) { }
        private void instructionsGroupBox_Enter(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e) { }
        private void tabPage4_Click(object sender, EventArgs e) { }
    }
}