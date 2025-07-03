using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmosGraficos
{
    public class UITabManager
    {
        private readonly TabControl optionsTabControl;
        private readonly GroupBox instructionsGroupBox;
        private TextBox txtStartX, txtStartY, txtEndX, txtEndY;
        private TextBox txtRadius;
        private TextBox txtSeedX, txtSeedY;
        private Panel colorPreviewPanel;
        private Button btnSelectColor;
        private Color selectedFillColor;

        public event EventHandler<Color> ColorSelected;

        public UITabManager(TabControl tabControl, GroupBox instructionsBox, Color defaultFillColor)
        {
            optionsTabControl = tabControl;
            instructionsGroupBox = instructionsBox;
            selectedFillColor = defaultFillColor;
        }

        public void InicializarControlesEnPestanas()
        {
            ConfigurarTabPageLineas();
            ConfigurarTabPageCirculos();
            ConfigurarTabPageRelleno();
        }

        private void ConfigurarTabPageLineas()
        {
            TabPage tabPage = optionsTabControl.TabPages[0];
            tabPage.Text = "Líneas";
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

            // Instrucciones
            Label lblInstructions = new Label
            {
                Text = "Ingrese el radio y presione 'Animar' para ver el proceso paso a paso",
                Location = new Point(10, 55),
                Size = new Size(400, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 100),
                Name = "lblInstructions"
            };
            tabPage.Controls.Add(lblInstructions);
        }

        private void ConfigurarTabPageRelleno()
        {
            TabPage tabPage = optionsTabControl.TabPages[2];
            tabPage.Text = "Relleno";
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

        private void BtnSelectColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = selectedFillColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFillColor = colorDialog.Color;
                colorPreviewPanel.BackColor = selectedFillColor;
                ColorSelected?.Invoke(this, selectedFillColor);
            }
        }

        public void ActualizarInstrucciones(string tipo)
        {
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

        public void ActualizarInstruccionesLinea(string mensaje)
        {
            foreach (Control c in optionsTabControl.TabPages[0].Controls)
            {
                if (c is Label && c.Name == "lblInstructions")
                {
                    c.Text = mensaje;
                    break;
                }
            }
        }

        // Propiedades para acceder a los valores de entrada
        public (int x1, int y1, int x2, int y2) GetLineCoordinates()
        {
            return (int.Parse(txtStartX.Text), int.Parse(txtStartY.Text),
                    int.Parse(txtEndX.Text), int.Parse(txtEndY.Text));
        }

        public void SetLineCoordinates(int x1, int y1, int x2, int y2)
        {
            txtStartX.Text = x1.ToString();
            txtStartY.Text = y1.ToString();
            txtEndX.Text = x2.ToString();
            txtEndY.Text = y2.ToString();
        }

        public int GetRadius() => int.Parse(txtRadius.Text);

        public (int x, int y) GetSeedCoordinates()
        {
            return (int.Parse(txtSeedX.Text), int.Parse(txtSeedY.Text));
        }

        public void SetSeedCoordinates(int x, int y)
        {
            txtSeedX.Text = x.ToString();
            txtSeedY.Text = y.ToString();
        }

        public Color GetSelectedFillColor() => selectedFillColor;
    }
}
