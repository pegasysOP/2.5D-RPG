using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// The display for a Battler in the battle environment
public class BattleUnit : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] Image image; // TODO: Need front and back sprites for player vs enemy POV
    [SerializeField] bool isPlayer;

    public Battler Battler { get; private set; }

    // Update the display to contain the information on the specified Battler
    public void SetDisplay(Battler battler)
    {
        this.Battler = battler;

        Name = battler.Name;
        HP = battler.HP.ToString();
        Sprite = battler.Sprite;

        UpdateHP();
    }

    public void UpdateHP()
    {
        if (Battler != null)
        {
            HP = Battler.HP.ToString();
        }
    }

    public bool TakeDamage(int damage)
    {
        bool targetEliminated = Battler.TakeDamage(damage);
        UpdateHP();
        return targetEliminated;
    }

    public bool ApplyStatus(StatusID status)
    {
        bool statusApplied = Battler.ApplyStatus(status);

        // Display the status in the HUD

        return statusApplied;
    }

    public string Name
    {
        get { return nameText.text; }
        set { nameText.text = value; }
    }
    public string HP
    {
        get { return hpText.text; }
        set { hpText.text = value; }
    }

    public Sprite Sprite
    {
        get { return image.sprite; }
        set { image.sprite = value; }
    }

    public bool IsPlayer
    { get { return isPlayer; } }
}