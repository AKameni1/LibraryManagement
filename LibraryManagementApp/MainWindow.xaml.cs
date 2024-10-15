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

        private void ShowError(string message, TextBlock errorTextBlock, Border errorBorder)
        {
            errorTextBlock.Text = message;
            errorTextBlock.Visibility = Visibility.Visible;
            errorBorder.Visibility = Visibility.Visible;

            // Animation (par exemple, faire apparaître progressivement)
            var fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            errorTextBlock.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ClearErrors()
        {
            var errorTextBlocks = new[]
            {
                UsernameErrorTextBlock,
                PasswordErrorTextBlock,
                SignUpUsernameErrorTextBlock,
                SignUpPasswordErrorTextBlock,
                ConfirmPasswordErrorTextBlock
            };

            var errorBorders = new[]
            {
                UsernameErrorBorder,
                PasswordErrorBorder,
                SignUpUsernameErrorBorder,
                SignUpPasswordErrorBorder,
                ConfirmPasswordErrorBorder
            };

            foreach (var textBlock in errorTextBlocks)
            {
                textBlock.Visibility = Visibility.Collapsed;
            }

            foreach (var border in errorBorders)
            {
                border.Visibility = Visibility.Collapsed;
            }
        }

        // Modifie la méthode SignInButton_Click
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors(); // Réinitialiser les messages d'erreur

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (!ValidateSignInForm(username, password))
            {
                return;
            }

            // Connexion réussie
            MessageBox.Show("Connexion réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Validation pour le panneau de connexion
        private bool ValidateSignInForm(string username, string password)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Le nom d'utilisateur ne peut pas être vide.", UsernameErrorTextBlock, UsernameErrorBorder);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Le mot de passe ne peut pas être vide.", PasswordErrorTextBlock, PasswordErrorBorder);
                isValid = false;
            }

            return isValid;
        }

        // Modifie la méthode SignUpButton_Click
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs saisies
            string username = SignUpUsernameTextBox.Text.Trim();
            string password = SignUpPasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            // Réinitialiser les messages d'erreur
            ClearErrors();

            // Valider le formulaire
            var errors = ValidateSignUpForm(username, password, confirmPassword);

            // Afficher les erreurs si présentes
            if (errors.Count > 0)
            {
                ShowErrors(errors);
                return;
            }

            // Inscription réussie
            MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private Dictionary<string, string> ValidateSignUpForm(string username, string password, string confirmPassword)
        {
            var errors = new Dictionary<string, string>();

            // Vérifications pour chaque champ
            if (string.IsNullOrWhiteSpace(username))
            {
                errors[nameof(SignUpUsernameErrorTextBlock)] = "Le nom d'utilisateur ne peut pas être vide.";
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors[nameof(SignUpPasswordErrorTextBlock)] = "Le mot de passe ne peut pas être vide.";
            }
            else if (password.Length < 6) // Exemple de vérification de longueur
            {
                errors[nameof(SignUpPasswordErrorTextBlock)] = "Le mot de passe doit contenir au moins 6 caractères.";
            }

            if (password != confirmPassword)
            {
                errors[nameof(ConfirmPasswordErrorTextBlock)] = "Les mots de passe ne correspondent pas.";
            }

            return errors;
        }

        private void ShowErrors(Dictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                string errorTextBlockName = error.Key;
                string errorMessage = error.Value;

                // Récupérer le TextBlock et la bordure d'erreur correspondants
                var errorTextBlock = (TextBlock)FindName(errorTextBlockName);
                var errorBorderName = errorTextBlockName.Replace("ErrorTextBlock", "ErrorBorder");
                var errorBorder = (Border)FindName(errorBorderName);

                ShowError(errorMessage, errorTextBlock, errorBorder);
            }
        }


    }
} 