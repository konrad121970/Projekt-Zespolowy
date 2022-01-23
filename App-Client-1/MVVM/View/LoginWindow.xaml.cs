using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Input;

namespace ClientApp.MVVM.View {

    public partial class LoginWindow : Window {
        public LoginWindow() {
            InitializeComponent();
        }


        private void ResizeWindow_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }


        private void MinimizeWindowButton_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindowButton_Click(object sender, RoutedEventArgs e) {
            if (WindowState != WindowState.Maximized) {
                ResizeMode = ResizeMode.NoResize;
                WindowState = WindowState.Maximized;
            }
            else {
                ResizeMode = ResizeMode.CanResizeWithGrip;
                WindowState = WindowState.Normal;
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }

}