using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dispenser : MonoBehaviour
{

    [Header("Dispenser Data")]
    private Image dispenserScreenImage;
    public TextMeshProUGUI DispenserScreenText { get; set; }

    Color originalDispenserColor;
    Color correctDispenserColor;
    Color incorrectDispenserColor;

    [Header("Keycard to be dispensed")]
    public Keycard keycard;


    void Awake()
    {
        Canvas dispenserScreen = GetComponentInChildren<Canvas>();
        dispenserScreenImage = dispenserScreen.GetComponentInChildren<Image>();
        DispenserScreenText = dispenserScreen.GetComponentInChildren<TextMeshProUGUI>();
        DispenserScreenText.text = "";
    }

    void Start()
    {
        originalDispenserColor = dispenserScreenImage.color;
        correctDispenserColor = Color.green;
        incorrectDispenserColor = Color.red;
    }

    public IEnumerator CheckSequenceCoroutine()
    {
        string currentText = DispenserScreenText.text;

        if (currentText.Length < 4)
        {
            yield break; // equivalent to "return" in a coroutine
        }

        bool isCorrect = (currentText == Numberpad.CorrectPin);

        if (isCorrect)
        {
            dispenserScreenImage.color = correctDispenserColor;
            if (keycard != null && !keycard.gameObject.activeSelf) keycard.ActivateCard();
        }
        else
        {
            dispenserScreenImage.color = incorrectDispenserColor;
            DispenserScreenText.text = "";
        }

        yield return new WaitForSeconds(1);
        dispenserScreenImage.color = originalDispenserColor;
    }
}
