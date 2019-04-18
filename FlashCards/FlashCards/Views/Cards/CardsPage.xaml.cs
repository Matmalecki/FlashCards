using FlashCards.Custom;
using FlashCards.Models;
using FlashCards.ViewModels.BundleViewModels;
using FlashCards.ViewModels.CardViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
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

            RefreshCards();
        }

        private async void RefreshCards()
        {
            if (_bundle.CardType == CardType.Photo)
                CardsView.ItemsSource = await App.Database.GetPhotoCardsFromBundle(_bundle.Id);
            else
                CardsView.ItemsSource = await App.Database.GetCardsFromBundle(_bundle.Id);
        }

        async void CreateCardHandler(object sender, EventArgs e)
        {
            if (_bundle.CardType == CardType.Photo)
                await Navigation.PushAsync(new PhotoCardDetailPage() { BindingContext = new PhotoCardDetailViewModel(_bundle) });
            else
                await Navigation.PushAsync(new CardDetailPage() { BindingContext = new CardDetailViewModel(_bundle) });
        }

        async void SelectedCardHandler(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            if (_bundle.CardType == CardType.Photo)
                await Navigation.PushAsync(new PhotoCardDetailPage() { BindingContext = new PhotoCardDetailViewModel( e.Item as PhotoCard, this._bundle) });
            else 
                await Navigation.PushAsync(new CardDetailPage() { BindingContext = new CardDetailViewModel(e.Item as Card, _bundle) });
        }

        async void DeleteCardHandler(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            ICard card = await App.Database.GetCardAsync(int.Parse(btn.CommandParameter.ToString()));
            if (card == null)
                card = await App.Database.GetPhotoCardAsync(int.Parse(btn.CommandParameter.ToString()));
            await App.Database.DeleteCardAsync(card);

            RefreshCards();
        }

        async void EditBundleHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BundleDetailPage()
            {
                BindingContext = new BundleDetailViewModel(_bundle)
            });
        }

        private void SetUpList()
        {
            if (_bundle.CardType == CardType.Photo)
            {
                CardsView.ItemTemplate = new DataTemplate(() =>
                {
                    Image image = new Image() { WidthRequest = 80, HeightRequest = 80 };
                    image.SetBinding(Image.SourceProperty, new Binding("Information",BindingMode.Default,new StreamToStringConverter()));
                    Label answerLabel = new Label() {
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        MinimumWidthRequest = 200
                    };
                    answerLabel.SetBinding(Label.TextProperty, new Binding("Answer"));
                    Button btn = new Button() { Text = "Delete", BackgroundColor = Color.OrangeRed , FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), HorizontalOptions = LayoutOptions.End };
                    btn.SetBinding(Button.CommandParameterProperty, new Binding("Id"));
                    btn.Clicked += (sender, args) => DeleteCardHandler(sender, args);

                    var grid = new Grid()
                    {
                        
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    grid.Children.Add(image, 0, 0);
                    grid.Children.Add(answerLabel, 1, 0);
                    grid.Children.Add(btn, 2, 0);
                    ViewCell viewCell = new ViewCell() { View = grid };
                    return viewCell;
                });
            }
            else
            {
                CardsView.ItemTemplate = new DataTemplate(() =>
                {
                    Label infoLabel = new Label() {
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    infoLabel.SetBinding(Label.TextProperty, new Binding("Information"));
                    Label answerLabel = new Label()
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        MinimumWidthRequest = 200
                    };
                    answerLabel.SetBinding(Label.TextProperty, new Binding("Answer"));
                    Button btn = new Button() { Text = "Delete", BackgroundColor = Color.OrangeRed, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) };
                    btn.SetBinding(Button.CommandParameterProperty, new Binding("Id"));
                    btn.Clicked += (sender, args) => DeleteCardHandler(sender, args);

                    var grid = new Grid()
                    {           
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    grid.Children.Add(infoLabel, 0, 0);
                    grid.Children.Add(answerLabel, 1, 0);
                    grid.Children.Add(btn, 2, 0);
                    ViewCell viewCell = new ViewCell() { View = grid };
                    return viewCell;
                });

            }

            

        }

      

    }
}
