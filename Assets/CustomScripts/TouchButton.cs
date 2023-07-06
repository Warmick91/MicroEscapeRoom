using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class TouchButton : XRBaseInteractable
{
    [Header("Button Data")]
    [SerializeField] string m_buttonSign;
    MeshRenderer m_visualButton;
    Color originalButtonColor;
    Color pressedButtonColor;
    Vector3 initialButtonPosition;
    Vector3 pressedButtonPosition;

    [SerializeField] Dispenser dispenser;
    TextMeshProUGUI dispenserScreenText;
    private static TouchButton m_pressedButton = null;

    bool isPressed = false;

    void Start()
    {  
        m_visualButton = transform.parent.GetComponentInChildren<MeshRenderer>();
        originalButtonColor = m_visualButton.material.color;
        pressedButtonColor = Color.green;
        initialButtonPosition = m_visualButton.transform.localPosition;
        pressedButtonPosition = initialButtonPosition - new Vector3(0, 0.03f, 0);
        dispenserScreenText = dispenser.DispenserScreenText;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (m_visualButton != null && m_pressedButton == null && !isPressed)
        {
            isPressed = true;
            m_pressedButton = this;
            m_visualButton.material.color = pressedButtonColor;
            m_visualButton.transform.localPosition = pressedButtonPosition;
            AlterVisibleSequence();
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (m_visualButton != null && m_pressedButton == this && isPressed)
        {   
            isPressed = false;
            m_pressedButton = null;
            m_visualButton.material.color = originalButtonColor;
            m_visualButton.transform.localPosition = initialButtonPosition;
        }
    }

    public void AlterVisibleSequence()
    {
        switch (m_buttonSign)
        {
            // Remove a single digit
            case "DEL":
                if (!string.IsNullOrEmpty(dispenserScreenText.text))
                {
                    string currentText = dispenserScreenText.text;
                    dispenserScreenText.text = currentText.Substring(0, currentText.Length - 1);
                }
                break;

            // Remove all digits
            case "RES":
                dispenserScreenText.text = "";
                break;

            default:
                dispenserScreenText.text += m_buttonSign;
                StartCoroutine(dispenser.CheckSequenceCoroutine());
                break;
        }
    }


}
