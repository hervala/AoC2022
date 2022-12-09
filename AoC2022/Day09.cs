using System.Data;
using Spectre.Console;

public class Day09 : PuzzleDay
{
    public override void Part01()
    {
        HashSet<Tail> tailPositions = new();
        var tail = new Tail(0, 0);
        var head = new Head(0, 0);
        var movements = GetMovements(Input);
        tailPositions.Add(tail);

        foreach (var movement in movements)
        {
            head = head.Move(movement);
            tail = tail.Move(head);
            tailPositions.Add(tail);
        }

        Console.WriteLine(tailPositions.Count);
    }

    public override void Part02()
    {
        HashSet<MutableTail> tailPositions = new();
        const int lastKnotIndex = 8;
        var head = new Head(0, 0);
        MutableTail[] tails =
        {
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
            new MutableTail(0, 0),
        };
        var movements = GetMovements(Input);
        tailPositions.Add(tails[lastKnotIndex]);
        foreach (var movement in movements)
        {
            head = head.Move(movement);
            for (var i = 0; i <= lastKnotIndex; i++)
            {
                if (i == 0)
                {
                    tails[i].Move(head.x, head.y);
                }
                else
                {
                    tails[i].Move(tails[i-1].x, tails[i-1].y);
                }
            }

            tailPositions.Add(tails[lastKnotIndex]);
            if (false) // rope animation
            {
                var canvas = new Canvas(200, 80)
                {
                    Scale = true
                };

                foreach (var tail in tails)
                {
                    canvas.SetPixel(tail.x+15, tail.y+60, Color.LightGreen);
                }
                canvas.SetPixel(head.x+15, head.y+60, Color.Red);
                Thread.Sleep(40);
                AnsiConsole.Clear();
                AnsiConsole.Write(canvas);
            }
        }

        Console.WriteLine(tailPositions.Count);
    }

    private IEnumerable<Direction> GetMovements(string input) =>
        input.Split("\n").SelectMany(o =>
        {
            var movement = o.Split(" ");
            return Enumerable.Repeat(movement[0].AsDirection(), int.Parse(movement[1]));
        });

    private record Tail(int x, int y)
    {
        public Tail Move(Head head)
        {
            var xDistance = Math.Abs(this.x - head.x);
            var yDistance = Math.Abs(this.y - head.y);
            var newX = this.x;
            var newY = this.y;

            // move towards if distance more than 1;
            if (head.x < this.x && xDistance > 1)
            {
                // left
                newX = head.x + 1;
                if (head.y != this.y)
                {
                    newY = head.y;   // OBS: this diagonal calculation is wrong.. correct implementation in MutableTail
                }

            }
            else if (head.x > this.x && xDistance > 1)
            {
                // right
                newX = head.x - 1;
                if (head.y != this.y)
                {
                    newY = head.y;
                }
            }
            else if (head.y < this.y && yDistance > 1)
            {
                // down
                newY = head.y + 1;
                if (head.x != this.x)
                {
                    newX = head.x;
                }
            }
            else if (head.y > this.y && yDistance > 1)
            {
                // up
                newY = head.y - 1;
                if (head.x != this.x)
                {
                    newX = head.x;
                }
            }

            return new Tail(newX, newY);
        }
    }

    private class MutableTail : IEquatable<MutableTail>
    {
        public bool Equals(MutableTail? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return y == other.y && x == other.x;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MutableTail)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(y, x);
        }

        public static bool operator ==(MutableTail? left, MutableTail? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MutableTail? left, MutableTail? right)
        {
            return !Equals(left, right);
        }

        public int y { get; private set; }
        public int x { get; private set; }

        public MutableTail(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Move(int x, int y)
        {
            var xDistance = Math.Abs(this.x - x);
            var yDistance = Math.Abs(this.y - y);
            var newX = this.x;
            var newY = this.y;

            // move towards if distance more than 1;
            if (x < this.x && xDistance > 1)
            {
                // left
                newX = x + 1;
                if (y < this.y && yDistance > 1)
                {
                    newY = y + 1;
                }
                else if (y > this.y && yDistance > 1)
                {
                    newY = y - 1;
                }
                else if (y != this.y)
                {
                    newY = y;
                }

            }
            else if (x > this.x && xDistance > 1)
            {
                // right
                newX = x - 1;
                if (y < this.y && yDistance > 1)
                {
                    newY = y + 1;
                }
                else if (y > this.y && yDistance > 1)
                {
                    newY = y - 1;
                }
                else if (y != this.y)
                {
                    newY = y;
                }
            }
            else if (y < this.y && yDistance > 1)
            {
                // down
                newY = y + 1;
                if (x < this.x && xDistance > 1)
                {
                    newX = x + 1;
                }
                else if (x > this.x && xDistance > 1)
                {
                    newX = x - 1;
                }
                else if (x != this.x)
                {
                    newX = x;
                }
            }
            else if (y > this.y && yDistance > 1)
            {
                // up
                newY = y - 1;
                if (x < this.x && xDistance > 1)
                {
                    newX = x + 1;
                }
                else if (x > this.x && xDistance > 1)
                {
                    newX = x - 1;
                }
                else if (x != this.x)
                {
                    newX = x;
                }
            }

            this.y = newY;
            this.x = newX;
        }
    }

    private record Head(int x, int y)
    {
        public Head Move(Direction direction) =>
            direction switch
            {
                Direction.Up => this with { y = this.y + 1},
                Direction.Down => this with { y = this.y - 1 },
                Direction.Left => this with { x = this.x - 1 },
                Direction.Right => this with { x = this.x + 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }

    protected override string InputExample => @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";  // tail in 13 positions

    private string InputExamplePart2 => @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";

    protected override string Input => @"D 2
U 2
L 2
D 2
L 2
D 2
L 1
D 2
U 1
L 1
D 1
R 1
D 1
L 2
D 1
U 2
D 2
R 2
D 1
U 2
L 2
D 1
R 1
D 1
R 2
U 2
L 2
U 2
D 1
U 2
R 2
L 2
R 2
D 2
L 1
R 2
D 2
R 2
D 1
R 2
D 2
R 2
L 1
R 1
U 2
D 2
R 2
D 1
L 2
D 1
L 1
U 2
L 1
U 2
R 2
D 1
L 1
D 2
L 1
D 1
R 1
D 2
R 2
D 2
L 2
D 2
U 2
L 1
R 2
L 1
D 2
L 2
D 2
U 2
L 1
R 2
U 2
D 2
R 1
L 2
D 1
R 2
D 2
L 1
R 1
L 2
D 1
R 1
D 1
L 1
U 2
R 1
U 1
L 1
U 1
L 2
U 2
L 2
U 2
R 1
L 2
D 2
R 1
D 1
U 2
D 1
U 1
L 1
R 1
D 2
R 1
D 2
U 1
D 2
L 1
R 2
U 2
D 3
U 1
R 1
U 1
D 1
R 1
L 2
D 3
U 3
L 3
U 2
R 2
U 1
D 2
U 1
R 3
D 1
R 2
U 1
D 2
U 2
L 2
U 1
D 1
L 2
U 3
R 3
D 3
R 1
U 1
L 3
R 1
D 1
L 1
U 1
R 3
U 2
D 1
U 1
L 2
U 3
L 2
D 3
L 2
R 3
D 3
R 3
L 2
R 3
U 3
D 1
U 2
L 3
U 2
L 1
U 1
D 1
L 1
D 2
U 2
L 1
U 1
L 3
U 2
D 3
R 1
U 1
R 3
L 3
R 2
U 1
L 3
D 2
U 1
D 3
R 2
U 3
L 1
R 2
D 2
L 1
R 3
L 3
D 3
R 3
L 2
U 2
R 3
D 2
L 2
U 1
R 2
U 2
R 3
L 2
D 1
R 1
D 3
L 3
U 1
R 2
L 3
D 1
R 3
U 2
D 4
L 2
D 2
L 2
R 3
U 2
D 1
R 3
L 2
U 3
R 3
U 3
L 4
R 3
D 3
U 1
D 2
R 1
U 1
L 2
D 1
R 2
U 4
R 3
U 3
L 4
U 3
R 2
L 1
R 4
L 2
U 4
L 1
D 1
L 1
U 2
L 1
U 3
L 1
U 1
R 1
U 1
R 2
L 4
D 3
R 4
D 2
U 2
L 2
D 1
R 2
L 4
U 4
R 3
D 3
L 2
R 3
L 2
D 4
R 4
L 1
R 3
U 3
R 4
L 4
U 3
R 2
D 3
L 3
D 1
U 2
D 1
L 4
D 3
R 4
L 4
U 3
R 2
U 1
D 3
R 3
D 4
U 3
L 3
D 4
L 2
U 2
L 2
D 2
U 2
L 2
D 3
R 4
U 2
D 3
R 2
D 4
U 2
R 2
D 3
L 2
U 1
L 4
R 2
L 4
D 3
L 1
D 1
U 2
L 1
R 4
L 4
R 2
L 3
R 5
D 5
L 4
U 2
R 2
U 3
D 4
L 2
U 3
D 2
R 2
D 1
R 5
D 1
R 2
U 5
R 3
D 5
L 2
R 5
L 4
R 2
U 3
L 3
D 2
L 1
D 5
L 1
R 1
U 2
R 4
L 5
R 2
D 2
U 1
L 3
D 2
R 2
U 4
D 5
R 4
U 3
R 1
D 2
R 4
L 3
D 4
L 4
R 4
D 4
L 5
R 5
D 2
R 2
L 1
D 5
L 3
R 3
U 4
R 1
U 3
L 4
D 5
R 3
D 4
U 5
L 5
R 5
U 3
L 2
D 4
U 1
R 4
U 2
D 2
U 2
L 1
U 2
D 1
U 4
R 3
U 1
R 2
L 5
D 3
R 1
U 3
L 2
U 5
D 5
U 3
D 4
R 1
U 5
L 4
U 5
R 3
D 5
U 1
D 1
L 1
U 2
L 3
R 1
U 2
D 5
R 5
U 1
L 1
R 6
D 1
L 6
D 2
U 6
R 4
D 3
R 1
U 1
L 3
U 6
D 4
R 4
D 4
R 5
D 6
L 5
U 3
R 6
U 6
D 2
U 4
L 5
D 3
L 2
R 6
D 5
R 2
U 1
R 6
L 3
R 4
U 5
L 1
D 2
U 5
R 4
D 6
R 6
D 1
U 2
R 5
D 6
R 3
U 3
L 3
R 1
L 4
R 1
D 5
U 5
L 6
U 3
D 3
R 6
D 6
R 1
D 4
L 2
D 5
R 4
U 2
R 5
L 1
D 6
U 6
R 5
U 4
D 4
U 6
L 6
D 1
L 6
R 1
D 3
R 4
L 5
D 2
L 5
R 2
U 5
D 3
R 6
U 1
D 1
R 6
U 6
D 1
L 4
D 4
L 1
U 5
R 2
L 3
U 5
R 4
L 6
R 5
D 5
L 4
R 6
U 3
R 3
D 3
R 5
U 3
R 4
U 3
L 4
U 4
L 5
R 6
U 5
D 1
U 4
L 7
D 7
U 5
R 3
D 4
R 4
D 3
L 1
U 4
L 2
D 4
R 4
U 7
L 7
U 4
D 7
U 6
D 3
U 6
L 6
R 1
D 1
R 3
D 5
U 1
L 5
R 5
U 3
L 7
R 6
D 7
U 3
L 2
U 1
R 3
L 2
U 1
R 7
U 1
D 7
U 4
R 6
U 2
R 3
U 6
D 2
L 4
D 2
L 2
U 2
L 2
D 6
R 2
D 1
U 4
D 4
U 4
R 2
U 3
R 2
D 1
R 1
L 7
R 2
D 6
L 6
D 2
L 6
R 3
L 5
D 7
U 3
R 5
D 1
U 7
D 7
U 4
R 5
D 1
U 7
L 6
D 7
U 5
L 5
U 4
R 4
U 7
D 3
R 7
U 2
R 2
L 4
R 6
U 5
R 6
L 5
U 1
D 7
U 1
D 1
L 6
D 6
U 2
L 3
U 6
L 6
R 6
U 5
L 4
R 1
L 1
D 4
L 5
D 3
L 4
R 6
L 6
D 6
U 5
D 1
R 2
D 6
R 6
U 8
L 8
U 7
D 7
R 3
U 4
R 4
U 8
D 4
R 6
D 1
L 6
D 6
R 8
U 1
D 2
R 3
D 8
U 4
R 3
U 7
R 3
D 6
L 8
D 3
L 7
R 1
L 7
D 5
U 3
D 7
U 4
D 7
L 7
D 2
U 7
R 5
U 5
L 4
D 3
R 6
U 1
L 3
D 3
R 2
U 3
D 2
R 3
D 3
R 3
L 3
D 6
U 2
L 2
R 5
U 4
D 2
U 5
L 3
R 1
U 5
D 1
U 5
L 3
D 6
R 6
D 1
U 4
L 2
D 5
U 2
L 8
U 6
D 6
R 7
L 3
D 3
L 1
R 2
D 5
L 1
D 3
L 3
R 8
L 3
D 7
R 4
U 3
R 1
L 7
D 1
U 1
R 4
L 2
U 2
L 5
D 4
R 8
U 3
R 7
L 9
R 5
U 8
R 9
U 1
D 2
U 5
L 5
R 5
U 3
R 1
U 7
D 2
R 2
L 4
D 7
U 4
R 3
D 6
R 9
L 1
D 1
U 2
R 8
D 4
L 2
U 7
D 9
U 8
D 4
R 1
L 3
R 6
D 6
R 1
L 2
R 9
U 7
R 4
U 2
D 6
L 5
U 5
R 2
L 5
R 1
D 6
R 1
D 5
L 4
U 1
D 8
L 9
R 1
U 6
D 3
L 7
U 7
R 5
D 8
U 2
D 8
U 4
L 2
R 4
D 4
L 6
R 4
L 5
U 7
R 8
L 6
D 9
L 2
R 4
L 6
U 3
L 6
R 9
U 7
D 3
R 1
D 1
R 6
D 8
L 5
R 8
U 9
D 9
U 2
R 7
U 9
R 4
L 6
U 9
R 2
D 9
U 7
R 2
U 9
R 2
L 2
R 8
L 9
R 3
D 4
R 10
U 10
D 7
R 7
D 4
R 8
D 9
U 7
L 8
D 10
U 10
R 4
U 6
D 4
L 9
R 9
L 10
D 3
L 6
D 2
L 5
U 1
R 4
D 3
U 7
L 1
R 6
L 10
U 2
R 4
D 5
U 7
R 2
U 3
R 3
L 2
R 9
U 8
D 8
R 10
U 9
R 6
U 5
L 2
D 10
U 4
R 4
L 5
U 10
R 6
L 1
R 10
D 9
L 7
D 3
L 3
R 5
D 3
L 2
R 1
L 3
R 2
D 2
U 4
L 3
D 5
R 6
D 1
U 8
L 6
D 10
R 6
U 5
L 1
D 8
U 1
R 2
D 6
U 8
D 10
L 2
D 1
R 7
L 2
D 4
U 10
L 7
D 10
U 2
D 7
U 1
R 5
D 8
R 4
D 9
R 6
L 6
R 7
L 2
R 5
D 6
L 2
D 1
L 9
U 4
D 10
R 10
U 7
D 10
U 7
L 8
U 9
L 2
D 7
R 4
D 10
R 1
L 5
U 11
L 2
R 9
L 6
U 10
D 1
L 8
U 4
R 3
U 1
R 6
D 6
L 1
R 9
L 5
R 8
U 6
D 2
U 10
D 2
L 4
U 7
D 10
L 7
D 8
L 8
D 5
L 8
U 10
D 7
U 1
L 9
R 2
U 6
L 5
R 2
D 5
L 3
D 6
L 2
D 6
L 2
R 9
D 7
R 1
L 7
R 9
D 11
R 8
U 1
L 7
R 3
L 2
U 4
R 1
D 10
L 3
R 9
U 10
R 1
D 7
R 6
D 11
U 2
L 9
R 10
L 4
R 6
U 5
L 1
U 10
D 6
U 2
L 11
U 4
R 3
D 9
U 1
D 9
L 9
D 9
U 5
D 4
U 2
D 3
L 8
R 5
D 7
R 8
D 4
L 2
U 4
D 1
L 9
D 3
R 7
U 4
L 2
R 8
L 2
U 10
D 9
R 7
U 8
D 4
U 3
D 10
L 3
R 4
L 5
R 9
L 8
R 10
U 5
D 10
R 3
U 8
R 2
U 4
D 1
R 9
L 2
U 6
L 1
R 6
D 9
L 8
U 8
D 6
L 7
U 6
D 1
R 3
D 6
R 2
D 7
R 2
D 11
L 7
U 9
R 8
L 10
U 1
R 11
L 5
D 1
R 4
D 9
L 12
U 2
R 10
D 2
L 8
R 2
L 4
R 1
U 5
R 8
U 3
D 12
U 12
L 11
R 6
L 1
R 3
L 9
D 6
L 10
D 6
U 4
L 9
U 5
D 9
R 9
D 12
R 4
D 9
L 4
R 2
L 9
R 4
D 3
U 10
L 8
U 11
D 12
L 4
D 2
U 11
L 9
R 3
U 6
D 3
L 12
U 9
D 2
L 6
R 3
U 2
D 3
R 5
L 10
U 5
D 2
L 12
R 8
U 9
R 4
D 8
R 3
D 9
L 11
D 8
U 10
L 12
R 4
L 8
R 7
U 7
L 9
R 1
D 5
R 11
U 2
D 8
L 11
U 5
R 2
U 1
L 5
U 10
D 2
L 3
U 5
L 11
U 3
D 3
U 11
L 13
D 7
U 4
L 12
U 10
L 13
R 4
L 12
U 12
D 4
U 6
D 5
R 5
U 13
R 7
D 2
U 9
L 7
R 7
D 2
U 12
D 5
L 11
D 13
L 10
D 10
U 7
D 13
U 7
R 9
L 7
R 1
L 6
R 9
D 8
U 7
L 12
D 9
U 4
D 13
R 2
L 8
R 10
D 8
L 2
D 5
R 8
D 2
R 8
D 4
L 9
R 4
U 4
L 13
D 2
L 3
R 6
U 5
L 9
U 9
R 10
L 11
U 11
D 13
U 4
L 2
D 13
R 9
L 13
D 9
U 9
D 5
U 11
L 8
R 7
U 11
D 3
L 5
R 12
D 2
R 5
L 10
U 10
D 10
R 8
L 2
R 3
U 9
D 7
U 14
L 6
D 11
R 12
L 13
U 2
L 11
R 13
D 13
L 3
R 3
L 10
R 11
D 12
R 8
U 14
D 4
U 4
L 12
R 8
L 10
R 4
U 12
D 5
L 3
U 1
D 7
R 12
L 4
R 9
U 5
R 11
D 12
R 14
L 7
U 2
L 7
U 1
D 8
R 3
U 7
R 2
L 2
R 7
D 3
U 3
R 5
L 11
U 13
D 14
L 7
D 5
R 11
L 1
U 5
D 11
U 2
D 8
L 10
D 8
R 5
L 2
U 11
L 4
R 7
D 2
L 4
R 5
L 6
U 12
L 9
D 4
R 8
U 4
L 2
R 12
D 11
R 12
U 10
R 12
U 9
L 1
U 4
L 9
U 13
R 10
L 9
U 11
R 6
D 9
L 1
D 14
R 1
D 12
U 5
D 3
U 13
R 2
L 9
R 8
D 7
L 13
R 11
D 12
L 6
R 11
L 8
U 9
D 5
U 10
R 8
L 12
D 6
R 8
U 5
D 13
R 3
U 12
L 1
U 13
D 4
L 10
U 7
L 10
U 13
D 4
R 13
D 5
R 15
D 8
U 13
L 6
U 14
L 4
R 11
U 13
D 8
U 11
D 2
R 13
L 14
R 6
L 3
R 13
L 11
R 14
L 6
U 14
D 2
R 1
L 6
U 5
D 13
L 7
R 6
U 11
R 10
D 5
U 11
D 2
L 3
U 4
L 2
U 5
R 14
U 9
D 9
U 12
R 15
L 11
R 15
L 9
R 14
U 7
D 11
R 14
D 14
L 4
U 4
D 10
R 10
D 9
L 14
R 14
D 11
L 7
R 9
L 14
U 11
D 12
L 13
D 11
U 4
D 10
U 14
D 8
U 12
R 15
U 1
D 1
U 11
D 10
L 6
D 11
U 11
R 10
L 7
U 5
L 13
R 8
U 9
L 8
U 12
D 2
R 11
U 6
R 5
L 2
U 9
L 3
R 3
D 6
L 15
U 15
L 10
D 4
U 13
R 4
U 7
R 14
U 13
L 8
R 14
U 5
L 7
R 5
D 4
L 4
U 12
L 10
D 9
R 10
U 9
L 5
D 16
R 10
D 7
U 4
R 14
D 13
R 10
L 15
D 16
R 15
L 4
D 2
L 6
R 16
U 5
R 9
U 16
R 4
U 11
D 5
U 2
D 3
L 11
D 4
L 4
U 1
L 7
U 12
R 15
U 16
R 12
D 8
R 3
U 16
L 4
R 4
D 12
L 5
R 6
D 16
L 13
D 3
L 13
R 4
L 6
D 12
U 12
L 9
D 2
U 16
D 10
L 1
U 3
D 1
L 8
D 3
L 7
R 7
L 3
D 11
U 13
R 13
U 12
L 16
D 1
R 14
L 11
R 3
D 13
U 10
R 14
U 2
L 1
D 1
U 9
D 5
U 9
L 7
U 7
L 6
D 16
U 16
L 7
U 11
D 16
U 7
D 15
R 7
L 15
D 4
R 14
D 10
U 16
L 10
U 10
D 16
L 12
U 10
R 6
L 1
U 7
D 3
L 1
R 6
D 1
R 12
U 1
D 3
U 10
R 17
L 2
R 13
L 4
D 3
U 9
R 8
U 3
D 4
L 17
R 4
L 9
U 5
D 16
R 9
U 17
D 17
U 5
L 7
U 9
D 10
R 14
D 2
U 2
D 16
L 2
D 16
U 17
D 13
U 4
R 16
D 12
L 17
R 10
L 13
R 17
U 4
D 17
U 10
R 12
U 16
D 6
U 1
L 1
U 17
R 6
D 11
L 3
U 7
L 12
R 8
U 16
D 2
U 12
D 7
U 9
R 6
U 7
D 17
U 6
L 3
U 7
R 12
L 16
U 3
R 12
U 2
D 3
R 16
L 4
U 5
L 8
D 3
R 9
D 13
R 2
L 14
R 2
U 13
R 14
D 2
U 16
L 16
D 11
U 4
L 3
R 9
U 12
L 15
U 8
R 11
L 16
R 5
U 6
R 8
U 16
D 16
L 3
U 11
R 9
U 12
D 18
L 13
D 3
R 4
L 10
R 15
U 13
D 15
U 12
D 3
L 1
U 16
L 3
U 17
L 10
D 11
R 16
L 13
R 18
U 18
R 7
D 13
R 15
U 12
R 1
L 9
U 16
R 11
L 11
U 7
R 14
L 16
U 3
D 11
U 17
D 14
U 14
R 7
U 4
R 13
U 4
D 18
U 14
L 4
R 5
L 3
R 14
U 12
D 17
U 13
D 8
U 9
L 1
R 15
U 3
L 11
R 5
L 1
D 14
R 10
U 16
R 2
U 17
D 1
L 5
U 15
R 4
D 13
R 5
D 7
L 3
U 14
R 7
D 9
L 9
R 4
U 6
L 8
U 3
L 5
D 4
R 14
L 13
D 4
U 13
R 17
L 14
D 16
R 1
L 2
D 8
R 11
L 10
U 12
R 4
U 9
L 4
D 13
L 10
R 8
L 1
U 12
L 16
U 12
D 9
R 13
L 2
R 5
L 1
D 18
U 6
D 13
U 17
R 7
U 5
R 5
L 12
R 2
U 4
L 7
U 11
D 17
L 7
U 9
D 1
U 8
L 15
U 2
D 9
U 9
D 10
U 12
R 19
D 16
L 10
U 14
L 18
R 5
U 6
L 4
U 18
L 14
R 2
U 13
D 18
L 2
U 6
L 10
D 9
L 3
D 1
R 6
U 7
D 12
U 1
R 15
U 7
L 8
R 8
L 16
U 8
R 8
U 11
R 19
U 2
L 6
R 1
D 16
U 3
L 7
D 10
R 14
D 11
R 8
U 14
L 6
D 14
R 7
U 18
D 19
R 4
U 10
R 14
U 3
L 12
D 7
U 18
D 10
L 8
D 9
L 6
U 12
R 1
L 16
R 2
U 15
R 14
U 7
L 6
R 10
D 18
R 14
U 3
R 4
D 17
L 10
U 19
R 4
D 12
U 2
D 18
R 4";
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public static class StringExtensions
{
    public static Direction AsDirection(this string value) =>
        value switch
        {
            "R" => Direction.Right,
            "L" => Direction.Left,
            "U" => Direction.Up,
            "D" => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(value), "Value must be R, L, U or D. It was: " + value),
        };
}
