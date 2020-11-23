using ApproxiMate.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file; 

        public Profile()
        {
            InitializeComponent();
            BindingContext = new UsersViewModel();
        }

        //private async void SelectPhoto(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();
        //    try
        //    {
        //        file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
        //        {
        //            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
        //        });
        //        if (file == null)
        //            return;
        //        photoImage.Source = ImageSource.FromStream(() =>
        //        {
        //            var imageStream = file.GetStream();
        //            return imageStream;
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        //private async void Upload(object sender, EventArgs e)
        //{
        //    var url = await firebaseStorageHelper.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
        //}
    }
}