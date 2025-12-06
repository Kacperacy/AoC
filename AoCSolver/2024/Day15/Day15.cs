namespace AoCSolver._2024.Day15;

public class Day15 : Solver<Day15.Warehouse, int>
{
    public record struct Point(int X, int Y)
    {
        public Point Step(Point move) => new Point(X + move.X, Y + move.Y);
    }

    public record struct Warehouse
    {
        public List<List<char>> Map { get; set; }
        public List<Point> Walls { get; set; }
        public List<Point> Boxes { get; set; }
        public List<(Point, Point)> WideBoxes { get; set; }
        public Point Robot { get; set; }
        public List<char> Directions { get; set; }

        public Warehouse(List<List<char>> map, List<char> directions, bool isWider = false)
        {
            Map = map;
            Directions = directions;
            Walls = new List<Point>();
            Boxes = new List<Point>();
            WideBoxes = new List<(Point, Point)>();
            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    switch (map[y][x])
                    {
                        case '#':
                            Walls.Add(new Point(x, y));
                            break;
                        case 'O':
                            Boxes.Add(new Point(x, y));
                            break;
                        case '@':
                            Robot = new Point(x, y);
                            break;
                        case '[':
                            Boxes.Add(new Point(x, y));
                            Boxes.Add(new Point(x + 1, y));
                            WideBoxes.Add((new Point(x, y), new Point(x + 1, y)));
                            break;
                    }
                }
            }
        }
    }
    private Dictionary<char, Point> Directions = new Dictionary<char, Point>
    {
        ['^'] = new Point(0, -1),
        ['v'] = new Point(0, 1),
        ['<'] = new Point(-1, 0),
        ['>'] = new Point(1, 0)
    };

    public override Warehouse PrepareData(List<string> input)
    {
        var map = input.TakeWhile(line => !string.IsNullOrWhiteSpace(line)).Select(line => line.ToList()).ToList();
        var directions = input.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).SelectMany(line => line.ToList())
            .ToList();
        
        return new Warehouse(map, directions);
    }

    public override int Part1(Warehouse data)
    {
        foreach (var direction in data.Directions)
        {
            var move = Directions[direction];
            var next = data.Robot.Step(move);
            if (data.Walls.Contains(next))
            {
                continue;
            }

            if (data.Boxes.Contains(next))
            {
                var nextBox = next;
                
                while (data.Boxes.Contains(nextBox))
                {
                    nextBox = nextBox.Step(move);
                }
                
                if (data.Walls.Contains(nextBox))
                {
                    continue;
                }
                
                data.Boxes.Remove(next);
                data.Boxes.Add(nextBox);
            }
            
            data.Robot = next;
        }

        return data.Boxes.Sum(box => box.X + box.Y * 100);
    }

    public override int Part2(Warehouse data)
    {
        var widerWarehouse = new Warehouse(GenerateWiderMap(data.Map), data.Directions, true);
        
        foreach (var direction in widerWarehouse.Directions)
        {
            var move = Directions[direction];
            var next = widerWarehouse.Robot.Step(move);
            if (widerWarehouse.Walls.Contains(next))
            {
                continue;
            }

            if (widerWarehouse.Boxes.Contains(next))
            {
                var isWall = false;
                var wideBoxesToPush = new List<(Point, Point)>();
                var nextBox = new Queue<Point>();
                nextBox.Enqueue(next);
               
                while (nextBox.Count > 0)
                {
                    var box = nextBox.Dequeue();
                    if (widerWarehouse.Boxes.Contains(box))
                    {
                        var wideBox = widerWarehouse.WideBoxes.First(wideBox => wideBox.Item1 == box || wideBox.Item2 == box);
                        if (wideBoxesToPush.All(wideBoxToPush => wideBoxToPush != wideBox))
                        {
                            wideBoxesToPush.Add(wideBox);
                            nextBox.Enqueue(wideBox.Item1.Step(move));
                            nextBox.Enqueue(wideBox.Item2.Step(move));
                        }
                    } else if (widerWarehouse.Walls.Contains(box))
                    {
                        isWall = true;
                        break;
                    }
                }
                
                if (isWall)
                {
                    continue;
                }
                
                foreach (var (box1, box2) in wideBoxesToPush)
                {
                    widerWarehouse.Boxes.Remove(box1);
                    widerWarehouse.Boxes.Remove(box2);
                    widerWarehouse.Boxes.Add(box1.Step(move));
                    widerWarehouse.Boxes.Add(box2.Step(move));
                    widerWarehouse.WideBoxes.Remove((box1, box2));
                    widerWarehouse.WideBoxes.Add((box1.Step(move), box2.Step(move)));
                }
            }
            
            widerWarehouse.Robot = next;
        }

        return widerWarehouse.WideBoxes.Sum(box => box.Item1.X + box.Item1.Y * 100); 
    }
    
    private static List<List<char>> GenerateWiderMap(List<List<char>> originalMap)
    {
        var widerMap = new List<List<char>>();

        foreach (var row in originalMap)
        {
            var newRow = new List<char>();
            foreach (var tile in row)
            {
                switch (tile)
                {
                    case '#':
                        newRow.Add('#');
                        newRow.Add('#');
                        break;
                    case 'O':
                        newRow.Add('[');
                        newRow.Add(']');
                        break;
                    case '.':
                        newRow.Add('.');
                        newRow.Add('.');
                        break;
                    case '@':
                        newRow.Add('@');
                        newRow.Add('.');
                        break;
                }
            }
            widerMap.Add(newRow);
        }
        
        return widerMap;
    }
}