namespace Avails.Xamarin
{
    public sealed class IntraAppCommunication
    {
        public static IntraAppCommunication Instance { get; } = new IntraAppCommunication();

        public int    IntegerValue      { get; set; }
        public string StringValue       { get; set; }
        public string CachedStringValue { get; set; }

        static IntraAppCommunication()
        {
        }

        private IntraAppCommunication()
        {
        }

        public void Clear()
        {
            Instance.IntegerValue = 0;
            Instance.StringValue  = string.Empty;
            CachedStringValue     = string.Empty;
        }
    }
}
