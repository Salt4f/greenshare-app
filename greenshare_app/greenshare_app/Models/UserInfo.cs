using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace greenshare_app.Models
{
    public class UserInfo
    {
        public string Dni { get; set; }
        public string FullName { get; set; }
        public string description { get; set; }
        public Image ProfilePicture { get; set; }
        public bool isBanned { get; set; }
        public int TotalEcoPoints { get; set; }
        public int TotalGreenCoins { get; set; }
        public int AverageValoration { get; set; }


    }
}
