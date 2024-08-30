using Application.MatchContactUserCase.Dto; 
using Domain.Model; 
using Domain.Services.Interfaces; 
using MediatR; 

namespace Application.MatchContactUserCase
{
   
    public class MatchContactHandler : IRequestHandler<MatchContactInput, MatchContactOutput>
    {
        private readonly IMatchContactService _matchContactService; 

      public MatchContactHandler(IMatchContactService matchContactService)
        {
            _matchContactService = matchContactService;
        }


        public Task<MatchContactOutput> Handle(MatchContactInput request, CancellationToken cancellationToken)
        {
            // Removes the first contact in the list, possibly a header or invalid entry.
            request.ContactsList.RemoveAt(0);

            // Checks if there are any contacts left to process.
            if (request.ContactsList.Any())
            {
                // Converts the list of string arrays to a list of Contact objects.
                var contacts = request.ContactsList.Select(line =>
                {
                    return new Contact
                    {
                        ContactID = Convert.ToInt32(line[0]), 
                        FirstName = line[1], 
                        LastName = line[2], 
                        EmailAddress = line[3], 
                        ZipCode = line[4], 
                        Address = line[5] 
                    };
                }).ToList();

                // Finds matches between the contacts using the MatchContactService.
                var result = _matchContactService.FindMatchesCases(contacts);

                // Returns the results wrapped in a MatchContactOutput object.
                return Task.FromResult(new MatchContactOutput() { ContactsMatches = result });
            }

            // Returns null if no contacts are left to process.
            return Task.FromResult<MatchContactOutput>(default);
        }
    }
}
