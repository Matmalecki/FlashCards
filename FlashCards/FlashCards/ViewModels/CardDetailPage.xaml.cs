using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlashCards.ViewModels
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardDetailPage : ContentPage
	{
		public CardDetailPage ()
		{
			InitializeComponent ();
		}
	}
}