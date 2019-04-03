using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectBehaviour : MonoBehaviour
    {
        #region Field

        protected Vector3 target;

        protected SampleBoundsBasedManagedObject sampleBoundsBasedManagedObject;

        #endregion Field

        #region Method

        protected virtual void Start()
        {
            // NOTE:
            // SampleBoundsBaseManagedObject cannot get in Awake()
            // because it is attached after registration into Manager with AddManagedObject().

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
                SetColor(Color.black);
            }
            else
            {
                SetColor(this.sampleBoundsBasedManagedObject.BelongBounds.gizmoColor);
            }
        }

        protected virtual void UpdateTarget()
        {
            var allBounds = this.sampleBoundsBasedManagedObject.Manager.Bounds;
            var bounds = allBounds[Random.Range(0, allBounds.Count)];

            this.target = new Vector3()
            {
                x = Random.Range(bounds.Min.x, bounds.Max.x),
                y = Random.Range(bounds.Min.y, bounds.Max.y),
                z = Random.Range(bounds.Min.z, bounds.Max.z),
            };
        }

        public void SetColor(Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                                  materialPropertyBlock.SetColor("_Color", color);

            base.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
        }

        #endregion Method
    }
}