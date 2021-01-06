using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Options : ContentPage
    {
        IAuth auth;

        public Options()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private void SignOut(object sender, EventArgs e)
        {
            var signout = auth.SignOut();

            if (signout)
            {
                Application.Current.MainPage = new MainPage();
            }
        }

        private void CreateProfile(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Profile();
        }
    }
}