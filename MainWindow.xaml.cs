using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Final_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbBookStore db = new DbBookStore();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_Submit_Click(object sender, RoutedEventArgs e)
        {

            

            string userName = tb_User.Text;
            string passWord = tb_Password.Password;
            string role = "";

            var user = db.Users.FirstOrDefault(u => u.userName == userName && u.passWord == passWord);

            if (user != null)
            {
                role = user.role;
                MessageBox.Show("Login Success!!! " + role);

                if (role == "Admin")
                {
                    // Admin login
                    AdminPanel adminPanel = new AdminPanel();
                    adminPanel.Show();
                }
                else if (role == "Seller")
                {
                    // Seller login
                    SellerPanel sellerPanel = new SellerPanel();
                    sellerPanel.Show();
                }
                else
                {
                    MessageBox.Show("Invalid user role.");
                }

                // Clear the input fields
                tb_User.Text = string.Empty;
                tb_Password.Password = string.Empty;
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Fail!!!");

                // Clear the input fields
                tb_User.Text = string.Empty;
                tb_Password.Password = string.Empty;
            }

        }

        private void tb_Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            Label watermark = (Label)passwordBox.Template.FindName("Watermark", passwordBox);

            if (passwordBox.SecurePassword.Length > 0)
            {
                watermark.Visibility = Visibility.Collapsed;
            }
            else
            {
                watermark.Visibility = Visibility.Visible;
            }
        }
    }
}
