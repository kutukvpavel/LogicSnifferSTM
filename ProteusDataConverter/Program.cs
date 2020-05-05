using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace ProteusDataConverter
{
    class Program
    {
        /*class Table : List<KeyValuePair<int, int>>
        {
            public Table(int capacity) : base(capacity) { }
            public void Add(int key, int value)
            {
                base.Add(new KeyValuePair<int, int>(key, value));
            }
        }*/
        static NumberFormatInfo nfi = (NumberFormatInfo)NumberFormatInfo.InvariantInfo.Clone();
        static void Main(string[] args)
        {
            nfi.NumberDecimalDigits = 10;
            if (args.Length == 0)
            {
                Console.WriteLine("No file path specified. Please enter a file path:");
                args = new string[] { Console.ReadLine() };
            }
            Console.WriteLine("Loading data...");
            args[0] = args[0].Trim('"');
            string[] lines;
            try
            {
                lines = File.ReadAllText(args[0]).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => x.Trim('\r', '\n')).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open file: " + ex.Message);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            //Table result = new Table(lines.Length);
            Console.WriteLine("Parsing...");
            StringBuilder res = new StringBuilder(20 * lines.Length);
            string[] buf;
            bool wait = false;
            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    buf = lines[i].Split(',');
                    res.Append(((int)Math.Round(1000000f * float.Parse(buf[0], nfi))).ToString("D"));
                    res.Append(':');
                    res.AppendLine(((buf[1][2] == 'H') ? 1u : 0u).ToString("D"));
                    //result.Add((int)Math.Round(1000000f * float.Parse(buf[0])), (buf[1][2] == 'H') ? 1 : 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    wait = true;
                    Console.WriteLine("Trying to correct damaged fragment...");
                    try
                    {
                        int index = res.ToString().LastIndexOf(Environment.NewLine) + Environment.NewLine.Length;
                        res.Remove(index, res.ToString().Length - index);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error can not be corrected: " + e.Message);
                        Console.WriteLine("Parsing stopped.");
                        break; 
                    }
                }
            }
            Console.WriteLine("Saving LSSTM-compatible file...");
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(args[0]), Path.GetFileName(args[0]).Replace(".", "_STM.")), res.ToString());
            if (wait)
            {
                Console.WriteLine("Some exceptions have been caught during parsing!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
