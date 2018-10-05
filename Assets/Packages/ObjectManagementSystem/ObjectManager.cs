using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem
{
    public abstract class ObjectManager<T> : MonoBehaviour
    {
        #region Field

        [SerializeField]
        protected int maxCount = 100;

        protected List<ManagedObject<T>> managedObjects;

        protected bool isInitialized;

        #endregion Field

        #region Property

        public int MaxCount
        {
            get { return this.maxCount; }
            set
            {
                this.maxCount = value;
                TrimManagedObjects();
            }
        }

        public ReadOnlyCollection<ManagedObject<T>> ManagedObjects
        {
            private set;
            get;
        }

        public bool IsInitialized
        {
            get { return this.isInitialized; }
        }

        #endregion Property

        #region Method

        protected virtual void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }

            this.managedObjects = new List<ManagedObject<T>>();
            this.ManagedObjects = new ReadOnlyCollection<ManagedObject<T>>(this.managedObjects);

            this.isInitialized = true;
        }

        public virtual U AddManagedObject<U>(GameObject gameObject) where U : ManagedObject<T>
        {
            if (CheckManagedObjectCountIsMax())
            {
                return null;
            }

            U managedObject = gameObject.AddComponent(typeof(U)) as U;
            T data = ReturnManagedObjectData(gameObject);

            managedObject.Initialize(this, data);

            this.managedObjects.Add(managedObject);

            return managedObject;
        }

        protected abstract T ReturnManagedObjectData(GameObject managedGameObject);

        public virtual bool ReleaseManagedObject(ManagedObject<T> managedObject, bool fromOnDestroy = false)
        {
            // NOTE:
            // (1) Remove reference from list when called from ManagedObject.OnDestroy.
            // (2) Destroy object when called from ReleaseManagedObject directly.

            if (fromOnDestroy)
            {
                this.managedObjects.Remove(managedObject);
            }
            else
            {
                GameObject.Destroy(managedObject);
            }

            return true;
        }

        public virtual void ReleaseAllManagedObjects()
        {
            int count = this.managedObjects.Count - 1;

            for (int i = count; i >= 0; i--)
            {
                ReleaseManagedObject(this.managedObjects[i]);
            }
        }

        public virtual void ReleaseOldManagedObjects(int releaseCount)
        {
            for (int i = 0; i < releaseCount; i++)
            {
                if (this.managedObjects.Count > 0)
                {
                    ReleaseManagedObject(this.managedObjects[0]);
                }
            }
        }

        public virtual bool RemoveManagedObject(ManagedObject<T> managedObject)
        {
            if (!CheckObjectIsManaged(managedObject))
            {
                return false;
            }

            GameObject.Destroy(managedObject.gameObject);

            return true;
        }

        public virtual void RemoveAllManagedObjects()
        {
            int count = this.managedObjects.Count - 1;

            for (int i = count; i >= 0; i--)
            {
                RemoveManagedObject(this.managedObjects[i]);
            }
        }

        public virtual void RemoveOldManagedObjects(int removeCount)
        {
            for (int i = 0; i < removeCount; i++)
            {
                if (this.managedObjects.Count > 0)
                {
                    RemoveManagedObject(this.managedObjects[0]);
                }
            }
        }

        public virtual void TrimManagedObjects()
        {
            int trimCount = this.managedObjects.Count - this.maxCount;

            if (trimCount > 0)
            {
                RemoveOldManagedObjects(trimCount);
            }
        }

        public bool CheckManagedObjectCountIsMax()
        {
            return this.managedObjects.Count >= this.maxCount;
        }

        public bool CheckObjectIsManaged(ManagedObject<T> managedObject)
        {
            return managedObject.ObjectManager == this;
        }

        #endregion Method
    }
}