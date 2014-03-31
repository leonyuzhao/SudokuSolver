using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SudokuSolver.Resources;

namespace SudokuSolver
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalControls();

        }
        
        private string[] values;

        private void BuildLocalControls()
        {
            // Value Matrix
            values = new string[81];
            for (int i = 0; i < 81; i++)
            {
                values[i] = ".";
            }

            // Title 
            TextBlock block = new TextBlock();
            block.Text = "Sudoku Solver";
            block.Style = (Style)Application.Current.Resources["PhoneTextNormalStyle"];
            block.Margin = new Thickness(12, 0, 0, 0);
            TitlePanel.Children.Add(block);

            // Set of input textbox
            TextBox box = null;
            System.Windows.Input.InputScope scope = new System.Windows.Input.InputScope();
            System.Windows.Input.InputScopeName name = new System.Windows.Input.InputScopeName();
            name.NameValue = System.Windows.Input.InputScopeNameValue.Number;
            scope.Names.Add(name);

            for (int i = 1; i <= 81; i++)
            {
                box = new TextBox();
                box.Name = "txt" + i;
                box.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                box.Height = 72;
                box.Margin = GetMargin(i);
                box.TextWrapping = TextWrapping.Wrap;
                box.TextAlignment = TextAlignment.Center;
                box.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                box.Width = 66;
                box.TextChanged += TextBox_TextChanged;
                box.InputScope = scope;
                ContentPanel.Children.Add(box);
            }

            // Button Area
            Button solveBtn = new Button();
            solveBtn.Name = "solveBtn";
            solveBtn.Content = "Solve";
            solveBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            solveBtn.Margin = new Thickness(71, 550, 0, 0);
            solveBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            solveBtn.Click += Solve_Click;
            ContentPanel.Children.Add(solveBtn);

            Button clearBtn = new Button();
            clearBtn.Content = "Clear";
            clearBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            clearBtn.Margin = new Thickness(174, 550, 0, 0);
            clearBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            clearBtn.Click += Clear_Click;
            ContentPanel.Children.Add(clearBtn);

            Button demoBtn = new Button();
            demoBtn.Content = "Demo";
            demoBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            demoBtn.Margin = new Thickness(273, 550, 0, 0);
            demoBtn.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            demoBtn.Click += Demo_Click;
            ContentPanel.Children.Add(demoBtn);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            int num;
            bool isNumeric = int.TryParse(box.Text, out num);
            if (isNumeric)
            {
                string name = box.Name;
                values[Convert.ToInt16(name.Replace("txt", "")) - 1] = box.Text;
            }
        }

        private void Solve_Click(object sender, RoutedEventArgs e)
        {
            string board = string.Join("", values);
            SudoKu.SudokuSolver solver = null;
            try
            {
                solver = new SudoKu.SudokuSolver(board);
                bool solved = solver.Solve();
                if (!solved) { MessageBox.Show("Can not solve this sudoku."); return; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (solver == null) { return; }

            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    values[index] = solver.SudokuMatrix[j, i].ToString();
                    index++;
                }
            }
            foreach (var item in ContentPanel.Children)
            {
                if (item is TextBox)
                {
                    string name = ((TextBox)item).Name;
                    int num = Convert.ToInt16(name.Replace("txt", ""));
                    if (values[num - 1] != ".")
                        ((TextBox)item).Text = values[num - 1];
                }
                else if(item is Button)
                {
                    if (((Button)item).Name == "solveBtn")
                    {
                        ((Button)item).IsEnabled = false;
                    }
                }
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 81; i++)
            {
                values[i] = ".";
            }      
            foreach (var item in ContentPanel.Children)
            { 
                if (item is TextBox)
                {
                    ((TextBox)item).Text = "";
                }
                else if (item is Button)
                {
                    if (((Button)item).Name == "solveBtn")
                    {
                        ((Button)item).IsEnabled = true;
                    }
                }
            }
        }

        private void Demo_Click(object sender, RoutedEventArgs e)
        {
            const string cDemoBoard = "8..........36......7..9.2...5...7.......457.....1...3...1....68..85...1..9....4.."; // World's hardest sudoku

            Clear_Click(sender, e);

            for (int i = 0; i < 81; i++)
            {
                values[i] = cDemoBoard[i].ToString();
            }

            foreach (var item in ContentPanel.Children)
            {
                if (item is TextBox)
                {
                    string name = ((TextBox)item).Name;
                    int num = Convert.ToInt16(name.Replace("txt", ""));
                    if (values[num - 1] != ".")
                        ((TextBox)item).Text = values[num - 1];
                }
            }           
        }

        private Thickness GetMargin(int num)
        {
            int x, y;
            if (num % 9 == 0)
            {
                x = 9;
                y = num / 9;
            }
            else
            {
                x = num % 9;
                y = num / 9 + 1;
            }
            int xValue = 0, yValue = 0;
            switch (x)
            { 
                case 1:
                    xValue = 1;
                    break;
                case 2:
                    xValue = 44;
                    break;
                case 3:
                    xValue = 87;
                    break;
                case 4:
                    xValue = 148;
                    break;
                case 5:
                    xValue = 191;
                    break;
                case 6:
                    xValue = 234;
                    break;
                case 7:
                    xValue = 295;
                    break;
                case 8:
                    xValue = 338;
                    break;
                case 9:
                    xValue = 381;
                    break;
            }
            switch (y)
            {
                case 1:
                    yValue = 10;
                    break;
                case 2:
                    yValue = 59;
                    break;
                case 3:
                    yValue = 108;
                    break;
                case 4:
                    yValue = 171;
                    break;
                case 5:
                    yValue = 220;
                    break;
                case 6:
                    yValue = 269;
                    break;
                case 7:
                    yValue = 331;
                    break;
                case 8:
                    yValue = 380;
                    break;
                case 9:
                    yValue = 429;
                    break;
            }
            return new Thickness(xValue, yValue, 0, 0);
        }
        
    }
}