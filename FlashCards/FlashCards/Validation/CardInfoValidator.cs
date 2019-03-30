using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.Validation
{
    public class CardInfoValidator : Behavior<Entry>
    {

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(CardInfoValidator), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public CardInfoValidator() { }
        public CardInfoValidator(int id_bundle, int id_card)
        {
            IdOfBundle = id_bundle;
            IdOfCard = id_card;
            if (IdOfCard != 0)
                IsValid = true;
        }

        public int IdOfBundle { get; set; }
        public int IdOfCard { get; set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (Entry)sender;
            List<Card> otherCards = App.Database.GetCardsFromBundle(IdOfBundle).Result.FindAll(c => c.Information == obj.Text && c.Id != IdOfCard);
            if (otherCards.Count > 0)
                IsValid = false;
            else IsValid = true;
            obj.TextColor = IsValid ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }


    }
}
