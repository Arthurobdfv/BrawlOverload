using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerText : RotateTowardsCamera
{
    public PlayerManager m_playerManager;

    private TMP_Text m_userNameText;

    public override void Awake()
    {
        base.Awake();
        m_playerManager = GetComponentInParent<PlayerManager>() ?? throw new MissingComponentException(nameof(m_playerManager));
        m_userNameText = GetComponent<TMP_Text>() ?? throw new MissingComponentException(nameof(m_userNameText));
    }

    private void Start()
    {
        SetText();
    }

    private void SetText()
    {
        m_userNameText.text = m_playerManager.userName;
    }
}
