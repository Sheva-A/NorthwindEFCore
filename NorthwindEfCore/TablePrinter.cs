namespace NorthwindEfCore;

// Виводить список об'єктів у консоль у вигляді таблиці
public static class TablePrinter
{
    private const string Separator = "  ";

    public static void Print<T>(string title, IEnumerable<T> rows)
    {
        var properties = typeof(T).GetProperties();
        var headers = properties.Select(p => p.Name).ToArray();
        var cells = rows
            .Select(row => properties.Select(p => Format(p.GetValue(row))).ToArray())
            .ToList();

        // Ширина колонки = найдовше значення в ній
        var widths = headers
            .Select((header, i) => cells.Select(c => c[i].Length).Append(header.Length).Max())
            .ToArray();

        Console.WriteLine($"\n{title}");
        PrintRow(headers, widths);
        PrintRow(widths.Select(w => new string('-', w)).ToArray(), widths);
        cells.ForEach(c => PrintRow(c, widths));
    }

    private static void PrintRow(string[] values, int[] widths)
    {
        var line = string.Join(Separator, values.Select((v, i) => v.PadRight(widths[i])));
        Console.WriteLine(line.TrimEnd());
    }

    private static string Format(object? value) => value switch
    {
        null => "NULL",
        DateTime date => date.ToString("yyyy-MM-dd"),
        decimal number => number.ToString("0.00"),
        _ => value.ToString() ?? ""
    };
}
