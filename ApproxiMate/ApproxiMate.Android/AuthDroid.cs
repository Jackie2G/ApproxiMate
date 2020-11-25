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

[assembly: Dependency(typeof(AuthDroid))]
namespace ApproxiMate.Droid
{
    class AuthDroid : IAuth
    {
        FirebaseClient client = new FirebaseClient("https://approximatefirebase.firebaseio.com/");
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
            User u = new User() { Name = name, Age = age, City = city, Description = description, Gender = gender, OppositeGender = oppositeGender, ImageUrl = imageUrl };
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
    }
}