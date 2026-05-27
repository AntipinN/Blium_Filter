using System.Collections;
using System;
using System.Collections.Generic;
using Blum_Filter_Library;
using Universal_Hash_Func_Generator;
using System.Diagnostics;
using System.Text;

namespace Blum_Filter;
internal class Program
{
    private static void Main(string[] args)
    {
        // Если задаём массив и предполагаемое число вносимых значений
        //int m = 100;
        //double n = 30;

        //Blum_Filter blum_Filter = new Blum_Filter(n, m);

        //Console.WriteLine($"Оптимальное число хеш функций равно {blum_Filter.k}");
        //Console.WriteLine($"Вероятность ложно положительного срабатывания не более {blum_Filter.e}");

        //Если задаём предполагаемое число значений и допустимую вероятность ложного положительного срабатывания
        Console.Write("Введите желаемое количество сохранённых значений: n = ");
        double n = double.Parse(Console.ReadLine());

        Console.Write("\nВведите желаемую вероятность коллизии (0 ; 1): e = ");
        double e = double.Parse(Console.ReadLine());

        BlumFilter blum_Filter = new BlumFilter(n, e, new Hash_Func_Generator());
        HashSet<int> list_Of_Added_Numbers = new HashSet<int>();
        Console.WriteLine($"Размер массива равен {blum_Filter.m}");
        Console.WriteLine($"Оптимальное число хеш функций равно {blum_Filter.k}");
        Console.WriteLine();
        bool exitFlag = false;
        while (!exitFlag)
        {
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Проверить существование элемента");
            Console.WriteLine("3. Наглядно проверить существование элемента");
            Console.WriteLine("4. Вывести массив");
            Console.WriteLine("5. Показать все хэш функции");
            Console.WriteLine("6. Автотест. Покажет совпадающие значения и выдаст вероятность по формуле (кол-во коллизий)/(мощность множества ключей 2^31)");
            Console.WriteLine("7. Пересоздать фильтр Блюма");
            Console.WriteLine();
            string entered_value = Console.ReadLine();
            Console.WriteLine();
            switch (entered_value)
            {
                case "1":
                    {
                        Console.WriteLine("Введите добавляемое число");
                        int number;
                        try
                        {
                            number = int.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine();
                            break;
                        }
                        if (list_Of_Added_Numbers.Contains(number))
                        {
                            Console.WriteLine("Число уже было добавлено в массив. Пропускаю.");
                            break;
                        }
                        if ((double)blum_Filter.number_added_items >= n)
                        {
                            Console.WriteLine("Внимание, фильтр не рассчитан на большее количество значений.");
                            Console.WriteLine("Остановить внесение нового значения? [д/н]");
                            if (Console.ReadLine().ToLower() == "д")
                            {
                                break;
                            }
                        }
                        list_Of_Added_Numbers.Add(number);
                        List<(int, bool)> answer_add = new List<(int, bool)>();
                        answer_add = blum_Filter.Add_Array(number);
                        Console.WriteLine();

                        Console.Write("Индексы: ");
                        Show_Indexes(answer_add);
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"Всего добавленных элементов {blum_Filter.number_added_items}");
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Введите число для проверки");
                        int number = int.Parse(Console.ReadLine());
                        bool belongs = blum_Filter.Сheck_element(number);
                        Console.WriteLine();
                        if (belongs)
                        {
                            Console.WriteLine("Элемент принадлежит множеству");
                        }
                        else
                        {
                            Console.WriteLine("Элемент не принадлежит множеству");
                        }
                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("Введите число для проверки");
                        int number = int.Parse(Console.ReadLine());
                        List<(int, bool)> answer_check = new List<(int, bool)>();
                        bool belongs;
                        (answer_check, belongs) = blum_Filter.Сheck_element_detailed(number);
                        Console.WriteLine();
                        Console.Write("Индексы: ");
                        Show_Indexes(answer_check);
                        Console.WriteLine();
                        if (belongs)
                        {
                            Console.WriteLine("Элемент принадлежит множеству");
                        }
                        else
                        {
                            Console.WriteLine("Элемент не принадлежит множеству");
                        }
                        break;
                    }
                case "4":
                    {
                        BitArray array = blum_Filter.Array_output();
                        for (int l = 0; l < array.Count; l++)
                        {
                            Show_Indexes(l, array[l]);
                            if ((l + 1) % 10 == 0)
                            {
                                Console.WriteLine();
                            }
                        }
                        Console.WriteLine();
                        foreach (int t in list_Of_Added_Numbers)
                        {
                            Console.Write($"{t} ");
                        }
                        break;
                    }
                case "5":
                    {
                        string[] hashStrings = blum_Filter.Show_Current_Has_Functions();
                        foreach (string k in hashStrings)
                        {
                            Console.WriteLine(k);
                        }
                        break;
                    }
                case "6":
                    {
                        blum_Filter = new BlumFilter(n, e, new Hash_Func_Generator());
                        list_Of_Added_Numbers = new HashSet<int>();

                        Random rnd = new Random();
                        for (int j = 0; j < n; j++)
                        {
                            int tmp = rnd.Next();
                            if (list_Of_Added_Numbers.Contains(tmp))
                            {
                                j--;
                            }
                            list_Of_Added_Numbers.Add(tmp);
                            blum_Filter.Add_Array(tmp);
                        }
                        Console.WriteLine();
                        int counter = (int)(CountExistingElements(blum_Filter) - n);

                        Console.WriteLine($"Вероятность коллизии в массиве длины m = {blum_Filter.m},\n" +
                            $" количестве внесённых элементов n = {n},\n" +
                            $"  мощности множества ключей 2^31\n" +
                            $"    желаемой вероятности коллизии e = {e}\n " +
                            $"     итоговая вероятность коллизии = {(counter) * 1.0 / int.MaxValue}" );
                        
                        break;
                    }
                case "7":
                    {
                        Console.Write("Введите желаемое количество сохранённых значений: n = ");
                        n = double.Parse(Console.ReadLine());

                        Console.Write("\nВведите желаемую вероятность коллизии (0 ; 1): e = ");
                        e = double.Parse(Console.ReadLine());

                        blum_Filter = new BlumFilter(n, e, new Hash_Func_Generator());
                        list_Of_Added_Numbers = new HashSet<int>();
                        Console.WriteLine($"Размер массива равен {blum_Filter.m}");
                        Console.WriteLine($"Оптимальное число хеш функций равно {blum_Filter.k}");
                        Console.WriteLine();
                        break;
                    }
                default:
                    Console.WriteLine("Хотите завершить работу? [д/н]");
                    if (Console.ReadLine().ToLower() == "д")
                    {
                        exitFlag = true;
                    }
                    break;
            }
            Console.WriteLine();
        }
        Console.WriteLine("Нажмите любую кнопку для завершения работы.");
        Console.ReadKey();
    }
    #region Нейро параллелелизм
    public static long CountExistingElements(BlumFilter blum_Filter)
    {
        long totalCount = (long)int.MaxValue + 1;
        const int ChunkSize = 10_000_000;
        int numberOfChunks = (int)(totalCount / ChunkSize) + 1;

        var chunks = Enumerable.Range(0, numberOfChunks)
            .Select(chunkIndex =>
            {
                long start = (long)chunkIndex * ChunkSize;
                int count = (int)Math.Min(ChunkSize, totalCount - start);
                return (Start: start, Count: count);
            })
            .Where(c => c.Count > 0)
            .ToArray();
        
        return chunks
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .Sum(chunk => ProcessChunk(chunk.Start, chunk.Count, blum_Filter));
    }

    private static int ProcessChunk(long start, int count, BlumFilter blum_Filter)
    {
        int localTrueCount = 0;
        long end = start + count;

        for (long i = start; i < end; i++)
        {
            if (blum_Filter.Сheck_element((int)i))
                localTrueCount++;
        }

        return localTrueCount;
    }
    #endregion

    private static void Show_Indexes(List<(int, bool)> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            Show_Indexes(array[i].Item1, array[i].Item2);
        }
    }

    private static void Show_Indexes(int i, bool b)
    {
        //ConsoleColor color = Console.BackgroundColor;
        if (b)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{i,8} ");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{i,8} ");
        }
        Console.ResetColor();
        Console.Write(" ");
    }

}