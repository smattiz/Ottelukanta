using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ottelukanta.DataHandling;
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

        private const string STANDARD_START_DATE = "11.11.1970";
        private const string STANDARD_END_DATE = "30.11.2018";

        private DataHandler handler = new DataHandler();
        private string matchFileName = "matches.json";

        public List<DataModels.Match> matchList= new List<DataModels.Match>();
        public bool isDateErrors { get; set; }
        public bool isFileError { get; set; }
        public string EvenCase {
            get
            {
                return "Tasapeli";
            }

        }
       


        public void OnGet()
        {
            isDateErrors = false;
            try
            { 
                matchList = handler.LoadMatches(matchFileName);
            }
            catch
            {
                isFileError = true;
            }
        }

        /// <summary>
        /// filter-button is clicked, if limitations are set, change matches-table
        /// TODO: Validation of date/month
        /// </summary>
        /// <returns> New view of page</returns>
        public IActionResult OnPost()
        {
            isDateErrors = false;
            var  startingTime = Request.Form["beginning"];
            var  endingTime = Request.Form["ending"];

            if (string.IsNullOrEmpty(startingTime) && string.IsNullOrEmpty(endingTime) ) 
            {
                matchList =  handler.LoadMatches(matchFileName);
                return Page();
            }
            else
            {
                DateTime Start = parseDate(startingTime, STANDARD_START_DATE);
                DateTime End = parseDate(endingTime, STANDARD_END_DATE);
                try
                { 
                   matchList = handler.FilterMatches(matchFileName,Start,End);
                }
                catch (ArgumentException e)
                {
                    isDateErrors = true;
                }
                catch (FileNotFoundException e)
                {
                    isFileError = true;
                }
                return Page();
            }
        }

        /// <summary>
        /// Simple helper function to help date parsing
        /// </summary>
        /// <param name="parsableDate"> String-representation of parsable date</param>
        /// <param name="ExceptionValue">Value used if no other is provided</param>
        /// <returns></returns>
        private DateTime parseDate(string parsableDate, string ExceptionValue)
        {
            var date = new DateTime();
            try
            {
                date = DateTime.Parse(parsableDate);
            }

            catch
            {
                date = DateTime.Parse(ExceptionValue);
            }
            return date;
        }
    }
}
