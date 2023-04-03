using System;
using Xamarin.Forms;

namespace Avails.Xamarin
{
    public static class UiUtilities
    {
        public static void AddCommandToGesture(object sender, Action command)
        {
            if (sender is not Image deleteImage)
                return;

            ( deleteImage.Parent as View )?.GestureRecognizers.Clear();

            ( deleteImage.Parent as View )?.GestureRecognizers
                                          .Add(new TapGestureRecognizer { Command = new Command(command) });
        }
    }
}