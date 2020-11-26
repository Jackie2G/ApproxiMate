using System;
using System.Collections.Generic;
using System.Text;

namespace ApproxiMate.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string Gender { get; set; }
        public string OppositeGender { get; set; }
        public string ImageUrl { get; set; }
    }
}
