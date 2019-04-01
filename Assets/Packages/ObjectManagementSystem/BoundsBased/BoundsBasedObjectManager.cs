using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem.BoundsBased
{
    public abstract class BoundsBasedObjectManager<BOUNDS, DATA>
        : MonoBehaviour, IInitializable where BOUNDS : TransformBoundsMonoBehaviour
    {
        #region Field

        public static readonly int OutOfBoundsIndex = -1;

        public int maxCount = 100;

        #endregion Field

        #region Property

        protected List<BoundsBasedManagedObject<BOUNDS, DATA>> managedObjects;

        public ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>> ManagedObjects
        {
            private set;
            get;
        }

        public bool IsFilled
        {
            get { return this.managedObjects.Count >= this.maxCount; }
        }

        public bool IsInitialized { get; protected set; }

        [SerializeField]
        protected List<BOUNDS> bounds;

        public ReadOnlyCollection<BOUNDS> Bounds { get; private set; }

        protected List<List<BoundsBasedManagedObject<BOUNDS, DATA>>> managedObjectsInBounds;

        public ReadOnlyCollection<ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>> ManagedObjectsInBounds 
        {
            get;
            private set;
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

            this.managedObjects = new List<BoundsBasedManagedObject<BOUNDS, DATA>>();
            this.ManagedObjects = new ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>(this.managedObjects);

            this.Bounds = new ReadOnlyCollection<BOUNDS>(this.bounds);

            this.managedObjectsInBounds
                = new List<List<BoundsBasedManagedObject<BOUNDS, DATA>>>();

            var managedObjectsInBoundsReadOnly
                = new List<ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>>();

            for (int i = 0; i < this.bounds.Count; i++)
            {
                this.managedObjectsInBounds.Add
                    (new List<BoundsBasedManagedObject<BOUNDS, DATA>>());

                managedObjectsInBoundsReadOnly.Add
                    (new ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>(this.managedObjectsInBounds[i]));
            }

            this.ManagedObjectsInBounds
                = new ReadOnlyCollection<ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>>
                (managedObjectsInBoundsReadOnly);

            return true;
        }

        public virtual MANAGED_OBJECT AddManagedObject<MANAGED_OBJECT>(GameObject gameObject)
            where MANAGED_OBJECT : BoundsBasedManagedObject<BOUNDS, DATA>
        {
            if (this.IsFilled)
            {
                return null;
            }

            MANAGED_OBJECT managedObject = gameObject.AddComponent(typeof(MANAGED_OBJECT)) as MANAGED_OBJECT;
            managedObject.Manager = this;
            managedObject.UpdateBelongBounds();

            this.managedObjects.Add(managedObject);

            return managedObject;
        }

        public virtual void ReleaseManagedObject(BoundsBasedManagedObject<BOUNDS, DATA> managedObject)
        {
            if (this.IsManage(managedObject))
            {
                if (this.managedObjects.Remove(managedObject))
                {
                    if (managedObject.BelongBoundsIndex != BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex)
                    {
                        this.managedObjectsInBounds[managedObject.BelongBoundsIndex].Remove(managedObject);
                    }

                    // NOTE:
                    // ManagedObject.OnDestroy() will call this function again,
                    // but this.IsManage() step will be failed.

                    GameObject.Destroy(managedObject);
                }
            }
        }

        public virtual void UpdateBelongBoundsIndex
            (BoundsBasedManagedObject<BOUNDS, DATA> managedObject, int previousBoundsIndex)
        {
            if (!this.IsManage(managedObject))
            {
                return;
            }

            if (previousBoundsIndex != BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex)
            {
                int index = this.managedObjectsInBounds[previousBoundsIndex].IndexOf(managedObject);

                if (index < 0)
                {
                    return;
                }

                this.managedObjectsInBounds[previousBoundsIndex].RemoveAt(index);
            }

            if (managedObject.BelongBoundsIndex != BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex)
            {
                this.managedObjectsInBounds[managedObject.BelongBoundsIndex].Add(managedObject);
            }
        }

        public int GetBelongBoundsIndex(Vector3 point, int currentIndex = 0)
        {
            // NOTE:
            // If the point not belong in any bounds, return OutOfBoundsIndex.

            if (0 <= currentIndex)
            {
                if (this.bounds[currentIndex].Contains(point))
                {
                    return currentIndex;
                }

                for (int i = 0; i < currentIndex; i++)
                {
                    if (this.bounds[i].Contains(point))
                    {
                        return i;
                    }
                }

                for (int i = currentIndex + 1; i < this.bounds.Count; i++)
                {
                    if (this.bounds[i].Contains(point))
                    {
                        return i;
                    }
                }

                return BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex;
            }

            for(int i = 0; i < this.bounds.Count; i++)
            {
                if (this.bounds[i].Contains(point))
                {
                    return i;
                }
            }

            return BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex;
        }

        public bool IsManage(BoundsBasedManagedObject<BOUNDS, DATA> managedObject)
        {
            return managedObject.Manager == this;
        }

        #endregion Method
    }
}