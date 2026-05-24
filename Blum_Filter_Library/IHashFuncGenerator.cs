namespace Blum_Filter_Library
{
    public interface IHashFuncGenerator
    {
        public List<IHashFunction> Generate_Hash_Func(int amount);
    }
}
