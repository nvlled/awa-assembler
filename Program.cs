using System.Text.RegularExpressions;

var prog = Path.GetFileName(Environment.ProcessPath);
var cliArgs = Environment.GetCommandLineArgs();
if (cliArgs.Length == 1)
{
    Console.WriteLine($"usage: {prog} <filename>");
    return;
}

var lineNum = 0;
var lines = File.ReadAllLines(cliArgs[1]);
var env = new Dictionary<string, int>();

// all programs begins with awa
Console.Write("awa");

foreach (var line in lines)
{
    lineNum++;

    var code = new Regex("//.*$").Replace(line, "").Trim();

    code = AwaExtension.SubstituteVars(code, env);

    var fields = new Regex("\\s+").Split(code, 2);

    if (code.Length == 0) continue;

    if (fields.Length < 1)
    {
        Console.WriteLine("invalid syntax: {0}, at line {1}", code, lineNum);
        return;
    }

    var command = fields[0];
    var strArg = fields.Length > 1 ? fields[1] : null;

    if (command.StartsWith("#"))
    {
        AwaExtension.RunDirective(env, command, strArg);
    }
    else
    {
        var value = Awa.Encode(command, strArg);
        Console.Write($" {value}");
    }
}

Console.WriteLine();
