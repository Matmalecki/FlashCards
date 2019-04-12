using FlashCards.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCards.Views;
using FlashCards.ViewModels.BundleViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FlashCards
{
    public partial class App : Application
    {
        private static FlashCardsDatabase _database;

        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage(new BundlesPage() { BindingContext = new BundlesViewModel() });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
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
