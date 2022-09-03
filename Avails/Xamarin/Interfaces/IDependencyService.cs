namespace Avails.Xamarin.Interfaces
{
    public interface IDependencyService
    {
        string GetExternalStorage();
        bool CopyFile( string sourcePath
                     , string destinationPath);
    }
}