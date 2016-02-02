using UnityEngine;
using System.Collections;

public static class BDUtility
{
    public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask)
    {
        Transform objectFound = null;
        var hitColliders = Physics.OverlapSphere(transform.position, viewDistance, objectLayerMask);
        if (hitColliders != null)
        {
            float minAngle = Mathf.Infinity;
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                float angle;
                Transform obj;
                // Call the WithinSight function to determine if this specific object is within sight
                if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform, false, out angle)) != null)
                {
                    // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        objectFound = obj;
                    }
                }
            }
        }
        return objectFound;
    }

    public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject)
    {
        float angle;
        return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, false, out angle);
    }

    // Determines if the targetObject is within sight of the transform. It will set the angle regardless of whether or not the object is within sight
    private static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject, bool usePhysics2D, out float angle)
    {
        // The target object needs to be within the field of view of the current object
        var direction = targetObject.position - (transform.position + positionOffset);
        if (usePhysics2D)
        {
            angle = Vector3.Angle(direction, transform.up);
        }
        else {
            angle = Vector3.Angle(direction, transform.forward);
        }
        if (direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f)
        {
            // The hit agent needs to be within view of the current agent
            if (LineOfSight(transform, positionOffset, targetObject, usePhysics2D) != null)
            {
                return targetObject; // return the target object meaning it is within sight
            }
            else if (targetObject.collider == null)
            {
                // If the linecast doesn't hit anything then that the target object doesn't have a collider and there is nothing in the way
                if (targetObject.gameObject.activeSelf)
                    return targetObject;
            }
        }
        // return null if the target object is not within sight
        return null;
    }

    public static Transform LineOfSight(Transform transform, Vector3 positionOffset, Transform targetObject, bool usePhysics2D)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.TransformPoint(positionOffset), targetObject.position, out hit))
        {
            if (hit.transform.Equals(targetObject))
            {
                return targetObject; // return the target object meaning it is within sight
            }
        }

        return null;
    }


}
