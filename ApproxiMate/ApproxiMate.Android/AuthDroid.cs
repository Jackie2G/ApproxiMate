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

        //public ObservableCollection<string> GetUserMessages()
        //{
        //    var uuid = FirebaseAuth.Instance.CurrentUser;
        //    var token = uuid.Uid;

        //    var messages =   client
        //                        .Child("Users")
        //                        .Child(token)
        //                        .Child("PairedList")
        //                        .AsObservable<string>()
        //                        .AsObservableCollection();

        //    return messages;
        //}

        public async Task SendMessage(string userMessage, string id)
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var user = await GetUserProfile();
            var userTo = await GetUserSend(id);

            if (user.PairedList == null)
                user.PairedList = new List<UserMessages>();

            user.PairedList.Where(x => x.Id == id).FirstOrDefault().Messages.Add("You" + ": " + userMessage);
            userTo.PairedList.Where(x => x.Id == user.Id).FirstOrDefault().Messages.Add(user.Name + ": " + userMessage);

            await client.
                Child("Users")
                .Child(token)
                .PutAsync(user);

            await client
                .Child("Users")
                .Child(id)
                .PutAsync(userTo);
        }

        public async Task <ObservableCollection<User>> GetUserMessages(ObservableCollection<User> users)
        {
            var uuid = FirebaseAuth.Instance.CurrentUser;
            var token = uuid.Uid;

            var user = await GetUserProfile();

            //var userData = client
            //    .Child("Users")
            //    .AsObservable<User>()
            //    .AsObservableCollection().ToList();

            var finaldata = users.Where(p => user.PairedList.Any(p2 => p2.Id == p.Id)).ToList();

            return new ObservableCollection<User>(finaldata);

        }

        public async Task<int> GetMatches()
        {
            var user = await GetUserProfile();

            int counter = 0;

            var usersData = await client
                .Child("Users")
                .OnceAsync<User>();

            foreach(var item in usersData)
            {
                for (int i = 0; i < item.Object.LoveList.Count(); i++)
                {
                    for (int j = 0; j < user.LoveList.Count(); j++)
                    {
                        if (user.LoveList[j] == item.Object.Id && item.Object.LoveList[i] == user.Id)
                        {
                            if (user.PairedList == null)
                                user.PairedList = new List<UserMessages>();

                            else if (item.Object.PairedList == null)
                                item.Object.PairedList = new List<UserMessages>();

                            bool userContains = false;
                            bool itemContains = false;

                            foreach(var it in user.PairedList)
                            {
                                if (it.Id == item.Object.Id)
                                    userContains = true;
                            }

                            foreach(var itt in item.Object.PairedList)
                            {
                                if (itt.Id == user.Id)
                                    itemContains = true;
                            }

                            if (userContains == false)
                            {
                                user.PairedList.Add(new UserMessages() { Id = item.Object.Id, Messages = new List<string>() { "" } });
                                counter++;
                            }

                            if (itemContains == false)
                            {
                                item.Object.PairedList.Add(new UserMessages() { Id = user.Id, Messages = new List<string>() { "" } });

                                await client.
                                 Child("Users")
                                .Child(item.Object.Id)
                                .PutAsync(item.Object);
                            }
                        }
                    }
                }
            }

            await client.
                Child("Users")
                .Child(user.Id)
                .PutAsync(user);

            return counter;
        }

        public async Task<User> GetUserSend(string id)
        {
            var userData = (await client.
                Child("Users")
                .Child(id)
                .OnceSingleAsync<User>());

            return userData;
        }
    }
}