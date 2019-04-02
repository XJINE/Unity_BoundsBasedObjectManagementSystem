using UnityEngine;

namespace ObjectManagementSystem
{
    // NOTE:
    // This is one of a sample.
    // ObjectManager<> is able to use without ObjectGenerator<>.

    [RequireComponent(typeof(ObjectManager<>))]
    public class ObjectGenerator<DATA> : MonoBehaviour
    {
        #region Field

        public GameObject[] objects;

        #endregion Field

        #region Property

        public ObjectManager<DATA> ObjectManager 
        {
            get;
            protected set;
        }

        #endregion Property

        #region Method

        public virtual void Awake()
        {
            this.ObjectManager = base.GetComponent<ObjectManager<DATA>>();
        }

        public virtual MANAGED_OBJECT Generate<MANAGED_OBJECT>(int index)
                 where MANAGED_OBJECT : ManagedObject<DATA>
        {
            if (this.ObjectManager.IsFilled)
            {
                return null;
            }

            GameObject generatedObject = GameObject.Instantiate(this.objects[index]);

            return this.ObjectManager.AddManagedObject<MANAGED_OBJECT>(generatedObject);
        }

        #endregion Method
    }
}