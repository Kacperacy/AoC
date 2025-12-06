namespace AoCSolver._2024.Day06;

public class Day06 : Solver<List<List<char>>, int>
{
    public override List<List<char>> PrepareData(List<string> input) =>
        input.Select(x => x.ToCharArray().ToList()).ToList();

    public override int Part1(List<List<char>> data)
    {
        var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
        var visited = new HashSet<(int, int)>();
        var pos = (from i in Enumerable.Range(0, data.Count)
            from j in Enumerable.Range(0, data[i].Count)
            where data[i][j] == '>' || data[i][j] == '<' || data[i][j] == '^' || data[i][j] == 'v'
            select (i, j)).FirstOrDefault();
        var dir = data[pos.Item1][pos.Item2] switch
        {
            '^' => 0,
            '>' => 1,
            'v' => 2,
            '<' => 3,
        };
        visited.Add(pos);
        
        while (true)
        {
            var newPos = (pos.Item1 + directions[dir].Item1, pos.Item2 + directions[dir].Item2);
            if (newPos.Item1 < 0 || newPos.Item1 >= data.Count || newPos.Item2 < 0 || newPos.Item2 >= data[0].Count)
            {
                break;
            }
            if (data[newPos.Item1][newPos.Item2] == '#')
            {
                dir = (dir + 1) % 4;
                continue;
            }
            visited.Add(newPos);
            pos = newPos;
        }
        
        return visited.Count;
    }

    public override int Part2(List<List<char>> data)
    {
        var sum = 0;
        for(var i = 0; i < data.Count; i++)
        {
            for(var j = 0; j < data[i].Count; j++)
            {
                if (data[i][j] != '.') continue;
                data[i][j] = '#';
                
                var directions = new[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
                var visited = new HashSet<(int, int)>();
                var pos = (from x in Enumerable.Range(0, data.Count)
                    from y in Enumerable.Range(0, data[x].Count)
                    where data[x][y] == '>' || data[x][y] == '<' || data[x][y] == '^' || data[x][y] == 'v'
                    select (x, y)).FirstOrDefault();
                var dir = data[pos.Item1][pos.Item2] switch
                {
                    '^' => 0,
                    '>' => 1,
                    'v' => 2,
                    '<' => 3,
                };
                visited.Add(pos);
                var dirChangesWithoutNewDir = 0;
        
                while (true)
                {
                    var newPos = (pos.Item1 + directions[dir].Item1, pos.Item2 + directions[dir].Item2);
                    if (newPos.Item1 < 0 || newPos.Item1 >= data.Count || newPos.Item2 < 0 || newPos.Item2 >= data[0].Count)
                    {
                        break;
                    }
                    if (data[newPos.Item1][newPos.Item2] == '#')
                    {
                        dir = (dir + 1) % 4;
                        dirChangesWithoutNewDir++;
                        if (dirChangesWithoutNewDir == 4)
                        {
                            sum++;
                            break;
                        }
                        continue;
                    }
                    if (!visited.Contains(newPos)) dirChangesWithoutNewDir = 0;
                    visited.Add(newPos);
                    pos = newPos;
                }
        
                data[i][j] = '.';
            }
        }

        return sum;
    }
}