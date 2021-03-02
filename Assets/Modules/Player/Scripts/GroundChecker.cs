using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public LayerMask CheckLayers;
    private bool isOverlapping;

    public bool IsOverlapping
    {
        get => isOverlapping;
        set
        {
            if (isOverlapping != value) isOverlapping = value; 
        }
    }
    public float boxWidth, boxHeight;

    public void FixedUpdate()
    {
        var extents = new Vector3(boxWidth, boxHeight, boxWidth)/2;
        IsOverlapping = Physics.OverlapBox(transform.position, extents, Quaternion.identity, CheckLayers).Length > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isOverlapping ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxWidth, boxHeight, boxWidth));
    }
}
