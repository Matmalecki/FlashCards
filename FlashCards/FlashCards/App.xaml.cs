using FlashCards.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCards.Views;
using FlashCards.ViewModels.BundleViewModels;
using FlashCards.Views.Exam;
using FlashCards.ViewModels.ExamViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FlashCards
{
    public partial class App : Application
    {
        private static FlashCardsDatabase _database;

        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new BundlesPage());
        }

        private NavigationPage navigationPage;

        protected override void OnStart()
        {
            //Current.Properties.Remove("firstVisit");
            if (!Current.Properties.ContainsKey("firstVisit"))
            {
                var message = new Label()
                {
                    Text = "Dodawaj własne zestawy a w nich własne słówka/zdjęcia!"
                };

                var button = new Button()
                {
                    Text = "OK!"
                };
                button.Clicked += (a, e) =>
                {
                    Current.MainPage.Navigation.PopModalAsync();
                };
                var layout = new StackLayout
                {
                    Padding = new Thickness(0, 40, 0, 0),
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = { message,  button },
                };


                var page = new ContentPage();
                page.Content = layout;

                Current.MainPage.Navigation.PushModalAsync(page);

                Current.Properties["firstVisit"] = false;
                Current.SavePropertiesAsync();
            }

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

            navigationPage = Current.MainPage as NavigationPage;

            ExamPage page = ((NavigationPage)Current.MainPage).CurrentPage as ExamPage;
            
            if (page != null)
            {
                ExamViewModel examViewModel = page.BindingContext as ExamViewModel;
                examViewModel.Timer.Stop();
            }
        }

        protected override void OnResume()
        {
            MainPage = navigationPage;

            navigationPage = Current.MainPage as NavigationPage;

            ExamPage page = ((NavigationPage)Current.MainPage).CurrentPage as ExamPage;

            if (page != null)
            {
                ExamViewModel examViewModel = page.BindingContext as ExamViewModel;
                examViewModel.Timer.Start();
            }

            // Handle when your app resumes
        }



        public static FlashCardsDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new FlashCardsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FlashCardsDB.db3"));
                }
                return _database;
            }
        }

    }
}
