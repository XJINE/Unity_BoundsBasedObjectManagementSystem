using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectManager
    : BoundsBasedObjectManager<SampleBounds, SampleBoundsBasedObjectBehaviour>
    {
        protected override SampleBoundsBasedObjectBehaviour ReturnManagedObjectData(GameObject managedGameObject)
        {
            return managedGameObject.GetComponent<SampleBoundsBasedObjectBehaviour>();
        }
    }
}