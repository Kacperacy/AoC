using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AoCSolver;

public abstract class Solver<TInput, TResult> : SolverBase where TInput : allows ref struct
{
    private string InputPath { get; }
    private Stopwatch StopwatchAllParts { get; } = new();
    private Stopwatch Stopwatch { get; } = new();

    protected Solver(string? inputPath = null, [CallerFilePath] string? sourceFile = null)
    {
        if (inputPath is not null)
        {
            InputPath = inputPath;
        }
        else if (!string.IsNullOrEmpty(sourceFile))
        {
            InputPath = Path.Combine(Path.GetDirectoryName(sourceFile)!, "input.txt");
        }
        else
        {
            var (y, d) = InferYearDayFromType(GetType());
            if (y == 0 || d == 0)
                throw new InvalidOperationException(
                    $"Nie udało się wyznaczyć Year/Day dla typu {GetType().FullName}. Upewnij się, że nazwy mają formę np. namespace: AoCSolver._2024.Day20 i klasa: Day20.");

            Year = y;
            Day = d;

            var root = FindProjectRoot() ?? AppContext.BaseDirectory;
            InputPath = Path.Combine(root, Year.ToString(), $"Day{Day:00}", "input.txt");
        }

        if (Year == 0 || Day == 0)
        {
            var inputDir = new DirectoryInfo(Path.GetDirectoryName(InputPath)!);
            var dayName = inputDir.Name; // "DayNN"
            if (dayName.StartsWith("Day", StringComparison.OrdinalIgnoreCase) && dayName.Length > 3 &&
                int.TryParse(dayName.AsSpan(3), out var d))
                Day = d;

            var yearDir = inputDir.Parent?.Name; // "2024"
            if (int.TryParse(yearDir, out var y))
                Year = y;
        }
    }

    private static (int year, int day) InferYearDayFromType(Type t)
    {
        int day = 0, year = 0;

        var name = t.Name;
        if (name.StartsWith("Day", StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(name.AsSpan(3), out var d1))
            day = d1;

        var ns = t.Namespace ?? string.Empty;
        foreach (var seg in ns.Split('.'))
        {
            if (seg.StartsWith("Day", StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(seg.AsSpan(3), out var d2))
                day = day == 0 ? d2 : day;

            if (seg.Length == 5 && seg[0] == '_' && int.TryParse(seg.AsSpan(1), out var y1))
                year = y1;
            else if (seg.Length == 4 && int.TryParse(seg, out var y2))
                year = year == 0 ? y2 : year;
        }

        return (year, day);
    }

    private static string? FindProjectRoot()
    {
        var markers = new[] { "AoCSolver.csproj" };
        string? probe = AppContext.BaseDirectory;
        for (var i = 0; i < 10 && probe is not null; i++)
        {
            if (markers.Any(m => File.Exists(Path.Combine(probe, m))))
                return probe;

            probe = Directory.GetParent(probe)?.FullName;
        }

        probe = Environment.CurrentDirectory;
        for (var i = 0; i < 10 && probe is not null; i++)
        {
            if (markers.Any(m => File.Exists(Path.Combine(probe, m))))
                return probe;
            probe = Directory.GetParent(probe)?.FullName;
        }

        return null;
    }

    public override void Solve()
    {
        List<TimeSpan> partTimes = [];
        Console.WriteLine($"\nSolving Year {Year}, Day {Day:00} ({GetType().Name})");

        StopwatchAllParts.Start();

        var input = PrepareData(File.ReadAllLines(InputPath).ToList());

        Stopwatch.Start();
        try
        {
            Console.WriteLine("Part 1 answer: " + Part1(input));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("Part 1 not implemented.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Part 1 error: " + e.Message + e.StackTrace);
        }
        Stopwatch.Stop();
        partTimes.Add(Stopwatch.Elapsed);

        Stopwatch.Start();
        try
        {
            Console.WriteLine("Part 2 answer: " + Part2(input));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("Part 2 not implemented.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Part 2 error: " + e.Message + e.StackTrace);
        }
        Stopwatch.Stop();
        partTimes.Add(Stopwatch.Elapsed);

        StopwatchAllParts.Stop();

        Console.WriteLine("Total time: " + StopwatchAllParts.Elapsed);
        Console.WriteLine("Part1 time: " + partTimes.First());
        Console.WriteLine("Part2 time: " + partTimes.Last());
    }

    public abstract TInput PrepareData(List<string> input);
    public abstract TResult Part1(TInput data);
    public abstract TResult Part2(TInput data);
}

public abstract class SolverBase
{
    public int Year { get; protected init; }
    public int Day  { get; protected init; }

    public abstract void Solve();
}