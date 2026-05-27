using Prime_Numbers_Generator_Library;
using Blum_Filter_Library;

namespace Universal_Hash_Func_Generator
{
    public class Hash_Func_Generator(int default_Amount_Limit = (int)5e5) : IHashFuncGenerator
    {
        public 
        long p = 2147483659;
        int Default_Amount_Limit = default_Amount_Limit;

        public List<IHashFunction> Generate_Hash_Func(int amount_Of_Functions)
        { 
            List<int> primes = Prime_Number_Generator.Generate_Prime_Numbers(Default_Amount_Limit);
            // Немного встряхнём простые числа
            Random rnd = new Random();
            PriorityQueue<int, int> que = new();
            for(int i = 0; i < primes.Count; i++)
            {
                que.Enqueue(primes[i], rnd.Next());
            }
            List<IHashFunction> result = new();
            
            while( que.Count > 1 && result.Count != amount_Of_Functions)
            {
                result.Add(new Hash_Function(p, que.Dequeue(), que.Dequeue()));
            }
            
            return result;
        }

    }
}
