using ApproxiMate.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoggedPage : ContentPage
    {
        IAuth auth;
        public LoggedPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private void SignOut(object sender, EventArgs e)
        {
            var signOut = auth.SignOut();

            if (signOut)
            {
                Application.Current.MainPage = new MainPage();
            }
        }

        private void CreatePRrofile(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Profile();
        }

        private void ViewUsers(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Users();
        }

        private void ViewUserProfile(object sender, EventArgs e)
        {
            Application.Current.MainPage = new UserProfile();
        }
    }
}