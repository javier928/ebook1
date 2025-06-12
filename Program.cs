/// Program.cs    /// EjemploC42.. /// Test de n preguntas con msgbox y grid implementados
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class GridForm : Form
{
    private const int NumColumns = 80;
    private const int NumRows = 25;    // grid para modo terminal

    private Rectangle[,] gridRectangles = new Rectangle[NumRows, NumColumns];
    private string?[,] textGrid = new string?[NumRows, NumColumns];
    private Font displayFont = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Pixel);
    private bool showGridLines = false;
    private bool isDarkMode = false;

    private int? cursorCol = null;
    private int? cursorRow = null;
    private bool showCursor = true;
    private System.Windows.Forms.Timer cursorTimer;

    private int superpuntos = 0; // Variable to track the total score

    public GridForm()
    {
        Text = "Test about Unity 6. Fixed 80x25 Grid with Superprint and Superinput";
        ClientSize = new Size(NumColumns * 10, NumRows * 20);
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;
        BackColor = Color.White;
        ForeColor = Color.Black;
        KeyPreview = true;

        int rectangleWidth = ClientSize.Width / NumColumns;
        int rectangleHeight = ClientSize.Height / NumRows;

        for (int row = 0; row < NumRows; row++)
        {
            for (int col = 0; col < NumColumns; col++)
            {
                gridRectangles[row, col] = new Rectangle(col * rectangleWidth, row * rectangleHeight, rectangleWidth, rectangleHeight);
                textGrid[row, col] = "";
            }
        }

        cursorTimer = new System.Windows.Forms.Timer();
        cursorTimer.Interval = 500;
        cursorTimer.Tick += (s, e) =>
        {
            showCursor = !showCursor;
            Invalidate();
        };
        cursorTimer.Start();

        Paint += GridForm_Paint;
        Resize += GridForm_Resize;
        KeyDown += GridForm_KeyDown;
        Shown += GridForm_Shown;
    }

    private void GridForm_Shown(object? sender, EventArgs e)
    {
        superprint(1, 1, "Bienvenido!");
        superprint(1, 3, "Presiona Ctrl+C para cambiar el color de la consola");
        superprint(1, 5, "Presiona Ctrl+G para mostrar/ocultar la cuadricula -grid-");

        string nombrex = superinput(1, 9, "Tu nombre: ");
        superprint(1, 9, $"Bienvenido, {nombrex}");

        var questions = new (string question, string answer)[]
        {
            ///("Madrid es la capital de Espana.", "V"), ///1
            ("Compilar un proyecto Unity para crear un archivo .exe implica. . .         ", "V"), ///1
            ("Los objetos en Unity 6 pueden ser tridimensionales.                        ", "V"),
            ("Un script en Unity 6 debe ir siempre asociado a un GameObject.             ","V"),///3
            ///("El sol no es un planeta.", "V"),
            ("Unity 6 permite generar un BUILD para Microsoft Windows 10 o Android. . .  ", "V"),
            ("La función Start() se llama una vez al inicio del juego o de la aplicación.", "V"),///5
            ("Un BUILD hecho para Android -archivo .apk y librerías- permite. . .        ", "V"),
            ///("La luna es un satélite de la Tierra.", "V"),
            ("Las escenas en Unity 6 son archivos .unity.", "V"),///7
            ("El agua es un líquido a temperatura ambiente.", "V"),
            ("El lenguaje de programación principal de Unity 6 es C#.", "V"),///9
            ("La gravedad en la Tierra es de aproximadamente 9.81 m/s².", "V"),
            ("Un prefab en Unity es un objeto que se puede reutilizar en varias escenas.", "V"),///11
            ("La tierra gira alrededor del sol.", "V"),
            ("Los colliders en Unity 6 son usados para detectar colisiones entre objetos.", "V"),///13
            ("Los humanos no pueden respirar bajo el agua sin equipo especial.", "V"),
            ("El componente Rigidbody permite a los objetos ser afectados por la física.", "V"),///15
            ("La función Update() se llama una vez por frame.", "V"),
            ("La formula quimica del agua es H2O.", "V"),///17
            ("Los materiales en Unity 6 definen cómo se ve la superficie de un objeto.", "V"),
            ("El agua hierve a 100 grados Celsius al nivel del mar.", "V"),///19
            ("El componente Animator se usa para controlar animaciones en Unity 6.", "V"),
            ("Los GameObjects en Unity 6 pueden tener más de un componente.", "V"),///21
            ("El sistema de coordenadas en Unity 6 es tridimensional.", "V"),
            ("La capital de Portugal es Lisboa.", "V"),///23
            ("Un script en Unity 6 puede ser escrito en C#.", "V"),
            ("El componente Light se usa para iluminar escenas en Unity 6.", "V"),///25
            ("Los océanos cubren aproximadamente el 70% de la superficie de la Tierra.", "V"),
            ("El componente Camera se usa para renderizar la vista del jugador en Unity 6.", "V"),///27
            ("Los árboles son plantas que producen oxígeno.", "V"),
            ("El sistema de partículas se usa para crear efectos visuales como el fuego.", "V"),///29
            ("La capital de Alemania es Berlín.", "V"),
            ("Los scripts en Unity 6 pueden interactuar con otros scripts y objetos.", "V"),///31
            ("Las islas Canrias se encuentran en el océano Atlántico.", "V"),
            ("Un sprite es un objeto gráfico 2D utlizado para presentar personajes u objetos.", "V"),///33
            ("El componente AudioSource se usa para reproducir sonidos en Unity 6.", "V"),
            ("La capital de Italia es Roma.", "V"),///35
            ("El sistema de partículas se usa para crear efectos visuales como el fuego.", "V"),
            ("Los pingüinos no pueden volar.", "V"),///37
            ("Los GameObjects en Unity 6 pueden tener múltiples componentes.", "V"),
            ("El cielo es azul debido a la dispersión de la luz solar por la atmósfera.", "V"),///39
            ("El componente Transform se usa para mover, rotar y escalar objetos en Unity.", "V"),
            ("La capital de Francia es París.", "V"),///41
            ("Un script en Unity puede manejar más de un objeto a la vez.", "V"),
            ("El océano Pacífico es el océano más grande del mundo.", "V"),///43
            ("La Tierra es el tercer planeta del sistema solar.", "V"),
            ("Suecia tiene más de cien mil islas.", "V"),///45
            ("El componente AudioSource puede usarse para reproducir archivos MP3.", "V"),///46
            ("Los GameObjects en Unity 6 pueden tener más de un componente.", "V"),///47
            ("El monte Fuji está en Japón.","V"),///48 
            ("La función Update() se llama una vez por frame.", "V"),///49 
            ("El pino canario puede encontrarse en zonas costeras casi a nivel del mar","V")    // 50
        };

        foreach (var (question, correctAnswer) in questions)
        {
            ClearGrid(); // Clear the grid before displaying each question
            int startRow = 1; // Reset the starting line for each question

            superprint(1, startRow, question);
            string userAnswer = superinput(1, startRow + 1, "V/F?");
            
            if (userAnswer.ToUpper() == correctAnswer)
            {
                superpuntos++; // Increment the score for a correct answer
                MessageBox.Show("Correcto!", "Respuesta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Incorrecto! La respuesta correcta es {correctAnswer}.", 
                                "Respuesta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            System.Threading.Thread.Sleep(1000); // Wait for 1 second before clearing the grid
        }

        // Display the final score in a new MessageBox
        MessageBox.Show($"Puntuación final: {superpuntos}/{questions.Length}", 
                        "Resultado Final", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ClearGrid()
    {
        for (int row = 0; row < NumRows; row++)
        {
            for (int col = 0; col < NumColumns; col++)
            {
                textGrid[row, col] = ""; // Clear each cell
            }
        }
        Invalidate(); // Refresh the grid to reflect changes
    }

    public void superprint(int column_num, int row_num, string stringx)
    {
        for (int i = 0; i < stringx.Length; i++)
        {
            int currentColumn = column_num + i;

            if (row_num >= 0 && row_num < NumRows && currentColumn >= 0 && currentColumn < NumColumns)
            {
                textGrid[row_num, currentColumn] = stringx[i].ToString();
            }
        }
        Invalidate();
    }

    public void superprintword(int column_num, int row_num, string wordx)
    {
        for (int i = 0; i < wordx.Length; i++)
        {
            int currentColumn = column_num + i;
            if (row_num >= 0 && row_num < NumRows && currentColumn < NumColumns)
            {
                superprint(currentColumn, row_num, wordx[i].ToString());
            }
        }
    }

    public string superinput(int column_num, int row_num, string prompt)
    {
        superprintword(column_num, row_num, prompt);
        int inputStartCol = column_num + prompt.Length;
        int currentCol = inputStartCol;
        string userInput = "";

        cursorRow = row_num;
        cursorCol = currentCol;
        showCursor = true;
        Invalidate();

        bool enterPressed = false;

        KeyPressEventHandler keyPressHandler = (sender, e) =>
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                enterPressed = true;
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                if (userInput.Length > 0)
                {
                    userInput = userInput[..^1];
                    currentCol--;
                    superprint(currentCol, row_num, " ");
                }
            }
            else if (!char.IsControl(e.KeyChar))
            {
                userInput += e.KeyChar;
                superprint(currentCol, row_num, e.KeyChar.ToString());
                currentCol++;
            }

            cursorCol = currentCol;
            Invalidate();
        };

        KeyPress += keyPressHandler;

        while (!enterPressed)
        {
            Application.DoEvents();
        }

        KeyPress -= keyPressHandler;
        cursorCol = null;
        cursorRow = null;
        Invalidate();

        return userInput;
    }

    private void GridForm_Paint(object sender, PaintEventArgs e)
    {
        using Pen pen = new Pen(ForeColor);
        using StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        using Brush textBrush = new SolidBrush(ForeColor);
        Brush cursorBrush = isDarkMode ? Brushes.Cyan : Brushes.Black;

        for (int row = 0; row < NumRows; row++)
        {
            for (int col = 0; col < NumColumns; col++)
            {
                if (showGridLines)
                    e.Graphics.DrawRectangle(pen, gridRectangles[row, col]);

                e.Graphics.DrawString(textGrid[row, col] ?? "", displayFont, textBrush, gridRectangles[row, col], sf);
            }
        }

        if (cursorCol.HasValue && cursorRow.HasValue && showCursor)
        {
            Rectangle cursorRect = gridRectangles[cursorRow.Value, cursorCol.Value];
            e.Graphics.FillRectangle(cursorBrush, cursorRect);
        }
    }

    private void GridForm_Resize(object sender, EventArgs e)
    {
        int rectangleWidth = ClientSize.Width / NumColumns;
        int rectangleHeight = ClientSize.Height / NumRows;

        for (int row = 0; row < NumRows; row++)
        {
            for (int col = 0; col < NumColumns; col++)
            {
                gridRectangles[row, col] = new Rectangle(col * rectangleWidth, row * rectangleHeight, rectangleWidth, rectangleHeight);
            }
        }

        Invalidate();
    }

    private void GridForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.C)
        {
            if (!isDarkMode)
            {
                BackColor = Color.Black;
                ForeColor = Color.Cyan;
                displayFont = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                isDarkMode = true;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
                displayFont = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                isDarkMode = false;
            }

            Invalidate();
        }

        if (e.Control && e.KeyCode == Keys.G)
        {
            showGridLines = !showGridLines;
            Invalidate();
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new GridForm());
    }
}
