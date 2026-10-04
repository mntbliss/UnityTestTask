using UnityEngine;

namespace _Bludoku.Scripts.Helpers.Pooling
{
    public class MonoPoolableItem : MonoBehaviour, IPoolableItem
    {
        public bool IsPoolable { get; protected set; }

        public virtual void SetPoolable(bool value)
        {
            if (IsPoolable == value) return;

            IsPoolable = value;
        }
    }
}
