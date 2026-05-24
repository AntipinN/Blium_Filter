using Prime_Numbers_Generator_Library;
using Blum_Filter_Library;

namespace Universal_Hash_Func_Generator
{
    public class Hash_Func_Generator: IHashFuncGenerator
    {
        public 
        long p = 2147483659;
        int Prime_Default_Amount = (int)1e5;
        public List<IHashFunction> Generate_Hash_Func(int amount)
        { 
            List<int> primes = Prime_Number_Generator.Generate_Prime_Numbers(Prime_Default_Amount);
            // Немного встряхнём простые числа
            Random rnd = new Random();
            PriorityQueue<int, int> que = new();
            for(int i = 0; i < primes.Count; i++)
            {
                que.Enqueue(primes[i], rnd.Next());
            }
            List<IHashFunction> result = new();
            
            while( que.Count > 1 && result.Count != amount )
            {
                result.Add(new Hash_Function(p, que.Dequeue(), que.Dequeue()));
            }
            
            return result;
        }

    }
}
