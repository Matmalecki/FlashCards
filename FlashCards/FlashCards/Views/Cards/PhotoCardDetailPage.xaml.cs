using FlashCards.Custom;
using FlashCards.Models;
using FlashCards.Validation;
using FlashCards.ViewModels.CardViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.Views.Cards
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhotoCardDetailPage : ContentPage, INotifyPropertyChanged
    {

        public PhotoCardDetailPage()
        {
            InitializeComponent();
        }
    }
}