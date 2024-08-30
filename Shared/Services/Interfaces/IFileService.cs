namespace Shared.Services.Interfaces;

public interface IFileService
{
    IList<string[]> ReadInputsFromCsvFile(string fileName, string dir);
}