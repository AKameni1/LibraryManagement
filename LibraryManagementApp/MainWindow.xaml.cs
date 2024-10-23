using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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
using Arthur_Jayson_UA2;
using MahApps.Metro.IconPacks;

namespace LibraryManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int nbChildResetPassword;
        private int nbChildResetUsername;
        public int NbChildResetPassword { get => nbChildResetPassword; set => nbChildResetPassword = ResetPasswordPanel.Children.Count; }
        public int NbChildResetUsername { get => nbChildResetUsername; set => nbChildResetUsername = ResetUsernamePanel.Children.Count; }

        public MainWindow() => InitializeComponent();

        private void SignInTransition_Completed(object sender, EventArgs e)
        {
            // Hide the UsernameResetControl after the fade-out animation
            ResetUsernamePanel.Visibility = Visibility.Collapsed;
            ResetPasswordPanel.Visibility = Visibility.Collapsed;
            
            // Make sure SignInPanel is visible
            SignInPanel.Visibility = Visibility.Visible;
            ResetFields();
        }

        private void SignIn_Completed(object sender, EventArgs e)
        {
            SignUpPanel.Visibility = Visibility.Collapsed;
            SignInPanel.Visibility = Visibility.Visible;
        }

        private void SignUp_Click(object sender, MouseButtonEventArgs e)
        {
            ToggleView(false); // false pour afficher la vue d'inscription
            ResetFields();
        }

        private void SignIn_Click(object sender, MouseButtonEventArgs e)
        {
            ToggleView(true); // true pour afficher la vue de connexion
        }

        private void ToggleView(bool isSignIn)
        {
            var fadeOutStoryboard = new Storyboard();
            var fadeInStoryboard = new Storyboard();

            DoubleAnimation fadeOutAnimation = new()
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5),
                FillBehavior = FillBehavior.HoldEnd
            };

            var panelToFadeOut = isSignIn ? SignUpPanel : SignInPanel;
            Storyboard.SetTarget(fadeOutAnimation, panelToFadeOut);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            fadeOutStoryboard.Children.Add(fadeOutAnimation);

            fadeOutStoryboard.Completed += (s, e) =>
            {
                panelToFadeOut.Visibility = Visibility.Collapsed;

                var panelToFadeIn = isSignIn ? SignInPanel : SignUpPanel;
                panelToFadeIn.Visibility = Visibility.Visible;

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

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SwitchToPanel(ResetPasswordPanel);
            ResetFields();
        }

        private void ForgotUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {

            SwitchToPanel(ResetUsernamePanel);
            ResetFields();
        }

        private void SignIn(object sender, MouseButtonEventArgs e)
        {
            // Déterminez le panneau courant
            var currentPanel = SignInPanel.Visibility == Visibility.Visible ? SignInPanel :
                               SignUpPanel.Visibility == Visibility.Visible ? SignUpPanel :
                               ResetPasswordPanel.Visibility == Visibility.Visible ? ResetPasswordPanel : ResetUsernamePanel;

            // Créez une animation d'opacité pour la fermeture progressive
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1, // Opacité initiale
                To = 0,   // Opacité finale
                Duration = TimeSpan.FromMilliseconds(300) // Durée de l'animation
            };

            // Gérer l'événement Completed de l'animation
            fadeOutAnimation.Completed += (s, args) =>
            {
                currentPanel.Visibility = Visibility.Collapsed; // Masquer le panneau
                SignIn_Click(sender, e); // Appeler la méthode de connexion
            };

            // Appliquer l'animation d'opacité au panneau courant
            currentPanel.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }


        private void SwitchToPanel(Panel panelToShow)
        {
            var currentPanel = SignInPanel.Visibility == Visibility.Visible ? SignInPanel :
                                SignUpPanel.Visibility == Visibility.Visible ? SignUpPanel :
                                ResetPasswordPanel.Visibility == Visibility.Visible ? ResetPasswordPanel : ResetUsernamePanel;

            if (currentPanel != panelToShow)
            {
                DoubleAnimation slideOutAnimation = new()
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300),
                    FillBehavior = FillBehavior.HoldEnd
                };

                slideOutAnimation.Completed += (s, a) =>
                {
                    currentPanel.Visibility = Visibility.Collapsed;
                    panelToShow.Visibility = Visibility.Visible;
                    panelToShow.Opacity = 1;

                    DoubleAnimation slideInAnimation = new()
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromMilliseconds(300),
                        FillBehavior = FillBehavior.HoldEnd
                    };

                    panelToShow.BeginAnimation(OpacityProperty, slideInAnimation);
                };

                currentPanel.BeginAnimation(OpacityProperty, slideOutAnimation);
            }
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            _ = ResetEmailTextBox.Text;
            // Ajouter votre logique de réinitialisation ici
        }

        private void ResetUsernameButton_Click(object sender, RoutedEventArgs e)
        {
            _ = ResetUsernameEmailTextBox.Text;
            // Ajouter votre logique de réinitialisation ici
        }

        private static void ShowError(string message, TextBlock errorTextBlock, Border errorBorder)
        {
            errorTextBlock.Text = message;
            errorTextBlock.Visibility = Visibility.Visible;
            errorBorder.Visibility = Visibility.Visible;

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
                    ConfirmPasswordErrorTextBlock,
                    ResetEmailErrorTextBlock,
                    ResetUsernameEmailErrorTextBlock
                };

            var errorBorders = new[]
                    {
                        UsernameErrorBorder,
                        PasswordErrorBorder,
                        SignUpUsernameErrorBorder,
                        SignUpPasswordErrorBorder,
                        ConfirmPasswordErrorBorder,
                        ResetEmailErrorBorder,
                        ResetUsernameEmailErrorBorder
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

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
            ClearErrors();

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (!ValidateSignInForm(username, password))
            {
                return;
            }

            MessageBox.Show("Connexion réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static bool ValidateResetForm(string email, TextBlock textBlock, Border border)
        {
            bool isValid = true;

            if (!IsValidEmail(email) || string.IsNullOrEmpty(email))
            {
                ShowError("Adresse e-mail invalide.", textBlock, border);
                isValid = false;
            }

            return isValid;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            
            ClearErrors();
            Button clickedButton = (Button)sender;
            string parentName = "";
            var parent = VisualTreeHelper.GetParent(clickedButton); 

            if (parent is StackPanel stackPanelParent)
            {
                parentName = stackPanelParent.Name;
            }

            if (parentName == "EmailVerificationPasswordPanel")
            {
                string email = ResetEmailTextBox.Text.Trim();

                if (!ValidateResetForm(email, ResetEmailErrorTextBlock, ResetEmailErrorBorder))
                {
                    return;
                }
                // Masquer tous les enfants existants
                for (int i = 0; i < ResetPasswordPanel.Children.Count; i++)
                {
                    ResetPasswordPanel.Children[i].Visibility = Visibility.Collapsed;
                }

                // Masquer tous les enfants existants
                for (int i = 0; i < ResetPasswordPanel.Children.Count; i++)
                {
                    ResetPasswordPanel.Children[i].Visibility = Visibility.Collapsed;
                }

                // Ajouter le nouveau contrôle de réinitialisation mais le garder masqué
                PasswordResetControl passwordResetControl = new PasswordResetControl();
                passwordResetControl.ResetCompleted += (s, args) =>
                {
                    // Afficher un message de succès ou rediriger si nécessaire
                    RestoreChildrenVisibility(ResetPasswordPanel);
                };

                ResetPasswordPanel.Children.Add(passwordResetControl);

                // Une fois la vérification réussie, afficher le contrôle
                passwordResetControl.Visibility = Visibility.Visible;


            }
            else if (parentName == "EmailVerificationUsernamePanel")
            {
                string email = ResetUsernameEmailTextBox.Text.Trim();

                if (!ValidateResetForm(email, ResetUsernameEmailErrorTextBlock, ResetUsernameEmailErrorBorder))
                {
                    return;
                }

                // Masquer tous les enfants existants
                for (int i = 0; i < ResetUsernamePanel.Children.Count; i++)
                {
                    ResetUsernamePanel.Children[i].Visibility = Visibility.Collapsed;
                }

                // Ajouter le nouveau contrôle de réinitialisation mais le garder masqué
                UsernameResetControl usernameResetControl = new UsernameResetControl();
                usernameResetControl.ResetCompleted += (s, args) =>
                {
                    // Afficher un message de succès ou rediriger si nécessaire
                    RestoreChildrenVisibility(ResetUsernamePanel);
                };

                ResetUsernamePanel.Children.Add(usernameResetControl);

                // Une fois la vérification réussie, afficher le contrôle
                usernameResetControl.Visibility = Visibility.Visible;

            }

            ResetFields();
        }

        private void RestoreChildrenVisibility(Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is UIElement uiElement)
                {
                    uiElement.Visibility = Visibility.Visible;
                }
            }
        }   

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

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SignUpUsernameTextBox.Text.Trim();
            string email = SignUpEmailTextBox.Text.Trim();
            string password = SignUpPasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            ClearErrors(); // Méthode pour effacer les erreurs précédentes

            var errors = ValidateSignUpForm(username, email, password, confirmPassword);

            if (errors.Count > 0)
            {
                ShowErrors(errors); // Appeler la nouvelle méthode ShowErrors
                return;
            }

            ResetFields();
            var transitionStoryboard = (Storyboard)Application.Current.MainWindow.FindResource("SignUpToSignIn");
            transitionStoryboard.Begin();

            //MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private static Dictionary<string, string> ValidateSignUpForm(string username, string email, string password, string confirmPassword)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("SignUpUsernameErrorTextBlock", "Le nom d'utilisateur ne peut pas être vide.");
            }

            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                errors.Add("SignUpEmailErrorTextBlock", "Adresse e-mail invalide.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("SignUpPasswordErrorTextBlock", "Le mot de passe ne peut pas être vide.");
            }

            if (password != confirmPassword)
            {
                errors.Add("ConfirmPasswordErrorTextBlock", "Les mots de passe ne correspondent pas.");
            }

            return errors;
        }


        private static bool IsValidEmail(string email)
        {
            // Utiliser une expression régulière pour valider l'adresse e-mail
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private void ShowErrors(Dictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                string errorTextBlockName = error.Key; // Nom du TextBlock d'erreur
                string errorMessage = error.Value; // Message d'erreur

                // Trouver le TextBlock d'erreur correspondant
                var errorTextBlock = (TextBlock)FindName(errorTextBlockName);
                var errorBorderName = errorTextBlockName.Replace("ErrorTextBlock", "ErrorBorder"); // Nom du Border d'erreur
                var errorBorder = (Border)FindName(errorBorderName); // Trouver le Border d'erreur correspondant

                ShowError(errorMessage, errorTextBlock, errorBorder); // Appeler la méthode ShowError
            }
        }

        private void ResetFields()
        {
            // Réinitialiser les champs du SignInPanel
            UsernameTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;

            // Réinitialiser les erreurs
            UsernameErrorBorder.Visibility = Visibility.Collapsed;
            PasswordErrorBorder.Visibility = Visibility.Collapsed;

            // Réinitialiser les champs du SignUpPanel
            SignUpUsernameTextBox.Text = string.Empty;
            SignUpEmailTextBox.Text = string.Empty;
            SignUpPasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;

            // Réinitialiser les erreurs
            SignUpUsernameErrorBorder.Visibility = Visibility.Collapsed;
            SignUpEmailErrorBorder.Visibility = Visibility.Collapsed;
            SignUpPasswordErrorBorder.Visibility = Visibility.Collapsed;
            ConfirmPasswordErrorBorder.Visibility = Visibility.Collapsed;

            // Réinitialiser les champs du ResetPasswordPanel
            ResetEmailTextBox.Text = string.Empty;
            ResetEmailErrorBorder.Visibility = Visibility.Collapsed;

            // Réinitialiser les champs du ResetUsernamePanel
            ResetUsernameEmailTextBox.Text = string.Empty;
            ResetUsernameEmailErrorBorder.Visibility = Visibility.Collapsed;

            // Réinitialiser le message de succès
            //SuccessBorder.Visibility = Visibility.Collapsed;
            //SuccessMessageTextBlock.Text = string.Empty;

        }
    }
}