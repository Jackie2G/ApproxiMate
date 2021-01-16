using ApproxiMate.iOS;
using Foundation;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Firebase.Auth;
using System.Collections.ObjectModel;

[assembly: Dependency(typeof(AuthIOS))]
namespace ApproxiMate.iOS
{
    public class AuthIOS : IAuth
    {
        public AuthIOS()
        {

        }

        public Task AddHateUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task AddLoveUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task AddUser(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMatches()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Models.User> GetUser()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Models.User>> GetUserMessages(ObservableCollection<Models.User> users)
        {
            throw new NotImplementedException();
        }

        public Task<Models.User> GetUserProfile()
        {
            throw new NotImplementedException();
        }

        public bool IsSignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassoword(string email, string password)
        {
            try
            {
                var newUser = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await newUser.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public Task SendMessage(string userMessage, string id)
        {
            throw new NotImplementedException();
        }

        public bool SignOut()
        {
            try
            {
                _ = Auth.DefaultInstance.SignOut(out NSError error);
                return error == null;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser = await Auth.DefaultInstance.CreateUserAsync(email, password);
                return await newUser.User.GetIdTokenAsync();
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }

    }
}