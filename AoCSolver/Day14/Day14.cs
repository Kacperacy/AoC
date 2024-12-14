using System.Text.RegularExpressions;
using SuperLinq;

namespace AoCSolver.Day14;

public class Day14() : Solver<List<Day14.Robot>, int>("Day14/input.txt")
{
    public readonly record struct Point(int X, int Y)
    {
        public Point Step(char dir) => dir switch
        {
            'U' => this with { Y = Y - 1 },
            'D' => this with { Y = Y + 1 },
            'L' => this with { X = X - 1 },
            'R' => this with { X = X + 1 },
            _ => this
        };
    }

    public class Robot(Point pos, Point vel)
    {
        public Point Pos = pos;

        public void Move(Point size)
        {
            var x = Pos.X + vel.X;
            var y = Pos.Y + vel.Y;
            
            x = x < 0 ? (size.X + x) % size.X : x % size.X;
            y = y < 0 ? (size.Y + y) % size.Y : y % size.Y;
            
            Pos = new Point(x, y);
        }
    }

    public override List<Robot> PrepareData(List<string> input)
    {
        var regex = new Regex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)", RegexOptions.Compiled);
        
        return input.Select(line =>
        {
            var match = regex.Match(line);
            return new Robot(new Point(int.Parse(match.Groups["px"].Value), int.Parse(match.Groups["py"].Value)),
                new Point(int.Parse(match.Groups["vx"].Value), int.Parse(match.Groups["vy"].Value)));
        }).ToList();
    }

    public override int Part1(List<Robot> data)
    {
        var size = new Point(101, 103);
        var half = new Point(size.X / 2, size.Y / 2);
        foreach (var robot in data)
        {
            for (var i = 0; i < 100; i++)
            {
                robot.Move(size);
            }
        }
        
        var positions = data.Select(robot => robot.Pos).ToList();
        
        return positions.Count(x => x.X < half.X && x.Y < half.Y) *
               positions.Count(x => x.X > half.X && x.Y < half.Y) *
               positions.Count(x => x.X < half.X && x.Y > half.Y) *
               positions.Count(x => x.X > half.X && x.Y > half.Y);
    }

    public override int Part2(List<Robot> data)
    {
        var size = new Point(101, 103);
        
        for (var i = 0; i < 10000; i++)
        {
            foreach (var robot in data)
            {
                robot.Move(size);
            }
            
            var positions = data.Select(robot => robot.Pos).ToList();

            foreach (var robot in data)
            {
                if (Flood(positions, robot.Pos).Count > 200)
                {
                    PrintPoints(positions);
                    return i + 101;
                }
            }
        }

        return 0;
    }
    
    private void PrintPoints (List<Point> points)
    {
        var minX = points.Min(p => p.X);
        var minY = points.Min(p => p.Y);
        var maxX = points.Max(p => p.X);
        var maxY = points.Max(p => p.Y);

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(points.Contains(new Point(x, y)) ? '#' : '.');
            }
            Console.WriteLine();
        }
    }
    
    private HashSet<Point> Flood(List<Point> positions, Point start)
    {
        var flood = new HashSet<Point>();
        var q = new Queue<Point>();
        q.Enqueue(start);

        while (q.TryDequeue(out var pos))
        {
            if (!flood.Add(pos))
            {
                continue;
            }

            foreach (var point in "UDLR".Select(pos.Step))
            {
                if (positions.Contains(point))
                {
                    q.Enqueue(point);
                }
            }
        }

        return flood;
    }
}