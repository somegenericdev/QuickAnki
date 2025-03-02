using AnkiNet;
using CommandLine;
using MoreLinq;
using QuickAnki;
using System.Collections.Immutable;
using System.Text.RegularExpressions;



Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed(async o =>
    {

        var filePath = o.Input;
        var deckName = o.DeckName;

        var tmpFilePath = Path.GetTempFileName();

        File.WriteAllText(tmpFilePath, PreProcess(File.ReadAllText(filePath)));


        var fileLines = File.ReadAllLines(tmpFilePath);

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
                                  .Select(x => new Card(Normalize(x[0]), Normalize(x[1])))
                                  .ToAnkiCollection(deckName);

        await AnkiFileWriter.WriteToFileAsync($"{deckName}.apkg", collection);

    });
string PreProcess(string input)
{
    if(input.First() == '`')
    {
        input = new string(input.Prepend('\n').ToArray()); 
    }

    string pattern = @"([^\\])(`[\s\S]+?[^\\]`)";
    RegexOptions options = RegexOptions.Multiline;
    var res = Regex.Replace(input, pattern, (m) => $"{m.Groups[1].Value}{m.Groups[2].Value.Replace("\r\n", @"\r\n").Replace("\n", @"\n")}");
    return res;
}

string Normalize(string input)
{
    if(input.First() == '`' && input.Last() == '`')
    {
        return input.RemoveFirstAndLastCharacter().Replace(@"\r\n", "<br/>").Replace(@"\n", "<br/>");
    }
    return input;
}