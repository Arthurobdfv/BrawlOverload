using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlOverloadPlayerManager : PlayerManager
{
    Quaternion m_lookAt;
    [Range(.001f,1f)]
    public float turnSmoothness;
    private void FixedUpdate()
    {
        SmoothRotate(); 
    }  

    void SmoothRotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, m_lookAt, turnSmoothness);
    }

    public override void SetRotation(Quaternion rot)
    {
        m_lookAt = rot;
    }
}
