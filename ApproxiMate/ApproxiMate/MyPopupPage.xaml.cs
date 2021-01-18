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
using System.Threading;
using System.Collections.ObjectModel;

namespace ApproxiMate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyPopupPage
    {
        IAuth auth;
        private ObservableCollection<string> _messages = new ObservableCollection<string>();

        public ObservableCollection<string> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public string UserMessage { get; set; }
        public string Id { get; set; }
        private Thread _refreshMessages;
        private bool _stop = false;

        public MyPopupPage(User user, string id)
        {
            BindingContext = this;
            Messages = new ObservableCollection<string>(user.PairedList.Where(x => x.Id == id).FirstOrDefault().Messages);
            Id = id;
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();

            _refreshMessages = new Thread(async () => 
            {
                while(!_stop)
                {
                    Messages = new ObservableCollection<string>((await auth.GetUserProfile()).PairedList.Where(x => x.Id == id).FirstOrDefault().Messages);
                    Thread.Sleep(10000);
                }
            });

            _refreshMessages.Start();
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
            _stop = true;
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}