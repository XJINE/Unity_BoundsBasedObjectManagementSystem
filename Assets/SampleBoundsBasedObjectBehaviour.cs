using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectBehaviour : MonoBehaviour
    {
        #region Field

        public float lifeCount = 1000f;

        protected Vector3 target;

        protected SampleBoundsBasedManagedObject sampleBoundsBasedManagedObject;

        #endregion Field

        #region Method

        protected virtual void Start()
        {
            // NOTE:
            // SampleBoundsBaseManagedObject cannot get in Awake()
            // because the component is added after instantiation of this.
            // Awake() is just called after instantiation.

            this.sampleBoundsBasedManagedObject = base.GetComponent<SampleBoundsBasedManagedObject>();
        }

        protected virtual void Update()
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.target, 0.1f);

            if (base.transform.position == this.target)
            {
                UpdateTarget();
            }

            // NOTE:
            // You can define any parameter to bounds and reference it from BoundsBasedManagedObject.
            // Ex.BelongBounds.gizmoColor.

            if (this.sampleBoundsBasedManagedObject.OutOfBounds)
            {
                UpdateColor(Color.black);
            }
            else
            {
                UpdateColor(this.sampleBoundsBasedManagedObject.BelongBounds.gizmoColor);
            }

            if (this.lifeCount < 0)
            {
                GameObject.Destroy(base.gameObject);
            }
        }

        protected virtual void UpdateTarget()
        {
            var bounds = this.sampleBoundsBasedManagedObject.BoundsBasedObjectManager.Bounds;
            this.target = bounds[Random.Range(0, bounds.Count)].GetRandomPoint();
        }

        protected virtual void UpdateColor(Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            materialPropertyBlock.SetColor("_Color", color);
            base.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
        }

        protected virtual void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(base.transform.position + Vector3.up, this.lifeCount.ToString());
            #endif
        }

        #endregion Method
    }
}