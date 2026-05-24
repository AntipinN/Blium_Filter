using System.Collections;
using System.Collections.Generic;

namespace Prime_Numbers_Generator_Library
{
    public static class Prime_Number_Generator
    {

        // написано нейронкой.
        public static unsafe List<int> Generate_Prime_Numbers(int limit)
        {
            if (limit < 2)
                return new List<int>();

            // Размер массива: только нечетные числа начиная с 3.
            // Индекс i представляет число (2i + 3).
            int size = (limit - 1) / 2; // количество нечетных чисел от 3 до limit
            var isPrime = new BitArray(size + 1, true); // +1 для запаса, BitArray индексируется до size

            int sqrtLimit = (int)Math.Sqrt(limit);
            // Переводим sqrtLimit в индекс нашего массива нечетных:
            // число -> индекс: (число - 3) / 2
            int sqrtIndex = (sqrtLimit - 3) / 2;

            // Основной цикл просеивания
            for (int i = 0; i <= sqrtIndex; i++)
            {
                if (isPrime[i])
                {
                    // Текущее простое число
                    int p = 2 * i + 3;

                    // Начинаем с p^2. Находим индекс для p^2.
                    // p^2 нечетное, преобразуем в индекс: (p^2 - 3) / 2
                    int pSquared = p * p;
                    int startIndex = (pSquared - 3) / 2;

                    // Шаг в терминах индексов: p (так как пропускаем четные)
                    // Каждое следующее нечетное кратное находится через 2p в числах,
                    // что соответствует шагу p в нашем массиве нечетных.
                    int step = p;

                    for (int j = startIndex; j <= size; j += step)
                    {
                        isPrime[j] = false;
                    }
                }
            }

            // Сбор результатов
            var primes = new List<int>();
            primes.Add(2); // Добавляем единственное четное простое

            for (int i = 0; i <= size; i++)
            {
                if (isPrime[i])
                {
                    primes.Add(2 * i + 3);
                }
            }

            return primes;
        }
    }
}
