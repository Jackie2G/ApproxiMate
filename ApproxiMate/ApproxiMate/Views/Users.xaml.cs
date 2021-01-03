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
        public Users()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

        private void nopeButton_Clicked(object sender, EventArgs e)
        {
            SwipeView1.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
            Console.WriteLine(((User)SwipeView1.TopItem).Id);
        }

        private void likeButton_Clicked(object sender, EventArgs e)
        {
            SwipeView1.InvokeSwipe((MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection)MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
        }
    }
}