using SACCO;
using SACCO.Models;
using SACCO.Services;
using SACCO.Models;
using SACCO.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SACCO
{
    public sealed partial class HomePage : Page
    {
        private Staff _staff;

        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _staff = e.Parameter as Staff;

            if (_staff == null)
            {
                // Someone reached Home without logging in
                Loaded += (s, args) =>
                {
                    Frame.Navigate(typeof(LoginPage));
                    Frame.BackStack.Clear();
                };
                return;
            }

            WelcomeText.Text = "Welcome, " + _staff.FullName;
            RoleText.Text = "Logged in as " + _staff.Role;
        }

        // One handler for all 12 hub buttons
        private async void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            await DialogHelper.ShowAsync("Kampala Community SACCO", button.Content + " selected.");
        }

        // Home / Refresh / Print / Settings in the top bar
        private async void TopBarButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (AppBarButton)sender;
            await DialogHelper.ShowAsync("Kampala Community SACCO", button.Label + " selected.");
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await DialogHelper.ShowAsync("Logout",
                "Dear " + _staff.FullName + ", you have successfully logged out.");

            Frame.Navigate(typeof(LoginPage));
            Frame.BackStack.Clear();
        }
    }
}