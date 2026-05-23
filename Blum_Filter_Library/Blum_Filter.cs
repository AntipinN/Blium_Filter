using System.Collections;

namespace Blum_Filter_Library
{
    public class Blum_Filter
    {
        public BitArray array;
        public int m;           /*размер массива*/
        public double n;           /*предпологаемое число элементов в массиве*/
        public int k;              /*число хеш функций*/
        public double e;          /*допустимую вероятность ложного положительного срабатывания*/
        public int number_added_items = 0; /*количество добавленых элементов*/

        public Blum_Filter (double n, int m)
        {
            this.n = n;
            this.m = m;
            k = (int)((m / n) * Math.Log(2));
            if(k<1)
            {
                k = 1;
            }
            e = Math.Pow(1.0 / 2.0, (m / n) * Math.Log(2));
            array = new BitArray(m,false);

        }

        public Blum_Filter(double n,double e)
        {
            this.n = n;
            this.e = e;
            m = (int)(-(n * Math.Log(e)) / (Math.Log(2) * Math.Log(2)));
            k = (int)(-Math.Log(e) / Math.Log(2));

            k = (int)((m / n) * Math.Log(2));
            if (k < 1)
            {
                k = 1;
            }
            array = new BitArray(m, false);

        }

        private int Hash(int i, int number)
        {
            return (number * (i + 2) + 7) % m;
        }

        public void Array_output()
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine($"{i} {array[i]}");
            }
        }

        public List<(int, bool)> Add_Array(int added_number)
        {
            int hash;
            List<(int, bool)> answer = new List<(int, bool)>();
            for (int i = 0; i < k; i++)
            {
                hash = Hash(i, added_number);
                answer.Add((hash, array[hash]));
                array[hash] = true;
            }
            number_added_items++;
            return answer;
        }

        public bool Сheck_element(int check_number)
        {
            bool answer = true;

            for (int i = 0; i < k; i++)
            {
                if (array[Hash(i, check_number)] == false)
                {
                    answer = false;
                }
            }

            return answer;
        }

        public (List<(int, bool)>,bool) Сheck_element_detailed(int check_number)
        {
            int hash;
            List<(int, bool)> answer_array = new List<(int, bool)>();
            bool answer_bool = true;
            
            for (int i = 0; i < k; i++)
            {
                hash = Hash(i, check_number);
                answer_array.Add((hash, array[hash]));
                if (array[hash] == false)
                {
                    answer_bool = false;
                }
            }
            return (answer_array, answer_bool);
           
        }
    }
}
