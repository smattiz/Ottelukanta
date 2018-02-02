using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ottelukanta.DataModels
{
    /// <summary>
    ///Example of One match, data that interests us is displayed here
    ///Actually it has no other meaning but to give our data a structure. Easier to handle
    /// </summary>
    public class Match
    {
        public int Id;
        public string Round;
        public int RoundNumber;
        public DateTime MatchDate;
        public Team HomeTeam;
        public Team AwayTeam;
        public int HomeGoals;
        public int AwayGoals;

    }
}
