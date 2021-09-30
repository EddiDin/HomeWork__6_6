using System;
using System.IO;
using System.Text.RegularExpressions;

namespace HomeWork__6_6
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = default;

            if (!File.Exists("data.txt"))
            {
                Console.WriteLine("Файл, в котором должно быть число, не обнаружен. Введите число с клавиатуры:");
                number = EnterNumber();
            }
            else
            {
                string data = Regex.Replace(File.ReadAllText("data.txt"), "[^0-9]", "", RegexOptions.IgnoreCase);
                bool successParse = Int32.TryParse(data, out number);

                if (!successParse)
                {
                    Console.WriteLine("Ошибка. Не удалось получить число из файла. Проверьте файл.");
                    return;
                }

                if (!IsValidNumber(number))
                {
                    Console.WriteLine($"Ошибка. Полученное число из файла не удовлетворяет условиям задачи ({number}). Число должно быть в диапазоне от 1 до 1000000000.");
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Число успешно считано/введено - {number}");



            int groupsCount = GetGroupsCountByNumber(number);
            Console.WriteLine($"Число групп  - {groupsCount}");

            WriteGroupsToFile(number, groupsCount);

            Console.ReadKey();
        }

        static void WriteGroupsToFile(int number, int groupsCount)
        {
            string fileName = "output.txt";
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                int counter = 0;
                while (number > 1)
                {
                    int numberInHalf = number / 2;

                    if (numberInHalf >= 1)
                    {
                        sw.Write($"Группа {++counter}: ");
                        for (int i = numberInHalf + 1; i <= number; i++)
                        {
                            sw.Write($"{i} ");
                        }

                        sw.WriteLine();
                        Console.WriteLine($"Записано групп {counter}/{groupsCount}");
                    }

                    number = numberInHalf;
                }

                if (number == 1) {
                    sw.WriteLine($"Группа {++counter}: {number}");
                    Console.WriteLine($"Записано групп {counter}/{groupsCount}");
                }

            }

            Console.WriteLine($"Все группы успешно записаны в файл {fileName}");
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
