﻿using FlashCards.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage, INotifyPropertyChanged
    {
        private Bundle _bundle;

        public CardsPage(Bundle bundle)
        {
            
            InitializeComponent();
            this._bundle = bundle;
            SetUpList();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
 
            CardsView.ItemsSource = await App.Database.GetCardsFromBundle(_bundle.Id);
        }

        async void CreateCardHandler(object sender, EventArgs e )
        {
            await Navigation.PushAsync(new CardDetailPage(this._bundle, new Card()));
        }

        async void SelectedCardHandler(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            await Navigation.PushAsync(new CardDetailPage(this._bundle, e.Item as Card));
        }

        async void DeleteCardHandler(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Card card = await App.Database.GetCardAsync(int.Parse(btn.CommandParameter.ToString()));
            await App.Database.DeleteCardAsync(card);
            CardsView.ItemsSource = await App.Database.GetCardsFromBundle(_bundle.Id);
        }

        async void EditBundleHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = _bundle
            });
        }

        private void SetUpList()
        {

            CardsView.ItemTemplate = new DataTemplate(() =>
            {
                Label infoLabel = new Label();
                infoLabel.SetBinding(Label.TextProperty, new Binding("Information"));
                Label answerLabel = new Label();
                answerLabel.SetBinding(Label.TextProperty, new Binding("Answer"));
                Button btn = new Button() { Text = "Delete", BackgroundColor = Color.OrangeRed };
                btn.SetBinding(Button.CommandParameterProperty, new Binding("Id"));
                btn.Clicked += (sender, args) => DeleteCardHandler(sender, args);

                var stackLayout = new StackLayout()
                {
                    Children = {
                    infoLabel,
                    answerLabel,
                    btn
                }
                };
                ViewCell viewCell = new ViewCell() { View = stackLayout };
                return viewCell;
            });


           // else photo ...

            

        }

      

    }
}