using System;
namespace Chronopost.Data.TableauBuild
{
	public class TableauBuild
	{
        static int tableWidth = 100;
        internal static void BuildFind(string RowLeft, string RowRight, string printLeft, string printright, string Raw1Right = "", string Raw1Left = "")
        {
            PrintLine();
            PrintRow(RowLeft, RowRight);
            PrintRow($"{Raw1Left}", $"{Raw1Right}");
            PrintLine();
            print(printLeft, printright);
        }

        internal static void BuildFind1(string RowLeft, string RowRight, string printLeft, string printright)
        {
            PrintLine();
            PrintRow(RowLeft, RowRight);
            PrintLine();
            print(printLeft, printright);
        }

        static void print(string arg, string arg1)
        {
            PrintRow($"{arg}", $"{arg1}");
            PrintLine();
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}

