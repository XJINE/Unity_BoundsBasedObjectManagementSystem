using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem
{
    public class ObjectManager<DATA> : MonoBehaviour, IInitializable
    {
        #region Field

        public int maxCount = 100;

        #endregion Field

        #region Property

        protected List<ManagedObject<DATA>> managedObjects;

        public ReadOnlyCollection<ManagedObject<DATA>> ManagedObjects
        {
            private set;
            get;
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

            this.managedObjects = new List<ManagedObject<DATA>>();
            this.ManagedObjects = new ReadOnlyCollection<ManagedObject<DATA>>(this.managedObjects);

            return true;
        }

        public virtual MANAGED_OBJECT AddManagedObject<MANAGED_OBJECT>(GameObject gameObject)
                 where MANAGED_OBJECT : ManagedObject<DATA>
        {
            // NOTE:
            // Because of Unity can not add generic MonoBehaviour to an object,
            // It is hard to handle/limit ManagedObject<> type.

            if (this.IsFilled)
            {
                return null;
            }

            var managedObject = gameObject.AddComponent(typeof(MANAGED_OBJECT)) as MANAGED_OBJECT;

            managedObject.Manager = this;

            this.managedObjects.Add(managedObject);

            return managedObject;
        }

        public virtual void ReleaseManagedObject(ManagedObject<DATA> managedObject)
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

        public bool IsManage(ManagedObject<DATA> managedObject)
        {
            return managedObject.Manager == this;
        }

        #endregion Method
    }
}