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
        // Declaraciones de variables para el manejo dinámico de opciones
        private Color selectedFillColor = Color.Red;
        private bool isInitialized = false;
        private TextBox txtStartX, txtStartY, txtEndX, txtEndY;      // Para líneas
        private TextBox txtCenterX, txtCenterY, txtRadius;           // Para círculos
        private TextBox txtSeedX, txtSeedY;                          // Para relleno
        private Panel colorPreviewPanel;                             // Para relleno
        private Button btnSelectColor;                               // Para relleno

        // Variables para manejo de clics interactivos
        private bool primerPuntoLinea = true;
        private Point puntoInicialLinea;
        private bool primerPuntoCirculo = true;
        private Point centroCirculo;

        // Variable para el rombo
        private Rombo romboActual;

        // Variables para animación de relleno
        private Timer animationTimer;
        private Queue<Point> pixelsToFill;
        private Color targetFillColor;
        private Bitmap originalImage;
        private bool isAnimating = false;
        private int animationSpeed = 50; // milisegundos entre píxeles
        private int pixelsFilledCount = 0;

        public Menu()
        {
            InitializeComponent();
            
            // Inicializar timer para animación
            animationTimer = new Timer();
            animationTimer.Interval = 50; // 50ms por defecto
            animationTimer.Tick += AnimationTimer_Tick;
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
                "Relleno por Inundación"
            });
            algorithmComboBox.SelectedIndex = 0;

            // Configurar TabControl para ocultar las cabeceras
            optionsTabControl.Appearance = TabAppearance.FlatButtons;
            optionsTabControl.ItemSize = new Size(0, 1);
            optionsTabControl.SizeMode = TabSizeMode.Fixed;

            // Inicializar las pestañas con sus respectivos controles
            InicializarControlesEnPestanas();

            // Inicialmente ocultar el TabControl
            optionsTabControl.Visible = false;

            // Inicializar el área de dibujo
            InicializarAreaDibujo();

            // Marcar como inicializado para evitar problemas con eventos durante la carga
            isInitialized = true;
        }

        private void InicializarControlesEnPestanas()
        {
            // PESTAÑA 1: LÍNEAS (DDA y Bresenham)
            ConfigurarTabPageLineas();

            // PESTAÑA 2: CÍRCULOS (Bresenham)
            ConfigurarTabPageCirculos();

            // PESTAÑA 3: RELLENO
            ConfigurarTabPageRelleno();

            // PESTAÑA 4 (si existe): otras opciones
            if (optionsTabControl.TabPages.Count >= 4)
            {
                // Configuración para la cuarta pestaña si es necesario
            }
        }

        private void ConfigurarTabPageLineas()
        {
            TabPage tabPage = optionsTabControl.TabPages[0];
            tabPage.Text = "Líneas";

            // Limpiar controles existentes
            tabPage.Controls.Clear();

            // Punto inicial
            Label lblStartPoint = new Label
            {
                Text = "Punto Inicial:",
                Location = new Point(10, 15),
                Size = new Size(80, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblStartPoint);

            txtStartX = new TextBox
            {
                Location = new Point(100, 12),
                Size = new Size(40, 22),
                Text = "100"
            };
            tabPage.Controls.Add(txtStartX);

            txtStartY = new TextBox
            {
                Location = new Point(150, 12),
                Size = new Size(40, 22),
                Text = "100"
            };
            tabPage.Controls.Add(txtStartY);

            // Punto final
            Label lblEndPoint = new Label
            {
                Text = "Punto Final:",
                Location = new Point(220, 15),
                Size = new Size(80, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblEndPoint);

            txtEndX = new TextBox
            {
                Location = new Point(300, 12),
                Size = new Size(40, 22),
                Text = "300"
            };
            tabPage.Controls.Add(txtEndX);

            txtEndY = new TextBox
            {
                Location = new Point(350, 12),
                Size = new Size(40, 22),
                Text = "200"
            };
            tabPage.Controls.Add(txtEndY);

            // Instrucciones
            Label lblInstructions = new Label
            {
                Text = "Haga clic para el punto inicial de la línea",
                Location = new Point(10, 45),
                Size = new Size(400, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Name = "lblInstructions"
            };
            tabPage.Controls.Add(lblInstructions);
        }

        private void ConfigurarTabPageCirculos()
        {
            TabPage tabPage = optionsTabControl.TabPages[1];
            tabPage.Text = "Círculos";

            // Limpiar controles existentes
            tabPage.Controls.Clear();

            // Radio
            Label lblRadius = new Label
            {
                Text = "Radio:",
                Location = new Point(10, 25),
                Size = new Size(50, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblRadius);

            txtRadius = new TextBox
            {
                Location = new Point(70, 22),
                Size = new Size(60, 22),
                Text = "50"
            };
            tabPage.Controls.Add(txtRadius);

            // Botón para graficar el círculo
            Button btnGraficarCirculo = new Button
            {
                Text = "Graficar",
                Location = new Point(140, 20),
                Size = new Size(80, 25),
                Name = "btnGraficarCirculo"
            };
            btnGraficarCirculo.Click += BtnGraficarCirculo_Click;
            tabPage.Controls.Add(btnGraficarCirculo);

            // Instrucciones actualizadas - sin mencionar clics
            Label lblInstructions = new Label
            {
                Text = "Ingrese el radio y presione 'Graficar' para dibujar en el centro del canvas",
                Location = new Point(10, 55),
                Size = new Size(400, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Name = "lblInstructions"
            };
            tabPage.Controls.Add(lblInstructions);
        }

        private void BtnGraficarCirculo_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar el canvas antes de dibujar el nuevo círculo
                if (picCanvas.Image != null)
                {
                    using (Graphics g = Graphics.FromImage(picCanvas.Image))
                    {
                        g.Clear(Color.White);
                    }
                }

                // Calcular el centro automáticamente (mitad del canvas)
                int centerX = picCanvas.Width / 2;
                int centerY = picCanvas.Height / 2;
                int radius = int.Parse(txtRadius.Text);

                // Actualizar los valores en los TextBox (aunque no se muestren)
                if (txtCenterX != null) txtCenterX.Text = centerX.ToString();
                if (txtCenterY != null) txtCenterY.Text = centerY.ToString();

                // Dibujar centro
                DibujarPuntoTemporal(new Point(centerX, centerY), Color.Green);

                // Ejecutar algoritmo para dibujar círculo
                EjecutarAlgoritmoCirculo();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese un valor numérico válido para el radio.",
                                "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al graficar círculo: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarTabPageRelleno()
        {
            TabPage tabPage = optionsTabControl.TabPages[2];
            tabPage.Text = "Relleno";

            // Limpiar controles existentes
            tabPage.Controls.Clear();

            // Color de relleno
            Label lblFillColor = new Label
            {
                Text = "Color de relleno:",
                Location = new Point(10, 15),
                Size = new Size(100, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblFillColor);

            btnSelectColor = new Button
            {
                Text = "Seleccionar",
                Location = new Point(110, 10),
                Size = new Size(90, 25)
            };
            btnSelectColor.Click += BtnSelectColor_Click;
            tabPage.Controls.Add(btnSelectColor);

            colorPreviewPanel = new Panel
            {
                Location = new Point(210, 10),
                Size = new Size(25, 25),
                BackColor = selectedFillColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            tabPage.Controls.Add(colorPreviewPanel);

            // Punto semilla
            Label lblSeedPoint = new Label
            {
                Text = "Punto semilla (x,y):",
                Location = new Point(260, 15),
                Size = new Size(120, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblSeedPoint);

            txtSeedX = new TextBox
            {
                Location = new Point(370, 12),
                Size = new Size(40, 22),
                Text = "200"
            };
            tabPage.Controls.Add(txtSeedX);

            txtSeedY = new TextBox
            {
                Location = new Point(420, 12),
                Size = new Size(40, 22),
                Text = "200"
            };
            tabPage.Controls.Add(txtSeedY);

            // Instrucciones
            Label lblInstructions = new Label
            {
                Text = "Haga clic en el área de dibujo para seleccionar el punto semilla",
                Location = new Point(10, 45),
                Size = new Size(400, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Name = "lblInstructions"
            };
            tabPage.Controls.Add(lblInstructions);
        }

        private void InicializarAreaDibujo()
        {
            // Inicializar el PictureBox con un Bitmap vacío
            Bitmap bmp = new Bitmap(picCanvas.Width, picCanvas.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            picCanvas.Image = bmp;

            // Inicializar ListBox de píxeles
            pixelListBox.Items.Clear();
            pixelListBox.Items.Add("Los píxeles aparecerán aquí durante la animación...");

            // Inicializar etiquetas de estado
            lblPoints.Text = "Puntos: 0";
            lblPixels.Text = "Píxeles: 0";
            lblAnimated.Text = "Animados: 0";
            lblVertices.Text = "Vértices: 0";
        }

        private void BtnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = selectedFillColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFillColor = colorDialog.Color;
                colorPreviewPanel.BackColor = selectedFillColor;
            }
        }

        private void algorithmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // No procesar el evento durante la inicialización para evitar problemas
            if (!isInitialized) return;

            switch (algorithmComboBox.SelectedIndex)
            {
                case 0: // Ningún algoritmo seleccionado
                    optionsTabControl.Visible = false;
                    btnStart.Enabled = false;
                    break;

                case 1: // DDA - Línea
                case 2: // Bresenham - Línea
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 0; // Mostrar pestaña de línea
                    btnStart.Enabled = true;
                    ActualizarInstrucciones("Línea");
                    // Resetear estado de línea
                    primerPuntoLinea = true;
                    ActualizarInstruccionesLinea("Haga clic para el punto inicial de la línea");
                    break;

                case 3: // Bresenham - Círculo
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 1; // Mostrar pestaña de círculo
                    btnStart.Enabled = true;
                    ActualizarInstrucciones("Círculo");
                    // No resetear estado de círculo ya que no se usan clics
                    break;

                case 4: // Relleno por Inundación
                    optionsTabControl.Visible = true;
                    optionsTabControl.SelectedIndex = 2; // Mostrar pestaña de relleno
                    btnStart.Enabled = true;
                    ActualizarInstrucciones("Relleno");
                    // Dibujar rombo por defecto automáticamente
                    DibujarRomboPorDefecto();
                    break;
            }
        }

        private void ActualizarInstrucciones(string tipo)
        {
            // Buscar un label dentro del instructionsGroupBox o crearlo si no existe
            Label instructionLabel = null;

            foreach (Control c in instructionsGroupBox.Controls)
            {
                if (c is Label)
                {
                    instructionLabel = (Label)c;
                    break;
                }
            }

            if (instructionLabel == null)
            {
                instructionLabel = new Label
                {
                    Location = new Point(10, 20),
                    Size = new Size(260, 90),
                    AutoSize = false
                };
                instructionsGroupBox.Controls.Add(instructionLabel);
            }

            switch (tipo)
            {
                case "Línea":
                    instructionLabel.Text = "Algoritmo para dibujar líneas:\n" +
                                           "1. Introduzca los puntos inicial y final\n" +
                                           "2. O haga clic en el área de dibujo para definirlos\n" +
                                           "3. Presione 'Animar' para ver el proceso";
                    break;

                case "Círculo":
                    instructionLabel.Text = "Algoritmo de circunferencia:\n" +
                                           "1. Introduzca el radio deseado\n" +
                                           "2. Presione 'Graficar' para dibujar en el centro\n" +
                                           "3. Use 'Animar' para ver el proceso paso a paso";
                    break;

                case "Relleno":
                    instructionLabel.Text = "Algoritmo de relleno:\n" +
                                           "1. Seleccione un color\n" +
                                           "2. Haga clic dentro del rombo para definir punto semilla\n" +
                                           "3. Presione 'Animar' para ver el proceso";
                    break;

                default:
                    instructionLabel.Text = "Seleccione un algoritmo para comenzar";
                    break;
            }
        }

        private void speedTrackBar_Scroll(object sender, EventArgs e)
        {
            // Actualizar el valor del nivel de velocidad
            speedValueLabel.Text = $"Nivel: {speedTrackBar.Value}";
            
            // Si hay una animación en curso, actualizar la velocidad
            if (isAnimating)
            {
                animationSpeed = Math.Max(1, 101 - (speedTrackBar.Value * 10));
                animationTimer.Interval = animationSpeed;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            // Si ya hay una animación en curso y el botón dice "Continuar"
            if (isAnimating && btnStart.Text == "Continuar")
            {
                // Continuar animación pausada
                animationTimer.Start();
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStart.Text = "Animar";
                return;
            }

            // Si es relleno por inundación, usar animación
            if (algorithmComboBox.SelectedIndex == 4) // Relleno por Inundación
            {
                // Verificar que tenemos coordenadas válidas
                try
                {
                    int seedX = int.Parse(txtSeedX.Text);
                    int seedY = int.Parse(txtSeedY.Text);

                    // Verificar que el punto está dentro de los límites del canvas
                    if (seedX < 0 || seedX >= picCanvas.Width || seedY < 0 || seedY >= picCanvas.Height)
                    {
                        MessageBox.Show("El punto semilla está fuera del área de dibujo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Si no hay rombo dibujado, dibujarlo primero
                    if (romboActual == null)
                    {
                        DibujarRomboPorDefecto();
                    }

                    // Verificar que el punto está dentro del rombo
                    if (!romboActual.ContieneElPunto(seedX, seedY))
                    {
                        MessageBox.Show("El punto semilla debe estar dentro del rombo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Guardar imagen original para la animación
                    originalImage = new Bitmap((Bitmap)picCanvas.Image);
                    
                    // Preparar y comenzar animación
                    PrepararAnimacionRelleno(seedX, seedY, selectedFillColor);
                    
                    if (pixelsToFill != null && pixelsToFill.Count > 0)
                    {
                        IniciarAnimacionRelleno();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Por favor, ingrese valores numéricos válidos para las coordenadas.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al preparar el relleno: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (algorithmComboBox.SelectedIndex == 3) // Bresenham - Círculo
            {
                // Para círculos, mostrar mensaje para usar el botón Graficar
                MessageBox.Show("Para dibujar círculos, use el botón 'Graficar' en las opciones.\n" +
                               "El botón 'Animar' está reservado para algoritmos con animación paso a paso.", 
                               "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // Reactivar controles
                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
            else
            {
                // Para otros algoritmos (líneas), ejecutar normalmente
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                EjecutarAlgoritmoSeleccionado();
                
                // Reactivar controles después de la ejecución
                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
        }

        private void EjecutarAlgoritmoSeleccionado()
        {
            try
            {
                switch (algorithmComboBox.SelectedIndex)
                {
                    case 1: // DDA - Línea
                        int x1 = int.Parse(txtStartX.Text);
                        int y1 = int.Parse(txtStartY.Text);
                        int x2 = int.Parse(txtEndX.Text);
                        int y2 = int.Parse(txtEndY.Text);

                        EjecutarAlgoritmoLinea();
                        break;

                    case 2: // Bresenham - Línea
                        x1 = int.Parse(txtStartX.Text);
                        y1 = int.Parse(txtStartY.Text);
                        x2 = int.Parse(txtEndX.Text);
                        y2 = int.Parse(txtEndY.Text);

                        EjecutarAlgoritmoLinea();
                        break;

                    case 3: // Bresenham - Círculo
                        int centerX = int.Parse(txtCenterX.Text);
                        int centerY = int.Parse(txtCenterY.Text);
                        int radius = int.Parse(txtRadius.Text);

                        EjecutarAlgoritmoCirculo();
                        break;

                    case 4: // Relleno por Inundación
                        int seedX = int.Parse(txtSeedX.Text);
                        int seedY = int.Parse(txtSeedY.Text);

                        EjecutarAlgoritmoRelleno();
                        break;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos en todos los campos de texto.",
                                "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al ejecutar el algoritmo: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
            finally
            {
                // Reactivar el botón Start y desactivar Pause al final
                btnStart.Enabled = true;
                btnPause.Enabled = false;
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            // Pausar la animación del algoritmo
            btnPause.Enabled = false;
            btnStart.Enabled = true;

            // Aquí iría el código para pausar la animación
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Botón Limpiar
            if (picCanvas.Image != null)
            {
                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    g.Clear(Color.White);
                }
                picCanvas.Refresh();
            }

            // Resetear controles y contadores
            btnStart.Enabled = algorithmComboBox.SelectedIndex > 0;
            btnPause.Enabled = false;

            // Limpiar la lista de píxeles
            pixelListBox.Items.Clear();
            pixelListBox.Items.Add("Los píxeles aparecerán aquí durante la animación...");

            // Resetear etiquetas de estado
            lblPoints.Text = "Puntos: 0";
            lblPixels.Text = "Píxeles: 0";
            lblAnimated.Text = "Animados: 0";
            lblVertices.Text = "Vértices: 0";

            // Resetear barra de progreso
            ProgressBar.Value = 0;

            // Resetear estados de interacción
            primerPuntoLinea = true;
            primerPuntoCirculo = true;

            // Actualizar instrucciones según el algoritmo actual
            if (algorithmComboBox.SelectedIndex == 1 || algorithmComboBox.SelectedIndex == 2)
            {
                ActualizarInstruccionesLinea("Haga clic para el punto inicial de la línea");
            }
            else if (algorithmComboBox.SelectedIndex == 3)
            {
                ActualizarInstruccionesCirculo("Haga clic para el centro del círculo");
            }
        }

        // IMPLEMENTACIÓN DEL EVENTO CLICK EN EL CANVAS
        private void picCanvas_Click(object sender, EventArgs e)
        {
            if (algorithmComboBox.SelectedIndex <= 0)
                return;

            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPoint = me.Location;

            switch (algorithmComboBox.SelectedIndex)
            {
                case 1: // DDA
                case 2: // Bresenham Línea
                    ManejarClickLinea(clickPoint);
                    break;

                case 3: // Bresenham Círculo
                    ManejarClickCirculo(clickPoint);
                    break;

                case 4: // Relleno
                    ManejarClickRelleno(clickPoint);
                    break;
            }
        }

        private void ManejarClickLinea(Point clickPoint)
        {
            if (primerPuntoLinea)
            {
                // Primer clic: establecer punto inicial
                puntoInicialLinea = clickPoint;
                txtStartX.Text = clickPoint.X.ToString();
                txtStartY.Text = (picCanvas.Height - clickPoint.Y).ToString(); // Invertir Y

                // Dibujar punto inicial
                DibujarPuntoTemporal(clickPoint, Color.Blue);

                primerPuntoLinea = false;
                ActualizarInstruccionesLinea("Haga clic en el punto final de la línea");
            }
            else
            {
                // Segundo clic: establecer punto final y dibujar línea
                txtEndX.Text = clickPoint.X.ToString();
                txtEndY.Text = (picCanvas.Height - clickPoint.Y).ToString(); // Invertir Y

                // Dibujar punto final
                DibujarPuntoTemporal(clickPoint, Color.Red);

                // Ejecutar algoritmo automáticamente
                EjecutarAlgoritmoLinea();

                // Resetear para próxima línea
                primerPuntoLinea = true;
                ActualizarInstruccionesLinea("Haga clic para el punto inicial de la línea");
            }
        }

        private void ManejarClickCirculo(Point clickPoint)
        {
            // Para círculos, no manejar clics ya que existe el botón "Graficar"
            MessageBox.Show("Para dibujar círculos, use el botón 'Graficar' en las opciones.", 
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManejarClickRelleno(Point clickPoint)
        {
            // Verificar si el clic está dentro del rombo
            if (romboActual != null && romboActual.ContieneElPunto(clickPoint.X, clickPoint.Y))
            {
                // Actualizar las coordenadas del punto semilla
                txtSeedX.Text = clickPoint.X.ToString();
                txtSeedY.Text = clickPoint.Y.ToString();

                // Ejecutar el relleno
                EjecutarAlgoritmoRelleno();
            }
            else
            {
                MessageBox.Show("Haga clic dentro del rombo para rellenarlo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DibujarPuntoTemporal(Point punto, Color color)
        {
            if (picCanvas.Image == null)
                InicializarAreaDibujo();

            using (Graphics g = Graphics.FromImage(picCanvas.Image))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    // Dibujar un pequeño círculo para marcar el punto
                    g.FillEllipse(brush, punto.X - 3, punto.Y - 3, 6, 6);
                }

                // Dibujar borde negro
                using (Pen pen = new Pen(Color.Black))
                {
                    g.DrawEllipse(pen, punto.X - 3, punto.Y - 3, 6, 6);
                }
            }
            picCanvas.Refresh();
        }

        private void EjecutarAlgoritmoLinea()
        {
            try
            {
                int x1 = int.Parse(txtStartX.Text);
                int y1 = int.Parse(txtStartY.Text);
                int x2 = int.Parse(txtEndX.Text);
                int y2 = int.Parse(txtEndY.Text);

                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    if (algorithmComboBox.SelectedIndex == 1) // DDA
                    {
                        // Llamar a DrawDDA con los puntos recopilados
                        DDALine.DrawDDA(g, x1, y1, x2, y2, Color.Black, picCanvas.Height);

                        // Obtener puntos para mostrar en la lista
                        var points = DDALine.GetDDAPoints(x1, y1, x2, y2);
                        MostrarPuntosEnLista(points, "DDA");
                    }
                    else if (algorithmComboBox.SelectedIndex == 2) // Bresenham
                    {
                        // Usar la clase BresenhamLine en lugar de DDALine.DrawBresenham
                        BresenhamLine.DrawBresenham(g, x1, y1, x2, y2, Color.Black, picCanvas.Height);

                        // Obtener y mostrar los puntos de Bresenham
                        var bresenhamPoints = BresenhamLine.GetBresenhamPoints(x1, y1, x2, y2);
                        MostrarPuntosBresenhamEnLista(bresenhamPoints, "Bresenham");
                    }
                }
                picCanvas.Refresh();

                // Actualizar estadísticas
                ActualizarEstadisticas("línea");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al dibujar línea: {ex.Message}");
            }
        }

        // Método para mostrar los puntos de Bresenham en la lista
        private void MostrarPuntosBresenhamEnLista(List<BresenhamLine.Point> points, string algoritmo)
        {
            pixelListBox.Items.Clear();
            pixelListBox.Items.Add($"=== Puntos del algoritmo {algoritmo} ===");

            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pixelListBox.Items.Add($"Punto {i + 1}: ({points[i].X}, {points[i].Y})");
                }

                lblPixels.Text = $"Píxeles: {points.Count}";
            }
            else
            {
                pixelListBox.Items.Add("Puntos generados por el algoritmo");
                lblPixels.Text = "Píxeles: Calculando...";
            }
        }

        private void EjecutarAlgoritmoCirculo()
        {
            try
            {
                // Para círculos, usar el centro del canvas si no hay valores específicos
                int centerX, centerY;
                
                if (txtCenterX != null && !string.IsNullOrEmpty(txtCenterX.Text))
                {
                    centerX = int.Parse(txtCenterX.Text);
                }
                else
                {
                    centerX = picCanvas.Width / 2;
                }
                
                if (txtCenterY != null && !string.IsNullOrEmpty(txtCenterY.Text))
                {
                    centerY = int.Parse(txtCenterY.Text);
                }
                else
                {
                    centerY = picCanvas.Height / 2;
                }
                
                int radius = int.Parse(txtRadius.Text);

                using (Graphics g = Graphics.FromImage(picCanvas.Image))
                {
                    // Llamar al algoritmo de círculo de Bresenham
                    BresenhamCircle.DrawCircle(g, centerX, centerY, radius, Color.Black, picCanvas.Height);

                    // Obtener los puntos para mostrar en la lista
                    var points = BresenhamCircle.GetCirclePoints(centerX, centerY, radius);
                    MostrarPuntosCirculoEnLista(points);
                }

                picCanvas.Refresh();
                ActualizarEstadisticas("círculo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al dibujar círculo: {ex.Message}");
            }
        }


        // Método para mostrar puntos del círculo en la lista
        private void MostrarPuntosCirculoEnLista(List<BresenhamCircle.Point> points)
        {
            pixelListBox.Items.Clear();
            pixelListBox.Items.Add("=== Puntos del algoritmo Bresenham para Círculos ===");

            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pixelListBox.Items.Add($"Punto {i + 1}: ({points[i].X}, {points[i].Y})");
                }

                lblPixels.Text = $"Píxeles: {points.Count}";
            }
            else
            {
                pixelListBox.Items.Add("Puntos generados por el algoritmo");
                lblPixels.Text = "Píxeles: Calculando...";
            }
        }


        private void EjecutarAlgoritmoRelleno()
        {
            try
            {
                int seedX = int.Parse(txtSeedX.Text);
                int seedY = int.Parse(txtSeedY.Text);

                // Verificar que el punto está dentro de los límites del canvas
                if (seedX < 0 || seedX >= picCanvas.Width || seedY < 0 || seedY >= picCanvas.Height)
                {
                    MessageBox.Show("El punto semilla está fuera del área de dibujo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Si no hay rombo dibujado, dibujarlo primero
                if (romboActual == null)
                {
                    DibujarRomboPorDefecto();
                }

                // Guardar imagen original para la animación
                originalImage = new Bitmap((Bitmap)picCanvas.Image);
                
                // Preparar animación
                PrepararAnimacionRelleno(seedX, seedY, selectedFillColor);
                
                // Iniciar animación
                IniciarAnimacionRelleno();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos para las coordenadas.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al rellenar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarPuntosEnLista(List<DDALine.Point> points, string algoritmo)
        {
            pixelListBox.Items.Clear();
            pixelListBox.Items.Add($"=== Puntos del algoritmo {algoritmo} ===");

            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pixelListBox.Items.Add($"Punto {i + 1}: ({points[i].X}, {points[i].Y})");
                }

                lblPixels.Text = $"Píxeles: {points.Count}";
            }
            else
            {
                pixelListBox.Items.Add("Puntos generados por el algoritmo");
                lblPixels.Text = "Píxeles: Calculando...";
            }
        }

        private void ActualizarInstruccionesLinea(string mensaje)
        {
            // Actualizar las instrucciones en la pestaña de líneas
            foreach (Control c in optionsTabControl.TabPages[0].Controls)
            {
                if (c is Label && c.Name == "lblInstructions")
                {
                    c.Text = mensaje;
                    break;
                }
            }
        }

        private void ActualizarInstruccionesCirculo(string mensaje)
        {
            // Actualizar las instrucciones en la pestaña de círculos
            foreach (Control c in optionsTabControl.TabPages[1].Controls)
            {
                if (c is Label && c.Name == "lblInstructions")
                {
                    c.Text = mensaje;
                    break;
                }
            }
        }

        private void ActualizarEstadisticas(string tipo)
        {
            // Actualizar estadísticas según el tipo de algoritmo
            switch (tipo)
            {
                case "línea":
                    lblPoints.Text = "Puntos: 2";
                    break;
                case "círculo":
                    lblPoints.Text = "Puntos: 1 (centro)";
                    lblVertices.Text = "Radio: " + txtRadius.Text;
                    break;
                case "relleno":
                    lblPoints.Text = "Puntos: 1 (semilla)";
                    break;
            }

            ProgressBar.Value = 100; // Marcar como completado
        }
        private void DibujarRomboPorDefecto()
        {
            if (picCanvas.Image == null)
                InicializarAreaDibujo();

            // Limpiar el canvas primero
            using (Graphics g = Graphics.FromImage(picCanvas.Image))
            {
                g.Clear(Color.White);
            }

            // Calcular puntos del rombo centrado en el canvas
            int centerX = picCanvas.Width / 2;
            int centerY = picCanvas.Height / 2;
            int size = 100; // Reducir un poco el tamaño para mejor detección

            // Crear la instancia del rombo
            romboActual = new Rombo(centerX, centerY, size);

            // Dibujar el rombo en el bitmap
            romboActual.DibujarEnBitmap((Bitmap)picCanvas.Image);

            picCanvas.Refresh();

            // Actualizar el punto semilla por defecto al centro del rombo
            txtSeedX.Text = centerX.ToString();
            txtSeedY.Text = centerY.ToString();
        }

private void PrepararAnimacionRelleno(int seedX, int seedY, Color fillColor)
{
    pixelsToFill = new Queue<Point>();
    targetFillColor = fillColor;
    pixelsFilledCount = 0;
    
    // Obtener el color original del pixel semilla
    Bitmap bitmap = (Bitmap)picCanvas.Image;
    Color originalColor = bitmap.GetPixel(seedX, seedY);
    
    // Si el color ya es el mismo que queremos, no hacer nada
    if (originalColor.ToArgb() == fillColor.ToArgb())
    {
        MessageBox.Show("El área ya tiene el color seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }
    
    // Implementar flood fill para obtener todos los píxeles a rellenar
    HashSet<Point> visitados = new HashSet<Point>();
    Stack<Point> stack = new Stack<Point>();
    stack.Push(new Point(seedX, seedY));
    
    while (stack.Count > 0)
    {
        Point current = stack.Pop();
        
        if (current.X < 0 || current.X >= bitmap.Width || 
            current.Y < 0 || current.Y >= bitmap.Height)
            continue;
            
        if (visitados.Contains(current))
            continue;
            
        Color currentColor = bitmap.GetPixel(current.X, current.Y);
        if (currentColor.ToArgb() != originalColor.ToArgb())
            continue;
            
        visitados.Add(current);
        pixelsToFill.Enqueue(current);
        
        // Agregar píxeles vecinos
        stack.Push(new Point(current.X + 1, current.Y));
        stack.Push(new Point(current.X - 1, current.Y));
        stack.Push(new Point(current.X, current.Y + 1));
        stack.Push(new Point(current.X, current.Y - 1));
    }
    
    // Actualizar información
    lblPixels.Text = $"Píxeles: {pixelsToFill.Count}";
    pixelListBox.Items.Clear();
    pixelListBox.Items.Add($"=== Animación de Relleno ===");
    pixelListBox.Items.Add($"Total de píxeles a rellenar: {pixelsToFill.Count}");
}

private void IniciarAnimacionRelleno()
{
    if (pixelsToFill == null || pixelsToFill.Count == 0)
    {
        MessageBox.Show("No hay píxeles para rellenar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }
    
    // Configurar velocidad según el trackbar
    animationSpeed = Math.Max(1, 101 - (speedTrackBar.Value * 10));
    animationTimer.Interval = animationSpeed;
    
    // Deshabilitar controles durante la animación
    isAnimating = true;
    btnStart.Enabled = false;
    btnPause.Enabled = true;
    algorithmComboBox.Enabled = false;
    
    // Inicializar barra de progreso
    ProgressBar.Minimum = 0;
    ProgressBar.Maximum = pixelsToFill.Count;
    ProgressBar.Value = 0;
    
    // Iniciar timer
    animationTimer.Start();
}

private void AnimationTimer_Tick(object sender, EventArgs e)
{
    if (pixelsToFill == null || pixelsToFill.Count == 0)
    {
        DetenerAnimacion();
        return;
    }
    
    // Rellenar siguiente grupo de píxeles (rellenar varios a la vez para velocidades altas)
    int pixelsPerTick = Math.Max(1, (speedTrackBar.Value / 2));
    
    for (int i = 0; i < pixelsPerTick && pixelsToFill.Count > 0; i++)
    {
        Point pixel = pixelsToFill.Dequeue();
        
        // Colorear el pixel
        ((Bitmap)picCanvas.Image).SetPixel(pixel.X, pixel.Y, targetFillColor);
        pixelsFilledCount++;
        
        // Actualizar información cada ciertos píxeles para no sobrecargar
        if (pixelsFilledCount % 10 == 0 || pixelsToFill.Count == 0)
        {
            // Actualizar lista de píxeles
            if (pixelListBox.Items.Count > 50) // Limitar items en la lista
            {
                pixelListBox.Items.RemoveAt(2); // Remover el más antiguo (después del header)
            }
            pixelListBox.Items.Add($"Pixel {pixelsFilledCount}: ({pixel.X}, {pixel.Y})");
            
            // Actualizar estadísticas
            lblAnimated.Text = $"Animados: {pixelsFilledCount}";
            ProgressBar.Value = pixelsFilledCount;
            
            // Refrescar canvas
            picCanvas.Refresh();
            
            // Hacer scroll automático en la lista
            pixelListBox.TopIndex = Math.Max(0, pixelListBox.Items.Count - pixelListBox.Height / pixelListBox.ItemHeight);
        }
    }
    
    // Si no quedan más píxeles, detener animación
    if (pixelsToFill.Count == 0)
    {
        DetenerAnimacion();
    }
}

private void DetenerAnimacion()
{
    animationTimer.Stop();
    isAnimating = false;
    
    // Reactivar controles
    btnStart.Enabled = true;
    btnPause.Enabled = false;
    algorithmComboBox.Enabled = true;
    
    // Refrescar canvas final
    picCanvas.Refresh();
    
    // Actualizar estadísticas finales
    ActualizarEstadisticas("relleno");
    ProgressBar.Value = ProgressBar.Maximum;
    
    MessageBox.Show($"Relleno completado. Se rellenaron {pixelsFilledCount} píxeles.", 
                    "Animación Finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
}

// ...existing code...
        // Eventos vacíos que no necesitan implementación específica
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