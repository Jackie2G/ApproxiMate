using ApproxiMate.Views;
using Rg.Plugins.Popup.Services;
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
    public partial class ProfilePopup
    {

        public ProfilePopup()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

        private void MainMenu(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}