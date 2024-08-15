using System.Text;

namespace GWBasicConverter;

public class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("GWBasicConverter.exe \"input-file.bas\" \"output-file\"");
            Console.WriteLine("                  or \"input-folder\" \"output-folder\"");
        }
        else
        {
            var src = args[0];
            var dst = args[1];
            if (Directory.Exists(src))
            {
                if (!Directory.Exists(dst))
                {
                    try
                    {
                        Directory.CreateDirectory(dst);
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine("Exception:" + e.Message);
                    }

                }

                foreach (var file in Directory.GetFiles(src, "*.BAS"))
                {
                    if (File.ReadAllBytes(file) is byte[] buffer && buffer.Length>0 
                        && buffer[0]== GWBasicCodeGenerator.MagicByte)
                    {
                        var name = Path.GetFileNameWithoutExtension(file);
                        Convert(file, Path.Combine(dst, $"{name}_Converted.BAS"));
                    }
                    else
                    {
                        Console.Error.WriteLine($"Input file \"{file}\" is not an encoded file.");
                    }
                }
                return 0;
            }
            else if (File.Exists(src))
            {
                if (File.ReadAllBytes(src) is byte[] buffer && buffer.Length > 0
                        && buffer[0] == GWBasicCodeGenerator.MagicByte)
                {
                    return Convert(src, dst) ? 0 : -1;
                }
                else
                {
                    Console.Error.WriteLine($"Input file \"{src}\" is not an encoded file.");
                    return -1;
                }
            }
        }
        return -1;
    }
    public static bool Convert(string src, string dst)
    {
        using var input = new FileStream(src, FileMode.Open);
        using var output = new StreamWriter(dst);
        var code = new GWBasicCodeGenerator();
        try
        {
            var lines = code.Parse(input, Encoding.UTF8); //Encoding.GetEncoding(437)
            if (lines != null)
            {
                lines.ForEach(output.WriteLine);
                Console.WriteLine($"Input file \"{src}\" converted to \"{dst}\" successfully!");
                return true;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Exception:" + e.Message);
        }
        return false;
    }
}
