using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickAnki
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = false, HelpText = "The input file, which will be compiled into an .apkg file.")]
        public string Input { get; set; }
        
        [Option('d', "deck", Required = false, HelpText = "The name of the deck to be generated.")]
        public string DeckName { get; set; }
    }
}
