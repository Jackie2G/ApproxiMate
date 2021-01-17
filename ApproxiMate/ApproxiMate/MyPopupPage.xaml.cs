using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using ApproxiMate.Models;

namespace ApproxiMate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPopupPage
    {
        IAuth auth;
        public List<string> Messages { get; set; }
        public string UserMessage { get; set; }
        public string Id { get; set; }

        public MyPopupPage(User user, string id)
        {
            BindingContext = this;
            Messages = user.PairedList.Where(x => x.Id == id).FirstOrDefault().Messages;
            Id = id;
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            auth.SendMessage(UserMessage, Id);

            //UserMessage = string.Empty;
            entry.Text = string.Empty;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new Messages();
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}