using UnityEngine;

namespace ObjectManagementSystem.BoundsBased
{
    public class SampleBoundsBasedObjectManager
    : BoundsBasedObjectManager<SampleBounds, SampleBoundsBasedObjectBehaviour>
    {
        protected override SampleBoundsBasedObjectBehaviour InitializeManagedObjectData(GameObject managedGameObject)
        {
            return managedGameObject.GetComponent<SampleBoundsBasedObjectBehaviour>();
        }
    }
}