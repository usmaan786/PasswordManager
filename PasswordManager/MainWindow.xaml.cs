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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Password _passwordManager;
        public MainWindow()
        {
            InitializeComponent();
            _passwordManager = new Password(new KeyVault());
        }

        private void generateBtn_Click(object sender, RoutedEventArgs e)
        {
            string generatedPassword = Generator.GeneratePassword((int)generateSlider.Value);
            generatedText.Text = generatedPassword;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTxt.Text;
            string password = passwordTxt.Password;

            _passwordManager.SavePassword(username, password);
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            Password passwordManager = new Password(new KeyVault());

            List<Password.PasswordEntry> passwords = passwordManager.LoadPassword();

            passwordsList.ItemsSource = passwords;
        }
    }
}
