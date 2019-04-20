using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlashCards.ViewModels.ExamViewModels
{
    public class PhotoExamViewModel : BaseViewModel
    {

        private Stack<PhotoCard> _questions;
        private PhotoCard currentQuestion;

        public PhotoExamViewModel(List<PhotoCard> cards, int nrOfQuestions)
        {
            Helpers.ShufflePhotoCards(cards);
            _questions = new Stack<PhotoCard>(cards.ConvertAll(o => (PhotoCard)o));
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

        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
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
                return _submitAnswer ?? (_submitAnswer = new Command(CheckAnswerAsync));
            }
        }



        private async void CheckAnswerAsync()
        {

            if (UserAnswer == currentQuestion.Answer)
            {
                Score++;
                // set image correct
                ImageSource = ImageSource.FromResource("FlashCards.Images.correct.png");
            }
            else
            {
                ImageSource = ImageSource.FromResource("FlashCards.Images.wrong.png");
            }

            if (_questions.Count > 0)
            {
                SetQuestion();
            }
            else
            {
                await Task.Delay(500);
                ShowScoreAndLeave();
                Application.Current.MainPage.Navigation.PopAsync();
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
                Children = { message, button },
            };

            var page = new ContentPage();
            page.Content = layout;
            await Application.Current.MainPage.Navigation.PushModalAsync(page);


        }




    }


}
