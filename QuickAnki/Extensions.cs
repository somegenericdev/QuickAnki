using AnkiNet;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuickAnki
{
    public static class Extensions
    {

        public static TSource? FirstOrLast<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.TryGetFirst(predicate, out var _);
        }

        private static TSource? TryGetFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out bool found)
        {

            foreach (TSource element in source)
            {
                if (predicate(element))
                {
                    found = true;
                    return element;
                }
            }

            found = false;
            return source.LastOrDefault();
        }
        public static AnkiCollection ToAnkiCollection(this IEnumerable<Card> list, string deckName)
        {

            var noteType = new AnkiNoteType(
                name: "Basic",
                cardTypes: new[] {
                    new AnkiCardType(
                        Name: "Card 1",
                        Ordinal: 0,
                        QuestionFormat: "{{Front}}",
                        AnswerFormat: "{{Front}}<hr id=\"answer\">{{Back}}"

                    )
                },
                fieldNames: new[] { "Front", "Back" },
                css: ".card {font-family: arial;font-size: 20px;text-align: center;color: black;background-color: white;}"
            );


            var collection = new AnkiCollection();

            var noteTypeId = collection.CreateNoteType(noteType);
            var deckId = collection.CreateDeck(deckName);
            list.ForEach(x =>
            {
                collection.CreateNote(deckId, noteTypeId, x.Front, x.Back);
            });
            return collection;
        }

        public static string RemoveFirstAndLastCharacter(this string input)
        {
            return input.Substring(1, input.Length - 2);
        }
    }

}
