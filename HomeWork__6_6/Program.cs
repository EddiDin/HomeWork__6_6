using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO.Compression;

namespace HomeWork__6_6
{
    class Program
    {
        /// <summary>
        /// Название входящего файла с числом.
        /// </summary>
        static readonly string inputFileName = "data.txt";

        /// <summary>
        /// Название файла результата, в котором будут содержаться группы.
        /// </summary>
        static readonly string outputFileName = "output.txt";

        /// <summary>
        /// Название архива.
        /// </summary>
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
                    if (File.Exists(archiveFileName)) File.Delete(archiveFileName);

                    Console.WriteLine("Запуск архивации файла ...");
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

        /// <summary>
        /// Ввод числа с клавиатуры
        /// </summary>
        /// <returns>Число</returns>
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

        /// <summary>
        /// Попытка получить число из файла.
        /// </summary>
        /// <param name="fileName">Название входящего файла</param>
        /// <param name="number">Число</param>
        /// <param name="message">Сообщение</param>
        /// <returns>true, если число успешно получено. В другом случае - false. Как out параметры само число и сообщение, с описание почему число не было получено</returns>
        static bool TryGetNumberFromFile(string fileName, out int number, out string message)
        {
            message = "";
            string data = Regex.Replace(File.ReadAllText(fileName), "[^0-9]", "", RegexOptions.IgnoreCase);
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

        /// <summary>
        /// Получение кол-ва групп.
        /// </summary>
        /// <param name="number">Число</param>
        /// <returns>Кол-во групп</returns>
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

        /// <summary>
        /// Запрос подтверждения пользователя в консоле.
        /// </summary>
        /// <param name="message">Сообщение для пользователя</param>
        /// <returns>true, если пользователь согласился. В другом случае - false</returns>
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

        /// <summary>
        /// Потоковая запись групп в файл.
        /// </summary>
        /// <param name="fileName">Название результирующего файла</param>
        /// <param name="number">Число</param>
        /// <param name="groupsCount">Кол-во групп</param>
        static void WriteGroupsToFile(string fileName, int number, int groupsCount)
        {
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
                    }

                    number = numberInHalf;
                }

                if (number == 1)
                {
                    sw.WriteLine($"Группа {++counter}: {number}");
                }
            }
        }

        /// <summary>
        /// Валидация числа
        /// </summary>
        /// <param name="number">Число</param>
        /// <returns>true или false</returns>
        static bool IsValidNumber(int number)
        {
            return (number > 0 && number < 1000000001);
        }
    }
}
