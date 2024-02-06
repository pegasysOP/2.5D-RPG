using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// The text/selection box displayed at the bottom of the battle environment
public class BattleTextBox : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
    [SerializeField] List<TMP_Text> buttonTexts;

    public UnityEvent<int> OnButtonPressed = new UnityEvent<int>();

    [Header("Settings")]
    [SerializeField, Tooltip("In characters per second")] int textSpeed;

    public IEnumerator WriteToBottomText(string message)
    {
        textBox.text = "";

        foreach (char c in message)
        {
            textBox.text += c;
            yield return new WaitForSeconds(Mathf.Pow(textSpeed, -1));
        }
    }

    public void SetButtonTexts(List<string> texts)
    {
        for (int i = 0; i < buttonTexts.Count; i++)
        {
            if (i < texts.Count)
                buttonTexts[i].text = texts[i];
            else
                buttonTexts[i].text = "";
        }
    }
    public void SetButtonTexts(List<Attack> attacks)
    {
        for (int i = 0; i < buttonTexts.Count; i++)
        {
            if (i < attacks.Count)
                buttonTexts[i].text = attacks[i].Name;
            else
                buttonTexts[i].text = "";
        }
    }

    public void ClearButtonTexts()
    {
        SetButtonTexts(new List<string> { "", "", "", "" });
    }    

    public void ButtonPressedHandler(int buttonNo)
    {
        OnButtonPressed.Invoke(buttonNo);
    }
}
