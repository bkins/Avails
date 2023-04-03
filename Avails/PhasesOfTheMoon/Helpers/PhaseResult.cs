using System;
using System.Text;

namespace Avails.PhasesOfTheMoon.Helpers
{
    public class PhaseResult
    {
        public Phase             Phase         { get; }
        public string            Emoji         { get; set; }
        public double            DaysIntoCycle { get; set; }
        public Earth.Hemispheres Hemisphere    { get; set; }
        public DateTime          Moment        { get; }
        public double Visibility
        {
            get
            {
                const int fullMoon  = 15;
                double    halfCycle = Moon.TotalLengthOfCycle / 2;

                var numerator = DaysIntoCycle > fullMoon
                                    // past the full moon, we want to count down
                                    ? halfCycle - ( DaysIntoCycle % halfCycle )
                                    
                                    // leading up to the full moon
                                    : DaysIntoCycle;

                return numerator / halfCycle * 100;
            }
        }

        public PhaseResult(Phase             phase
                         , string            emoji
                         , double            daysIntoCycle
                         , Earth.Hemispheres hemisphere
                         , DateTime          moment)
        {
            Phase         = phase;
            Emoji         = emoji;
            DaysIntoCycle = daysIntoCycle;
            Hemisphere    = hemisphere;
            Moment        = moment;
        }

        public override string ToString()
        {
            var percent = Math.Round(Visibility, 2);

            var message = new StringBuilder();
            message.AppendLine($"The Moon for {Moment} is {DaysIntoCycle} days");
            message.AppendLine($"into the cycle, and is showing as '{Phase}'");
            message.AppendLine($"with {percent}% visibility, and a face of {Emoji} from the {Hemisphere.ToString()} hemisphere.");

            return message.ToString();
        }
    }
}