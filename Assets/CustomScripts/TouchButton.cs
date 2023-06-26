using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class TouchButton : XRBaseInteractable
{
    [Header("Button Data")]
    [SerializeField] string m_buttonSign;
    MeshRenderer m_buttonMesh;
    Color originalButtonColor;
    Color pressedButtonColor;

    [SerializeField] Dispenser dispenser;
    TextMeshProUGUI dispenserScreenText;
    private static TouchButton m_pressedButton = null;

    void Start()
    {
        m_buttonMesh = GetComponent<MeshRenderer>();
        originalButtonColor = m_buttonMesh.material.color;
        pressedButtonColor = Color.green;

        dispenserScreenText = dispenser.DispenserScreenText;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        if (m_buttonMesh != null && m_pressedButton == null)
        {
            m_pressedButton = this;
            m_buttonMesh.material.color = pressedButtonColor;
            AlterVisibleSequence();
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (m_buttonMesh != null && m_pressedButton == this)
        {
            m_pressedButton = null;
            m_buttonMesh.material.color = originalButtonColor;
        }
    }

    private void AlterVisibleSequence()
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
