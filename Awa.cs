
using System.Text;


// See file AWA5.0 Specs.pdf for reference,
// downloaded from https://github.com/TempTempai/AWA5.0

public static class Awa
{
    static char[] chars = new char[] {
            'A', 'W', 'a', 'w',

            'J', 'E', 'L', 'Y', 'H', 'O', 'S', 'I', 'U', 'M',
            'j', 'e', 'l', 'y', 'h', 'o', 's', 'i', 'u', 'm',

            'P', 'C', 'N', 'T',
            'p', 'c', 'n', 't',

            'B', 'D', 'F', 'G', 'R',
            'b', 'd', 'f', 'g', 'r',

            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',

            ' ', '.', ',', '!', '‘',
             '(', ')', '~', '_',
            '/', ';', '\n',
    };


    // TODO: return a proper error type
    public static string Encode(string command, string? strArg)
    {
        switch (command)
        {

            case "nop":
            case "prn":
            case "pr1":
            case "red":
            case "r3d":
            case "pop":
            case "dpl":
            case "mrg":
            case "4dd":
            case "sub":
            case "mul":
            case "div":
            case "cnt":
            case "eql":
            case "lss":
            case "gr8":
            case "dbg":
            case "trm":
                if (strArg is not null)
                {
                    return $"unexpected arg: cmd='{command}' arg='{strArg}'";
                }
                return Awawafy(EncodeCommand(command));

            case "blo":
                {
                    var (arg, ok) = ParseCommandArg(strArg);
                    if (!ok)
                        return $"invalid arg: cmd='{command}' arg='{strArg}'";
                    return $"{Awawafy(EncodeCommand(command))} {Awawafy(arg, 8)}";
                }

            case "srn":
            case "lbl":
            case "jmp":
            case "sbm":
                {
                    var (arg, ok) = ParseCommandArg(strArg);
                    if (!ok)
                        return $"invalid arg: cmd='{command}' arg='{strArg}'";
                    return $"{Awawafy(EncodeCommand(command))} {Awawafy(arg, 5)}";
                }


            default:
                return $"invalid command: cmd='{command}' arg='{strArg}'";
        }

    }

    public static int EncodeCommand(string command)
    {
        switch (command)
        {
            case "nop": return 0x0;
            case "prn": return 0x1;
            case "pr1": return 0x2;
            case "red": return 0x3;
            case "r3d": return 0x4;
            case "blo": return 0x5;
            case "sbm": return 0x6;
            case "pop": return 0x7;
            case "dpl": return 0x8;
            case "srn": return 0x9;
            case "mrg": return 0xa;
            case "4dd": return 0xb;
            case "sub": return 0xc;
            case "mul": return 0xd;
            case "div": return 0xe;
            case "cnt": return 0xf;
            case "lbl": return 0x10;
            case "jmp": return 0x11;
            case "eql": return 0x12;
            case "lss": return 0x13;
            case "gr8": return 0x14;
            case "dbg": return 0x1e;
            case "trm": return 0x1f;
            default: return -1;
        }

    }

    public static string Awawafy(int opcode, int numBits = 5)
    {
        var sb = new StringBuilder();
        numBits--;

        if (opcode >> numBits == 1)
        {
            sb.Append('~');
        }


        while (numBits >= 0)
        {
            var b = (opcode >> numBits) & 1;
            if (b == 0)
                sb.Append(' ');
            sb.Append(b == 1 ? "wa" : "awa");
            //sb.Append(b == 1 ? 1 : 0);
            numBits--;
        }

        return sb.ToString().Trim();
    }

    public static int GetCharacterCode(char ch)
    {
        var index = Array.IndexOf(chars, ch);
        return index;
    }

    public static (byte, bool) ParseCommandArg(string? arg)
    {
        if (arg is null || arg.Length == 0) return (0, false);
        if (arg[0] == '\'')
        {
            if (arg.Length != 3 || arg[2] != '\'') return (0, false);
            var code = GetCharacterCode(arg[1]);
            return ((byte)code, code >= 0);
        }

        byte n;
        var ok = byte.TryParse(arg, out n);

        return (n, ok);
    }

}