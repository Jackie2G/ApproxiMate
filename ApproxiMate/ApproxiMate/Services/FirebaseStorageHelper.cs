using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ApproxiMate.Services
{
    public class FirebaseStorageHelper
    {
        FirebaseStorage firebaseStorage;

        public FirebaseStorageHelper()
        {
            firebaseStorage = new FirebaseStorage("approximatefirebase.appspot.com");
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("UserPhotos")
                .Child(fileName)
                .PutAsync(fileStream);

            return imageUrl;
        }

        public async Task<string> GetFile(string fileName)
        {
            var test = await firebaseStorage
                .Child("UserPhotos")
                .Child(fileName)
                .GetDownloadUrlAsync();

            return test;
        }
    }
}
