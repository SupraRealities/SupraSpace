using UnityEngine;

namespace SupraRealities.SupraSpace.Utilities.ObjectPoolPattern
{
    public abstract class PooledObject : MonoBehaviour
    {
        private ObjectPool pool;

        internal void SetPool(ObjectPool pool)
        {
            this.pool = pool;
        }

        internal virtual void OnTakenFromPool()
        {
            gameObject.SetActive(true);
        }

        internal virtual void OnLeftOnPool()
        {
            gameObject.SetActive(false);
        }

        public void Recycle()
        {
            if (pool == null)
            {
                Debug.LogWarning("No recycling pool was found for this PooledObject. Instead, it has been destroyed. Its name was " + name);
                Destroy(gameObject);
                return;
            }

            pool.RecycleObject(this);
        }
    }
}