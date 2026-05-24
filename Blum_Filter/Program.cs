using System.Collections;
using System;
using System.Collections.Generic;
using Blum_Filter_Library;
using Universal_Hash_Func_Generator;

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

        //Если задаём предпологаемое число значений и допустимую вероятность ложного положительного срабатывания
        double m = 100.0;
        double e = 2E-07;
        BlumFilter blum_Filter = new BlumFilter(m, e, new Hash_Func_Generator());
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
            Console.WriteLine("6. Автотест. Покажет совпадающие значения и выдаст вероятность по формуле (кол-во коллизий)/(размер инт 32)");
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
                        if ((double)blum_Filter.number_added_items >= m)
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
                        Random rnd = new Random();
                        for (int j = 0; (double)j < m; j++)
                        {
                            int tmp = rnd.Next();
                            blum_Filter.Add_Array(tmp);
                            Console.Write($"{tmp,12} ");
                        }
                        Console.WriteLine();
                        int counter = 0;
                        for (int i = 0; i < int.MaxValue; i++)
                        {
                            if (blum_Filter.Сheck_element(i))
                            {
                                counter++;
                                Console.Write($"{i,12} ");
                            }
                        }
                        Console.WriteLine(((double)counter - m) * 1.0 / int.MaxValue);
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

    private static void Show_Indexes(List<(int, bool)> array)
    {
        ConsoleColor color = Console.BackgroundColor;
        for (int i = 0; i < array.Count; i++)
        {
            if (array[i].Item2)
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.Write($"{array[i].Item1,6}");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{array[i].Item1,6}");
            }
            Console.BackgroundColor = color;
            Console.Write(" ");
        }
    }

    private static void Show_Indexes(int i, bool b)
    {
        ConsoleColor color = Console.BackgroundColor;
        if (b)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{i,6}");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write($"{i,6}");
        }
        Console.BackgroundColor = color;
        Console.Write(" ");
    }

}