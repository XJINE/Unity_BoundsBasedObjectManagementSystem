using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ObjectManagementSystem.BoundsBased
{
    public abstract class BoundsBasedObjectManager<S, T> : ObjectManager<T> where S : TransformBoundsMonoBehaviour
    {
        #region Property

        [SerializeField]
        protected List<S> bounds;

        public ReadOnlyCollection<S> Bounds { get; private set; }

        protected List<List<BoundsBasedManagedObject<S, T>>> managedObjectsInBounds;

        public ReadOnlyCollection<ReadOnlyCollection<BoundsBasedManagedObject<S, T>>> ManagedObjectsInBounds 
        {
            get;
            private set;
        }

        #endregion Property

        #region Method

        public override bool Initialize()
        {
            if (!base.Initialize())
            {
                return false;
            }

            this.Bounds = new ReadOnlyCollection<S>(this.bounds);

            this.managedObjectsInBounds
                = new List<List<BoundsBasedManagedObject<S, T>>>();

            var managedObjectsInBoundsReadOnly
                = new List<ReadOnlyCollection<BoundsBasedManagedObject<S, T>>>();

            for (int i = 0; i < this.bounds.Count; i++)
            {
                this.managedObjectsInBounds.Add
                    (new List<BoundsBasedManagedObject<S, T>>());

                managedObjectsInBoundsReadOnly.Add
                    (new ReadOnlyCollection<BoundsBasedManagedObject<S, T>>(this.managedObjectsInBounds[i]));
            }

            this.ManagedObjectsInBounds
                = new ReadOnlyCollection<ReadOnlyCollection<BoundsBasedManagedObject<S, T>>>
                (managedObjectsInBoundsReadOnly);

            return true;
        }

        public override U AddManagedObject<U>(GameObject gameObject)
        {
            // NOTE:
            // Can not be able to override generic's "where".

            U managedObject = base.AddManagedObject<U>(gameObject);

            if (managedObject == null)
            {
                return null;
            }

            var boundsBasedManagedObject = managedObject as BoundsBasedManagedObject<S, T>;
            int belongBoundsIndex = GetBelongBoundsIndex(boundsBasedManagedObject.transform.position);

            this.managedObjectsInBounds[belongBoundsIndex == -1 ? 0 : belongBoundsIndex].Add(boundsBasedManagedObject);
            boundsBasedManagedObject.UpdateBelongBounds(this, belongBoundsIndex);

            return managedObject;
        }

        public override void ReleaseManagedObject(ManagedObject<T> managedObject)
        {
            if (!base.IsManage(managedObject))
            {
                return;
            }

            BoundsBasedManagedObject<S, T> boundsBaseManagedObject = managedObject as BoundsBasedManagedObject<S, T>;

            if (boundsBaseManagedObject == null)
            {
                return;
            }

            if (fromOnDestroy)
            {
                base.managedObjects.Remove(managedObject);

                // NOTE:
                // Comment out following code when refactored this.
                // Maybe thorwing Exception is more better for debug.
                // 
                // if (boundsBaseManagedObject.BelongBoundsIndex != -1
                //  && boundsBaseManagedObject.BelongBoundsIndex < this.managedObjectsInBounds.Count)
                // {
                // }

                this.managedObjectsInBounds[boundsBaseManagedObject.BelongBoundsIndex].Remove(boundsBaseManagedObject);
            }
            else
            {
                GameObject.Destroy(managedObject);
            }
        }

        public virtual void UpdateBelongBoundsIndex(BoundsBasedManagedObject<S, T> managedObject)
        {
            if (!CheckObjectIsManaged(managedObject))
            {
                return;
            }

            int belongBoundsIndex = GetBelongBoundsIndex(managedObject.transform.position,
                                                         managedObject.BelongBoundsIndex);

            if (managedObject.BelongBoundsIndex == belongBoundsIndex)
            {
                return;
            }

            if (managedObject.BelongBoundsIndex != -1)
            {
                this.managedObjectsInBounds[managedObject.BelongBoundsIndex].Remove(managedObject);
            }

            if (belongBoundsIndex != -1)
            {
                this.managedObjectsInBounds[belongBoundsIndex].Add(managedObject);
            }

            managedObject.UpdateBelongBounds(this, belongBoundsIndex);
        }

        public int GetBelongBoundsIndex(Vector3 point, int currentIndex = 0)
        {
            // NOTE:
            // If the point not belong in any bounds, return -1.

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

                return -1;
            }

            for(int i = 0; i < this.bounds.Count; i++)
            {
                if (this.bounds[i].Contains(point))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion Method
    }
}