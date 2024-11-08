using AnkiNet;
using CommandLine;
using MoreLinq;
using QuickAnki;
using System.Collections.Immutable;



Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed(async o =>
    {

        //var filePath = o.Input;
        //var deckName = o.DeckName;
        var filePath = "C:\\Users\\USERNAME\\Desktop\\cyrillictmp";
        var deckName = "Serbian cyrillic - Maiuscole";

        var fileLines = File.ReadAllLines(filePath);

        var validationState = fileLines.Scan(new ValidationState(true, true, 0), (acc, x) =>
        {

            var isEmpty = string.IsNullOrWhiteSpace(x);
            var tokenCount = isEmpty ? 0 : acc.TokenCount + 1;

            var currentValidationState = new ValidationState(isEmpty, !(tokenCount > 2 || (isEmpty && acc.TokenCount == 1)), tokenCount);
            return currentValidationState;
        }
        ).FirstOrLast(x => !x.WasLastStateValid);

        if (!validationState.WasLastStateValid)
        {
            Console.WriteLine("Input file is in an invalid format.");
            Environment.Exit(-1);
        }

        var collection = fileLines.Where(x => !string.IsNullOrWhiteSpace(x))
                                  .Chunk(2)
                                  .Select(x => new Card(x[0], x[1]))
                                  .ToAnkiCollection(deckName);

        await AnkiFileWriter.WriteToFileAsync($"{deckName}.apkg", collection);

    });