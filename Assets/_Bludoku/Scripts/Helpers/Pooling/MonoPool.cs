using System.Collections.Generic;
using UnityEngine;

namespace _Bludoku.Scripts.Helpers.Pooling
{
    public class MonoPool<TPoolItem>
        where TPoolItem : MonoPoolableItem
    {
        public HashSet<TPoolItem> Pool { get; protected set; }

        private readonly TPoolItem prefab;
        private readonly Transform parent;

        public MonoPool(TPoolItem prefab, Transform parent, int basePoolAmount)
        {
            this.prefab = prefab;
            this.parent = parent;

            Initialize(basePoolAmount);
        }

        public TPoolItem GetFreeElement()
        {
            if (Pool == null) Initialize();

            foreach (TPoolItem item in Pool)
            {
                if (!item.IsPoolable) continue;

                item.SetPoolable(false);
                return item;
            }

            TPoolItem newItem = CreateItem();
            newItem.SetPoolable(false);
            return newItem;
        }

        public void SetFreeElement(TPoolItem element)
        {
            if (element == null) return;

            element.SetPoolable(true);
        }

        public void FreePool(bool value = true)
        {
            if (Pool == null || Pool.Count <= 0)
                return;

            foreach (TPoolItem item in Pool)
            {
                if (item.IsPoolable == value) continue;

                item.SetPoolable(value);
            }
        }

        protected void Initialize(int basePoolAmount = 2)
        {
            Pool = new HashSet<TPoolItem>();
            for (int i = 0; i < basePoolAmount; i++)
            {
                CreateItem();
            }
        }

        protected TPoolItem CreateItem()
        {
            TPoolItem item = Object.Instantiate(prefab, parent);
            item.SetPoolable(true);
            Pool.Add(item);
            return item;
        }
    }
}
