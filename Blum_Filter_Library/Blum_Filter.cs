using System.Collections;

namespace Blum_Filter_Library
{
    public class BlumFilter
    {
        public BitArray array;
        public int m;           /*размер массива*/
        public double n;           /*предпологаемое число элементов в массиве*/
        public int k;              /*число хеш функций*/
        public double e;          /*допустимую вероятность ложного положительного срабатывания*/
        public int number_added_items = 0; /*количество добавленых элементов*/

        private List<IHashFunction> _hashFunctions;

        public BlumFilter(double n, int m, IHashFuncGenerator hFG)
        {
            this.n = n;
            this.m = m;
            k = (int)((double)m / n * Math.Log(2.0));
            if (k < 1)
            {
                k = 1;
            }
            e = Math.Pow(0.5, (double)m / n * Math.Log(2.0));
            array = new BitArray(m, defaultValue: false);
            _hashFunctions = hFG.Generate_Hash_Func(k);
        }

        public BlumFilter(double n, double e, IHashFuncGenerator hFG)
        {
            this.n = n;
            this.e = e;
            m = (int)((0.0 - n * Math.Log(e)) / (Math.Log(2.0) * Math.Log(2.0)));
            k = (int)(-Math.Log(e) / Math.Log(2.0));
            
            if (k < 1)
            {
                k = 1;
            }
            array = new BitArray(m, defaultValue: false);
            _hashFunctions = hFG.Generate_Hash_Func(k);
        }

        public BitArray Array_output()
        {
            return array;
        }

        public List<(int, bool)> Add_Array(int added_number)
        {
            List<(int, bool)> answer = new List<(int, bool)>();
            for (int i = 0; i < k; i++)
            {
                int hash = _hashFunctions[i].GetHashValue(added_number, m);
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
                if (!array[_hashFunctions[i].GetHashValue(check_number, m)])
                {
                    answer = false;
                    break;
                }
            }
            return answer;
        }

        public (List<(int, bool)>, bool) Сheck_element_detailed(int check_number)
        {
            List<(int, bool)> answer_array = new List<(int, bool)>();
            bool answer_bool = true;
            for (int i = 0; i < k; i++)
            {
                int hash = _hashFunctions[i].GetHashValue(check_number, m);
                answer_array.Add((hash, array[hash]));
                if (!array[hash])
                {
                    answer_bool = false;
                }
            }
            return (answer_array, answer_bool);
        }

        public string[] Show_Current_Has_Functions()
        {
            string[] result = new string[_hashFunctions.Count];
            for (int i = 0; i < _hashFunctions.Count; i++)
            {
                result[i] = _hashFunctions[i].Show_Equation();
            }
            return result;
        }
    }
}
