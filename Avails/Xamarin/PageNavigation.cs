using System;
using Avails.Xamarin.Logger;
using Xamarin.Forms;

// using Microsoft.Maui.Controls;

namespace Avails.Xamarin
{
    public static class PageNavigation
    {
        public static void NavigateTo(string nameOfPage)
        {
            try
            {
                Navigate($"{nameOfPage}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }

            //await 
        }

        public static void NavigateTo(string nameOfPage
                                          , string nameOfParameter1
                                          , string valueOfParameter1)
        {
            Navigate($"{nameOfPage}?{nameOfParameter1}={valueOfParameter1}");
        }

        public static void NavigateTo(string nameOfPage
                                          , string nameOfParameter1
                                          , string valueOfParameter1
                                          , string nameOfParameter2
                                          , string valueOfParameter2)
        {
            Navigate($"{nameOfPage}?{nameOfParameter1}={valueOfParameter1}&{nameOfParameter2}={valueOfParameter2}");
        }
        
        public static void NavigateTo(string nameOfPage
                                          , string nameOfParameter1
                                          , string valueOfParameter1
                                          , string nameOfParameter2
                                          , string valueOfParameter2
                                          , string nameOfParameter3
                                          , string valueOfParameter3)
        {
            var path = $"{nameOfPage}?{nameOfParameter1}={valueOfParameter1}&{nameOfParameter2}={valueOfParameter2}&{nameOfParameter3}={valueOfParameter3}";
            Navigate(path);
        }

        public static void NavigateTo(string nameOfPage
                                          , string nameOfParameter1
                                          , string valueOfParameter1
                                          , string nameOfParameter2
                                          , string valueOfParameter2
                                          , string nameOfParameter3
                                          , string valueOfParameter3
                                          , string nameOfParameter4
                                          , string valueOfParameter4)
        {
            var path
            = $"{nameOfPage}?{nameOfParameter1}={valueOfParameter1}&{nameOfParameter2}={valueOfParameter2}&{nameOfParameter3}={valueOfParameter3}&{nameOfParameter4}={valueOfParameter4}";

            Navigate(path);
        }

        private static void Navigate(string path)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync(path);    
                });
            }
            catch (Exception e)
            {
                Logger.Logger.WriteLineToToastForced(e.Message, Category.Error, e);
            }
        }

        public static void NavigateBackwards()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }
    }
}
