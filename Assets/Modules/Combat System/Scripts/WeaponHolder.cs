using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private GameObject[] m_carriedGuns;
    private GameObject m_activeGun;

    private GameObject m_carryingObject;

    private void Update()
    {
        ReadInputs();
    }

    void ReadInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Player Has Shot");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Switch Weapon");
        }
    }

    void SwitchGun()
    {

    }

    void PickUp()
    {

    }
}