﻿using Android.App;
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

[assembly: Dependency(typeof(AuthDroid))]
namespace ApproxiMate.Droid
{
    class AuthDroid : IAuth
    {
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
                var token = newUser.User.GetIdToken(false);
                return token.ToString();
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
                var token = newUser.User.GetIdToken(false);
                return token.ToString();
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
    }
}