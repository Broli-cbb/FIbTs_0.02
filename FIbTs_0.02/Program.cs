
using System;
using System.IO;
using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Fibonacci Sequence Generator");
            Console.WriteLine("1. Create a new Fibonacci document");
            Console.WriteLine("2. Continue an existing Fibonacci document");
            Console.WriteLine("3. Generate Fibonacci Sequence as Times of Day (hh:mm:ss)");
            Console.WriteLine("4. Continuous Fibonacci Writer (Seconds)");
            Console.WriteLine("5. Continuous Fibonacci Writer (Milliseconds)");
            Console.WriteLine("6. Continuous Fibonacci Writer (Nanoseconds, BigInteger)");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreateNewFibonacciFile();
                    break;
                case "2":
                    ContinueExistingFibonacciFile();
                    break;
                case "3":
                    GenerateFibonacciAsTimes();
                    break;
                case "4":
                    RunContinuousFibonacciWriter(1, "seconds");
                    break;
                case "5":
                    RunContinuousFibonacciWriter(0.001, "milliseconds");
                    break;
                case "6":
                    RunContinuousFibonacciWriterNanoseconds();
                    break;
                case "7":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid input. Please try again.");
                    break;
            }
        }
    }

    static void CreateNewFibonacciFile()
    {
        Console.Write("Enter the name of the file to create (e.g., fibonacci.txt): ");
        string filePath = Console.ReadLine();

        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                long a = 0, b = 1;
                writer.WriteLine(a);
                Console.WriteLine($"Fibonacci: {a}");
                writer.WriteLine(b);
                Console.WriteLine($"Fibonacci: {b}");

                Console.WriteLine($"File '{filePath}' created with initial Fibonacci numbers.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating file: {ex.Message}");
        }
    }

    static void ContinueExistingFibonacciFile()
    {
        Console.Write("Enter the file path to continue: ");
        string filePath = Console.ReadLine();

        try
        {
            long[] lastNumbers = GetLastTwoNumbersFromFile(filePath);

            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                long a = lastNumbers[0];
                long b = lastNumbers[1];

                Console.WriteLine($"Continuing from: {a}, {b}");

                for (int i = 0; i < 10; i++) // Generate 10 more values
                {
                    long c = checked(a + b); // Check for overflow
                    writer.WriteLine(c);
                    Console.WriteLine($"Fibonacci: {c}");
                    a = b;
                    b = c;
                }
            }

            Console.WriteLine("Fibonacci sequence extended successfully.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Reached the limit of long data type.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void GenerateFibonacciAsTimes()
    {
        Console.Write("Enter the number of Fibonacci numbers to generate: ");
        if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                BigInteger fib = CalculateFibonacci(i);
                TimeSpan time = TimeSpan.FromSeconds((double)fib);
                Console.WriteLine($"Fibonacci: {fib}, Time: {time:hh\\:mm\\:ss}");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
        }
    }

    static void RunContinuousFibonacciWriter(double multiplier, string fileSuffix)
    {
        Console.Write($"Enter a name for the continuous Fibonacci time file (without extension): ");
        string baseFileName = Console.ReadLine();
        string filePath = $"{baseFileName}_{fileSuffix}.txt";

        long a = 0, b = 1;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            Console.WriteLine($"Writing to file: {filePath}");

            for (int i = 0; i < 50; i++) // Example limit instead of infinite loop
            {
                long c = checked(a + b);

                TimeSpan time = TimeSpan.FromSeconds(c * multiplier);
                writer.WriteLine($"Fibonacci: {c}, Time: {time:hh\\:mm\\:ss\\.fff}");
                Console.WriteLine($"Fibonacci: {c}, Time: {time:hh\\:mm\\:ss\\.fff}");
                a = b;
                b = c;

                if (i % 10 == 0) writer.Flush(); // Save every 10 entries
            }
        }

        Console.WriteLine("Finished writing Fibonacci times.");
    }

    static void RunContinuousFibonacciWriterNanoseconds()
    {
        Console.Write($"Enter a name for the Fibonacci time file (nanoseconds, BigInteger): ");
        string baseFileName = Console.ReadLine();
        string filePath = $"{baseFileName}_nanoseconds.txt";

        BigInteger a = 0, b = 1;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < 50; i++) // Example limit instead of infinite loop
            {
                BigInteger c = a + b;
                a = b;
                b = c;

                long seconds = (long)(c / 1_000_000_000);
                long nanoseconds = (long)(c % 1_000_000_000);

                string formattedTime = $"{seconds:D2}:{nanoseconds:D9}";

                writer.WriteLine($"Fibonacci: {c}, Time: {formattedTime}");
                Console.WriteLine($"Fibonacci: {c}, Time: {formattedTime}");
                if (i % 10 == 0) writer.Flush();
            }
        }

        Console.WriteLine("Finished writing Fibonacci nanoseconds.");
    }

    static long[] GetLastTwoNumbersFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2) throw new Exception("File contains less than two numbers.");

        long last = long.Parse(lines[^1]);
        long secondLast = long.Parse(lines[^2]);

        return new[] { secondLast, last };
    }

    static BigInteger CalculateFibonacci(int n)
    {
        if (n == 0) return 0;
        if (n == 1) return 1;

        BigInteger a = 0, b = 1;
        for (int i = 2; i <= n; i++)
        {
            BigInteger temp = a + b;
            a = b;
            b = temp;
        }
        return b;
    }
}
