namespace ClassLibrary2
{
    public class DortIslem
    {
        public int Topla(int x, int y)
        {
            return x + y;
        }
        public int Cikar(int x, int y)
        {
            return x - y;
        }
        public int Carp(int x, int y)
        {
            return x * y;
        }
        public int Bol(int x, int y)
        {
            return x / y;
        }
        private double Hesapla(int x, int y)
        {
            return Topla(x, y);
        }
    }
}