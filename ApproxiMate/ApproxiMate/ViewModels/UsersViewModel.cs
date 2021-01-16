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
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string OppositeGender { get; set; }
        public string ImageUrl { get; set; }
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
            Users = _services.getUsers();
            //Messages = auth.GetUserMessages();
            //AddUserCommand = new Command(async () => await AddStudentAsync(Name, Age, City, Description, Gender, OppositeGender, ImageUrl));
            AddUserCommand = new Command(async () => await auth.AddUser(Name, Age, City, Description, Gender, OppositeGender, ImageUrl));
            //UserProfile = auth.GetUser();
            DownloadPhoto();
            
            SelectPhotoCommand = new Command(async () => await SelectPhoto());
            UploadPhotoCommand = new Command(async () => await UploadPhoto());
            var test = auth.GetUserProfile();
            //if (UserProfile.Count != 0) 
                //DownloadPhoto();
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

        public async Task AddStudentAsync(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            await _services.AddUser(name, age, city, description, gender, oppositeGender, imageUrl);
        }

        public async Task UploadPhoto()
        {
            var test = await _firebaseStorageHelper.UploadFile(file.GetStream(), Path.GetFileName(file.Path));
            ImageUrl = test;
        }

        public async Task DownloadPhoto()
        {
            var list = new List<User>(UserProfile);
            testUser = await auth.GetUserProfile();
            //var list1 = Users.Where(p => testUser.PairedList.Any(p2 => p2.Id == p.Id)).ToList();
            //var userMessages = new ObservableCollection<User>();
            var testowo = new ObservableCollection<User>();
            testowo.Add(testUser);
            //foreach (var item in list1)
            //    userMessages.Add(item);
            UserProfile = testowo;
            Messages = await auth.GetUserMessages(Users);
            //var test = await _firebaseStorageHelper.GetFile("https://firebasestorage.googleapis.com/v0/b/approximatefirebase.appspot.com/o/UserPhotos%2FIMG_20200805_192157.jpg?alt=media&token=b020a653-88f3-49b5-b862-b4b59a501e53");
            var test = testUser.ImageUrl;
            _photo = ImageSource.FromUri(new System.Uri(test));
            Photo = _photo;
        }
    } 
}