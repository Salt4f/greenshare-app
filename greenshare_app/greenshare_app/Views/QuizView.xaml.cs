using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using greenshare_app.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace greenshare_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizView : ContentPage
    {
        public QuizView()
        {
            InitializeComponent();
            BindingContext = new QuizPagePostViewModel(Navigation, this);
        }
    }
}