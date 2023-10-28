using System.Collections.Generic;

namespace SupraRealities.SupraSpace.Utilities.ObjectPoolPattern
{
    public class ObjectPool
    {
        private PooledObject prefab;
        private Queue<PooledObject> pool;

        public ObjectPool(PooledObject prefab)
        {
            this.prefab = prefab;
            pool = new Queue<PooledObject>();
        }

        public PooledObject GetObject()
        {
            PooledObject objectToReturn;
            if (pool.Count == 0)
            {
                objectToReturn = CreateNewObject();
            }
            else
            {
                objectToReturn = pool.Dequeue();
            }
            objectToReturn.OnTakenFromPool();
            return objectToReturn;
        }

        private PooledObject CreateNewObject()
        {
            PooledObject newObject = UnityEngine.Object.Instantiate(prefab);
            newObject.SetPool(this);
            return newObject;
        }

        internal void RecycleObject(PooledObject objectToRecycle)
        {
            pool.Enqueue(objectToRecycle);
            objectToRecycle.OnLeftOnPool();
        }
    }
}