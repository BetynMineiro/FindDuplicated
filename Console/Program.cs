using Application.MatchContactUserCase.Dto; 
using Domain; 
using MediatR;
using Shared; 
using Shared.Services.Interfaces; 
using Microsoft.Extensions.DependencyInjection; 

namespace Console
{
    class Program
    {
        private static readonly CancellationTokenSource cancellationToken = new CancellationTokenSource(); 

        static void Main(string[] args)
        {
            // Sets up the service collection with application, domain, message handling and shared layer services.
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureSharedLayerServices(); 
            serviceCollection.ConfigureMessageHandlerLayer(); 
            serviceCollection.ConfigureDomainLayerServices(); 

            var serviceProvider = serviceCollection.BuildServiceProvider(); 
            var fileService = serviceProvider.GetService<IFileService>(); 
            var mediator = serviceProvider.GetService<IMediator>(); 
            System.Console.WriteLine("Application has started. Ctrl-C to end"); 

            // Event handler for handling cancellation (Ctrl-C) and terminating the application.
            System.Console.CancelKeyPress += (sender, eventArgs) =>
            {
                System.Console.WriteLine("Cancel event triggered"); 
                cancellationToken.Cancel(); 
                eventArgs.Cancel = true; 
                Environment.Exit(0); 
            };

            // Start the worker task that handles file processing and contact matching.
            _ = Worker(fileService, mediator);
        }

        // Worker task that repeatedly processes input files and performs contact matching.
        static async Task Worker(IFileService fileService, IMediator mediator)
        {
            while (!cancellationToken.IsCancellationRequested) 
            {
                System.Console.WriteLine();
                System.Console.WriteLine();
               
                // Prompts the user to ensure the file exists in the current directory.
                System.Console.WriteLine("* Please check if the file exists in the directory below");
                System.Console.WriteLine($"Path: {Directory.GetCurrentDirectory()}");
                System.Console.WriteLine();
                System.Console.WriteLine("* Input the file name with the extension and press Enter");
                var file = System.Console.ReadLine(); // Reads the file name from the user.

                // Reads the CSV file into a list of string arrays.
                var list = fileService.ReadInputsFromCsvFile(file, Directory.GetCurrentDirectory()); 
                
                // Checks if the list has any data.
                if (list.Any())
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                    System.Console.WriteLine("****** Result: ******");
                    
                    // Creates a MatchContactInput with the list of contacts.
                    var input = new MatchContactInput() { ContactsList = list }; 
                    
                    // Sends the input to MediatR for processing.
                    var result = await mediator.Send(input, cancellationToken.Token); 

                    // Displays the matching results.
                    if (result.ContactsMatches.Any())
                    {
                        System.Console.WriteLine("| ContactID Source | ContactID Source | Accuracy |");
                        foreach (var match in result.ContactsMatches)
                        {
                            System.Console.WriteLine(
                                $"| {match.SourceContactID} | {match.MatchContactID} | {match.Accuracy} |");
                        }
                    }
                    else
                    {
                        // Notify user if no matches are found.
                        System.Console.WriteLine("****** No Matches ******"); 
                    }
                }
            }
        }
    }
}
