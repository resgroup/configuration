using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Diagnostics.Contracts.Contract;

namespace RES.Configuration
{
    public static class ParseIntegerListFromCsv
    {
        const char comma = ',';

        public static bool CanParse(string separatedIntegers)
        {
            Requires(separatedIntegers != null);

            if (EmptyList(separatedIntegers))
                return true;

            return CanParseList(separatedIntegers);
        }

        static bool CanParseList(string separatedIntegers)
        {
            Requires(separatedIntegers != null);

            int irrelevant;

            return Split(separatedIntegers).All((s) => int.TryParse(s, out irrelevant));
        }

        public static IEnumerable<int> Parse(string separatedIntegers)
        {
            Requires(separatedIntegers != null);
            Ensures(Result<IEnumerable<int>>() != null);

            if (EmptyList(separatedIntegers))
                yield break;

            foreach (string id in Split(separatedIntegers))
                yield return int.Parse(id);
        }

        static bool EmptyList(string separatedIntegers)
        {
            Requires(separatedIntegers != null);

            return string.IsNullOrWhiteSpace(separatedIntegers);
        }

        static IEnumerable<string> Split(string separatedIntegers)
        {
            Requires(separatedIntegers != null);

            return separatedIntegers.Split(comma);
        }
    }
}
