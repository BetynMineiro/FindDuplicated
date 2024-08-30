using Domain.Model; 
using Domain.Services.Interfaces; 

namespace Domain.Services
{
    // Service to find and match similar contacts based on certain criteria.
    public class MatchContactService : IMatchContactService
    {
        // Finds potential matches between contacts by comparing them and assigning scores based on similarity.
        public IList<ContactMatch> FindMatchesCases(IList<Contact> contacts)
        {
            var matches = new List<ContactMatch>(); // Stores the matched contacts.

            // Iterate through each pair of contacts.
            for (int i = 0; i < contacts.Count; i++)
            {
                for (int j = i + 1; j < contacts.Count; j++)
                {
                    var matchScore = CalculateScore(contacts[i], contacts[j]); // Calculate the similarity score.

                    if (matchScore <= 0) continue; // If the score is 0 or less, skip to the next pair.

                    var accuracy = SetAccuracy(matchScore); // Determine the accuracy level based on the score.
                    matches.Add(new ContactMatch // Add the match details to the list.
                    {
                        SourceContactID = contacts[i].ContactID,
                        MatchContactID = contacts[j].ContactID,
                        Accuracy = accuracy
                    });
                }
            }

            // Return the list of matches, ordered by accuracy in descending order.
            return matches.OrderByDescending(match => SetPriorityOrder(match.Accuracy)).ToList();
        }

        // Calculates the similarity score between two contacts.
        private int CalculateScore(Contact contact1, Contact contact2)
        {
            int score = 0; // Initialize the score.

            // Check if the email addresses start with the same string.
            if (IsStartsWithMatch(contact1.EmailAddress, contact2.EmailAddress))
            {
                score += 3; // Increase score by 3 for email match.
            }

            // Check if the first names start with the same string.
            if (IsStartsWithMatch(contact1.FirstName, contact2.FirstName))
            {
                score += 1; // Increase score by 1 for first name match.
            }

            // Check if the last names start with the same string.
            if (IsStartsWithMatch(contact1.LastName, contact2.LastName))
            {
                score += 1; // Increase score by 1 for last name match.
            }

            // Check if the zip codes start with the same string.
            if (IsStartsWithMatch(contact1.ZipCode, contact2.ZipCode))
            {
                score += 2; // Increase score by 2 for zip code match.
            }

            // Check if the addresses start with the same string.
            if (IsStartsWithMatch(contact1.Address, contact2.Address))
            {
                score += 2; // Increase score by 2 for address match.
            }

            return score; // Return the final calculated score.
        }

        // Checks if one string starts with the other, considering case and trimming whitespace.
        private bool IsStartsWithMatch(string value1, string value2)
        {
            if (string.IsNullOrEmpty(value1) || string.IsNullOrEmpty(value2)) return false; // Return false if either string is null or empty.

            // Normalize and trim the strings.
            value1 = value1.ToLower().Trim();
            value2 = value2.ToLower().Trim();

            // Check if one value starts with the other.
            return value1.StartsWith(value2) || value2.StartsWith(value1);
        }

        // Sets the accuracy label based on the score.
        private static string SetAccuracy(int score)
        {
            if (score >= 6)
                return "High"; // High accuracy for scores 6 and above.
            if (score >= 3)
                return "Medium"; // Medium accuracy for scores 3 to 5.
            return "Low"; // Low accuracy for scores below 3.
        }

        // Sets the priority order for sorting based on the accuracy.
        private int SetPriorityOrder(string accuracy)
        {
            // Assigns a numerical value to each accuracy level for ordering purposes.
            return accuracy switch
            {
                "High" => 3,
                "Medium" => 2,
                "Low" => 1,
                _ => 0
            };
        }
    }
}
