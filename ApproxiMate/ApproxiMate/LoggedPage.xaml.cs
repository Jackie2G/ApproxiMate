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
    }
}