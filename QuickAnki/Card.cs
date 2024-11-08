using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickAnki
{
    public class Card
    {
        public string Front { get; set; }
        public string Back { get; set; }

        public Card(string front, string back)
        {
            Front = front;
            Back = back;
        }
    }
}
