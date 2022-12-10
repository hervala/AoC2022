using Spectre.Console;

public class Day10 : PuzzleDay
{
    public override void Part01()
    {
        var simpleCpu = new SimpleCpu(Input);
        Console.WriteLine(simpleCpu.SumOfSignalStrength());
    }

    public override void Part02()
    {
        var simpleCpu = new SimpleCpu(Input);
        simpleCpu.DrawCrt();
    }

    private class SimpleCpu
    {
        private readonly Queue<int> operations = new();
        private int xRegister = 1;
        private int cycle = 0;

        public SimpleCpu(string instructions)
        {
            var instructionsParsed = instructions.Split("\n").Select(o =>
            {
                var parts = o.Split(" ");
                return (opcode: parts[0], value: parts.Length > 1 ? int.Parse(parts[1]) : 0);
            });

            foreach (var instruction in instructionsParsed)
            {
                switch (instruction.opcode)
                {
                    case "noop":
                        operations.Enqueue(0);
                        break;
                    case "addx":
                        operations.Enqueue(0);
                        operations.Enqueue(instruction.value);
                        break;
                }
            }


        }

        public int SumOfSignalStrength()
        {
            Dictionary<int ,int> signalStrengths = new()
            {
                { 20, 0 },
                { 60, 0 },
                { 100, 0 },
                { 140, 0 },
                { 180, 0 },
                { 220, 0 },
            };

            while (operations.TryDequeue(out var value))
            {
                cycle++;
                if (signalStrengths.TryGetValue(cycle, out var checkpointValue))
                {
                    checkpointValue = xRegister * cycle;
                    signalStrengths.Remove(cycle);
                    signalStrengths.Add(cycle, checkpointValue);
                }

                xRegister += value;
            }

            return signalStrengths.Values.Sum();
        }

        public void DrawCrt()
        {
            var crtRow = 0;
            var crtline = 0;
            var canvas = new Canvas(40, 6);
            while (operations.TryDequeue(out var value))
            {
                cycle++;
                if (crtRow >= xRegister - 1 && crtRow <= xRegister + 1)
                {
                    canvas.SetPixel(crtRow, crtline, 118 + crtline);
                }

                xRegister += value;
                if (cycle % 40 == 0)
                {
                    crtRow = 0;
                    crtline++;
                }
                else
                {
                    crtRow++;
                }
            }

            AnsiConsole.Write(canvas);
        }

    }

    private string SmallExample = @"noop
addx 3
addx -5";

    protected override string InputExample => @"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop";

    protected override string Input => @"addx 1
noop
addx 4
noop
noop
addx 7
noop
noop
noop
addx 3
noop
noop
addx 5
addx -1
addx 1
addx 5
addx 3
noop
addx 3
noop
addx -1
noop
addx 3
addx 5
addx -38
addx 7
addx 10
addx -14
addx 5
addx 30
addx -25
noop
addx 2
addx 3
addx -2
addx 2
addx 5
addx 2
addx 2
addx -21
addx 22
addx 5
addx 2
addx 3
noop
addx -39
addx 1
noop
noop
addx 3
addx 5
addx 4
addx -5
addx 4
addx 4
noop
addx -9
addx 12
addx 5
addx 2
addx -1
addx 6
addx -2
noop
addx 3
addx 3
addx 2
addx -37
addx 39
addx -33
addx -1
addx 1
addx 8
noop
noop
noop
addx 2
addx 20
addx -19
addx 4
noop
noop
noop
addx 3
addx 2
addx 5
noop
addx 1
addx 4
addx -21
addx 22
addx -38
noop
noop
addx 7
addx 32
addx -27
noop
addx 3
addx -2
addx 2
addx 5
addx 2
addx 2
addx 3
addx -2
addx 2
noop
addx 3
addx 5
addx 2
addx 3
noop
addx -39
addx 2
noop
addx 4
addx 8
addx -8
addx 6
addx -1
noop
addx 5
noop
noop
noop
addx 3
addx 5
addx 2
addx -11
addx 12
addx 2
noop
addx 3
addx 2
addx 5
addx -6
noop
";
}
