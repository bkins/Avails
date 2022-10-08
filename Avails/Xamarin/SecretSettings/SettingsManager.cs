using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xamarin.Essentials;
using static Avails.Xamarin.Logger.Logger;

namespace Avails.Xamarin.SecretSettings
{
    public class SettingsManager
    {
        private string Provider { get; set; }
        
        public SettingsManager (string provider)
        {
            Provider = provider;

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("secrets.json"
                                                                     , optional: false
                                                                     , reloadOnChange: true)
                                                          .AddUserSecrets<KeySettings>(true)
                                                          .Build();

            var test = configuration["KeySettings:License"];

        }
        
        /// <summary>
        /// Get the license for the provider passed into the constructor
        /// </summary>
        /// <returns>The license associated with the provider.
        /// Returns null if error is thrown or Provider is not found</returns>
        public async Task<string> GetLicense()
        {
            try
            {
                var license = await SecureStorage.GetAsync(Provider);

                return license;
            }
            catch (Exception ex)
            {
                //Write error to Logger
                WriteLine("Could not get license from SecureStorage"
                        , Logger.Category.Error
                        , ex);
                return null;
            }
        }
    }
}