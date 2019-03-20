using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class BoundsBasedManagedObject<T, U> : ManagedObject<U> where T : TransformBoundsMonoBehaviour, ITransform
    {
        #region Property

        // NOTE:
        // Keep reference to reduce cost of cast from ManagedObject<T>.ObjectManager.

        public BoundsBasedObjectManager<T, U> BoundsBasedObjectManager { get; protected set; }

        public int BelongBoundsIndex { get; protected set; }

        public T BelongBounds { get { return this.BoundsBasedObjectManager.Bounds[this.BelongBoundsIndex]; } }

        public bool OutOfBounds { get; private set; }

        public new Transform transform { get; protected set; }

        #endregion Property

        #region Method

        protected override void Awake()
        {
            this.BoundsBasedObjectManager = (BoundsBasedObjectManager<T, U>)objectManager;
            this.transform = base.transform;
        }

        protected virtual void LateUpdate()
        {
            UpdateBelongBounds();
        }

        public virtual void UpdateBelongBounds()
        {
            // NOTE:
            // This method called from every LateUpdate().
            // However, you can call this method to manual update if you need.

            this.BoundsBasedObjectManager.UpdateBelongBoundsIndex(this);
        }

        public virtual void UpdateBelongBounds(BoundsBasedObjectManager<T, U> manager, int belongBoundsIndex)
        {
            // CAUTION:
            // This function called from BoundsBaseObjectManager regurally.
            // Unfortunately, this is not safe.
            
            if (this.BoundsBasedObjectManager != manager)
            {
                return;
            }

            // NOTE:
            // If managed object belong in out of bounds, keep previous data.

            if (belongBoundsIndex == -1)
            {
                this.OutOfBounds = true;
            }
            else
            {
                this.OutOfBounds = false;
                this.BelongBoundsIndex = belongBoundsIndex;
            }
        }

        #endregion Method
    }
}