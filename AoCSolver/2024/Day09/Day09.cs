using System.Text;
using SuperLinq;

namespace AoCSolver._2024.Day09;

public class Day09 : Solver<string, long>
{
    public override string PrepareData(List<string> input) => input[0];

    public override long Part1(string input)
    {
        Span<ushort> disk = new ushort[20_000 * 9];
        var flag = true;
        ushort id = 0;
        var index = 0;

        foreach (var ch in input.ToCharArray())
        {
            if (flag)
            {
                for (var i = 0; i < (ch - '0'); i++)
                    disk[index++] = id;
                id++;
            }
            else
            {
                for (var i = 0; i < (ch - '0'); i++)
                    disk[index++] = ushort.MaxValue;
            }
            flag = !flag;
        }
        
        disk = disk[..index];
        
        index = 0;
        for (var i = disk.Length - 1; i >= index; i--)
        {
            if (disk[i] == ushort.MaxValue)
                continue;

            while (index < disk.Length && disk[index] != ushort.MaxValue)
                index++;

            if (index >= disk.Length)
                break;

            disk[index] = disk[i];
            disk = disk[..i];
        }

        var sum = 0L;
        for (var i = 0; i < disk.Length; i++)
            sum += i * disk[i];

        return sum;
    }
    
    public override long Part2(string input)
    {
	    var files = new List<(int Pos, int Len, int Id)>();
	    var frees = new List<(int Pos, int Len)>();

	    for (int i = 0, pos = 0, id = 0; i < input.Length; i++)
	    {
		    var n = input[i] - '0';

		    if (i % 2 == 0)
		    {
			    files.Add((pos, n, id++));
		    }
		    else
		    {
			    frees.Add((pos, n));
		    }

		    pos += n;
	    }

	    for (int i = files.Count - 1; i >= 0; i--)
	    {
		    var file = files[i];

		    for (int j = 0; j < frees.Count; j++)
		    {
			    var free = frees[j];

			    if (free.Pos > file.Pos)
			    {
				    break;
			    }

			    if (free.Len >= file.Len)
			    {
				    files[i] = files[i] with { Pos = free.Pos };

				    if (free.Len == file.Len)
				    {
					    frees.RemoveAt(j);
				    }
				    else
				    {
					    frees[j] = (free.Pos + file.Len, free.Len - file.Len);
				    }

				    break;
			    }
		    }
	    }

	    var sum = 0L;

	    foreach (var (pos, len, id) in files)
	    {
		    for (int i = 0; i < len; i++)
		    {
			    sum += (pos + i) * id;
		    }
	    }

	    return sum;
    }
}