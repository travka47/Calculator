using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Calculator.Controllers;
using Calculator.Models;

namespace Calculator.Views {
    public partial class MainWindow {
        readonly string[][] buttons = {
            new string[] {"MC", "MR", "MS", "M+", "M-"},
            new string[] {"🠔", "CE", "C", "±", "√"},
            new string[] {"7", "8", "9", "/", "%"},
            new string[] {"4", "5", "6", "*", "1/x"},
            new string[] {"1", "2", "3", "-", "="},
            new string[] {"0", "0", ",", "+", "="}
        };

        private State _state;
        public MainWindow() {
            InitializeComponent();
            ButtonsMarkup();
            _state = new State { Input = "0", History = new Stack<string>()};
        }

        private void Btn_Click(object sender, RoutedEventArgs e) {
            var b = sender as Button;
            var content = b.Content.ToString();
            try {
                CalculatorController.Dispatch(ref _state, content);
            }
            catch (InvalidOperationException invalidOperationException) {
                MessageBox.Show(invalidOperationException.Message);
                _state = new State { Input = "0", History = new Stack<string>(), MemoryOperand = _state.MemoryOperand };
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            Input.Text = _state.Input;
            History.Text = string.Join(" ", _state.History.Reverse().ToArray());
        }

        private void ButtonsMarkup() {
            for (var i = 0; i < buttons.Length; i++) {
                for (var j = 0; j < buttons[i].Length; j++) {
                    
                    var b = new Button() {
                        Content = buttons[i][j],
                        Background = Brushes.MintCream,
                        Margin = new Thickness(2.5),
                    };
                    
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);

                    if (i == 5 && j <= 1) {
                        if (j == 0) {
                            var next = Grid.GetColumnSpan(b);
                            Grid.SetColumnSpan(b, ++next);
                        }
                        else continue;
                    }
                    
                    if (j == 4 && i >= 4) {
                        if (i == 4) {
                            var next = Grid.GetColumnSpan(b);
                            Grid.SetRowSpan(b, ++next);
                        }
                        else continue;
                    }

                    b.Click += Btn_Click;
                    Buttons.Children.Add(b);
                }
            }
        }
    }
}