using Domain.Model;

namespace Domain.Services.Interfaces;

public interface IMatchContactService
{
    IList<ContactMatch> FindMatchesCases(IList<Contact> contacts);
}