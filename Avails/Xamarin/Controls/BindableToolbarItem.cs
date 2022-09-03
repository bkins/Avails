using Xamarin.Forms;

// using Microsoft.Maui;
// using Microsoft.Maui.Controls;

namespace Avails.Xamarin.Controls
{
    public class BindableToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible)
                                                                                          , typeof(bool)
                                                                                          , typeof(BindableToolbarItem)
                                                                                          ,  true
                                                                                          , BindingMode.TwoWay
                                                                                          , propertyChanged: OnIsVisibleChanged);

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if ( ! (bindable is BindableToolbarItem item) 
              || item.Parent == null)
                return;

            var toolbarItems = ((ContentPage)item.Parent).ToolbarItems;

            switch ((bool)newValue)
            {
                case true when ! toolbarItems.Contains(item):

                    Device.BeginInvokeOnMainThread(() => { toolbarItems.Add(item); });

                    break;

                case false when toolbarItems.Contains(item):

                    Device.BeginInvokeOnMainThread(() => { toolbarItems.Remove(item); });

                    break;
            }
        }
    }
}