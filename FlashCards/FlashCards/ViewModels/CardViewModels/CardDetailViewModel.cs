using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.ViewModels.CardViewModels
{
    public class CardDetailViewModel : BaseViewModel
    {

        private Card _card;

        private Bundle _bundle;

        public CardDetailViewModel(Bundle bundle)
        {
            _card = new Card();
            _bundle = bundle;
            _card.BundleId = _bundle.Id;
        }

        public CardDetailViewModel(Card card, Bundle bundle)
        {
            _card = card;
            _bundle = bundle;
        }

        public string Information
        {
            get
            {
                return _card.Information;
            }
            set
            {

                _card.Information = value;
                OnPropertyChanged();
                SaveButtonCommand.ChangeCanExecute();
            }
        }

        public string Answer
        {
            get {
                return _card.Answer;
            }
            set {
                _card.Answer = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get
            {
                return _card.Id;
            }
        }

        public int BundleId
        {
            get { return _card.BundleId; }
        }

        private Command _saveButtonCommand;

        public Command SaveButtonCommand
        {
            get
            {
                return _saveButtonCommand ?? (_saveButtonCommand = new Command(SaveButtonHandler, CanBeSaved));
            }
        }

        private bool CanBeSaved()
        {
            List<Card> otherCards = App.Database.GetCardsFromBundle(BundleId).Result.FindAll(c => c.Information == Information && c.Id != Id);
            if (otherCards.Count > 0)
                return false;
            else return true;
        }

        private async void SaveButtonHandler()
        {
            

            int result = await App.Database.SaveCardAsync(_card);
            await Application.Current.MainPage.Navigation.PopAsync();
        }


    }
}
