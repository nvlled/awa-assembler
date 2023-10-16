

public static class AwaExtension
{
    public static void RunDirective(Dictionary<string, int> env, string command, string? strArg)
    {
        command = command.Replace("#", "");
        switch (command)
        {
            case "define":
                {
                    var fields = strArg?.Split(" ").Select(x => x.Trim()).ToArray();
                    fields ??= new string[] { };

                    int value;
                    if (fields.Length >= 0 && int.TryParse(fields[1], out value))
                    {
                        env[fields[0]] = value;
                    }
                    break;
                }
        }
    }

    public static string SubstituteVars(string code, Dictionary<string, int> env)
    {
        foreach (var e in env)
        {
            code = code.Replace("$" + e.Key, e.Value.ToString());
        }
        return code;
    }
}