using ApproxiMate.Models;
using ApproxiMate.Services;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
//using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ApproxiMate.Views
{
    public class UsersViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string OppositeGender { get; set; }
        public string ImageUrl { get; set; }
        public ImageSource photoImage { get; set; }

        private DBFirebase _services;
        private FirebaseStorageHelper _firebaseStorageHelper;
        MediaFile file;

        public Command AddUserCommand { get; }
        public Command SelectPhotoCommand { get; }
        public Command UploadPhotoCommand { get; }

        private ObservableCollection<User> _users = new ObservableCollection<User>();

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        public UsersViewModel()
        {
            _firebaseStorageHelper = new FirebaseStorageHelper();
            _services = new DBFirebase();
            Users = _services.getUsers();
            AddUserCommand = new Command(async () => await AddStudentAsync(Name, Age, City, Description, Gender, OppositeGender, ImageUrl));
            SelectPhotoCommand = new Command(async () => await SelectPhoto());
            UploadPhotoCommand = new Command(async () => await UploadPhoto());
        }

        public async Task SelectPhoto()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                photoImage = ImageSource.FromStream(() =>
                {
                    var imageStream = file.GetStream();
                    return imageStream;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task AddStudentAsync(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            await _services.AddUser(name, age, city, description, gender, oppositeGender, imageUrl);
        }

        public async Task UploadPhoto()
        {
            var test = await _firebaseStorageHelper.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            ImageUrl = test;
        }
    }
}