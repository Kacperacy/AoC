using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace AoCSolver.Day13;

public class Day13() : Solver<Dictionary<Day13.Point, Day13.Machine>, long>("Day13/input.txt")
{
    public record struct Point(long X, long Y);

    public readonly record struct Machine(Point ButtonA, Point ButtonB)
    {
    }

    public override Dictionary<Point, Machine> PrepareData(List<string> input)
    {
        var data = new Dictionary<Point, Machine>();
        var regex = new Regex(@"X\+(?<x>\d+), Y\+(?<y>\d+)|X=(?<px>\d+), Y=(?<py>\d+)", RegexOptions.Compiled);

        for (var i = 0; i < input.Count; i += 4)
        {
            var matchA = regex.Match(input[i]);
            var matchB = regex.Match(input[i + 1]);
            var matchP = regex.Match(input[i + 2]);

            var machine =
                new Machine(new Point(long.Parse(matchA.Groups["x"].Value), long.Parse(matchA.Groups["y"].Value)),
                    new Point(long.Parse(matchB.Groups["x"].Value), long.Parse(matchB.Groups["y"].Value)));
            var point = new Point(long.Parse(matchP.Groups["px"].Value), long.Parse(matchP.Groups["py"].Value));

            data.Add(point, machine);
        }

        return data;
    }

    public override long Part1(Dictionary<Point, Machine> data)
    {
        long totalTokens = 0;

        foreach (var (prize, machine) in data)
        {
            long minTokens = long.MaxValue;
            bool canWin = false;

            for (long a = 0; a <= 100; a++)
            {
                for (long b = 0; b <= 100; b++)
                {
                    var pos = new Point(a * machine.ButtonA.X + b * machine.ButtonB.X, a * machine.ButtonA.Y + b * machine.ButtonB.Y);
                    if (pos == prize)
                    {
                        long tokens = a * 3 + b;
                        if (tokens < minTokens)
                        {
                            minTokens = tokens;
                            canWin = true;
                        }
                    }
                }
            }

            if (canWin)
            {
                totalTokens += minTokens;
            }
        }

        return totalTokens;
    }

    public override long Part2(Dictionary<Point, Machine> data)
    {
        return data
            .Aggregate(
                0L,
                (agg, machine) =>
                {
                    var det = machine.Value.ButtonA.X * machine.Value.ButtonB.Y -
                              machine.Value.ButtonA.Y * machine.Value.ButtonB.X;
                    
                    if (det == 0)
                        return agg;

                    var target = new Point(machine.Key.X + 10_000_000_000_000L, machine.Key.Y + 10_000_000_000_000L);

                    var a = (target.X * machine.Value.ButtonB.Y - machine.Value.ButtonB.X * target.Y) / det;
                    var b = (target.Y * machine.Value.ButtonA.X - machine.Value.ButtonA.Y * target.X) / det;

                    return agg +
                           (a <= 0 || b <= 0 || a * machine.Value.ButtonA.X + b * machine.Value.ButtonB.X != target.X ||
                            a * machine.Value.ButtonA.Y + b * machine.Value.ButtonB.Y != target.Y
                               ? 0
                               : 3 * a + b);
                }
            );
    }
}