using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMate
{
    public interface IAuth
    {
        Task<string> LoginWithEmailAndPassoword(string email, string password);
        Task<string> SignUpWithEmailAndPassword(string email, string password);
        Task AddUser(string name, int age, string city, string description, string gender, string oppositeGender, string imageUrl);
        bool SignOut();
        bool IsSignIn();
    }
}
