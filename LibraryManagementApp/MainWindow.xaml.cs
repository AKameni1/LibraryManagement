using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            ToggleView(false); // false pour afficher la vue d'inscription
        }


        private void SignIn_Click(object sender, MouseButtonEventArgs e)
        {
            ToggleView(true); // true pour afficher la vue de connexion
        }



        private void ToggleView(bool isSignIn)
        {
            // Préparez les Storyboards pour l'animation
            var fadeOutStoryboard = new Storyboard();
            var fadeInStoryboard = new Storyboard();

            // Animation pour la vue actuelle (SignIn ou SignUp)
            DoubleAnimation fadeOutAnimation = new()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd
            };

            // Déterminez quel panneau est visible et doit s'estomper
            var panelToFadeOut = isSignIn ? SignUpPanel : SignInPanel;
            Storyboard.SetTarget(fadeOutAnimation, panelToFadeOut);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            fadeOutStoryboard.Children.Add(fadeOutAnimation);

            fadeOutStoryboard.Completed += (s, e) =>
            {
                // Changer la visibilité une fois l'animation terminée
                panelToFadeOut.Visibility = Visibility.Collapsed;

                // Déterminer quel panneau doit apparaître
                var panelToFadeIn = isSignIn ? SignInPanel : SignUpPanel;
                panelToFadeIn.Visibility = Visibility.Visible;

                // Animation pour la vue suivante
                DoubleAnimation fadeInAnimation = new()
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.HoldEnd
                };

                Storyboard.SetTarget(fadeInAnimation, panelToFadeIn);
                Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));

                fadeInStoryboard.Children.Add(fadeInAnimation);
                fadeInStoryboard.Begin();
            };

            fadeOutStoryboard.Begin();
        }

        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibilityFunction(PasswordBox, PasswordTextBox, TogglePasswordVisibility, "EyeIcon");
        }

        private void ToggleSignUpPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibilityFunction(SignUpPasswordBox, SignUpPasswordTextBox, ToggleSignUpPasswordVisibility, "SignUpEyeIcon");
        }

        private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibilityFunction(ConfirmPasswordBox, ConfirmPasswordTextBox, ToggleConfirmPasswordVisibility, "ConfirmPasswordEyeIcon");
        }


        private static void TogglePasswordVisibilityFunction(PasswordBox passwordBox, TextBox textBox, ToggleButton toggleButton, string iconName)
        {
            if (toggleButton.Template.FindName(iconName, toggleButton) is PackIconMaterial icon)
            {
                if (toggleButton.IsChecked == true)
                {
                    icon.Kind = PackIconMaterialKind.Eye;
                    textBox.Visibility = Visibility.Visible;
                    passwordBox.Visibility = Visibility.Collapsed;
                    textBox.Text = passwordBox.Password;
                }
                else
                {
                    icon.Kind = PackIconMaterialKind.EyeOff;
                    textBox.Visibility = Visibility.Collapsed;
                    passwordBox.Visibility = Visibility.Visible;
                    passwordBox.Password = textBox.Text;
                }
            }
        }


        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SignUpUsernameTextBox.Text;
            string password = SignUpPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (!ValidateSignUpForm(username, password, confirmPassword))
            {
                return;
            }

            // Inscription réussie
            MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private static bool ValidateSignUpForm(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Le nom d'utilisateur ne peut pas être vide.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Le mot de passe ne peut pas être vide.");
                return false;
            }

            if (password != confirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.");
                return false;
            }

            return true;
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}