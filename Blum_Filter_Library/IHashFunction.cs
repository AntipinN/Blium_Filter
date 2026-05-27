namespace Blum_Filter_Library
{
    public interface IHashFunction
    {
        public int GetHashValue(int x, int len);
        public string Show_Equation();
    }
}
