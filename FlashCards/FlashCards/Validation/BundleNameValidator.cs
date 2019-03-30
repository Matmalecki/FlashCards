using FlashCards.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FlashCards.Validation
{
    public class BundleNameValidator : Behavior<Entry>
    {

        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(BundleNameValidator), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        public int IdOfBundle{ get;set; }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (Entry)sender;
            List<Bundle> otherBundles = App.Database.GetBundlesAsync().Result.FindAll(b => b.Name == obj.Text && b.Id != IdOfBundle);
            if (otherBundles.Count > 0)
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
