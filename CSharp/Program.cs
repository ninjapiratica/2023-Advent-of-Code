//await Day1_Part1("../../../Day1.txt");
//await Day1_Part2("../../../Day1.txt");
//await Day2_Part1("../../../Day2.txt");
//await Day2_Part2("../../../Day2.txt");
//await Day3_Part1("../../../Day3.txt");
//await Day3_Part2("../../../Day3.txt");
//await Day4_Part1("../../../Day4.txt");
//await Day4_Part2("../../../Day4.txt");
//await Day5_Part1("../../../Day5.txt");
await Day5_Part2("../../../Day5.txt");


async Task Day1_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    var total = lines.Sum(x =>
    {
        var firstIndex = x.IndexOfAny(numbers);
        var lastIndex = x.LastIndexOfAny(numbers);

        var innerTotal = ((int)x[firstIndex] - 48) * 10 + (int)x[lastIndex] - 48;

        return innerTotal;
    });

    Console.WriteLine($"Day 1 Part 1 Result: {total}");
}

async Task Day1_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numbers = new Dictionary<string, int> {
        { "0",     0 },
        { "1",     1 },
        { "2",     2 },
        { "3",     3 },
        { "4",     4 },
        { "5",     5 },
        { "6",     6 },
        { "7",     7 },
        { "8",     8 },
        { "9",     9 },
        { "zero",  0 },
        { "one",   1 },
        { "two",   2 },
        { "three", 3 },
        { "four",  4 },
        { "five",  5 },
        { "six",   6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine" , 9 },
    };

    int FindFirstNumber(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            foreach (var number in numbers.Where(n => n.Key.Length <= line.Length - i))
            {
                for (int j = 0; j < number.Key.Length; j++)
                {
                    if (number.Key[j] != line[i + j])
                    {
                        goto nextNumber;
                    }
                }

                return number.Value;

            nextNumber:;
            }
        }
        return 0;
    }

    int FindLastNumber(string line)
    {
        for (int i = line.Length - 1; i >= 0; i--)
        {
            foreach (var number in numbers.Where(n => n.Key.Length <= i + 1))
            {
                for (int j = 0; j < number.Key.Length; j++)
                {
                    if (number.Key[number.Key.Length - 1 - j] != line[i - j])
                    {
                        goto nextNumber;
                    }
                }

                return number.Value;

            nextNumber:;
            }
        }

        return 0;
    }

    var total = lines.Sum(x =>
    {
        var innerTotal = FindFirstNumber(x) * 10 + FindLastNumber(x);

        return innerTotal;
    });

    Console.WriteLine($"Day 1 Part 2 Result: {total}");
}

async Task Day2_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var total = 0;
    foreach (var line in lines)
    {
        var parts = line.Split(':');

        var gameNumber = int.Parse(parts[0].Split(' ')[1]);

        var sets = parts[1].Split(';');

        foreach (var set in sets)
        {
            var pulls = set.Split(',');

            foreach (var pull in pulls)
            {
                var colorParts = pull.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(colorParts[0]);

                var possible = colorParts[1] switch
                {
                    "blue" => count <= 14,
                    "red" => count <= 12,
                    "green" => count <= 13,
                    _ => false
                };

                if (!possible)
                {
                    goto done;
                }
            }
        }

        total += gameNumber;

    done:;

    }

    Console.WriteLine($"Day 2 Part 1 Result: {total}");
}

async Task Day2_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var total = 0;
    foreach (var line in lines)
    {
        var parts = line.Split(':');

        var gameNumber = int.Parse(parts[0].Split(' ')[1]);

        var sets = parts[1].Split(';');

        var red = 0;
        var green = 0;
        var blue = 0;

        foreach (var set in sets)
        {
            var pulls = set.Split(',');

            foreach (var pull in pulls)
            {
                var colorParts = pull.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var count = int.Parse(colorParts[0]);

                switch (colorParts[1])
                {
                    case "blue":
                        blue = Math.Max(blue, count);
                        break;
                    case "red":
                        red = Math.Max(red, count);
                        break;
                    case "green":
                        green = Math.Max(green, count);
                        break;
                };
            }
        }

        total += (red * green * blue);
    }

    Console.WriteLine($"Day 2 Part 2 Result: {total}");
}

async Task Day3_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numberPositions = new Dictionary<(int X, int Y), (List<(int X, int Y)> Positions, int Number)>();
    var parts = new HashSet<(int X, int Y)>();

    for (int x = 0; x < lines.Length; x++)
    {
        for (int y = 0; y < lines[x].Length; y++)
        {
            var val = lines[x][y];
            if (char.IsNumber(val))
            {
                numberPositions.TryGetValue((x, y - 1), out var previousNumberPosition);
                var positions = previousNumberPosition.Positions ?? [];
                positions.Add((x, y));

                var number = previousNumberPosition.Number;
                number = number * 10 + Math.Max((int)char.GetNumericValue(val), 0);

                numberPositions.Add((x, y), (positions, number));
            }
            else if (val != '.')
            {
                parts.Add((x, y));
            }
        }
    }

    var total = 0;

    foreach (var part in parts)
    {
        for (int i = part.X - 1; i < part.X + 2; i++)
        {
            for (int j = part.Y - 1; j < part.Y + 2; j++)
            {
                if (numberPositions.TryGetValue((i, j), out var numberPosition))
                {
                    total += numberPositions[numberPosition.Positions.Last()].Number;
                    foreach (var item in numberPosition.Positions)
                    {
                        numberPositions.Remove(item);
                    }
                }
            }
        }
    }

    Console.WriteLine($"Day 3 Part 1 Result: {total}");
}

async Task Day3_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var numberPositions = new Dictionary<(int X, int Y), (List<(int X, int Y)> Positions, int Number)>();
    var parts = new HashSet<(int X, int Y)>();

    for (int x = 0; x < lines.Length; x++)
    {
        for (int y = 0; y < lines[x].Length; y++)
        {
            var val = lines[x][y];
            if (char.IsNumber(val))
            {
                numberPositions.TryGetValue((x, y - 1), out var previousNumberPosition);
                var positions = previousNumberPosition.Positions ?? [];
                positions.Add((x, y));

                var number = previousNumberPosition.Number;
                number = number * 10 + Math.Max((int)char.GetNumericValue(val), 0);

                numberPositions.Add((x, y), (positions, number));
            }
            else if (val == '*')
            {
                parts.Add((x, y));
            }
        }
    }

    var total = 0;

    foreach (var part in parts)
    {
        var adjacentNumbers = new Dictionary<List<(int X, int Y)>, int>();

        for (int i = part.X - 1; i < part.X + 2; i++)
        {
            for (int j = part.Y - 1; j < part.Y + 2; j++)
            {
                if (numberPositions.TryGetValue((i, j), out var numberPosition))
                {
                    adjacentNumbers.TryAdd(numberPosition.Positions, numberPositions[numberPosition.Positions.Last()].Number);
                }
            }
        }

        if (adjacentNumbers.Count == 2)
        {
            total += adjacentNumbers.First().Value * adjacentNumbers.Last().Value;
        }
    }

    Console.WriteLine($"Day 3 Part 2 Result: {total}");
}

async Task Day4_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var total = 0;

    foreach (var line in lines)
    {
        var cardValues = line.Split(':')[1];
        var cardNumbers = cardValues.Split('|');

        var winningNumbers = cardNumbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var yourNumbers = cardNumbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var winningCount = yourNumbers.Where(n => winningNumbers.Contains(n)).Count();

        if (winningCount > 0)
        {
            total += (int)Math.Pow(2, winningCount - 1);
        }
    }

    Console.WriteLine($"Day 4 Part 1 Result: {total}");
}

async Task Day4_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var winningTickets = lines.Select((l, ix) => ix).ToDictionary(ix => ix, ix => 1);

    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        var cardValues = line.Split(':')[1];
        var cardNumbers = cardValues.Split('|');

        var winningNumbers = cardNumbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var yourNumbers = cardNumbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var winningCount = yourNumbers.Where(n => winningNumbers.Contains(n)).Count();
        winningTickets.TryGetValue(i, out var copies);

        for (int j = 0; j < winningCount; j++)
        {
            if (winningTickets.ContainsKey(i + j + 1))
            {
                winningTickets[i + j + 1] += copies;
            }
            else
            {
                winningTickets.Add(i + j + 1, copies);
            }
        }
    }

    Console.WriteLine($"Day 4 Part 2 Result: {winningTickets.Sum(x => x.Value)}");
}

async Task Day5_Part1(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var seeds = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    var maps = new List<List<(long SourceStart, long TargetStart, long Range)>>(7);

    for (int i = 1; i < lines.Length; i++)
    {
        if (lines[i] == string.Empty) continue;

        if (lines[i].EndsWith(':'))
        {
            maps.Add([]);
        }
        else
        {
            var map = maps.Last();
            var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            map.Add((parts[1], parts[0], parts[2]));
        }
    }

    var minLocation = long.MaxValue;

    foreach (var seed in seeds)
    {
        var currentLocation = seed;

        foreach (var map in maps)
        {
            foreach (var rangeMap in map)
            {
                if (rangeMap.SourceStart <= currentLocation && rangeMap.SourceStart + rangeMap.Range > currentLocation)
                {
                    currentLocation = rangeMap.TargetStart + (currentLocation - rangeMap.SourceStart);
                    break;
                }
            }
        }

        minLocation = Math.Min(minLocation, currentLocation);
    }

    Console.WriteLine($"Day 5 Part 1 Result: {minLocation}");
}

async Task Day5_Part2(string filePath)
{
    var lines = await File.ReadAllLinesAsync(filePath);

    var seedRanges = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
    var seeds = new List<long>();

    for (int i = 0; i < seedRanges.Length; i+=2)
    {
        for (int j = 0; j < seedRanges[i + 1]; j++)
        {
            seeds.Add(seedRanges[i] + j);
        }
    }

    var maps = new List<List<(long SourceStart, long TargetStart, long Range)>>(7);

    for (int i = 1; i < lines.Length; i++)
    {
        if (lines[i] == string.Empty) continue;

        if (lines[i].EndsWith(':'))
        {
            maps.Add([]);
        }
        else
        {
            var map = maps.Last();
            var parts = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            map.Add((parts[1], parts[0], parts[2]));
        }
    }

    var minLocation = long.MaxValue;

    foreach (var seed in seeds)
    {
        var currentLocation = seed;

        foreach (var map in maps)
        {
            foreach (var rangeMap in map)
            {
                if (rangeMap.SourceStart <= currentLocation && rangeMap.SourceStart + rangeMap.Range > currentLocation)
                {
                    currentLocation = rangeMap.TargetStart + (currentLocation - rangeMap.SourceStart);
                    break;
                }
            }
        }

        minLocation = Math.Min(minLocation, currentLocation);
    }

    Console.WriteLine($"Day 5 Part 1 Result: {minLocation}");
}