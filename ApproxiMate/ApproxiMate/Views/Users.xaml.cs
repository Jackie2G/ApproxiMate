using ApproxiMate.Models;
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
    public partial class Users : ContentPage
    {
        IAuth auth;

        public Users()
        {
            auth = DependencyService.Get<IAuth>();
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

        private void nopeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                SwipeView1.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
                auth.AddHateUser(((User)SwipeView1.TopItem).Id);
            }
            catch
            {
                DisplayAlert("Alert", "User doesn't exists", "Ok");
            }

        }

        private void likeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                SwipeView1.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
                auth.AddLoveUser(((User)SwipeView1.TopItem).Id);
            }
            catch
            {
                DisplayAlert("Alert", "User doesn't exists", "Ok");
            }
        }
    }
}