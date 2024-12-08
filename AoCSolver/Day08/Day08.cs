namespace AoCSolver.Day08;

public class Antena
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Value { get; set; }
}

public class Day08() : Solver<List<Antena>, int> ("Day08/input.txt")
{
    private int Width { get; set; }
    private int Height { get; set; }
    
    public override List<Antena> PrepareData(List<string> input)
    {
        Width = input[0].Length;
        Height = input.Count;
        var data = new List<Antena>();
        for (var y = 0; y < input.Count; y++)
        {
            for (var x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '.')
                {
                    data.Add(
                        new Antena
                        {
                            X = x,
                            Y = y,
                            Value = input[y][x]
                        }
                    );
                }
            }
        }

        return data;
    }

    public override int Part1(List<Antena> data)
    {
        var antenaList = data.GroupBy(x => x.Value).Where(x => x.Count() > 1).ToList();
        var possibleAntinodes = new List<(int, int)>();

        foreach (var antenas in antenaList)
        {
            foreach (var antena in antenas)
            {
                possibleAntinodes.AddRange(
                    antenas
                        .Where(x => x != antena)
                        .SelectMany(x => new[]
                        {
                            (x.X - (antena.X - x.X), x.Y - (antena.Y - x.Y)),
                            (antena.X - (x.X - antena.X), antena.Y - (x.Y - antena.Y))
                        })
                        .Where(IsAntinodeValid)
                    );
            }
        }

        return possibleAntinodes
            .Distinct()
            .Count();
    }

    public override int Part2(List<Antena> data)
    {
        var antenaList = data.GroupBy(x => x.Value).Where(x => x.Count() > 1).ToList();
        var possibleAntinodes = new List<(int, int)>();

        foreach (var antenas in antenaList)
        {
            foreach (var antena in antenas)
            {
                possibleAntinodes.AddRange(
                    antenas
                        .Where(x => x != antena)
                        .SelectMany(x => 
                            GetPossibleAntinodes(antena, x, new List<(int, int)>())
                        )
                        .ToList()
                    );
            }
        }

        return possibleAntinodes
            .Distinct()
            .Count(); 
    }
    
    private List<(int, int)> GetPossibleAntinodes(Antena antena1, Antena antena2, List<(int, int)> antinodes, int depth = 0)
    {
        var antinode1 = (antena2.X - (antena1.X - antena2.X) * depth , antena2.Y - (antena1.Y - antena2.Y) * depth);
        var antinode2 = (antena1.X - (antena2.X - antena1.X) * depth, antena1.Y - (antena2.Y - antena1.Y) * depth);
        var antinode1Valid = IsAntinodeValid(antinode1);
        var antinode2Valid = IsAntinodeValid(antinode2);
        
        if (antinode1Valid)
        {
            antinodes.Add(antinode1);
        }
        if (antinode2Valid)
        {
            antinodes.Add(antinode2);
        }
        if (!antinode1Valid && !antinode2Valid)
        {
            return antinodes;
        }
        return GetPossibleAntinodes(antena1, antena2, antinodes, depth + 1);
    }
    
    private bool IsAntinodeValid((int, int) antinode)
    {
        return antinode.Item1 >= 0 && antinode.Item2 >= 0 && antinode.Item1 < Width && antinode.Item2 < Height;
    }
}