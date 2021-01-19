using ApproxiMate.Models;
using ApproxiMate.Services;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
//using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ApproxiMate.Views
{
    public class UsersViewModel: BaseViewModel
    {
        IAuth auth;
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }
        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        private string _gender;
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                OnPropertyChanged();
            }
        }
        private string _oppositeGender;
        public string OppositeGender
        {
            get
            {
                return _oppositeGender;
            }
            set
            {
                _oppositeGender = value;
                OnPropertyChanged();
            }
        }
        private string _imageUrl;
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }
        public string UserMessage { get; set; }
        private ImageSource _photoImage;
        public User testUser;

        public ImageSource photoImage
        {
            get 
            {
                return _photoImage;
            }
            set
            {
                _photoImage = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _photo;

        public ImageSource Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged();
            }
        }

       // public ImageSource photoImage { get; set; }

        private DBFirebase _services;
        private FirebaseStorageHelper _firebaseStorageHelper;
        MediaFile file;

        public Command AddUserCommand { get; }
        public Command SelectPhotoCommand { get; }
        public Command UploadPhotoCommand { get; }
        public Command RefreshMessages { get; }

        private ObservableCollection<User> _users = new ObservableCollection<User>();

        private ObservableCollection<User> _userProfile = new ObservableCollection<User>();
        private ObservableCollection<User> _messages = new ObservableCollection<User>();

        public ObservableCollection<User> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<User> UserProfile
        {
            get { return _userProfile; }
            set
            {
                _userProfile = value;
                //DownloadPhoto();
                OnPropertyChanged();
            }
        }

        public UsersViewModel()
        {
            auth = DependencyService.Get<IAuth>();
            _firebaseStorageHelper = new FirebaseStorageHelper();
            _services = new DBFirebase();
            AddUserCommand = new Command(async () => await auth.AddUser(Name, Age, City, Description, Gender, OppositeGender, ImageUrl = await UploadPhoto(ImageUrl)));
            RefreshMessages = new Command(async () => await RefreshUserMessages());
            DownloadPhoto();
            
            SelectPhotoCommand = new Command(async () => await SelectPhoto());
            var test = auth.GetUserProfile();
        }

        public async Task SelectPhoto()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large
                });
                if (file == null)
                    return;
                _photoImage = ImageSource.FromStream(() =>
                {
                    var imageStream = file.GetStream();
                    return imageStream;
                });
                photoImage = _photoImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task<string> UploadPhoto(string url)
        {
            string newUrl;
            if (url != null && file == null)
            {
                return ImageUrl;
            }
            else
            {
                newUrl = await _firebaseStorageHelper.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
                ImageUrl = newUrl;
                return ImageUrl;
            }         
        }

        public async Task RefreshUserMessages()
        {
            Messages = await auth.GetUserMessages();
        }

        public async Task DownloadPhoto()
        {
            Users = await auth.GetUsersDisplay();
            var list = new List<User>(UserProfile);
            testUser = await auth.GetUserProfile();
            if (testUser != null)
                ReturnUserData(testUser);
            var testowo = new ObservableCollection<User>();
            testowo.Add(testUser);
            UserProfile = testowo;
            Messages = await auth.GetUserMessages();
            var test = testUser.ImageUrl;
            _photo = ImageSource.FromUri(new System.Uri(test));
            Photo = _photo;
        }

        public void ReturnUserData(User user)
        {
            photoImage = user.ImageUrl;
            Name = user.Name;
            Age = user.Age;
            Gender = user.Gender;
            OppositeGender = user.OppositeGender;
            City = user.City;
            Description = user.Description;
            ImageUrl = user.ImageUrl;
        }
    } 
}