using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HUDSystem : MonoBehaviour
{
    public Image fillHealthPlayer;
    public Image fillSpecialPlayer;
    public Image fillChronometerPlayer;
    public MainCharacter character;
    public float maxHealth = 100;
    public float maxSpecial = 50;
    bool chronometerON = true;
    bool isTheRoundFinished = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetReferencePlayer(MainCharacter c)
    {
        character = c;
    }
    void Start()
    {
        maxHealth = character.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (character.changeHealthPoints)
        {
            Debug.Log(character.health);
            fillHealthPlayer.fillAmount = character.health / maxHealth;
            character.changeHealthPoints = false;
        }
        if (character.increaseSpecialBar && fillSpecialPlayer.fillAmount <= maxSpecial)
        {
            fillSpecialPlayer.fillAmount = character.specialPoints / maxSpecial;
            character.increaseSpecialBar = false;
        }
        if (chronometerON && fillChronometerPlayer.fillAmount > 0)
        {
            StartCoroutine(ChronometerRound());
        }
        else if (fillChronometerPlayer.fillAmount <= 0 && !isTheRoundFinished)
        {
            isTheRoundFinished = true;
            Time.timeScale = 0.0f;
        }
    }
    IEnumerator ChronometerRound()
    {
        fillChronometerPlayer.fillAmount -= Time.deltaTime;
        chronometerON = false;
        yield return new WaitForSeconds(1);
        chronometerON = true;
    }
}
