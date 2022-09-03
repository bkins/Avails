using System;
using System.IO;
using Avails.D_Flat.Extensions;

namespace Avails.Xamarin
{
    public class BackupRestoreDatabase
    {
        public string DatabaseDestinationLocation { get; set; }
        public string DatabaseSourceLocation      { get; set; }

        public BackupRestoreDatabase(string databaseSourceLocation
                                   , string databaseDestinationLocation)
        {
            DatabaseSourceLocation      = databaseSourceLocation;
            DatabaseDestinationLocation = databaseDestinationLocation;
        }

        public string Backup(string fileName)
        {
            var uniqueFileNameSuffix = DateTime.Now.ToLong();

            var backedUpFileFullPath = Path.Combine(DatabaseDestinationLocation
                                                  , fileName.Replace(".db3"
                                                                   , $"_{uniqueFileNameSuffix}.db3"));

            if ( ! Directory.Exists(DatabaseDestinationLocation))
            {
                Directory.CreateDirectory(DatabaseDestinationLocation);
            }
            
            File.Copy(DatabaseSourceLocation
                    , backedUpFileFullPath);

            return backedUpFileFullPath;
        }

        public void Restore(string fileName)
        {
                File.Copy(Path.Combine(DatabaseDestinationLocation, fileName)
                        , DatabaseSourceLocation
                        , overwrite: true);
        }
    }
}
