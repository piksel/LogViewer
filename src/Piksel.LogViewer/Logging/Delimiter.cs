using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Piksel.LogViewer.Logging
{
    public class Delimiter
    {
        private const string OptionalSpaces = @"\s*";

        private static Dictionary<char, string> CharDisplayOverrides = new Dictionary<char, string>()
        {
            { '\t', @"\t" }
        };

        private static string DisplayChar(char c)
            => CharDisplayOverrides.ContainsKey(c)
             ? CharDisplayOverrides[c]
             : c.ToString();

        public string Name { get; set; }

        public char Start { get; set; }
        public string StartString => DisplayChar(Start);

        public char? End { get; set; }
        public string EndString => End.HasValue ? DisplayChar(End.Value) : "";


        public Delimiter(char start, string name, char? end = null)
        {
            Start = start;
            Name = name;
            End = end;
        }

        public static Delimiter[] GetDefaults() => new []
        {
            new Delimiter(' ', "Space"),
            new Delimiter('|', "Pipe"),
            new Delimiter('\t', "Tab"),
            new Delimiter('<', "Angle brackets", '>'),
            new Delimiter('[', "Square brackets", ']'),
        };

        public override string ToString()
            => string.Concat(Name, " (", StartString, EndString, ')');

        public Regex GetRegex(string line)
        {
            var partCount = line.Split(Start).Length;
            var escStart = Regex.Escape(Start.ToString());
            string escEnd = "";
            var delimIsWhiteSpace = char.IsWhiteSpace(Start);

            var sb = new StringBuilder();

            if (End.HasValue)
            {
                escEnd = Regex.Escape(End.Value.ToString());
            }

            for(int i=0; i<partCount; i++)
            {
                if(End.HasValue)
                {
                    if (i != 0 && !delimIsWhiteSpace)
                    {
                        sb.Append(OptionalSpaces);
                    }

                    sb.AppendFormat("{0}([^{1}]+){1}", escStart, escEnd);
                }
                else
                {
                    if (i != 0)
                    {
                        if (delimIsWhiteSpace)
                        {
                            sb.Append(escStart);
                        }
                        else
                        {
                            sb.Append(OptionalSpaces)
                            .Append(escStart)
                            .Append(OptionalSpaces);
                        }
                    }
                }

                sb.AppendFormat("([^{0}]+)", escStart);
            }

            Debug.WriteLine($"Regex: \"{sb}\"");

            return new Regex(sb.ToString());
        }
    }
}