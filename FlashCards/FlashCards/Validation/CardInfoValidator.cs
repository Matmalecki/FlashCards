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


        public static readonly BindableProperty IdProperty = BindableProperty.CreateAttached("Id", typeof(int), typeof(BundleNameValidator), 0);
        public static readonly BindableProperty BundleIdProperty = BindableProperty.CreateAttached("BundleId", typeof(int), typeof(BundleNameValidator), 0);
        public int BundleId
        {
            get { return (int)GetValue(BundleIdProperty); }
            set { SetValue(BundleIdProperty, value); }
        }
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (Entry)sender;
            List<Card> otherCards = App.Database.GetCardsFromBundle(BundleId).Result.FindAll(c => c.Information == obj.Text && c.Id != Id);
            if (otherCards.Count > 0)
                IsValid = false;
            else IsValid = true;
            obj.TextColor = IsValid ? Color.DarkBlue : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }


    }
}
