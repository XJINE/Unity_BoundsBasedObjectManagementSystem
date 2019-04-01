using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem
{
    public class ObjectManager<T> : MonoBehaviour, IInitializable
    {
        #region Property

        protected List<ManagedObject<T>> managedObjects;

        public ReadOnlyCollection<ManagedObject<T>> ManagedObjects
        {
            private set;
            get;
        }

        [SerializeField]
        protected int maxCount = 100;

        public int MaxCount
        {
            get { return this.maxCount; }
            set
            {
                this.maxCount = value;
                TrimManagedObjects();
            }
        }

        public bool IsFilled
        {
            get { return this.managedObjects.Count >= this.maxCount; }
        }

        public bool IsInitialized
        {
            get; protected set;
        }

        #endregion Property

        #region Method

        protected virtual void Awake()
        {
            Initialize();
        }

        public virtual bool Initialize()
        {
            if (this.IsInitialized)
            {
                return false;
            }
            
            this.IsInitialized = true;

            this.managedObjects = new List<ManagedObject<T>>();
            this.ManagedObjects = new ReadOnlyCollection<ManagedObject<T>>(this.managedObjects);

            return true;
        }

        public virtual U AddManagedObject<U>(GameObject gameObject) where U : ManagedObject<T>
        {
            if (this.IsFilled)
            {
                return null;
            }

            U managedObject = gameObject.AddComponent(typeof(U)) as U;
            managedObject.ObjectManager = this;

            this.managedObjects.Add(managedObject);

            return managedObject;
        }

        public virtual void ReleaseManagedObject(ManagedObject<T> managedObject)
        {
            // CAUTION:
            // Release must be done immediately.
            // So it must be done Remove() before GameObject.Destroy().

            if (this.IsManage(managedObject))
            {
                if (this.managedObjects.Remove(managedObject))
                {
                    // NOTE:
                    // ManagedObject.OnDestroy() will call this function again,
                    // but this.IsManage() step will be failed.

                    GameObject.Destroy(managedObject);
                }
            }
        }

        public virtual void ReleaseAllManagedObjects()
        {
            int count = this.managedObjects.Count - 1;

            for (int i = count; i >= 0; i--)
            {
                ReleaseManagedObject(this.managedObjects[i]);
            }
        }

        public virtual void RemoveManagedObject(ManagedObject<T> managedObject)
        {
            if (this.IsManage(managedObject))
            {
                this.managedObjects.Remove(managedObject);
                GameObject.Destroy(managedObject.gameObject);
            }
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

        public bool IsManage(ManagedObject<T> managedObject)
        {
            return managedObject.ObjectManager == this;
        }

        #endregion Method
    }
}