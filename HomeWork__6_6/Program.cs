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

                if (!successParse) {
                    Console.WriteLine("Ошибка. Не удалось получить число из файла. Проверьте файл.");
                    return;
                }

                if (!IsValidNumber(number)) {
                    Console.WriteLine($"Ошибка. Полученное число из файла не удовлетворяет условиям задачи ({number}). Число должно быть в диапазоне от 1 до 1000000000.");
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Число успешно считано/введено - {number}");
            Console.ReadKey();
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
