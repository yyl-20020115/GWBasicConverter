using System;
using System.IO;
using System.Text;

namespace GWBasicConverter
{
    class Program
    {
       
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("GWBasicConverter.exe input-file.bas output-file");
            }
            else
            {
                using var input = new FileStream(args[0], FileMode.Open);
                using var output = new StreamWriter(args[1]);
                GWBasicCode code = new GWBasicCode();
                try
                {
                    var lines = code.Parse(input,Encoding.UTF8); //Encoding.GetEncoding(437)
                    if (lines != null)
                    {
                        lines.ForEach((line) => output.WriteLine(line));
                        Console.WriteLine("Input file converted successfully!");
                        return 0;
                    }

                }catch(Exception e)
                {
                    Console.WriteLine("Exception:" + e.Message);
                }
            }
            return -1;
        }
    }
}
