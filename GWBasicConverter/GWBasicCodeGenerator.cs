using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GWBasicConverter
{
    public class GWBasicCodeGenerator
    {
        public const byte MagicByte = 0xff;
        public static readonly Dictionary<int, string> Tokens = new Dictionary<int, string>
        {
            [0x81] = "END",
            [0x82] = "FOR",
            [0x83] = "NEXT",
            [0x84] = "DATA",
            [0x85] = "INPUT",
            [0x86] = "DIM",
            [0x87] = "READ",
            [0x88] = "LET",
            [0x89] = "GOTO",
            [0x8A] = "RUN",
            [0x8B] = "IF",
            [0x8C] = "RESTORE",
            [0x8D] = "GOSUB",
            [0x8E] = "RETURN",
            [0x8F] = "REM",
            [0x90] = "STOP",
            [0x91] = "PRINT",
            [0x92] = "CLEAR",
            [0x93] = "LIST",
            [0x94] = "NEW",
            [0x95] = "ON",
            [0x96] = "WAIT",
            [0x97] = "DEF",
            [0x98] = "POKE",
            [0x99] = "CONT",
            [0x9A] = "(Undefined)",
            [0x9B] = "(Undefined)",
            [0x9C] = "OUT",
            [0x9D] = "LPRINT",
            [0x9E] = "LLIST",
            [0x9F] = "(Undefined)",
            [0xA0] = "WIDTH",
            [0xA1] = "ELSE",
            [0xA2] = "TRON",
            [0xA3] = "TROFF",
            [0xA4] = "SWAP",
            [0xA5] = "ERASE",
            [0xA6] = "EDIT",
            [0xA7] = "ERROR",
            [0xA8] = "RESUME",
            [0xA9] = "DELETE",
            [0xAA] = "AUTO",
            [0xAB] = "RENUM",
            [0xAC] = "DEFSTR",
            [0xAD] = "DEFINT",
            [0xAE] = "DEFSNG",
            [0xAF] = "DEFDBL",
            [0xB0] = "LINE",
            [0xB1] = "WHILE",
            [0xB2] = "WEND",
            [0xB3] = "CALL",
            [0xB4] = "(Undefined)",
            [0xB5] = "(Undefined)",
            [0xB6] = "(Undefined)",
            [0xB7] = "WRITE",
            [0xB8] = "OPTION",
            [0xB9] = "RANDOMIZE",
            [0xBA] = "OPEN",
            [0xBB] = "CLOSE",
            [0xBC] = "LOAD",
            [0xBD] = "MERGE",
            [0xBE] = "SAVE",
            [0xBF] = "COLOR",
            [0xC0] = "CLS",
            [0xC1] = "MOTOR",
            [0xC2] = "BSAVE",
            [0xC3] = "BLOAD",
            [0xC4] = "SOUND",
            [0xC5] = "BEEP",
            [0xC6] = "PSET",
            [0xC7] = "PRESET",
            [0xC8] = "SCREEN",
            [0xC9] = "KEY",
            [0xCA] = "LOCATE",
            [0xCB] = "(Undefined)",
            [0xCC] = "TO",
            [0xCD] = "THEN",
            [0xCE] = "TAB(",
            [0xCF] = "STEP",
            [0xD0] = "USR",
            [0xD1] = "FN",
            [0xD2] = "SPC(",
            [0xD3] = "NOT",
            [0xD4] = "ERL",
            [0xD5] = "ERR",
            [0xD6] = "STRING$",
            [0xD7] = "USING",
            [0xD8] = "INSTR",
            [0xD9] = "'",
            [0xDA] = "VARPTR",
            [0xDB] = "CSRLIN",
            [0xDC] = "POINT",
            [0xDD] = "OFF",
            [0xDE] = "INKEY$",
            [0xDF] = "(Undefined)",
            [0xE0] = "(Undefined)",
            [0xE1] = "(Undefined)",
            [0xE2] = "(Undefined)",
            [0xE3] = "(Undefined)",
            [0xE4] = "(Undefined)",
            [0xE5] = "(Undefined)",
            [0xE6] = ">",
            [0xE7] = "=",
            [0xE8] = "<",
            [0xE9] = "+",
            [0xEA] = "-",
            [0xEB] = "*",
            [0xEC] = "/",
            [0xED] = "^",
            [0xEE] = "AND",
            [0xEF] = "OR",
            [0xF0] = "XOR",
            [0xF1] = "EQV",
            [0xF2] = "IMP",
            [0xF3] = "MOD",
            [0xF4] = "\\",
            [0xF5] = "(Undefined)",
            [0xF6] = "(Undefined)",
            [0xF7] = "(Undefined)",
            [0xF8] = "(Undefined)",
            [0xF9] = "(Undefined)",
            [0xFA] = "(Undefined)",
            [0xFB] = "(Undefined)",
            [0xFC] = "(Undefined)",

            [0xFD81] = "CVI",
            [0xFD82] = "CVS",
            [0xFD83] = "CVD",
            [0xFD84] = "MKI$",
            [0xFD85] = "MKS$",
            [0xFD86] = "MKD$",
            [0xFD8B] = "EXTERR",
            [0xFE81] = "FILES",
            [0xFE82] = "FIELD",
            [0xFE83] = "SYSTEM",
            [0xFE84] = "NAME",
            [0xFE85] = "LSET",
            [0xFE86] = "RSET",
            [0xFE87] = "KILL",
            [0xFE88] = "PUT",
            [0xFE89] = "GET",
            [0xFE8A] = "RESET",
            [0xFE8B] = "COMMON",
            [0xFE8C] = "CHAIN",
            [0xFE8D] = "DATE$",
            [0xFE8E] = "TIME$",
            [0xFE8F] = "PAINT",
            [0xFE90] = "COM",
            [0xFE91] = "CIRCLE",
            [0xFE92] = "DRAW",
            [0xFE93] = "PLAY",
            [0xFE94] = "TIMER",
            [0xFE95] = "ERDEV",
            [0xFE96] = "IOCTL",
            [0xFE97] = "CHDIR",
            [0xFE98] = "MKDIR",
            [0xFE99] = "RMDIR",
            [0xFE9A] = "SHELL",
            [0xFE9B] = "ENVIRON",
            [0xFE9C] = "VIEW",
            [0xFE9D] = "WINDOW",
            [0xFE9E] = "PMAP",
            [0xFE9F] = "PALETTE",
            [0xFEA0] = "LCOPY",
            [0xFEA1] = "CALLS",
            [0xFEA4] = "NOISE",  //   (PCjr only) DEBUG   (Sperry PC only)",
            [0xFEA5] = "PCOPY",
            [0xFEA6] = "TERM",
            [0xFEA7] = "LOCK",
            [0xFEA8] = "UNLOCK",
            [0xFF81] = "LEFT$",
            [0xFF82] = "RIGHT$",
            [0xFF83] = "MID$",
            [0xFF84] = "SGN",
            [0xFF85] = "INT",
            [0xFF86] = "ABS",
            [0xFF87] = "SQR",
            [0xFF88] = "RND",
            [0xFF89] = "SIN",
            [0xFF8A] = "LOG",
            [0xFF8B] = "EXP",
            [0xFF8C] = "COS",
            [0xFF8D] = "TAN",
            [0xFF8E] = "ATN",
            [0xFF8F] = "FRE",
            [0xFF90] = "INP",
            [0xFF91] = "POS",
            [0xFF92] = "LEN",
            [0xFF93] = "STR$",
            [0xFF94] = "VAL",
            [0xFF95] = "ASC",
            [0xFF96] = "CHR$",
            [0xFF97] = "PEEK",
            [0xFF98] = "SPACE$",
            [0xFF99] = "OCT$",
            [0xFF9A] = "HEX$",
            [0xFF9B] = "LPOS",
            [0xFF9C] = "CINT",
            [0xFF9D] = "CSNG",
            [0xFF9E] = "CDBL",
            [0xFF9F] = "FIX",
            [0xFFA0] = "PEN",
            [0xFFA1] = "STICK",
            [0xFFA2] = "STRIG",
            [0xFFA3] = "EOF",
            [0xFFA4] = "LOC",
            [0xFFA5] = "LOF"
        };

        public static readonly Regex Canon1 = new Regex(@"^([\-])*0\.");
        public static readonly Regex Canon2 = new Regex(@"\.0$");

        // Float binary format: http://www.chebucto.ns.ca/~af380/GW-BASIC-tokens.html
        // Rounding and postfixes: https://www-user.tu-chemnitz.de/~heha/viewchm.php/hs/gwBasic.chm/Chapter6.html
        protected string ParseFloat32(byte[] data,int pos)
        {
            if (data[pos + 3] == 0) return "0";
            int exp = data[pos + 3] - 152;// -152 = -128 + -24 (24 because the significand is behind a decimal dot)
            int mantissa = ((data[pos + 2] | 0x80) << 16) | (data[pos + 1] << 8) | data[pos];

            float nf = 0.0f;
            if ((data[pos + 2] & 0x80) != 0)
            {
                nf = -(float)(mantissa * Math.Pow(2.0, exp)); 
            }
            else
            {
                nf = +(float)(mantissa * Math.Pow(2.0, exp));
            }
            // Must round to 6 significant figures (from 7) when displaying
            string ns = this.CanonizeNumber(string.Format("{0:0.000000}",nf));
            //If nothing indicates that this is a float, then add the "!" postfix
            if (ns.IndexOfAny(new[] { '.', 'E' }) <0)
                ns += '!';
            return ns;
        }
        // Double binary format: http://www.chebucto.ns.ca/~af380/GW-BASIC-tokens.html
        // Rounding and postfixes: https://www-user.tu-chemnitz.de/~heha/viewchm.php/hs/gwBasic.chm/Chapter6.html
        protected string ParseFloat64(byte[] data, int pos)
        {
            if (data[pos + 7] == 0) return "0";
            int exp = data[pos + 7] - 184;// -184 = -128 + -56 (56 because the significand is behind a decimal dot)
            long mantissa 
                = (((long)(data[pos + 6] | 0x80)) << 48) 
                | (((long)data[pos + 5]) << 40) 
                | (((long)data[pos + 4]) << 32)
                | (((long)data[pos + 3]) << 24) 
                | (((long)data[pos + 2]) << 16) 
                | (((long)data[pos + 1]) << 8) 
                | data[pos];

            // We must always output a positive number for doubles,
            // because a token for '-' is already added before the negative ones.

            double nf = mantissa * Math.Pow(2.0,exp);

            // Must round to 16 significant figures (from 17) when displaying
            // The exponent sign for doubles is 'D' instead of 'E'
            string ns = this.CanonizeNumber(string.Format("{0:0.0000000000000000}", nf)).Replace('E','D');

            // Doubles only get their postfix '#' when they don't contain the exponentiation letter 'D'
            if (ns.IndexOf('D') < 0)
                ns += '#';
            return ns;
        }

        protected string CanonizeNumber(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            //s = Canon1.Replace(s, @"\1.");
            //s = Canon2.Replace(s, string.Empty);
            return s.ToUpper();
        }

        protected void CheckBoundary(List<string> lines,byte[] data, int pos,int required)
        {
            if (data.Length - pos - 1 < required)
            {
                if (lines.Count > 0)
                {
                    throw new Exception($"Unexpected end of file after line {lines.Count}.");
                }
                else
                {
                    throw new Exception($"Unexpected end of file at byte position {pos}.");
                }
            }
        }
        protected string DecodeText(byte[] buffer, Encoding encoding) => encoding.GetString(buffer);
        protected string DecodeText(List<byte> buffer, Encoding encoding) => this.DecodeText(buffer.ToArray(), encoding);
        protected void EnsureBufferSaved(StringBuilder builder, List<byte> buffer, Encoding encoding)
        {
            if (buffer.Count > 0)
            {
                builder.Append(this.DecodeText(buffer, encoding));
                buffer.Clear();
            }
        }
        protected int ParseLine(List<string> lines,byte[] data,int pos, Encoding encoding)
        {
            this.CheckBoundary(lines, data, pos, 2);

            if (data[pos + 0] == 0 && data[pos + 1] == 0) return -1;
            var o = pos;

            pos += 2;
            this.CheckBoundary(lines, data, pos, 2);

            var isRemming = false;
            var isQuoting = false;
            var builder = new StringBuilder();
            var buffer = new List<byte>();
            var ln = BitConverter.ToUInt16(data, pos);

            builder.Append($"{ln}\t");

            pos += 2;

            while (data[pos] != 0)
            {
                this.CheckBoundary(lines, data, pos, 1);
                var code = data[pos];
                var ext = (((int)code)<<8) | data[pos + 1];
                if (code == 0x22 && !isRemming)
                {
                    // There was no quote escaping. You had to use CHR$() to
                    // output a quote character.
                    isQuoting = !isQuoting;
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append('"');
                    pos++;
                }
                else if (code == 0x3a 
                    && !(isQuoting || isRemming) 
                    && data.Length - pos - 1 > 2
                    && data[pos + 1] == 0x8f 
                    && data[pos + 2] == 0xd9)
                {
                    // REM block starts
                    // A single quote is an alias for a REM instruction
                    // It is stored with 3 bytes: 0x3a8fd9
                    isRemming = true;    // a REM block never ends (inside a line)
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append('\'');
                    pos += 3;
                }
                else if ((code >= 0x20 && code <= 0x7e) || isQuoting || isRemming)
                {
                    buffer.Add(code);//save bytes for decoding
                    pos++;
                }
                else if (code == 0x8f)
                {
                    // REM block starts
                    isRemming = true; // a REM block never ends (inside a line)
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append("REM");
                    pos++;
                }
                else if (code == 0x0b) //octal constant
                {
                    //Signed, but that's not visible in octal representation
                    this.CheckBoundary(lines, data, pos, 2);
                    List<string> os = new List<string>();
                    var oct = BitConverter.ToUInt16(data, pos + 1);
                    while (oct > 0)
                    {
                        os.Add((oct & 0x07).ToString());
                        oct >>= 3;
                    }
                    if (os.Count == 0) os.Add("0");
                    os.Reverse();
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(os.Aggregate("&0", (a, b) => a + b));
                    pos += 3;
                }
                else if (code == 0x0c) //hex constant
                {
                    //signed, but that's not visible in hexa representation
                    this.CheckBoundary(lines, data, pos, 2);
                    var hex = BitConverter.ToUInt16(data, pos + 1);
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.AppendFormat("&H{0:X}", hex);
                    pos += 3;
                }
                else if (code == 0x0d) // line pointer (unsigned)
                {
                    throw new InvalidDataException(
                        "line pointer (0x0d) shouldn't occur in saved program.");
                }
                else if (code == 0x0e) // line number (unsigned)
                {
                    this.CheckBoundary(lines, data, pos, 2);
                    var lineNumber = BitConverter.ToUInt16(data, pos + 1);
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.AppendFormat("{0} ", lineNumber);
                    pos += 3;
                }
                else if (code == 0x0f) //one byte constant
                {
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.AppendFormat("{0}", data[pos + 1]);
                    pos += 2;
                }
                else if (code == 0x10) //Flags constant (unused)
                {
                    throw new InvalidDataException("unexpected 0x10 token");
                }
                else if (code >= 0x11 && code <= 0x1b) // Numbers from 0 to 10 have their own tokens
                {
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.AppendFormat("{0}", code - 0x11);
                    pos++;
                }
                else if (code == 0x1c) //two byte integer constant (signed)
                {
                    this.CheckBoundary(lines, data, pos, 2);
                    var si = BitConverter.ToInt16(data, pos + 1);

                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(si);

                    pos += 3;
                }
                else if (code == 0x1d) //four byte floating point constant
                {
                    this.CheckBoundary(lines, data, pos, 4);
                    var si = this.ParseFloat32(data, pos + 1);

                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(si);

                    pos += 5;
                }
                else if (code == 0x1e) //unused
                {
                    throw new InvalidDataException("unexpected 0x1e token");
                }
                else if (code == 0x1f) //eight byte double value
                {
                    this.CheckBoundary(lines, data, pos, 8);
                    var si = this.ParseFloat64(data, pos + 1);

                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(si);

                    pos += 9;
                }
                else if (Tokens.ContainsKey(code)) //1-byte tokens
                {                    
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(Tokens[code]);
                    pos++;
                }
                else if (Tokens.ContainsKey(ext)) //2-byte tokens
                {
                    //The boundary for that +1 is already checked at the beginning of the loop.
                    this.EnsureBufferSaved(builder, buffer, encoding);
                    builder.Append(Tokens[ext]);
                    pos += 2;
                }
                else
                {                    
                    throw new InvalidDataException("unexpected token: " + code);
                }
            }
            pos++;

            this.EnsureBufferSaved(builder, buffer, encoding);

            lines.Add(builder.ToString());

            return pos - o;
        }

        public List<string> Parse(Stream stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead ||!stream.CanSeek) throw new ArgumentException(nameof(stream));
            stream.Seek(0, SeekOrigin.Begin);
            long len = stream.Length;
            if (len>=int.MaxValue) throw new ArgumentOutOfRangeException(nameof(stream));
            var data = new byte[len];
            int r = stream.Read(data, 0, (int)len);
            
            return this.Parse(data, encoding);
        }
        public List<string> Parse(byte[] data, Encoding encoding = null)
        {
            encoding ??= Encoding.Default;
            var lines = new List<string>(); 
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
            {
                //OK    
            }
            else if (data[0] != MagicByte)
            {
                throw new BadImageFormatException($"{nameof(MagicByte)} ({MagicByte}) is not found!");
            }
            else //
            {
                var pos = 1;
                while (pos < data.Length - 1)
                {
                    var delta = this.ParseLine(lines, data, pos, encoding);
                    if (delta < 0) break;
                    pos += delta;
                }
            }
            
            return lines;
        }
    }
}
