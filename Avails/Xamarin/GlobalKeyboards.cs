using Xamarin.Forms;

namespace Avails.Xamarin
{
    public static class GlobalKeyboards
    {
        public static Keyboard None                   { get; }
        public static Keyboard All                    { get; }
        public static Keyboard Character              { get; }
        public static Keyboard NoCapitalization       { get; set; }
        public static Keyboard SentenceCapitalization { get; set; }
        public static Keyboard WordCapitalization     { get; set; }
        public static Keyboard SpellCheck             { get; set; }
        public static Keyboard Suggestions            { get; set; }

        public static Keyboard SuggestionsWithWordCapitalization     { get; set; }
        public static Keyboard SuggestionsWithSentenceCapitalization { get; set; }

        static GlobalKeyboards ()
        {
            None                   = Keyboard.Create(KeyboardFlags.None);
            All                    = Keyboard.Create(KeyboardFlags.All);
            Character              = Keyboard.Create(KeyboardFlags.CapitalizeCharacter);
            NoCapitalization       = Keyboard.Create(KeyboardFlags.CapitalizeNone);
            SentenceCapitalization = Keyboard.Create(KeyboardFlags.CapitalizeSentence);
            WordCapitalization     = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            SpellCheck             = Keyboard.Create(KeyboardFlags.Spellcheck);
            Suggestions            = Keyboard.Create(KeyboardFlags.Suggestions);
            
            SuggestionsWithWordCapitalization     = Keyboard.Create(KeyboardFlags.Suggestions | KeyboardFlags.CapitalizeWord);
            SuggestionsWithSentenceCapitalization = Keyboard.Create(KeyboardFlags.Suggestions | KeyboardFlags.CapitalizeSentence);
        }


    }
}
