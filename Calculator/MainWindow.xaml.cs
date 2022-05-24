using System.Windows;
using System.Windows.Controls;

namespace Calculator {
    public partial class MainWindow {
        readonly string[][] buttons = {
            new string[] {"MC", "MR", "MS", "M+", "M-"},
            new string[] {"🠔", "CE", "C", "±", "√"},
            new string[] {"7", "8", "9", "/", "%"},
            new string[] {"4", "5", "6", "*", "1/x"},
            new string[] {"1", "2", "3", "-", "="},
            new string[] {"0", "0", ",", "+", "="}
        };
        public MainWindow() {
            InitializeComponent();
            ButtonsMarkup();
        }

        private void ButtonsMarkup() {
            for (var i = 0; i < buttons.Length; i++) {
                for (var j = 0; j < buttons[i].Length; j++) {
                    
                    var b = new Button() {
                        Content = buttons[i][j],
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
                    
                    Buttons.Children.Add(b);
                }
            }
        }
    }
}