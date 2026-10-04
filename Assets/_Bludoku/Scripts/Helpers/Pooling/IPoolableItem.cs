namespace _Bludoku.Scripts.Helpers.Pooling
{
    public interface IPoolableItem
    {
        bool IsPoolable { get; }

        void SetPoolable(bool value);
    }
}
