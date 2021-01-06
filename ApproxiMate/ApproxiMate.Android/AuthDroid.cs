using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;
using System.Reflection;
using ApproxiMate.Droid;
using ApproxiMate.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using Java.Util;

[assembly: Dependency(typeof(AuthDroid))]
namespace ApproxiMate.Droid
{
    class AuthDroid : IAuth
    {
        FirebaseClient client = new FirebaseClient("https://approximatefirebase.firebaseio.com/");
        User user;

        public AuthDroid()
        {

        }

        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassoword(string email, string password)
        {
            try
            {
                var newUser = await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
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
                var newUser = await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.Uid;
                return token;
            }
            catch(FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch(FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public async Task AddUser(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            User u = new User() { Name = name, Age = age, City = city, Description = description, Gender = gender, OppositeGender = oppositeGender, ImageUrl = imageUrl, Id = FirebaseAuth.Instance.CurrentUser.Uid, LoveList = new List<string>() { "empty" }, HateList = new List<string>() { "empty" } };
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            //await client
            //    .Child("Users")
            //    .Child(uuid)
            //    .PutAsync<User>(u);

            await client
                .Child("Users")
                .Child(token)
                .PutAsync(u);
        }

        public ObservableCollection<User> GetUser()
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var userData = client.
                Child("Users")
                .Child(token)
                .AsObservable<User>()
                .AsObservableCollection();

            //var test = userData.Where(p => p.Id.Equals(token));

            return userData;
        }

        public async Task <User> GetUserProfile()
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var userData = (await client.
                Child("Users")
                .Child(token)
                .OnceSingleAsync<User>());

            return userData;

        }

        public async Task AddLoveUser(string id)
        {
            HashMap loveMap = new HashMap();

            loveMap.Put(0, id);

            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var user = await GetUserProfile();

            user.LoveList.Add(id);
            if (user.PairedList == null)
                user.PairedList = new List<UserMessages>();

            user.PairedList.Add(new UserMessages() { Id = id, Messages = new List<string>() { "cos" } });

            await client.
                Child("Users")
                .Child(token)
                .PutAsync(user);

        }

        public async Task AddHateUser(string id)
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var user = await GetUserProfile();

            user.HateList.Add(id);

            await client.
                Child("Users")
                .Child(token)
                .PutAsync(user);
        }

        public ObservableCollection<string> GetUserMessages()
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var messages =   client
                                .Child("Users")
                                .Child(token)
                                .Child("PairedList")
                                .Child("IbGI3bPBtbSpV7qiCZmOY0tRce02")
                                .AsObservable<string>()
                                .AsObservableCollection();

            return messages;
        }

        public async Task SendMessage(string userMessage, string id)
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var user = await GetUserProfile();

            if (user.PairedList == null)
                user.PairedList = new List<UserMessages>();

            user.PairedList.Where(x => x.Id == id).FirstOrDefault().Messages.Add(user.Name + ": " + userMessage);

            await client.
                Child("Users")
                .Child(token)
                .PutAsync(user);
        }
    }
}