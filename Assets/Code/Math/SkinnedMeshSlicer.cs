using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshSlicer : MonoBehaviour
{
    public void Slice(GameObject hitComponent, Vector3 hitPosition)
    {
        var parentGameObject = hitComponent.transform.root.gameObject;

        var cutParts = new GameObject($"{gameObject.name}: Cut Parts");
        cutParts.transform.SetPositionAndRotation(
            parentGameObject.transform.position,
            parentGameObject.transform.rotation
        );

        var hitComponentClone = Instantiate(
            hitComponent,
            hitComponent.transform.position,
            hitComponent.transform.rotation,
            hitComponent.transform.parent
        );

        for (
            var childIndex = hitComponentClone.transform.childCount - 1;
            childIndex >= 0;
            childIndex--
        )
        {
            Destroy(hitComponentClone.transform.GetChild(childIndex).gameObject);
        }
        hitComponent.transform.SetParent(cutParts.transform);

        // Not helping
        // hitComponent.GetComponent<Rigidbody>().useGravity = false;
    }
}
