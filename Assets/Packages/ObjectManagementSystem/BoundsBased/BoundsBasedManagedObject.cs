using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class BoundsBasedManagedObject<T, U> : ManagedObject<U> where T : TransformBoundsMonoBehaviour, ITransform
    {
        #region Property

        public BoundsBasedObjectManager<T, U> BoundsBasedObjectManager { get; protected set; }

        public int BelongBoundsIndex { get; protected set; } = -1;

        public T BelongBounds { get { return this.BoundsBasedObjectManager.Bounds[this.BelongBoundsIndex]; } }

        public bool OutOfBounds { get; private set; }

        public new Transform transform { get; protected set; }

        #endregion Property

        #region Method

        protected override void Awake()
        {
            this.BoundsBasedObjectManager = (BoundsBasedObjectManager<T, U>)base.objectManager;
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

            int previousBoundsIndex = this.BelongBoundsIndex;

            this.BelongBoundsIndex = this.BoundsBasedObjectManager.GetBelongBoundsIndex
                                     (this.transform.position, this.BelongBoundsIndex);

            this.OutOfBounds = this.BelongBoundsIndex == -1;

            if (previousBoundsIndex != this.BelongBoundsIndex)
            {
                this.BoundsBasedObjectManager.UpdateBelongBoundsIndex(this, previousBoundsIndex);
            }
        }

        #endregion Method
    }
}