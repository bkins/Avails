using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Avails.Xamarin
{
    public static class LocaleInfo
    {
        private static Location _currentLocation;

        public static Location CurrentLocation
        {
            get
            {
                if (_currentLocation is null)
                {
                    /*TODO:
                       LocaleInfo.cs(18, 21): [CS4014] Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                     */
                    SetCurrentLocation().ConfigureAwait(false);
                }
                return _currentLocation;
            }
            private set => _currentLocation = value;
        }

        public static Task<Location> RefreshLocation()
        {
            _currentLocation = null;
            return SetCurrentLocation();
        }
        
        private static async Task<Location> SetCurrentLocation()
        {
            if (_currentLocation != null) { return  _currentLocation; }
            
            try
            {
                var request         = new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(10));
                var source          = new CancellationTokenSource();
                var cancelToken     = source.Token;
                var currentLocation = await Geolocation.GetLocationAsync(request, cancelToken);
                
                _currentLocation = currentLocation;
                
                if (_currentLocation is null)
                {
                    await SetDefaultLocation().ConfigureAwait(false);
                }
                Configuration.Latitude  = _currentLocation.Latitude;
                Configuration.Longitude = _currentLocation.Longitude;

                return _currentLocation;

            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                Logger.Logger
                      .WriteLineToToastForced("GeoLocation is not supported on this device (Default location will be used)"
                                            , Logger.Category.Warning);

                await SetDefaultLocation().ConfigureAwait(false);

                return _currentLocation;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                Logger.Logger
                      .WriteLineToToastForced("GeoLocation is not enabled on this device (Default location will be used)"
                                            , Logger.Category.Warning);

                await SetDefaultLocation().ConfigureAwait(false);

                return _currentLocation;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                Logger.Logger
                      .WriteLineToToastForced("GeoLocation was not given permission to use on this device (Default location will be used)"
                                            , Logger.Category.Warning);

                await SetDefaultLocation().ConfigureAwait(false);

                return _currentLocation;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Logger.Logger
                      .WriteLineToToastForced("Something went wrong while trying to GeoLocation on this device (Default location will be used)"
                                            , Logger.Category.Error
                                            , ex);

                await SetDefaultLocation().ConfigureAwait(false);

                return _currentLocation;
            }
        }

        private static async Task SetDefaultLocation()
        {
            //First try to get the "Last Known Location" from device
            //If that doesn't work, get hard-coded default
            var currentLocation = await Geolocation.GetLastKnownLocationAsync()
                               ?? new Location(latitude: 47.6062d
                                             , longitude: -122.3321d);

            _currentLocation = currentLocation;
                
            Configuration.Latitude  = _currentLocation.Latitude;
            Configuration.Longitude = _currentLocation.Longitude;
        }
    }
}