namespace AoCSolver._2025.Day01;

public class Day01 : Solver<List<(char, int)>, int>
{
    public override List<(char, int)> PrepareData(List<string> input)
    {
        var result = input
            .Select(line => (line[0], int.Parse(line[1..])))
            .ToList();
        
        return result;
    }

    public override int Part1(List<(char, int)> data)
    {
        var dial = 50;
        var result = 0;

        foreach (var move in data)
        {
            var sign = move.Item1 == 'R' ? 1 : -1;
            
            dial += move.Item2 * sign;

            dial %= 100;
            
            if (dial == 0) result++;
        }
        
        return result;
    }

    public override int Part2(List<(char, int)> data)
    {
        var dial = 50;
        var result = 0;

        foreach (var move in data)
        {
            var sign = move.Item1 == 'R' ? 1 : -1;

            for (int i = 0; i < move.Item2; i++)
            {
                dial += sign;
                dial %= 100;
                if (dial == 0) result++;
            }
        }
        
        return result;
    }
}