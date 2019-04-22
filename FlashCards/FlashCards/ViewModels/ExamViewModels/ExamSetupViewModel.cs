using FlashCards.Models;
using FlashCards.Views.Exam;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.ViewModels.ExamViewModels
{
    public class ExamSetupViewModel : BaseViewModel
    {
        private Bundle bundle;


        public ExamSetupViewModel(Bundle bundle)
        {
            this.bundle = bundle;
            if (bundle.CardType == CardType.Basic)
                Maximum = App.Database.GetCardsFromBundle(bundle.Id).Result.Count;
            else Maximum = App.Database.GetPhotoCardsFromBundle(bundle.Id).Result.Count;

            Minimum = 0;
            Value = Maximum;
            Seconds = 5;

        }


        private int _minimum;
        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
                OnPropertyChanged();
                
            }
        }

        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged();
            }

        }
        private int _maximum;
        public int Maximum
        {
            get { return _maximum; }
            set {
                _maximum = value;
                OnPropertyChanged();
            }
        }

        private int _seconds;
        public int Seconds
        {
            get { return _seconds; }
            set {
                _seconds = value;
                OnPropertyChanged();
                OnPropertyChanged("SecondsText");
            }
        }

        
        public string SecondsText
        {
            get
            {
                if (_seconds == 4)
                    return "Unlimited";
                else return _seconds.ToString();
            }
            set
            {
                if (SecondsText != value)
                {
                    SecondsText = value;
                    OnPropertyChanged();
                }
            }
        }

        private Command _startCommand;

        public Command StartCommand
        {
            get
            {
                return _startCommand ?? (_startCommand = new Command(StartTest));
            }
        }

        private async void StartTest()
        {
            if (Value != 0)
            {
                int seconds;
                if (!int.TryParse(SecondsText, out seconds))
                {
                    seconds = -1;
                }
                if (bundle.CardType == CardType.Basic)
                    await Application.Current.MainPage.Navigation.PushAsync(new ExamPage()
                    {

                        BindingContext = new ExamViewModel(await App.Database.GetCardsFromBundle(bundle.Id), Value, seconds)
                    });
                else await Application.Current.MainPage.Navigation.PushAsync(new PhotoExamPage()
                {

                    BindingContext = new PhotoExamViewModel(await App.Database.GetPhotoCardsFromBundle(bundle.Id), Value, seconds)
                });
            }
        }



    }
}
