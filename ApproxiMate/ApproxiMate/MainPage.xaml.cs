using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApproxiMate
{
    public partial class MainPage : ContentPage
    {
        IAuth auth;
        public MainPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            string token = await auth.LoginWithEmailAndPassoword(EmailInput.Text, PasswordInput.Text);

            if (token != string.Empty)
            {
                await DisplayAlert("Uid", token, "Ok");
                Application.Current.MainPage = new LoggedPage();
            }
            else
            {
                await DisplayAlert("Authentication failed", "Email or password is incorrect", "Ok");
            }
        }

        private void SignUpClicked(object sender, EventArgs e)
        {
            var signOut = auth.SignOut();
            
            if (signOut)
            {
                Application.Current.MainPage = new RegisterPage();
            }
        }
    }
}
