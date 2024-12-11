using System.Diagnostics;

namespace AoCSolver;

public abstract class Solver<TInput, TResult>(string? inputPath) : SolverBase where TInput : allows ref struct
{
    private string InputPath { get; set; } = inputPath ?? "input.txt";
    private Stopwatch StopwatchAllParts { get; set; } = new Stopwatch();
    private Stopwatch Stopwatch { get; set; } = new Stopwatch();

    public override void Solve()
    {
        List<TimeSpan> partTimes = [];
        Console.WriteLine("\nSolving " + GetType().Name);
        
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
            Console.WriteLine("Part 2 error: " + e.Message);
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
    public abstract void Solve();
}