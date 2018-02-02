namespace Ottelukanta.DataModels
{
    /// <summary>
    /// An example of team.
    /// Actually it has no other meaning but to give our data a structure. Easier to handle that way
    /// </summary>
    public class Team
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Logourl { get; set; }
        public int Ranking { get; set; }
        public string Message { get; set; }
    }
}