using Domain.Model;

namespace Application.MatchContactUserCase.Dto;

public class MatchContactOutput
{
    public IList<ContactMatch> ContactsMatches { get; set; }
}