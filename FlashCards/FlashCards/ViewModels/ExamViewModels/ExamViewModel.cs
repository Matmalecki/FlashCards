using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlashCards.ViewModels.ExamViewModels
{
    public class ExamViewModel :BaseViewModel
    {
        public Timer Timer;
        private Stack<Card> _questions;
        private Card currentQuestion;

        private int maxScore;
        private readonly int secondsPerQuestion;

        public ExamViewModel(List<Card> cards, int nrOfQuestions, int secondsPerQuestion = 0)
        {
            maxScore = nrOfQuestions;
            this.secondsPerQuestion = secondsPerQuestion;
            if (secondsPerQuestion != -1)
            {
                _totalSeconds = secondsPerQuestion;
                Timer = new Timer(TimeSpan.FromSeconds(1), CountDown);
                Timer.Start();
            }

            Helpers.ShuffleCards(cards);

            _questions = new Stack<Card>(cards.ConvertAll( o => (Card)o));
            while (_questions.Count > nrOfQuestions)
            {
                _questions.Pop();
            }


            SetQuestion();

        }

        private void ResetTimeCountDown()
        {
            _totalSeconds = secondsPerQuestion;
        }

        private void CountDown()
        {
            _totalSeconds--;
            if (_totalSeconds == 0)
            {
                CheckAnswerAsync();
            }
            OnPropertyChanged("TimeLeft");
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

        private int _totalSeconds;

        public string TimeLeft
        {
            get {
                if (_totalSeconds > 0)
                    return _totalSeconds.ToString();
                else return "";
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
            } else
            {
                Timer.Stop();
                await Task.Delay(500);
                ShowScoreAndLeave();
                Application.Current.MainPage.Navigation.PopAsync() ;
            }
            ResetTimeCountDown();

        }

        private async void ShowScoreAndLeave()
        {
            var message = new Label { Text = $"Your score is : {Score} / {maxScore}" };

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

    public class Timer
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action _callback;

        private static CancellationTokenSource _cancellationTokenSource;

        public Timer(TimeSpan timeSpan, Action callback)
        {
            _timeSpan = timeSpan;
            _callback = callback;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public void Start()
        {
            CancellationTokenSource cts = _cancellationTokenSource; // safe copy
            Device.StartTimer(_timeSpan, () =>
            {
                if (cts.IsCancellationRequested)
                {
                    return false;
                }
                _callback.Invoke();
                return true; //true to continuous, false to single use
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
        }
    }

    public static class Helpers
    {

        private static Random rng = new Random();

        public static void ShuffleCards(List<Card> list)
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
        public static void ShufflePhotoCards(List<PhotoCard> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                PhotoCard value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
