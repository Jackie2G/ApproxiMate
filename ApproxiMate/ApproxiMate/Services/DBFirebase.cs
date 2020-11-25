using ApproxiMate.Models;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;

namespace ApproxiMate.Services
{
    public class DBFirebase
    {
        FirebaseClient client;

        public DBFirebase()
        {
            client = new FirebaseClient("https://approximatefirebase.firebaseio.com/");
        }

        public ObservableCollection<User> getUsers()
        {

            var userData = client.
                Child("Users")
                .AsObservable<User>()
                .AsObservableCollection();

            //if (userData.Count == 0)
            //{
            //    var user = new User() { Name = "Imie", Age = 20, City = "Katowice", Description = "opis" };
            //    userData = new ObservableCollection<User>();
            //    return userData;
            //}
            return userData;
        }

        public async Task AddUser(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl)
        {
            User u = new User() { Name = name, Age = age, City = city, Description = description, Gender = gender, OppositeGender = oppositeGender, ImageUrl = imageUrl };

            await client
                .Child("Users")
                .PostAsync(u);
        }
    }
}
