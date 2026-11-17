using SACCO.Models;
using SACCO.Services;
using SACCO.Models;
using SACCO.Services;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SACCO
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            Loaded += (s, e) => EmailTextBox.Focus(FocusState.Programmatic);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await SignInAsync();
        }

        private async void Input_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                await SignInAsync();
            }
        }

        private async Task SignInAsync()
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            // 1. Validate empty fields
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            {
                await DialogHelper.ShowAsync("Login Message",
                    "Please enter both your email address and password.");
                return;
            }

            LoginButton.IsEnabled = false;   // prevent double clicks

            Staff staff = null;
            try
            {
                // 2. Ask the database
                staff = await StaffRepository.AuthenticateAsync(email, password);
            }
            catch (SqlException ex)
            {
                await DialogHelper.ShowAsync("Login Message",
                    "Could not connect to the database. " + ex.Message);
                LoginButton.IsEnabled = true;
                return;
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowAsync("Login Message", "Unexpected error: " + ex.Message);
                LoginButton.IsEnabled = true;
                return;
            }

            LoginButton.IsEnabled = true;

            // 3a. Wrong email/password
            if (staff == null)
            {
                await DialogHelper.ShowAsync("Login Message", "Login Failed. Invalid email or password.");
                PasswordBox.Password = string.Empty;
                PasswordBox.Focus(FocusState.Programmatic);
                return;
            }

            // 3b. Success
            await DialogHelper.ShowAsync("Login Message", "Login Successful. Welcome " + staff.FullName + ".");
            Frame.Navigate(typeof(HomePage), staff);   // pass staff to the next page
            Frame.BackStack.Clear();                   // Back can't return to login
        }
    }
}