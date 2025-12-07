namespace AoCSolver._2025.Day03;

public class Day03 : Solver<List<string>, long>
{
    public override List<string> PrepareData(List<string> input) => input;
    
    public override long Part1(List<string> data)
    {
        var result = 0L;
        
        foreach (var battery in data)
        {
            result += GetHighestNumber(battery, 2);
        }
        
        return result;
    }

    public override long Part2(List<string> data)
    {
        var result = 0L;
        
        foreach (var battery in data)
        {
            result += GetHighestNumber(battery, 12);
        }
        
        return result;
    }

    private long GetHighestNumber(string number, int amount)
    {
        var values = number.ToCharArray();
        var result = new char[amount];
        var currentIndex = 0;

        for (int i = 0; i < amount; i++)
        {
            var next = values[currentIndex..^(amount - 1 - i)].Max();
            currentIndex += Array.IndexOf(values[currentIndex..], next) + 1;
            result[i] = next;
        }
        
        return long.Parse(string.Concat(result));
    }
}