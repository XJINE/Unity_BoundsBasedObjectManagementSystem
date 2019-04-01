namespace ObjectManagementSystem.BoundsBased
{
    public class BoundsBasedManagedObject<BOUNDS, DATA>
        : TransformMonoBehaviour where BOUNDS : TransformBoundsMonoBehaviour
    {
        #region Property

        protected BoundsBasedObjectManager<BOUNDS, DATA> manager;

        public BoundsBasedObjectManager<BOUNDS, DATA> Manager
        {
            get { return this.manager; }
            set { if (this.manager == null) this.manager = value; }
        }

        public int BelongBoundsIndex { get; protected set; } = BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex;

        public BOUNDS BelongBounds { get { return this.manager.Bounds[this.BelongBoundsIndex]; } }

        public bool OutOfBounds { get; private set; }

        public DATA Data { get; protected set; }

        #endregion Property

        #region Method

        protected override void Awake()
        {
            base.Awake();
            this.Data = GetComponent<DATA>();
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

            this.BelongBoundsIndex = this.manager.GetBelongBoundsIndex
                                    (this.transform.position, this.BelongBoundsIndex);

            this.OutOfBounds = this.BelongBoundsIndex == BoundsBasedObjectManager<BOUNDS, DATA>.OutOfBoundsIndex;

            if (previousBoundsIndex != this.BelongBoundsIndex)
            {
                this.manager.UpdateBelongBoundsIndex(this, previousBoundsIndex);
            }
        }

        #endregion Method
    }
}