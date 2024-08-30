using Shared.Services.Interfaces; 

namespace Shared.Services
{

    public class FileService : IFileService
    {
        // Method to read all lines from a CSV file and return them as a list of string arrays.
        public IList<string[]> ReadInputsFromCsvFile(string fileName, string dir)
        {
            // Combines the directory and filename to create the full path to the CSV file.
            var path = Path.Combine(dir, fileName);

            var listOutput = new List<string[]>(); 

            // Checks if the file exists at the specified path.
            if (File.Exists(path))
            {
                // Opens the file for reading.
                using (StreamReader file = new StreamReader(path))
                {
                    // Reads the file line by line until the end of the stream.
                    while (!file.EndOfStream)
                    {
                        // Splits each line by commas and adds the resulting string array to the list.
                        listOutput.Add(file.ReadLine().Split(','));
                    }
                }
            }

            // Returns the list of string arrays representing the lines of the CSV file.
            return listOutput;
        }
    }
}