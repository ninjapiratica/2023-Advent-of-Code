await Day1_Part1("../../../Day1.txt");
await Day1_Part2("../../../Day1.txt");


static async Task Day1_Part1(string filePath)
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

    Console.WriteLine($"Part 1 Result: {total}");
}

static async Task Day1_Part2(string filePath)
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

    Console.WriteLine($"Part 1 Result: {total}");
}