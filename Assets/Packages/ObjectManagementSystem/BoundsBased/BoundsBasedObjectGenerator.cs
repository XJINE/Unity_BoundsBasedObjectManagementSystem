using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class BoundsBasedObjectGenerator<BOUNDS, DATA>
        : MonoBehaviour where BOUNDS : TransformBoundsMonoBehaviour
    {
        #region Field

        public GameObject[] objects;

        #endregion Field

        #region Property

        public BoundsBasedObjectManager<BOUNDS, DATA> Manager
        {
            get;
            protected set;
        }

        #endregion Property

        #region Method

        public virtual void Awake()
        {
            this.Manager = base.GetComponent<BoundsBasedObjectManager<BOUNDS, DATA>>();
        }

        public virtual MANAGED_OBJECT Generate<MANAGED_OBJECT>(int index)
            where MANAGED_OBJECT : BoundsBasedManagedObject<BOUNDS, DATA>
        {
            if (this.Manager.IsFilled)
            {
                return null;
            }

            GameObject generatedObject = GameObject.Instantiate(this.objects[index]);

            return this.Manager.AddManagedObject<MANAGED_OBJECT>(generatedObject);
        }

        #endregion Method
    }
}