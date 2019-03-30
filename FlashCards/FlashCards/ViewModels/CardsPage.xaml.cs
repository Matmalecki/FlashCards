﻿using FlashCards.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
    {
        Bundle bundle;

        public CardsPage(Bundle bundle)
        {
            InitializeComponent();
            this.bundle = bundle;
			
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
 
            CardsView.ItemsSource = await App.Database.GetCardsFromBundle(bundle.Id);
        }

        async void CreateCardHandler(object sender, EventArgs e )
        {
            await Navigation.PushAsync(new CardDetailPage(this.bundle, new Card()));
        }

        async void SelectedCardHandler(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            await Navigation.PushAsync(new CardDetailPage(this.bundle, e.Item as Card));
        }

        async void DeleteCardHandler(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Card card = await App.Database.GetCardAsync(int.Parse(btn.CommandParameter.ToString()));
            await App.Database.DeleteCardAsync(card);
            CardsView.ItemsSource = await App.Database.GetCardsFromBundle(bundle.Id);
        }

        async void EditBundleHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = bundle
            });
        }

      

    }
}