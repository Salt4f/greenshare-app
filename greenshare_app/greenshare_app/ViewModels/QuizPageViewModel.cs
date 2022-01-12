using greenshare_app.Models;
using greenshare_app.Utils;
using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Essentials;
//using Command = MvvmHelpers.Commands.Command;
//using System.IO;

namespace greenshare_app.ViewModels
{
    class QuizPagePostViewModel : BaseViewModel
    {
        private event EventHandler Starting = delegate { };
        public QuizPagePostViewModel(INavigation navigation, Page view)
        {
            this.navigation = navigation;
            this.view = view;
            IsBusy = true;
            Starting += OnStart;
            Starting(this, EventArgs.Empty);
        }

        private ObservableRangeCollection<QuizQuestion> ecoQuiz;

        private INavigation navigation;
        private Page view;

        private string pregunta;
        private string respuesta1;
        private string respuesta2;
        private string respuesta3;
        private string respuesta4;
        private int iterator;

        //private bool isNotFirst;
        private bool isNotLast;
        private bool isFirst;
        private bool isLast;
        private bool isRespuesta2Checked;
        private bool isRespuesta1Checked;
        private bool isRespuesta3Checked;
        private bool isRespuesta4Checked;
        private List<int> responses;
        private async void OnStart(object sender, EventArgs args)
        {
            await navigation.PopModalAsync();
            responses = new List<int>()
            { 0,0,0,0,0,0,0,0 };
            try
            {
                var questions = await ThirdPartyServiceUtil.Instance().GetEcoQuiz();
                ecoQuiz = new ObservableRangeCollection<QuizQuestion>();
                ecoQuiz.AddRange(questions);
                if (ecoQuiz.Count == 0) await view.DisplayAlert("error while retrieving questions", "error while sending request to backend", "OK");
                InitQuestions();
            }
            catch (Exception)
            {
                await view.DisplayAlert("error while retrieving questions", "something went wrong", "OK");
                IsBusy = false;
            }
        }

        private void InitQuestions()
        {
            //IsNotFirst = false;
            IsNotLast = true;
            IsFirst = true;
            IsLast = false;
            Iterator = 1;
            SetQuestions();
        }

        private void SetQuestions()
        {
            Pregunta = ecoQuiz[Iterator - 1].Question;
            Respuesta1 = ecoQuiz[Iterator - 1].Response1;
            Respuesta2 = ecoQuiz[Iterator - 1].Response2;
            Respuesta3 = ecoQuiz[Iterator - 1].Response3;
            if (Iterator == 1) Respuesta4 = ecoQuiz[Iterator - 1].Response4;
            IsBusy = false;
        }
        private void SetResponses()
        {
            if (IsRespuesta1Checked)
            {
                responses[Iterator - 1] = 1;
                IsRespuesta1Checked = false;
            }
            else if (IsRespuesta2Checked)
            {
                responses[Iterator - 1] = 2;
                IsRespuesta2Checked = false;
            }
            else if (IsRespuesta3Checked)
            {
                responses[Iterator - 1] = 3;
                IsRespuesta3Checked = false;
            }
            else if (IsRespuesta4Checked)
            {
                responses[Iterator - 1] = 4;
                IsRespuesta4Checked = false;
            }            
        }

        public string Pregunta
        {
            get => pregunta;
            set => SetProperty(ref pregunta, value);
        }

        public string Respuesta1
        {
            get => respuesta1;
            set => SetProperty(ref respuesta1, value);
        }

        public string Respuesta2
        {
            get => respuesta2;
            set => SetProperty(ref respuesta2, value);
        }

        public string Respuesta3
        {
            get => respuesta3;
            set => SetProperty(ref respuesta3, value);
        }

        public string Respuesta4
        {
            get => respuesta4;
            set => SetProperty(ref respuesta4, value);
        }
        public bool IsRespuesta1Checked
        {
            get => isRespuesta1Checked;
            set => SetProperty(ref isRespuesta1Checked, value);
        }
        public bool IsRespuesta2Checked
        {
            get => isRespuesta2Checked;
            set => SetProperty(ref isRespuesta2Checked, value);
        }
        public bool IsRespuesta3Checked
        {
            get => isRespuesta3Checked;
            set => SetProperty(ref isRespuesta3Checked, value);
        }
        public bool IsRespuesta4Checked
        {
            get => isRespuesta4Checked;
            set => SetProperty(ref isRespuesta4Checked, value);
        }
        /*
        public bool IsNotFirst
        {
            get => isNotFirst;
            set => SetProperty(ref isNotFirst, value);
        }
        */
        public bool IsFirst
        {
            get => isFirst;
            set => SetProperty(ref isFirst, value);
        }
        public bool IsLast
        {
            get => isLast;
            set => SetProperty(ref isLast, value);
        }
        public bool IsNotLast
        {
            get => isNotLast;
            set => SetProperty(ref isNotLast, value);
        }
        public int Iterator
        {
            get => iterator;
            set => SetProperty(ref iterator, value);
        }
        public AsyncCommand OnNextButtonCommand => new AsyncCommand(OnNext);
        //public AsyncCommand OnPreviousButtonCommand => new AsyncCommand(OnPrevious);
        public AsyncCommand OnFinishButtonCommand => new AsyncCommand(OnFinish);

        private async Task OnNext()
        {
            IsBusy = true;
            if (IsRespuesta1Checked || IsRespuesta2Checked || IsRespuesta3Checked || IsRespuesta4Checked)
            {
                SetResponses();
                if (Iterator < 8)
                {
                    Iterator += 1;
                    //IsNotFirst = true;
                    IsFirst = false;
                    if (Iterator == 8)
                    {
                        IsLast = true;
                        IsNotLast = false;
                    }
                }
                SetQuestions();
            }
            else await view.DisplayAlert("Please select an option first","All questions must be answered", "OK");
            IsBusy = false;
        }
        /*
        private async Task OnPrevious()
        {
            if (Iterator > 1)
            {
                Iterator -= 1;
                IsLast = false;
                IsNotLast = true;
                if (Iterator == 1)
                {
                    IsFirst = true;
                    IsNotFirst = false;
                }
            }
            SetQuestions();
        }
        */

        private async Task OnFinish()
        {
            IsBusy = true;
            //IsNotFirst = false;
            if (IsRespuesta1Checked || IsRespuesta2Checked || IsRespuesta3Checked || IsRespuesta4Checked)
            {
                SetResponses();
                if (await ThirdPartyServiceUtil.Instance().PostEcoQuizResults(responses))
                {
                    Application.Current.MainPage = new MainView();
                }
                else
                {
                    await view.DisplayAlert("error while sending responses", "please try again later", "OK");
                    IsFirst = true;
                    IsLast = false;
                    IsNotLast = true;
                    Iterator = 1;
                    responses = new List<int>();
                    SetQuestions();
                    IsBusy = false;
                }
            }
            else await view.DisplayAlert("Please select an option first", "All questions must be answered", "OK");
            IsBusy = false;
        }
    }
}