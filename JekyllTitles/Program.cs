using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JekyllTitles
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.md").ToArray();
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                if (lines.Length == 0)
                    continue;
                Console.WriteLine(Path.GetFileName(file));
                if (lines[0] != "---")
                    throw new InvalidDataException("Content doesn't start with \"---\"");
                var newlines = new List<string>();
                newlines.Add("---");
                var i = 1; //skip opening "---"
                for (; lines[i] != "---"; i++)
                    if (lines[i].Length > 0 && !lines[i].StartsWith("contents: ["))
                        newlines.Add(lines[i]);
                i++; //skip closing "---"
                var titlePos = newlines.Count;
                newlines.Add("");
                newlines.Add("");
                newlines.Add("---");
                var titles = new List<string>();
                for (; i < lines.Length; i++)
                {
                    var line = lines[i];
                    newlines.Add(line);
                    if (line.StartsWith("##") && line[2] != '#')
                        titles.Add("\"" + line.Substring(2).Trim().Replace("\"", "\\\"") + "\"");
                }

                newlines[titlePos] = string.Format("contents: [{0}]", string.Join(", ", titles));
                File.WriteAllText(file, string.Join("\n", newlines));
            }
        }
    }
}
