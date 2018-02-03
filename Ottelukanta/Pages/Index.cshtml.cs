using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;
using Newtonsoft.Json; //for JSON parsing

namespace Ottelukanta.Pages
{
    /// <summary>
    /// Interaction logic of  Index-page
    /// Has two entry points: OnGet and OnPOST
    /// If Entry is OnPost, it will check whether it has been given dates and are they correct. In case of failure it will raise isDateErrors -flag and error will be displayed
    /// If error is displayed, all matches will be displayed, until proper date is given.
    /// </summary>
    public class IndexModel : PageModel
    {
        private string Matches;
        public List<DataModels.Match> kokeiluLista= new List<DataModels.Match>();
        public bool isDateErrors { get; set; }
        public bool isFileError { get; set; }
        public string EvenCase {
            get
            {
                return "Tasapeli";
            }

        }
  
        private string StartingTime { get; set; }
        private string EndingTime { get; set; }

        public void OnGet()
        {
            isDateErrors = false;
            LoadMatches();
        }


        /// <summary>
        /// filter-button is clicked, if limitations are set, change matches-table
        /// TODO: Validation of date/month
        /// </summary>
        /// <returns> New view of page</returns>
        public IActionResult OnPost()
        {
            isDateErrors = false;
            StartingTime = Request.Form["beginning"];
            EndingTime = Request.Form["ending"];
            if (string.IsNullOrEmpty(StartingTime) && string.IsNullOrEmpty(EndingTime) ) //Toimii 
            {
                LoadMatches();
                return Page();
            }
            else
            {
                filterMatches();
                return Page();
            }
        }

        /// <summary>
        /// Filter matches based on limiting values
        /// </summary>
        private void filterMatches()
        {
            DateTime Start = parseDate(StartingTime,"11.11.1970");
            DateTime End = parseDate(EndingTime, "30.11.2018");
            if(Start>End)
            {
                DateTime Temp = Start;
                Start = End;
                End = Temp;
            }
            if (( Start == new DateTime(1970,11,11) && End== new DateTime(2018,11,30)) )
            {
                isDateErrors = true;
              //  LoadMatches(Start, End);
            }
            LoadMatches(Start, End);
        }

       

        /// <summary>
        /// Simple helper function to help date parsing
        /// </summary>
        /// <param name="parsableDate"> String-representation of parsable date</param>
        /// <param name="ExceptionValue">Value used if no other is provided</param>
        /// <returns></returns>
        private DateTime parseDate(string parsableDate, string ExceptionValue)
        {
            DateTime Start = new DateTime();
            try
            {
                 Start = DateTime.Parse(parsableDate);
            }

            catch
            {
                Start = DateTime.Parse(ExceptionValue);
            }
            return Start;
        }

        /// <summary>
        /// Load all matches from .JSON -file
        /// </summary>
        /// <returns></returns>
        private void LoadMatches()
        {
            try
            {
                Matches= System.IO.File.ReadAllText("matches.json");
                kokeiluLista = JsonConvert.DeserializeObject<List<DataModels.Match>>(Matches);
            }
            catch
            {
             
                isFileError = true;
            }
           //  kokeiluLista = JsonConvert.DeserializeObject<List<DataModels.Match>>(Matches);
         
        }

        /// <summary>
        /// Load matches based on starting date and ending
        /// </summary>
        /// <param name="start">starting time from which to load dates</param>
        /// <param name="end"> ending time to which to stop</param>
        private void LoadMatches(DateTime Start, DateTime End)
        {
            try
            {
                Matches = System.IO.File.ReadAllText("matches.json");
                kokeiluLista = JsonConvert.DeserializeObject<List<DataModels.Match>>(Matches);
            }
            catch
            {
               
                isFileError = true;
            }




            List<DataModels.Match> AllMatches = JsonConvert.DeserializeObject<List<DataModels.Match>>(Matches);
            List<DataModels.Match> FilteredMatches = new List<DataModels.Match>();
            foreach (DataModels.Match match in AllMatches)
            {
                DateTime matchDate = match.MatchDate;
                if(matchDate >= Start && matchDate<= End)
                {
                    FilteredMatches.Add(match);
                }
            }
            kokeiluLista = FilteredMatches;
        }
    }
}
