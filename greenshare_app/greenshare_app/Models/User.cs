using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class User
    {     
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Description { get; set; }
        public Image ProfilePicture { get; set; }
        public bool Banned { get; set; }
        public int TotalEcoPoints { get; set; }
        public int TotalGreenCoins { get; set; }
        public double AverageValoration { get; set; }


    }
}
