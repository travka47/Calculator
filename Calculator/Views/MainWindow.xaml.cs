using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void Btn_Logic(string content) {
            try {
                CalculatorController.Dispatch(ref _state, content);
            }
            catch (InvalidOperationException invalidOperationException) {
                MessageBox.Show(invalidOperationException.Message);
                _state = new State { Input = "0", History = new Stack<string>(), MemoryOperand = _state.MemoryOperand };
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                MessageBox.Show(keyNotFoundException.Message);
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
            Input.Text = _state.Input;
            History.Text = string.Join(" ", _state.History.Reverse().ToArray());
        }

        private void Btn_Click(object sender, RoutedEventArgs e) {
            var b = sender as Button;
            var content = b.Content.ToString();
            Btn_Logic(content);
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            string content = e.Key switch
            {
                Key.Back => "🠔",
                Key.OemPlus when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "+",
                Key.Return or Key.OemPlus => "=",
                Key.Escape => "C",
                Key.Delete => "CE",
                Key.D0 or Key.NumPad0 => "0",
                Key.D1 or Key.NumPad1 => "1",
                Key.D2 or Key.NumPad2 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "√",
                Key.D2 or Key.NumPad2 => "2",
                Key.D3 or Key.NumPad3 => "3",
                Key.D4 or Key.NumPad4 => "4",
                Key.D5 or Key.NumPad5 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "%",
                Key.D5 or Key.NumPad5 => "5",
                Key.D6 or Key.NumPad6 => "6",
                Key.D7 or Key.NumPad7 => "7",
                Key.D8 or Key.NumPad8 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "*",
                Key.D8 or Key.NumPad8 => "8",
                Key.D9 or Key.NumPad9 => "9",
                Key.Multiply => "*",
                Key.Add => "+",
                Key.Subtract or Key.OemMinus => "-",
                Key.Decimal or Key.OemComma or Key.OemPeriod => ",",
                Key.Divide or Key.OemQuestion => "/",
                Key.M when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MS",
                Key.P when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "M+",
                Key.Q when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "M-",
                Key.R when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MR",
                Key.L when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MC",
                Key.R => "1/x",
                Key.F9 => "±",
                _ => "&"
            };
            Btn_Logic(content);
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