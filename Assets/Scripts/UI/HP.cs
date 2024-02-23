using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private Text hpText;

    private void Start()
    {
        hpText = GetComponent<Text>();
    }

    private void Update()
    {
        hpText.text = "";
        for (int i = 0; i < GameStateManager.instance.playerHp; i++)
        {
            hpText.text += "+";
        }

        bool oneHitToDeath = (GameStateManager.instance.playerHp - GameStateManager.instance.zombieDamage) <= 0;

        if (oneHitToDeath)
        {
            hpText.color = Color.red;
        }
        else
        {
            hpText.color = Color.white;
        }
    }
}
