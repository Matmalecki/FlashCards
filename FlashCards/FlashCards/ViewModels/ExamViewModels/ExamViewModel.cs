using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
            }else
            {
                
                PageContent = 
                //finish game
            }


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
