using UnityEngine;

namespace ObjectManagementSystem
{
    public class ManagedObject<DATA> : MonoBehaviour
    {
        #region Property

        protected ObjectManager<DATA> manager;

        public ObjectManager<DATA> Manager
        {
            get { return this.manager; }
            set { if (this.manager == null) this.manager = value; }
        }

        public DATA Data { get; protected set; }

        #endregion Property

        #region Method

        protected virtual void Awake()
        {
            this.Data = GetComponent<DATA>();
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

        #endregion Method
    }
}