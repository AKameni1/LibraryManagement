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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Arthur_Jayson_UA2
{
    /// <summary>
    /// Logique d'interaction pour UsernameResetControl.xaml
    /// </summary>
    public partial class UsernameResetControl : UserControl
    {
        public UsernameResetControl()
        {
            InitializeComponent();
        }

        public event EventHandler? ResetCompleted;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = NewUsernameTextBox.Text;

            // Validate the username
            if (string.IsNullOrEmpty(newUsername))
            {
                UsernameErrorTextBlock.Text = "Le nom d'utilisateur ne peut pas être vide.";
                UsernameErrorBorder.Visibility = Visibility.Visible;
                UsernameErrorTextBlock.Visibility = Visibility.Visible;
                SuccessBorder.Visibility = Visibility.Collapsed;
                SuccessMessageTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Logic to change the username here

                // If the change is successful
                SuccessMessageTextBlock.Text = "Nom d'utilisateur changé avec succès!";
                SuccessBorder.Visibility = Visibility.Visible;
                SuccessMessageTextBlock.Visibility = Visibility.Visible;

                // Start the display animation
                var storyboard = (Storyboard)FindResource("ShowSuccessMessageStoryboard");
                storyboard.Begin();

                UsernameErrorBorder.Visibility = Visibility.Collapsed;
                UsernameErrorTextBlock.Visibility = Visibility.Collapsed;

                // After a short delay, transition to SignInPanel
                var transitionStoryboard = (Storyboard)Application.Current.MainWindow.FindResource("TransitionToSignInStoryboard");
                transitionStoryboard.Begin();
            }

            //if (!hasError)
            //{
            //    // Si tout est bon, afficher le message de succès
            //    SuccessMessageTextBlock.Text = "Réinitialisation réussie!";
            //    SuccessBorder.Visibility = Visibility.Visible;
            //    SuccessMessageTextBlock.Visibility = Visibility.Visible;

            //    // Déclencher l'événement ResetCompleted
            //    ResetCompleted?.Invoke(this, EventArgs.Empty);

            //    // Réinitialiser le contrôle après une courte attente
            //    Task.Delay(1000).ContinueWith(t =>
            //    {
            //        Dispatcher.Invoke(() =>
            //        {
            //            // Ici, tu peux masquer le contrôle
            //            this.Visibility = Visibility.Collapsed;
            //        });
            //    });
            //}
        }
    }
}
