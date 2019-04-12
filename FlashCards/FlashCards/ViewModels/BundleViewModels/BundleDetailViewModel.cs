using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FlashCards.Models;
using FlashCards.Validation;

namespace FlashCards.ViewModels.BundleViewModels
{
    public class BundleDetailViewModel : BaseViewModel
    {
        Bundle _bundle;


        public BundleDetailViewModel(Bundle bundle)
        {
            _bundle = bundle;
        }
        public BundleDetailViewModel()
        {
            _bundle = new Bundle();
        }

        public int Id
        {
            get { return _bundle.Id; }
            set { _bundle.Id = value; }
        }
            
        public string Name
        {
            get
            {
                return _bundle.Name;
            }
            set
            {
                _bundle.Name = value;
            }
        }
        public CardType CardType
        {
            get
            {
                return _bundle.CardType;
            }
            set
            {
                _bundle.CardType = value;
            }
        }
        private Command _saveButtonCommand;

        public Command SaveButtonCommand
        {
            get
            {
                    return _saveButtonCommand ?? (_saveButtonCommand = new Command(SaveButtonHandler));
            }
        }


        private async void SaveButtonHandler()
        {
            int result = await App.Database.SaveBundleAsync(_bundle);
            await Application.Current.MainPage.Navigation.PopAsync(); 
        }

    }
}
