using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Configuration
{
    static class ParseIntegerListFromCsv
    {
        const char comma = ',';

        public static bool CanParse(string separatedIntegers)
        {
            if (EmptyList(separatedIntegers))
                return true;

            return CanParseList(separatedIntegers);
        }

        static bool CanParseList(string separatedIntegers)
        {
            int irrelevant;

            return Split(separatedIntegers).All((s) => int.TryParse(s, out irrelevant));
        }

        public static IEnumerable<int> Parse(string separatedIntegers)
        {
            if (EmptyList(separatedIntegers))
                yield break;

            foreach (string id in Split(separatedIntegers))
                yield return int.Parse(id);
        }

        static bool EmptyList(string separatedIntegers) =>
            string.IsNullOrWhiteSpace(separatedIntegers);

        static IEnumerable<string> Split(string separatedIntegers) =>
            separatedIntegers.Split(comma);
    }
}
