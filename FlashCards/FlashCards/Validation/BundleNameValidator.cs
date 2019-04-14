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

        private bool _first = true;

        private string _initialText;

        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            this.BindingContext = bindable.BindingContext;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {

            var obj = (Entry)sender;
            if (_first)
            {
                _first = false;
                _initialText = obj.Text;
            }
            List<Bundle> otherCards = App.Database.GetBundlesAsync().Result.FindAll(c => c.Name == obj.Text && c.Name != _initialText);
            if (otherCards.Count > 0)
            {
                IsValid = false;
            } 
            else if (string.IsNullOrWhiteSpace(obj.Text) || obj.Text.Length > 10)
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
