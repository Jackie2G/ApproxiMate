using ApproxiMate.Models;
using Rg.Plugins.Popup.Services;
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
    public partial class Messages : ContentPage
    {
        IAuth auth;
        public Messages()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
            auth = DependencyService.Get<IAuth>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            var item = btn.BindingContext as User;
            var test = await auth.GetUserProfile();
            //Application.Current.MainPage = new Chat(test, item.Id);
            PopupNavigation.Instance.PushAsync(new MyPopupPage(test, item.Id));

        }
    }
}