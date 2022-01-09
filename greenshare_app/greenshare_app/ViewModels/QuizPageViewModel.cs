//using greenshare_app.Models;
//using greenshare_app.Utils;
//using greenshare_app.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
//using System;
//using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Xamarin.Essentials;
//using Command = MvvmHelpers.Commands.Command;
//using System.IO;

namespace greenshare_app.ViewModels
{
    class QuizPagePostViewModel : BaseViewModel
    {
        public QuizPagePostViewModel(INavigation navigation, Page view)
        {
            this.navigation = navigation;
            this.view = view;
            this.pregunta = "¿Con qué frecuencia consumes productos de origen animal (incluyendo huevo, productos lácteos, pescado y similares) ?";
            this.respuesta1 = "Mi dieta incluye productos de origen animal en prácticamente todas mis comidas.";
            this.respuesta2 = "Sigo el modelo de dieta mediterránea, o soy vegetariano con algunas excepciones de vez en cuando.";
            this.respuesta3 = "Soy vegetariano (entendiendo como vegetariano aquel que no consume carne ni pescado, pero si huevos y productos lácteos).";
            this.respuesta4 = "No consumo nunca productos de origen animal, soy vegano.";
            this.isNotFirst = false;
            this.isNotLast = true;
            this.isFirst = true;
            this.isLast = false;
            this.iterator = 1;
        }

        private INavigation navigation;
        private Page view;

        private string pregunta;
        private string respuesta1;
        private string respuesta2;
        private string respuesta3;
        private string respuesta4;
        private int iterator;

        private bool isNotFirst;
        private bool isNotLast;
        private bool isFirst;
        private bool isLast;

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
        public bool IsNotFirst
        {
            get => isNotFirst;
            set => SetProperty(ref isNotFirst, value);
        }
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
        public AsyncCommand OnPreviousButtonCommand => new AsyncCommand(OnPrevious);
        public AsyncCommand OnFinishButtonCommand => new AsyncCommand(OnFinish);

        private async Task OnNext()
        {
            if (Iterator < 8)
            {
                Iterator += 1;
                IsNotFirst = true;
                IsFirst = false;
                if (Iterator == 8)
                {
                    IsLast = true;
                    IsNotLast = false;
                }
            }
            //TODO PERE: assignar preguntes y respostes iterator-essimes a les variables corresponents

        }
        private async Task OnPrevious()
        {
            if (Iterator > 1)
            {
                Iterator -= 1;
                IsLast = false;
                IsNotLast = true;
                if(Iterator == 1)
                {
                    IsFirst = true;
                    IsNotFirst = false;
                }
            }
            //TODO PERE: assignar preguntes y respostes iterator-essimes a les variables corresponents 


        }
        private async Task OnFinish()
        {
            IsNotFirst = false;
            IsFirst = true;
            IsLast = false;
            IsNotLast = true;
            Iterator = 1;
            //TODO PERE: cridar API

        }


    }
}