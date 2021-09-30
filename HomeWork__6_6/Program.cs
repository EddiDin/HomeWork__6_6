using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO.Compression;

namespace HomeWork__6_6
{
    class Program
    {
        static readonly string inputFileName = "data.txt";

        static readonly string outputFileName = "output.txt";

        static readonly string archiveFileName = "output.zip";

        static void Main(string[] args)
        {
            int number;
            if (!File.Exists(inputFileName))
            {
                Console.WriteLine("Файл, в котором должно быть число, не обнаружен. Введите число с клавиатуры:");
                number = EnterNumber();
            }
            else
            {
                bool isGetNumber = TryGetNumberFromFile(inputFileName, out number, out string message);
                if (!isGetNumber)
                {
                    Console.WriteLine(message);
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Число успешно считано/введено: {number}");

            int groupsCount = GetGroupsCountByNumber(number);
            Console.WriteLine($"Число групп: {groupsCount}");

            Console.WriteLine();
            if (IsUserConfirm("Хотите ли вы записать группы в файл?"))
            {
                Console.WriteLine();
                Console.WriteLine($"Запись групп в файл {outputFileName} ...");
                WriteGroupsToFile(outputFileName, number, groupsCount);
                FileInfo outputFileInfo = new FileInfo(outputFileName);
                Console.WriteLine($"Все группы успешно записаны в файл {outputFileName}. Размер файла на диске = {outputFileInfo.Length / 1024} Кб.");
                Console.WriteLine();

                if (IsUserConfirm("Архивировать получившийся файл?"))
                {
                    using (ZipArchive zipArchive = ZipFile.Open(archiveFileName, ZipArchiveMode.Create))
                    {
                        zipArchive.CreateEntryFromFile(outputFileName, outputFileName);
                    }

                    FileInfo archiveFileInfo = new FileInfo(archiveFileName);
                    Console.WriteLine($"Файл заархивирован успешно. Размер архива на диске = {archiveFileInfo.Length / 1024} Кб.");
                }
            }

            Console.ReadKey();
        }

        static bool TryGetNumberFromFile(string fileName, out int number, out string message)
        {
            message = "";
            string data = Regex.Replace(File.ReadAllText("data.txt"), "[^0-9]", "", RegexOptions.IgnoreCase);
            bool successParse = Int32.TryParse(data, out number);

            if (!successParse)
            {
                message = "Ошибка. Не удалось получить число из файла. Проверьте файл.";
                return false;
            }

            if (!IsValidNumber(number))
            {
                message = $"Ошибка. Полученное число из файла не удовлетворяет условиям задачи ({number}). Число должно быть в диапазоне от 1 до 1000000000.";
                return false;
            }

            return true;
        }

        static bool IsUserConfirm(string message)
        {
            Console.WriteLine(message + " Введите \"д\"(Да) или \"н\"(Нет):");
            while (true)
            {
                string userInput = Console.ReadLine();
                if (userInput != "д" && userInput != "н")
                {
                    Console.WriteLine("Ошибка при вводе. Введите символ \"д\" или символ \"н\".");
                    continue;
                }

                return userInput == "д";
            }

        }

        static void WriteGroupsToFile(string fileName, int number, int groupsCount)
        {
            Stopwatch swatch = new Stopwatch();
            TimeSpan ts;
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                int counter = 0;
                while (number > 1)
                {
                    int numberInHalf = number / 2;

                    if (numberInHalf >= 1)
                    {
                        swatch.Reset();
                        swatch.Start();
                        sw.Write($"Группа {++counter}: ");
                        for (int i = numberInHalf + 1; i <= number; i++)
                        {
                            sw.Write($"{i} ");
                        }

                        sw.WriteLine();
                        swatch.Stop();
                        ts = swatch.Elapsed;
                        Console.WriteLine($"Записано групп {counter}/{groupsCount}. Затрачено {ts.Minutes} мин {ts.Seconds} сек {ts.Milliseconds} мс");
                    }

                    number = numberInHalf;
                }

                if (number == 1)
                {
                    sw.WriteLine($"Группа {++counter}: {number}");
                    Console.WriteLine($"Записано групп {counter}/{groupsCount}");
                }
            }
        }

        static int GetGroupsCountByNumber(int number)
        {
            int groupsCount = 1;
            if (number < 2) return groupsCount;

            while (number > 1)
            {
                groupsCount++;
                number /= 2;
            }

            return groupsCount;
        }

        static int EnterNumber()
        {
            while (true)
            {
                string userInput = Console.ReadLine();
                bool successParse = Int32.TryParse(userInput, out int parsedNumber);
                if (!successParse)
                {
                    Console.WriteLine("Ошибка. Введите число");
                    continue;
                }

                if (!IsValidNumber(parsedNumber))
                {
                    Console.WriteLine("Ошибка. Число должно быть в диапазоне от 1 до 1000000000.");
                    continue;
                }

                return parsedNumber;
            }
        }

        static bool IsValidNumber(int number)
        {
            return (number > 0 && number < 1000000001);
        }
    }
}
