using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    private Camera m_mainCamera;
    public virtual void Awake()
    {
        m_mainCamera = Camera.main ?? throw new MissingComponentException($"Couldn't find main camera for object: {gameObject} of type ({this})");
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        var dir = m_mainCamera.transform.position - transform.position;
        transform.LookAt(m_mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(m_mainCamera.transform.forward);
    }

}
