using FlashCards.Custom;
using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.ViewModels.CardViewModels
{
    class PhotoCardDetailViewModel : BaseViewModel
    {
        private PhotoCard _photoCard;

        private Bundle _bundle;

        public PhotoCardDetailViewModel(Bundle bundle)
        {
            _photoCard = new PhotoCard();
            _bundle = bundle;
            _photoCard.BundleId = _bundle.Id;
        }

        public PhotoCardDetailViewModel(PhotoCard card, Bundle bundle)
        {
            _photoCard = card;
            _bundle = bundle;
        }

        public string Information
        {
            get
            {
                return _photoCard.Information;
            }
            set
            {

                _photoCard.Information = value;
                OnPropertyChanged();
                SaveButtonCommand.ChangeCanExecute();
            }
        }

        public string Answer
        {
            get
            {
                return _photoCard.Answer;
            }
            set
            {
                _photoCard.Answer = value;
                OnPropertyChanged();
            }
        }

        public int Id
        {
            get
            {
                return _photoCard.Id;
            }
        }

        public int BundleId
        {
            get { return _photoCard.BundleId; }
        }

        private Command _pickPhotoCommand;

        public Command PickPhotoCommand
        {
            get
            {
                return _pickPhotoCommand ?? (_pickPhotoCommand = new Command(PickPhotoButtonHandler));
            }
        }

        private Command _saveButtonCommand;

        public Command SaveButtonCommand
        {
            get
            {
                return _saveButtonCommand ?? (_saveButtonCommand = new Command(SaveButtonHandler, CanBeSaved));
            }
        }


        private async void PickPhotoButtonHandler()
        {
            Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                Information = ImagesConverter.ImageToStringBase64(stream);
            }
        }


        private bool CanBeSaved()
        {
            List<PhotoCard> otherCards = App.Database.GetPhotoCardsFromBundle(BundleId).Result.FindAll(c => c.Information == Information && c.Id != Id);
            if (otherCards.Count > 0)
                return false;
            else return true;
        }

        private async void SaveButtonHandler()
        {


            int result = await App.Database.SaveCardAsync(_photoCard);
            await Application.Current.MainPage.Navigation.PopAsync();
        }


    }
}
