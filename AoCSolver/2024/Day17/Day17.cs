namespace AoCSolver._2024.Day17;

public class Day17 : Solver<Day17.Computer, string>
{
    public record struct Computer
    {
        private long A { get; set; }
        private long B { get; set; }
        private long C { get; set; }
        private int instructionPointer;
        public List<int> program;
        private List<int> output;

        public Computer(long a, long b, long c, List<int> program)
        {
            A = a;
            B = b;
            C = c;
            instructionPointer = 0;
            this.program = program;
            output = new List<int>();
        }

        public List<int> Run()
        {
            while (instructionPointer < program.Count)
            {
                int opcode = program[instructionPointer];
                int operand = program[instructionPointer + 1];
                ExecuteInstruction(opcode, operand);
                if (opcode != 3 || A == 0)
                {
                    instructionPointer += 2;
                }
            }

            return output;
        }

        private void ExecuteInstruction(int opcode, int operand)
        {
            switch (opcode)
            {
                case 0: // adv
                    A = A >>> (int)Combo(operand);
                    break;
                case 1: // bxl
                    B ^= operand;
                    break;
                case 2: // bst
                    B = Combo(operand) % 8;
                    break;
                case 3: // jnz
                    if (A != 0)
                    {
                        instructionPointer = operand;
                    }
                    break;
                case 4: // bxc
                    B ^= C;
                    break;
                case 5: // out
                    output.Add((int)(Combo(operand) % 8));
                    break;
                case 6: // bdv
                    B = A >>> (int)Combo(operand);
                    break;
                case 7: // cdv
                    C = A >>> (int)Combo(operand);
                    break;
            }
        }

        private long Combo(int operand)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new ArgumentException("Invalid combo operand")
            };
        }
    }

    public override Computer PrepareData(List<string> input)
    {
        var registers = input.Take(3).Select(line => int.Parse(line.Split(" ")[^1])).ToList();
        var program = input.Skip(4).Select(line => line.Split(" ")[^1])
            .SelectMany(line => line.Split(",").Select(int.Parse)).ToList();
        return new Computer(registers[0], registers[1], registers[2], program);
    }

    public override string Part1(Computer computer)
    {
        return string.Join(",", computer.Run());
    }

    public override string Part2(Computer computer)
    {
        var nums = new List<long>();
        nums.Add(0);

        for (var i = computer.program.Count - 1; i >= 0; i--)
        {
            var count = nums.Count;
            for (var x = 0; x < count; x++)
            {
                var currentA = nums[x] * 8;

                for (var j = 0; j < 8; j++)
                {
                    var output = new Computer(currentA + j, 0L, 0L, computer.program).Run();
                    if (computer.program.Skip(i).SequenceEqual(output))
                    {
                        nums.Add(currentA + j);
                    }
                }
            }
            
            nums.RemoveRange(0, count);
        }

        return nums.Min().ToString();
    }
}