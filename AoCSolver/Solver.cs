using System.Diagnostics;

namespace AoCSolver;

public abstract class Solver<TInput, TResult>(string? inputPath) : SolverBase
{
    private string InputPath { get; set; } = inputPath ?? "input.txt";
    private Stopwatch Stopwatch { get; set; } = new Stopwatch();

    public override void Solve()
    {
        Console.WriteLine("Solving " + GetType().Name);
        Stopwatch.Start();
        var input = PrepareData(File.ReadAllLines(InputPath).ToList());
        Console.WriteLine("Part 1: " + Part1(input));
        Console.WriteLine("Part 2: " + Part2(input));
        Stopwatch.Stop();
        Console.WriteLine("Time: " + Stopwatch.Elapsed);
    }

    public abstract TInput PrepareData(List<string> input);

    public abstract TResult Part1(TInput data);

    public abstract TResult Part2(TInput data);
}

public abstract class SolverBase
{
    public abstract void Solve();
}