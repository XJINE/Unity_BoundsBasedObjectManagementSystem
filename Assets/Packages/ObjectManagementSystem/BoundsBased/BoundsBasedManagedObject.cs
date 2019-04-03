namespace ObjectManagementSystem.BoundsBased
{
    public class BoundsBasedManagedObject<BOUNDS, DATA>
        : TransformMonoBehaviour where BOUNDS : BoundsMonoBehaviour
    {
        #region Property

        protected BoundsBasedObjectManager<BOUNDS, DATA> manager;

        public BoundsBasedObjectManager<BOUNDS, DATA> Manager
        {
            get { return this.manager; }
            set { if (this.manager == null) this.manager = value; }
        }

        public BOUNDS BelongBounds { get; protected set; }

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

        protected virtual void OnDestroy()
        {
            // CAUTION:
            // ManagedObject might be removed from the outside of the ObjectManager.

            if (this.manager != null)
            {
                this.manager.ReleaseManagedObject(this);
                this.manager = null;
            }
        }

        public virtual void UpdateBelongBounds()
        {
            // NOTE:
            // This method called from every LateUpdate().
            // However, you can call this method to manual update if you need.

            BOUNDS previousBounds = this.BelongBounds;

            this.BelongBounds = this.manager.GetBelongBounds(this.transform.position, previousBounds);
            this.OutOfBounds  = this.BelongBounds == null;

            if (previousBounds != this.BelongBounds)
            {
                this.manager.UpdateBelongBounds(this, previousBounds);
            }
        }

        #endregion Method
    }
}