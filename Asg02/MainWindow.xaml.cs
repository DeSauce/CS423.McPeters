using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asg02
{
    public partial class MainWindow : Window
    {
        private int turtleX;
        private int turtleY;
        private bool penDown;
        private int[,] grid;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
            penDown = false; // Initializes the pen to be up by default
            UpdateTurtleInfo();
        }

        private void InitializeGrid()
        {
            int rows = 50;
            int cols = 50;
            grid = new int[rows, cols];
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string command = CommandTextBox.Text.Trim();
            ProcessCommand(command);
            CommandTextBox.Clear();
            ExecuteCommand();
        }

        //Roland's default direction
        private string currentDirection = "North";

        private void ProcessCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            string[] parts = command.Split(',');

            if (int.TryParse(parts[0], out int cmd))
            {
                switch (cmd)
                {
                    case 1:
                        if (!penDown)
                        {
                            MessageBox.Show("Pen is already up.");
                        }
                        else
                        {
                            penDown = false; // Pen Up
                            UpdateTurtleInfo(); // Updates turtle information
                        }
                        break;

                    case 2:
                        if (penDown)
                        {
                            MessageBox.Show("Pen is already down.");
                        }
                        else
                        {
                            penDown = true; // Pen Down
                            UpdateTurtleInfo();
                        }
                        break;

                    case 3: // Roland turns to the right
                            // Updates the direction variable to turn right
                        if (currentDirection == "North") currentDirection = "East";
                        else if (currentDirection == "East") currentDirection = "South";
                        else if (currentDirection == "South") currentDirection = "West";
                        else if (currentDirection == "West") currentDirection = "North";
                        UpdateTurtleInfo();
                        break;

                    case 4: // Roland turns to the left
                            // Updates the direction variable after Roland turns left.
                        if (currentDirection == "North") currentDirection = "West";
                        else if (currentDirection == "West") currentDirection = "South";
                        else if (currentDirection == "South") currentDirection = "East";
                        else if (currentDirection == "East") currentDirection = "North";
                        UpdateTurtleInfo();
                        break;

                    case 5: // Moves Roland forward
                        if (parts.Length == 2 && int.TryParse(parts[1], out int steps))
                        {
                            MoveTurtle(steps);
                            UpdateTurtleInfo();
                        }
                        else
                        {
                            MessageBox.Show("Invalid command format. Use '5, <steps>' to move forward.");
                        }
                        break;

                    case 6:
                        RecordGrid(); // Writes the contents to standard output
                        UpdateTurtleInfo();
                        break;

                    case 7: // Clears grid completely
                        ClearGrid();
                        UpdateTurtleInfo();
                        break;

                    case 9:
                        Environment.Exit(0); // Terminates program
                        break;
                }
            }

            // Updates canvas
            DrawCanvas();
        }

        private void MoveTurtle(int steps)
        {
            // Calculates the step values based on the current direction
            int stepX = 0;
            int stepY = 0;

            switch (currentDirection)
            {
                case "North":
                    stepY = 1;
                    break;
                case "East":
                    stepX = 1;
                    break;
                case "South":
                    stepY = -1;
                    break;
                case "West":
                    stepX = -1;
                    break;
            }

            // Moves Roland in steps given
            for (int i = 0; i < steps; i++)
            {
                // Updates the new position for the next step
                int newX = turtleX + stepX;
                int newY = turtleY + stepY;

                // Updates the grid if the pen is down
                if (penDown)
                {
                    // Checks to see if the new position is within the grid boundaries
                    if (newX >= 0 && newX < grid.GetLength(1) && newY >= 0 && newY < grid.GetLength(0))
                    {
                        grid[newY, newX] = 1; // Marks the tile as 1 (drawing)
                    }
                }

                // Updates Roland's position
                turtleX = newX;
                turtleY = newY;
            }

            // Redraws the canvas to reflect the changes made
            DrawCanvas();

            // Updates the turtle information
            UpdateTurtleInfo();
        }

        private void ExecuteCommand()
        {
            string command = CommandTextBox.Text.Trim();
            ProcessCommand(command);
            CommandTextBox.Clear();
        }

        private void UpdateTurtleInfo()
        {
            string penState = penDown ? "Down" : "Up";
            TurtleInfo.Text = $"Roland's Location: ({turtleX}, {turtleY}), Direction: {currentDirection}, Pen: {penState}";
        }

        private void RecordGrid()
        {
            // Prints the grid contents to the standard output
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    Console.Write(grid[row, col] == 1 ? "*" : " ");
                }
                Console.WriteLine(); // Moves output to the next row
            }
        }

        private void ClearGrid()
        {
            // Sets all tile values to 0
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    grid[row, col] = 0;
                }
            }

            // Resets Roland's position to its default location (0, 0)
            turtleX = 0;
            turtleY = 0;

            TurtleCanvas.Children.Clear();

            TurtleInfo.Text = $"Turtle Location: ({turtleX}, {turtleY}), Direction: North, Pen: Up";
        }


        //Draws the canvas
        private void DrawCanvas()
        {
            TurtleCanvas.Children.Clear();

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 1)
                    {
                        DrawTile(row, col);
                    }
                }
            }
        }

        //Marks Roland's path
        private void DrawTile(int row, int col)
        {
            Rectangle rect = new Rectangle
            {
                Width = TurtleCanvas.ActualWidth / grid.GetLength(1),
                Height = TurtleCanvas.ActualHeight / grid.GetLength(0),
                Fill = (row == turtleY && col == turtleX) ? Brushes.Green : Brushes.Black
            };

            Canvas.SetLeft(rect, col * rect.Width);
            Canvas.SetTop(rect, (grid.GetLength(0) - row - 1) * rect.Height);

            TurtleCanvas.Children.Add(rect);
        }

        private void CommandTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteCommand(); // Call the function to execute the command when Enter/Return is pressed
            }
        }
    }
}