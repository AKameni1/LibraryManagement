using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.IconPacks;

namespace LibraryManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (TogglePasswordVisibility.IsChecked == true)
            {
                // Change l'icône pour afficher le mot de passe
                EyeIcon.Kind = PackIconMaterialKind.Eye;

                // Affiche le mot de passe en clair et masque le PasswordBox
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;

                // Copie le mot de passe du PasswordBox au TextBox
                PasswordTextBox.Text = PasswordBox.Password;
            }
            else
            {
                // Change l'icône pour masquer le mot de passe
                EyeIcon.Kind = PackIconMaterialKind.EyeOff;

                // Masque le TextBox et affiche le PasswordBox
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;

                // Copie le mot de passe du TextBox au PasswordBox
                PasswordBox.Password = PasswordTextBox.Text;
            }
        }


    }
}