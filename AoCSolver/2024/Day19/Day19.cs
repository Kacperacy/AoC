namespace AoCSolver._2024.Day19;

public class Day19 : Solver<Day19.Towels, long>
{
    public record struct Towels(List<string> Patterns, List<string> Designs);

    public override Towels PrepareData(List<string> input)
    {
        var patterns = input[0].Split(", ").ToList();
        var designs = input.Skip(2).ToList();
        return new Towels(patterns, designs);
    }

    public override long Part1(Towels data)
    {
        return data.Designs.Count(design => CanFormDesign(design, data.Patterns));
    }

    public override long Part2(Towels data)
    {
        return data.Designs.Sum(design => CountWaysToFormDesign(design, data.Patterns));
    }

    private static bool CanFormDesign(string design, List<string> towelPatterns)
    {
        var n = design.Length;
        var dp = new bool[n + 1];
        dp[0] = true;

        for (var i = 1; i <= n; i++)
        {
            foreach (var pattern in towelPatterns)
            {
                var len = pattern.Length;
                if (i >= len && design.Substring(i - len, len) == pattern)
                {
                    dp[i] = dp[i] || dp[i - len];
                }
            }
        }

        return dp[n];
    }

    private static long CountWaysToFormDesign(string design, List<string> towelPatterns)
    {
        long n = design.Length;
        var dp = new long[n + 1];
        dp[0] = 1;

        for (var i = 1; i <= n; i++)
        {
            foreach (var pattern in towelPatterns)
            {
                var len = pattern.Length;
                if (i >= len && design.Substring(i - len, len) == pattern)
                {
                    dp[i] += dp[i - len];
                }
            }
        }

        return dp[n];
    }
}