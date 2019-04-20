using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlashCards.ViewModels.ExamViewModels
{
    public class ExamViewModel :BaseViewModel
    {
      
        private Stack<Card> _questions;
        private Card currentQuestion;

        public ExamViewModel(List<Card> cards, int nrOfQuestions)
        {
            Helpers.Shuffle(cards);
            _questions = new Stack<Card>(cards);
            while (_questions.Count > nrOfQuestions)
            {
                _questions.Pop();
            }


            SetQuestion();
        }

        private void SetQuestion()
        {
            currentQuestion = _questions.Pop();
            OnPropertyChanged("Information");
            UserAnswer = "";

        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public string Information
        {
            get
            {
                return currentQuestion.Information;
            }

        }

        private string _userAnswer;

        public string UserAnswer
        {
            get
            {
                return _userAnswer;
            }
            set
            {
                _userAnswer = value;
                OnPropertyChanged();
            }
        }

        private Command _submitAnswer;

        public Command SubmitAnswerCommand
        {
            get
            {
                return _submitAnswer ?? (_submitAnswer = new Command(CheckAnswer));
            }
        }



        private void CheckAnswer()
        {

            if (UserAnswer == currentQuestion.Answer)
            {
                Score++;
                // set image correct
            }
            else
            {
                //set image false async?
            }

            if (_questions.Count > 0)
            {
                SetQuestion();
            } else
            {
                ShowScoreAndLeave();
                Application.Current.MainPage.Navigation.PopAsync() ;
            }


        }

        private async void ShowScoreAndLeave()
        {
            var message = new Label { Text = $"Your score is : {Score}" };

            Button button = new Button { Text = "Go back" };

            button.Clicked += (a, e) =>
            {
               Application.Current.MainPage.Navigation.PopModalAsync();

            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = {  message, button },
            };

            var page = new ContentPage();
            page.Content = layout;
            await Application.Current.MainPage.Navigation.PushModalAsync(page);


        }

     


    }

    public static class Helpers
    {

        private static Random rng = new Random();

        public static void Shuffle(this List<Card> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
