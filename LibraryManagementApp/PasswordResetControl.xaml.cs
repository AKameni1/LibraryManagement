using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using LibraryManagementApp;
using MahApps.Metro.IconPacks;

namespace Arthur_Jayson_UA2
{
    /// <summary>
    /// Logique d'interaction pour PasswordResetControl.xaml
    /// </summary>
    public partial class PasswordResetControl : UserControl
    {
        public PasswordResetControl() => InitializeComponent();

        private void ToggleNewPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibilityFunction(NewPasswordBox, NewPasswordTextBox, ToggleNewPasswordVisibility, "EyeIcon");
        }

        private void ToggleConfirmPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibilityFunction(ConfirmNewPasswordBox, ConfirmNewPasswordTextBox, ToggleConfirmNewPasswordVisibility, "ConfirmNewPasswordEyeIcon");
        }

        private static void TogglePasswordVisibilityFunction(PasswordBox passwordBox, TextBox textBox, ToggleButton toggleButton, string iconName)
        {
            if (toggleButton.Template.FindName(iconName, toggleButton) is PackIconMaterial icon)
            {
                if (toggleButton.IsChecked == true)
                {
                    textBox.Text = passwordBox.Password;
                    icon.Kind = PackIconMaterialKind.Eye;
                    textBox.Visibility = Visibility.Visible;
                    passwordBox.Visibility = Visibility.Collapsed;
                    textBox.Text = passwordBox.Password;
                }
                else
                {
                    passwordBox.Password = textBox.Text;
                    icon.Kind = PackIconMaterialKind.EyeOff;
                    textBox.Visibility = Visibility.Collapsed;
                    passwordBox.Visibility = Visibility.Visible;
                    passwordBox.Password = textBox.Text;
                }
            }
        }
        public event EventHandler? ResetCompleted;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les mots de passe saisis
            string newPassword = NewPasswordBox.Password.Trim();
            string confirmPassword = ConfirmNewPasswordBox.Password.Trim();

            // Réinitialiser les messages d'erreur
            ConfirmNewPasswordErrorBorder.Visibility = Visibility.Collapsed;
            ConfirmNewPasswordErrorTextBlock.Text = string.Empty;
            PasswordErrorBorder.Visibility = Visibility.Collapsed; // Réinitialiser également l'erreur de mot de passe
            PasswordErrorTextBlock.Text = string.Empty; // Réinitialiser le texte d'erreur
            SuccessMessageTextBlock.Visibility = Visibility.Collapsed; // Réinitialiser le message de succès

            bool hasError = false;

            // Vérifier si les mots de passe sont vides
            if (string.IsNullOrEmpty(newPassword))
            {
                ShowError("Le mot de passe ne peut pas être vide.", PasswordErrorBorder, PasswordErrorTextBlock);
                hasError = true;
            }
            else if (newPassword.Length < 6) // Vérifier la longueur minimum
            {
                ShowError("Le mot de passe doit avoir au moins 6 caractères.", PasswordErrorBorder, PasswordErrorTextBlock);
                hasError = true;
            }

            // Vérifier si le mot de passe de confirmation est vide
            if (string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("La confirmation du mot de passe ne peut pas être vide.", ConfirmNewPasswordErrorBorder, ConfirmNewPasswordErrorTextBlock);
                hasError = true;
            }

            // Vérifier si les mots de passe correspondent
            if (newPassword != confirmPassword)
            {
                ShowError("Les mots de passe ne correspondent pas.", ConfirmNewPasswordErrorBorder, ConfirmNewPasswordErrorTextBlock);
                hasError = true;
            }

            // Si tout est bon, afficher le message de succès et diriger vers le panneau de connexion
            if (!hasError)
            {
                FadeOutAndShowLoginPanel();
            }

            // Déclencher l'événement ResetCompleted
            if (!hasError)
            {
                // Si tout est bon, afficher le message de succès
                SuccessMessageTextBlock.Text = "Réinitialisation réussie!";
                SuccessBorder.Visibility = Visibility.Visible;
                SuccessMessageTextBlock.Visibility = Visibility.Visible;

                // Déclencher l'événement ResetCompleted
                ResetCompleted?.Invoke(this, EventArgs.Empty);

                // Réinitialiser le contrôle après une courte attente
                Task.Delay(1000).ContinueWith(t =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        // Ici, tu peux masquer le contrôle
                        this.Visibility = Visibility.Collapsed;
                    });
                });
            }
        }

        private async void FadeOutAndShowLoginPanel()
        {
            // Simulation de la mise à jour du password
            await Task.Delay(1000); // Attendre 1000 millisecondes (1 seconde)
            // Afficher le message de succès
            SuccessMessageTextBlock.Text = "Mot de passe changé avec succès !";
            SuccessBorder.Visibility = Visibility.Visible;
            SuccessMessageTextBlock.Visibility = Visibility.Visible;

            // Attendre un moment pour permettre à l'utilisateur de voir le message
            await Task.Delay(1000); // Attendre 1000 millisecondes (1 seconde)

            // After a short delay, transition to SignInPanel
            var transitionStoryboard = (Storyboard)Application.Current.MainWindow.FindResource("TransitionToSignIn");
            transitionStoryboard.Begin();
        }



        private static void ShowError(string message, Border errorBorder, TextBlock errorTextBlock)
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
                    PasswordErrorTextBlock,
                    ConfirmNewPasswordErrorTextBlock
                };

            var errorBorders = new[]
                    {
                        PasswordErrorBorder,
                        ConfirmNewPasswordErrorBorder
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
    }
}
