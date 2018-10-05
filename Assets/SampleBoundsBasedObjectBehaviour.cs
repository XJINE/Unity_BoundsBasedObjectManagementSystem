using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectBehaviour : MonoBehaviour
    {
        #region Field

        public float scale = 1f;

        protected Vector3 target;

        protected SampleBoundsBasedManagedObject sampleBoundsBasedManagedObject;

        #endregion Field

        #region Method

        void Start()
        {
            // NOTE:
            // SampleBoundsBaseManagedObject() cannot get in Awake()
            // because the component is added after instantiation of this.

            this.sampleBoundsBasedManagedObject = base.GetComponent<SampleBoundsBasedManagedObject>();
        }

        void Update()
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.target, 0.1f);

            if (base.transform.position == this.target)
            {
                UpdateTarget();
            }

            if (this.sampleBoundsBasedManagedObject.BelongBoundsIndex == -1)
            {
                UpdateColor(Color.black);
            }
            else
            {
                UpdateColor(this.sampleBoundsBasedManagedObject.BelongBounds.gizmoColor);
            }

            base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
        }

        void UpdateTarget()
        {
            var bounds = this.sampleBoundsBasedManagedObject.BoundsBasedObjectManager.Bounds;
            this.target = bounds[Random.Range(0, bounds.Count)].GetRandomPoint();
        }

        void UpdateColor(Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color", color);
            base.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
        }

        #endregion Method
    }
}