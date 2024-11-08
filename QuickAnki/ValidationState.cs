using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickAnki
{
    public class ValidationState
    {
        public bool WasLastEmpty { get; set; }
        public bool WasLastStateValid { get; set; }
        public int TokenCount { get; set; }

        public ValidationState(bool wasLastEmpty, bool wasLastStateValid, int tokenCount)
        {
            WasLastEmpty = wasLastEmpty;
            WasLastStateValid = wasLastStateValid;
            TokenCount = tokenCount;
        }
    }
}
