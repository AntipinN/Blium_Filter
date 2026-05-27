using Blum_Filter_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal_Hash_Func_Generator
{
    public record Hash_Function: IHashFunction
    {
        long p;
        int t, s;
        public Hash_Function(long p, int t, int s)
        {
            this.p = p;
            this.t = t;
            this.s = s;
        }

        public int GetHashValue(int x, int m)
        {
            int hash = (int)((t * x + s) % p) % m;
            if (hash < 0) hash += m;
            return hash;
        }

        public string Show_Equation()
        {
            return $"(({t}*x + {s}) % {p}) % m";
        }
    }
}
