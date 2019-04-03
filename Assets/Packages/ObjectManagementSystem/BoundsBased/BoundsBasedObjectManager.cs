using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem.BoundsBased
{
    public abstract class BoundsBasedObjectManager<BOUNDS, DATA>
        : MonoBehaviour, IInitializable where BOUNDS : TransformBoundsMonoBehaviour
    {
        #region Field

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

        public bool IsInitialized
        {
            get; protected set;
        }

        [SerializeField]
        protected List<BOUNDS> bounds;

        public ReadOnlyCollection<BOUNDS> Bounds { get; private set; }

        protected Dictionary<BOUNDS, List<BoundsBasedManagedObject<BOUNDS, DATA>>> managedObjectsInBounds;

        public ReadOnlyDictionary<BOUNDS, ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>> ManagedObjectsInBounds 
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

            this.managedObjectsInBounds        = new Dictionary<BOUNDS, List<BoundsBasedManagedObject<BOUNDS, DATA>>>();
            var managedObjectsInBoundsReadOnly = new Dictionary<BOUNDS, ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>>();

            foreach (var bounds in this.bounds)
            {
                this.managedObjectsInBounds.Add
                (bounds, new List<BoundsBasedManagedObject<BOUNDS, DATA>>());
                
                managedObjectsInBoundsReadOnly.Add
                (bounds, new ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>(this.managedObjectsInBounds[bounds]));
            }

            this.ManagedObjectsInBounds
            = new ReadOnlyDictionary<BOUNDS, ReadOnlyCollection<BoundsBasedManagedObject<BOUNDS, DATA>>>(managedObjectsInBoundsReadOnly);

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
                    if (managedObject.BelongBounds != null)
                    {
                        this.managedObjectsInBounds[managedObject.BelongBounds].Remove(managedObject);
                    }

                    // NOTE:
                    // ManagedObject.OnDestroy() will call this function again,
                    // but this.IsManage() step will be failed.

                    GameObject.Destroy(managedObject);
                }
            }
        }

        public virtual void UpdateBelongBounds
             (BoundsBasedManagedObject<BOUNDS, DATA> managedObject, BOUNDS previousBounds)
        {
            if (!this.IsManage(managedObject))
            {
                return;
            }

            if (previousBounds != null)
            {
                var managedObjectsInBounds = this.managedObjectsInBounds[previousBounds];

                int index = managedObjectsInBounds.IndexOf(managedObject);

                if (index < 0)
                {
                    return;
                }

                managedObjectsInBounds.RemoveAt(index);
            }

            if (managedObject.BelongBounds != null)
            {
                this.managedObjectsInBounds[managedObject.BelongBounds].Add(managedObject);
            }
        }

        public BOUNDS GetBelongBounds(Vector3 point, BOUNDS currentBounds = null)
        {
            if (currentBounds != null)
            {
                if (currentBounds.Contains(point))
                {
                    return currentBounds;
                }

                for (int i = 0; i < this.bounds.Count; i++)
                {
                    if (this.bounds[i].Contains(point))
                    {
                        return this.bounds[i];
                    }
                }

                return null;
            }

            // if (currentBounds == null)

            for(int i = 0; i < this.bounds.Count; i++)
            {
                if (this.bounds[i].Contains(point))
                {
                    return this.bounds[i];
                }
            }

            return null;
        }

        public bool IsManage(BoundsBasedManagedObject<BOUNDS, DATA> managedObject)
        {
            return managedObject.Manager == this;
        }

        #endregion Method
    }
}