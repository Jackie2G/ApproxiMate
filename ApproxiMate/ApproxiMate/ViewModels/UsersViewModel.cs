using ApproxiMate.Models;
using ApproxiMate.Services;
using MvvmHelpers;
//using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
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
        public string ImageUrl { get; set; } = "test";

        private DBFirebase _services;

        public Command AddUserCommand { get; }

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
            _services = new DBFirebase();
            Users = _services.getUsers();
            AddUserCommand = new Command(async () => await AddStudentAsync(Name, Age, City, Description, Gender, OppositeGender, ImageUrl));
        }

        public async Task AddStudentAsync(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            await _services.AddUser(name, age, city, description, gender, oppositeGender, imageUrl);
        }
    }
}