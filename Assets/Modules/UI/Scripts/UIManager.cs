using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public TMP_InputField userNameField;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }

    public void ConnectedToServer()
    {
        startMenu.SetActive(false);
        userNameField.interactable = false;
        Client.instance.ConnectedToServer();
    }
}
