using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Blum_Filter_Library;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        double n = 10;
        double e = 0.2;
         
        Blum_Filter blum_Filter = new Blum_Filter(n, e);

        Console.WriteLine($"Размер массива равен {blum_Filter.m}");
        Console.WriteLine($"Оптимальное число хеш функций равно {blum_Filter.k}");

        Console.WriteLine();


        string entered_value;
        bool exitFlag = false;
        int number;
        bool belongs;

        while (!exitFlag)
        {
            Console.WriteLine("1.Добавить элемент");
            Console.WriteLine("2.Проверить существование элемента");
            Console.WriteLine("3.Наглядно проверить существование элемента");
            Console.WriteLine("4.Вывести массив");
            Console.WriteLine();
            entered_value = Console.ReadLine();

            switch (entered_value)
            {
                case "1":
                    Console.WriteLine();
                    Console.WriteLine("Введите добавляемое число");
                    number = int.Parse(Console.ReadLine());              //number = int.TryParse(Console.ReadLine(),out number) ? number : 0;
                    
                    List<(int, bool)> answer_add = new List<(int, bool)>();
                    answer_add = blum_Filter.Add_Array(number);
                    
                    Console.WriteLine() ;
                    Console.Write("Индексы: ");
                    for (int i=0;i< answer_add.Count;i++)
                    {
                        if (answer_add[i].Item2 == true)
                        {
                            Console.Write($"{answer_add[i].Item1}(есть) ");
                        }
                        else
                        {
                            Console.Write($"{answer_add[i].Item1}(не было) ");
                        }
                        
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"Всего добавленных элементов {blum_Filter.number_added_items}");
                    break;
                    
                case "2":               
                    Console.WriteLine();
                    Console.WriteLine("Введите число для проверки");
                    number = int.Parse(Console.ReadLine());

                    belongs = blum_Filter.Сheck_element(number);

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

                case "3":

                    Console.WriteLine();
                    Console.WriteLine("Введите число для проверки");
                    number = int.Parse(Console.ReadLine());

                    List<(int, bool)> answer_check = new List<(int, bool)>();
                    (answer_check, belongs) = blum_Filter.Сheck_element_detailed(number);
                    
                    Console.WriteLine();
                    Console.Write("Индексы: ");
                    for (int i = 0; i < answer_check.Count; i++)
                    {
                        if (answer_check[i].Item2 == true)
                        {
                            Console.Write($"{answer_check[i].Item1}(есть) ");
                        }
                        else
                        {
                            Console.Write($"{answer_check[i].Item1}(не было) ");
                        }

                    }

                    Console.WriteLine();
                    if (belongs)
                    {
                        Console.WriteLine("Элемент принадлежит множеству");
                    }
                    else
                    {
                        Console.WriteLine("Элемент не принадлежит множеству");
                    }

                    Console.WriteLine();

                break;


                case "4":
                    Console.WriteLine();

                    blum_Filter.Array_output();

                break;

                default:
                    exitFlag = true;
                    break;
            }
            Console.WriteLine();
            
            Thread.Sleep(2000);
        }
        Console.WriteLine("Программа завершена. Нажмите любую клавишу для закрытия.");
        Console.ReadKey();
    }
   
}