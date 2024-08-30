namespace Domain.Model;

public class ContactMatch
{
    public int SourceContactID { get; set; }
    public int MatchContactID { get; set; }
    public string Accuracy { get; set; }
}