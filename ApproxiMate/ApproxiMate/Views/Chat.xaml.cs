﻿using ApproxiMate.Models;
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
    public partial class Chat : ContentPage
    {
        IAuth auth;

        public Chat(User user, string id)
        {
            BindingContext = this;
            Messages = user.PairedList.Where(x => x.Id == id).FirstOrDefault().Messages;
            Id = user.Id;
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }
        
        public List<string> Messages { get; set; }
        public string UserMessage { get; set; }
        public string Id { get; set; }

        private void Button_Clicked(object sender, EventArgs e)
        {
            auth.SendMessage(UserMessage, Id);

            
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new Messages();
            Application.Current.MainPage = new Messages();
        }
    }
}