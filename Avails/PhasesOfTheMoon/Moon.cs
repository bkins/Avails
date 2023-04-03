using System;
using System.Collections.Generic;
using System.Linq;
using Avails.PhasesOfTheMoon.Helpers;

namespace Avails.PhasesOfTheMoon
{
    public static class Moon
    {
        private static readonly IReadOnlyList<string> NorthernHemisphere = new List<string>
                                                                           { "🌑", "🌒", "🌓", "🌔", "🌕", "🌖", "🌗", "🌘", "🌑" };
        private static readonly IReadOnlyList<string> SouthernHemisphere = NorthernHemisphere.Reverse().ToList();

        private static readonly List<string> Names = new List<string>
                                                     {
                                                         Phase.NewMoon
                                                       , Phase.WaxingCrescent
                                                       , Phase.FirstQuarter
                                                       , Phase.WaxingGibbous
                                                       , Phase.FullMoon
                                                       , Phase.WaningGibbous
                                                       , Phase.ThirdQuarter
                                                       , Phase.WaningCrescent
                                                     };

        public static readonly double TotalLengthOfCycle = 29.53;//3059;//0588861;

        public static DateTime MinimumDateTime
        {
            // get => new DateTime(1920, 1, 21, 5, 25, 00, DateTimeKind.Utc);
            get => new DateTime(1999, 8, 11, 4, 8, 00, DateTimeKind.Utc);
        }

        public static readonly List<Phase> AllPhases;

        static Moon()
        {
            var period = TotalLengthOfCycle / Names.Count;

            // divide the phases into equal parts 
            // making sure there are no gaps
            AllPhases = Names.Select((phase, i) =>
                             {
                                 if (phase == null)
                                     throw new ArgumentNullException(nameof(phase));

                                 return new Phase(phase
                                                , period * i
                                                , period * ( i + 1 ));
                             })
                             .ToList();
        }

        public static Phase GetPhaseByName(string phaseName)
        {
            return AllPhases.FirstOrDefault(phase => phase.Name == phaseName);
        }

        ///<summary>
        /// Calculate the current phase of the moon.
        /// Note: this calculation uses the last recorded new moon to calculate the cycles of
        /// of the moon since then. Any date in the past before 1920 might not work.
        /// </summary> 
        /// <remarks>"https://www.subsystems.us/uploads/9/8/9/4/98948044/moonphase.pdf"</remarks>/>
        public static PhaseResult Calculate(DateTime          utcDateTime
                                          , Earth.Hemispheres viewFromEarth = Earth.Hemispheres.Northern)
        {
            const double julianConstant = 2415018.5;
            var          julianDate     = utcDateTime.ToOADate() + julianConstant;

            // London New Moon (1920)
            // https://www.timeanddate.com/moon/phases/uk/london?year=1920
            var daysSinceLastNewMoon = MinimumDateTime.ToOADate() + julianConstant;

            var newMoons      = ( julianDate - daysSinceLastNewMoon ) / TotalLengthOfCycle;
            var daysIntoCycle = ( newMoons - Math.Truncate(newMoons) ) * TotalLengthOfCycle;
            var phaseOfMoon   = AllPhases.First(phase => phase.Start < daysIntoCycle && phase.End > daysIntoCycle);
            var indexOfPhase  = AllPhases.IndexOf(phaseOfMoon);

            string currentEmoji = viewFromEarth switch
            {
                Earth.Hemispheres.Northern => NorthernHemisphere[indexOfPhase]
              , _ => SouthernHemisphere[indexOfPhase]
            };

            return new PhaseResult(phaseOfMoon
                                 , currentEmoji
                                 , Math.Round(daysIntoCycle, 2)
                                 , viewFromEarth
                                 , utcDateTime);
        }
        
        /// <summary>
        /// Returns the date of the phase in the future, rounded to the nearest day.
        /// </summary>
        /// <param name="phase">The phase that you want the date of when it will happen next</param>
        /// <param name="utcOffset">UTC Offset of your timezone</param>
        /// <param name="viewFromEarth">The hemisphere (Default is Northern)</param>
        /// <returns>The date of the phase in the future, rounded to the nearest day.</returns>
        public static DateTime CalculateDateOfThePhase(Phase             phase
                                                     , DateTime useDateTime
                                                     , Earth.Hemispheres viewFromEarth = Earth.Hemispheres.Northern)
        {
            var currentPhaseInfo = Calculate(useDateTime
                                           , viewFromEarth);

            var daysUntilPhase = 0d;

            if (currentPhaseInfo.DaysIntoCycle > phase.Start)
            {
                //Then Add the remaining days left in current lunar cycle plus the phase.Start 
                daysUntilPhase = TotalLengthOfCycle - currentPhaseInfo.DaysIntoCycle + phase.Start;
            }
            else
            {
                //The difference between phase.Start and DaysIntoCycle
                daysUntilPhase = phase.Start - currentPhaseInfo.DaysIntoCycle;
            }

            var dateOfPhase = useDateTime.AddDays(daysUntilPhase);

            var dateToTheNearestDay = new DateTime(dateOfPhase.Year
                                                 , dateOfPhase.Month
                                                 , dateOfPhase.Day
                                                 , 0, 0, 0);

            return dateOfPhase.Hour >= 12 
                        ? dateToTheNearestDay.AddDays(1) 
                        : dateToTheNearestDay;
        }

        public static PhaseResult UtcNow(Earth.Hemispheres viewFromEarth = Earth.Hemispheres.Northern)
        {
            return Calculate(DateTime.UtcNow
                           , viewFromEarth);
        }

        public static PhaseResult Now(Earth.Hemispheres viewFromEarth = Earth.Hemispheres.Northern)
        {
            return Calculate(DateTime.Now.ToUniversalTime()
                           , viewFromEarth);
        }

    }
}