using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main(string[] args)
    {
        // Если задаём массив и предполагаемое число вносимых значений
        //double m = 100;
        //double n = 30;

        //int k = (int)((m / n) * Math.Log(2));

        //double e = Math.Pow(1.0 / 2.0, (m / n) * Math.Log(2));


        //Console.WriteLine($"Оптимальное число хеш функций равно {k}");
        //Console.WriteLine($"Вероятность ложно положительного срабатывания не более {e}");

        //Если задаём предпологаемое число значений и допустимую вероятность ложного положительного срабатывания
        double n = 10;
        double e = 0.2;

        int m = (int)(-(n * Math.Log(e)) / (Math.Log(2) * Math.Log(2)));

        int k = (int)(-Math.Log(e) / Math.Log(2));

        if (k < 1)
        {
            k = 1;
        }

        Console.WriteLine($"Размер массива равен {m}");
        Console.WriteLine($"Оптимальное число хеш функций равно {k}");

        BitArray array = new BitArray(m, false);

        Console.WriteLine();


        string entered_value;
        bool exitFlag = false;
        int number_added_items = 0;
        int number;

        while (!exitFlag)
        {
            Console.WriteLine("1.Добавить элемент");
            Console.WriteLine("2.Проверить существование элемента");
            Console.WriteLine("3.Вывести массив");
            Console.WriteLine();
            entered_value = Console.ReadLine();

            switch (entered_value)
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("Введите добавляемое число");
                    number = int.Parse(Console.ReadLine());
                    List<(int, bool)> answer = new List<(int, bool)>();
                    answer = Add_Array(k,ref array, number);
                    
                    Console.WriteLine() ;
                    Console.Write("Индексы: ");
                    for (int i=0;i<answer.Count;i++)
                    {
                        if (answer[i].Item2 == true)
                        {
                            Console.Write($"{answer[i].Item1}(есть) ");
                        }
                        else
                        {
                            Console.Write($"{answer[i].Item1}(не было) ");
                        }
                        
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    number_added_items++;
                    Console.WriteLine($"Всего добавленных элементов {number_added_items}");
                    break;
                    
                case "2":
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Введите число для проверки");
                    number = int.Parse(Console.ReadLine());

                    bool belongs = Сheck_element(k,ref array,number);

                    if(belongs)
                    {
                        Console.WriteLine("Элемент принадлежит множеству");
                    }
                    else
                    {
                        Console.WriteLine("Элемент не принадлежит множеству");
                    }    

                break;

                case "3":
                    Console.WriteLine();

                    Array_output(array);

                break;

                default:
                    exitFlag = true;
                    break;
            }
            Console.WriteLine();

        }
        Console.WriteLine("Программа завершена. Нажмите любую клавишу для закрытия.");
        Console.ReadKey();
    }

    static private int Hash(int i, int length_array, int number)
    {
        return (number * (i + 2) + 7) % length_array;
    }

    static public void Array_output(BitArray array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.WriteLine($"{i} {array[i]}");
        }
    }

    static public List<(int,bool)> Add_Array(int k, ref BitArray array, int added_number)
    {
        int hash;
        List<(int, bool)> answer = new List<(int, bool)>();
        for (int i = 0; i < k; i++)
        {
            hash = Hash(i, array.Length, added_number);
            answer.Add((hash, array[hash]));
            array[hash] = true;
        }
        return answer;
    }

    static public bool Сheck_element(int k, ref BitArray array, int check_number)
    {
        bool answer = true;

        for (int i = 0; i < k; i++)
        {
            if (array[Hash(i, array.Length, check_number)] == false)
            {
                answer = false;
            }
        }

        return answer;

    }

}