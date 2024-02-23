using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text lblBuffNerf;

    public void AnimationLooped()
    {
        GameStateManager.instance.GenerateBuffNerf();

        if (GameStateManager.instance.currentTarget.Equals(GameStateManager.BuffNerfTarget.PLAYER))
        {
            lblBuffNerf.color = new Color(0, 255, 233); // Cyan
            switch (GameStateManager.instance.currentPlayerBuffsNerfs)
            {
                case GameStateManager.PlayerBuffsNerfs.PLAYER_HP:
                    lblBuffNerf.text = "You: Health ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;

                case GameStateManager.PlayerBuffsNerfs.PLAYER_SPEED:
                    lblBuffNerf.text = "You: Speed ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.PlayerBuffsNerfs.PLAYER_DAMAGE:
                    lblBuffNerf.text = "You: Damage ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.PlayerBuffsNerfs.PLAYER_RATE_OF_FIRE:
                    lblBuffNerf.text = "You: Rate of fire ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.PlayerBuffsNerfs.PLAYER_SPREAD:
                    lblBuffNerf.text = "You: Gun spread ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "down!" : "up!";
                    break;
            }
        }
        else
        {
            lblBuffNerf.color = Color.yellow;
            switch (GameStateManager.instance.currentEnemiesBuffsNerfs)
            {

                case GameStateManager.EnemiesBuffsNerfs.ZOMBIE_MAX_COUNT:
                    lblBuffNerf.text = "Zombies: Number of zombies ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.EnemiesBuffsNerfs.ZOMBIE_HP:
                    lblBuffNerf.text = "Zombies: Health ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.EnemiesBuffsNerfs.ZOMBIE_SPEED:
                    lblBuffNerf.text = "Zombies: Speed ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;


                case GameStateManager.EnemiesBuffsNerfs.ZOMBIE_DISTANCE_TO_PLAYER:
                    lblBuffNerf.text = "Zombies: Will spawn ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "closer!" : "farther!";
                    break;

                case GameStateManager.EnemiesBuffsNerfs.ZOMBIE_DAMAGE:
                    lblBuffNerf.text = "Zombies: Damage ";
                    lblBuffNerf.text += GameStateManager.instance.currentType.Equals(GameStateManager.BuffNerf.BUFF) ? "up!" : "down!";
                    break;
            }
        }
    }
}
