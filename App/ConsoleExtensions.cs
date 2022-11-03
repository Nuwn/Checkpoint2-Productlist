using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Checkpoint2_Productlist.App
{
    public static class ConsoleEx
    {
        public static void WriteColoredLine(string text, ConsoleColor textColor)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = defaultColor;
        }

        public static void WriteColored(string text, ConsoleColor textColor)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.Write(text);
            Console.ForegroundColor = defaultColor;
        }
    }
}
