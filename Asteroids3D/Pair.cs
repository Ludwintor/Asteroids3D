namespace Asteroids3D
{
    public struct Pair<T>
    {
        public T Left;
        public T Right;

        public Pair(T left, T right)
        {
            Left = left;
            Right = right;
        }
    }
}
