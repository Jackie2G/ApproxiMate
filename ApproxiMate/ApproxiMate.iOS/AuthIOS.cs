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

[assembly: Dependency(typeof(AuthIOS))]
namespace ApproxiMate.iOS
{
    public class AuthIOS : IAuth
    {
        public AuthIOS()
        {

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