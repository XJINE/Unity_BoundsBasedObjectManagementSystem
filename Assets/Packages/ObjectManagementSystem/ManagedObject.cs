using UnityEngine;

namespace ObjectManagementSystem
{
    public class ManagedObject<T> : MonoBehaviour
    {
        #region Property

        protected ObjectManager<T> objectManager;

        public ObjectManager<T> ObjectManager
        {
            get { return this.objectManager; }
            set { if (this.objectManager == null) this.objectManager = value; }
        }

        public T Data { get; protected set; }

        #endregion Property

        #region Method

        protected virtual void Awake()
        {
            this.Data = GetComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            // CAUTION:
            // ManagedObject might be removed from the outside of the ObjectManager.

            if (this.objectManager != null)
            {
                this.objectManager.ReleaseManagedObject(this);
                this.objectManager = null;
            }
        }

        #endregion Method
    }
}