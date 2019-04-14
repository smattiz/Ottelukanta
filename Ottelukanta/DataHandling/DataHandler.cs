using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ottelukanta.DataModels;
using Newtonsoft.Json; //for JSON parsing
using System.IO;
namespace Ottelukanta.DataHandling
{
    /// <summary>
    /// Data handling functions for Ottelukanta.
    /// </summary>
    public class DataHandler
    {

     

        public DataHandler()
        { }

        /// <summary>
        /// loads matches and returns them as a list
        /// </summary>
        public List<Match> LoadMatches(string fileName)
        {
            try
            {
                string textFile;
                textFile = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<List<Match>>(textFile);
            }
            catch(FileNotFoundException e)
            {
                throw new FileNotFoundException($" Unable to locate file: {fileName}, {e.ToString()}" );
            }
        }

        /// <summary>
        /// Filter match list based on given datetimes
        /// </summary>
        /// <returns></returns>
        public List<Match> FilterMatches(string matchfileName, DateTime startTime, DateTime endTime)
        {
         
            // swap datetimes if start date is later than end time
            if (startTime > endTime)
            {
                DateTime Temp = startTime;
                startTime = endTime;
                endTime = Temp;
            }
            
            if (startTime == new DateTime(1970, 11, 11) && endTime == new DateTime(2018, 11, 30))
            {
                throw new ArgumentException();
            }
           return LoadMatches(matchfileName, startTime, endTime);
        }

        /// <summary>
        /// loads matches based on starting time and ending time
        /// </summary>
        private List<Match> LoadMatches(string fileName, DateTime startTime, DateTime EndTime)
        {
            var matches = LoadMatches(fileName);
            List<Match> FilteredMatches = new List<Match>();
            foreach (Match match in matches)
            {
                DateTime matchDate = match.MatchDate;
                if (matchDate >= startTime && matchDate <= EndTime)
                {
                    FilteredMatches.Add(match);
                }
            }
            return FilteredMatches;
        }

    }
}
