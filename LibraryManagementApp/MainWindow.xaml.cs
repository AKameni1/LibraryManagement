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
            if (TogglePasswordVisibility.Template.FindName("EyeIcon", TogglePasswordVisibility) is PackIconMaterial EyeIcon)
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

        private void ToggleSignUpPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleSignUpPasswordVisibility.Template.FindName("SignUpEyeIcon", ToggleSignUpPasswordVisibility) is PackIconMaterial SignUpEyeIcon)
            {
                if (ToggleSignUpPasswordVisibility.IsChecked == true)
                {
                    // Change l'icône pour afficher le mot de passe
                    SignUpEyeIcon.Kind = PackIconMaterialKind.Eye;

                    // Affiche le mot de passe en clair et masque le PasswordBox
                    SignUpPasswordTextBox.Visibility = Visibility.Visible;
                    SignUpPasswordBox.Visibility = Visibility.Collapsed;

                    // Copie le mot de passe du PasswordBox au TextBox
                    SignUpPasswordTextBox.Text = SignUpPasswordBox.Password;
                }
                else
                {
                    // Change l'icône pour masquer le mot de passe
                    SignUpEyeIcon.Kind = PackIconMaterialKind.EyeOff;

                    // Masque le TextBox et affiche le PasswordBox
                    SignUpPasswordTextBox.Visibility = Visibility.Collapsed;
                    SignUpPasswordBox.Visibility = Visibility.Visible;

                    // Copie le mot de passe du TextBox au PasswordBox
                    SignUpPasswordBox.Password = SignUpPasswordTextBox.Text;
                }
            }
        }


        private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (ToggleConfirmPasswordVisibility.Template.FindName("ConfirmPasswordEyeIcon", ToggleConfirmPasswordVisibility) is PackIconMaterial ConfirmPasswordEyeIcon)
            {
                if (ToggleConfirmPasswordVisibility.IsChecked == true)
                {
                    // Change l'icône pour afficher le mot de passe
                    ConfirmPasswordEyeIcon.Kind = PackIconMaterialKind.Eye;

                    // Affiche le mot de passe en clair et masque le PasswordBox
                    ConfirmPasswordTextBox.Visibility = Visibility.Visible;
                    ConfirmPasswordBox.Visibility = Visibility.Collapsed;

                    // Copie le mot de passe du PasswordBox au TextBox
                    ConfirmPasswordTextBox.Text = ConfirmPasswordBox.Password;
                }
                else
                {
                    // Change l'icône pour masquer le mot de passe
                    ConfirmPasswordEyeIcon.Kind = PackIconMaterialKind.EyeOff;

                    // Masque le TextBox et affiche le PasswordBox
                    ConfirmPasswordTextBox.Visibility = Visibility.Collapsed;
                    ConfirmPasswordBox.Visibility = Visibility.Visible;

                    // Copie le mot de passe du TextBox au PasswordBox
                    ConfirmPasswordBox.Password = ConfirmPasswordTextBox.Text;
                }
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des champs
            string username = SignUpUsernameTextBox.Text;
            string password = SignUpPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Validation des entrées
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Le nom d'utilisateur ne peut pas être vide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Le mot de passe ne peut pas être vide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Les mots de passe ne correspondent pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si toutes les validations passent, procéder à l'inscription
            // (ajouter votre logique d'inscription ici, comme l'enregistrement dans une base de données)

            MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}