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
    public partial class RegisterPage : ContentPage
    {
        IAuth auth;
        public RegisterPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private async void SignUp(object sender, EventArgs e)
        {
            var user = auth.SignUpWithEmailAndPassword(EmailInput.Text, PasswordInput.Text);

            if (user != null)
            {
                await DisplayAlert("Success", "New user created", "Ok");

                var signOut = auth.SignOut();

                if (signOut)
                {
                    Application.Current.MainPage = new MainPage();
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, please try again", "Ok");
                }
            }
        }
    }
}