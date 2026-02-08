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
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetReferencePlayer(MainCharacter c)
    {
        character = c;
    }
    void Start()
    {
        maxHealth = character.health;
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

 
    void Update()
    {
        if (character.changeHealthPoints && fillHealthPlayer.fillAmount > 0)
        {
            Debug.Log(character.health);
            fillHealthPlayer.fillAmount = character.health / maxHealth;
            character.changeHealthPoints = false;
        }
        if (fillHealthPlayer.fillAmount == 0 && !isTheRoundFinished)
        {
            isTheRoundFinished = true;
            Time.timeScale = 0.0f;
            loseScreen.SetActive(true);
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
        if (fillChronometerPlayer.fillAmount <= 0 && !isTheRoundFinished)
        {
            isTheRoundFinished = true;
            Time.timeScale = 0.0f;
            winScreen.SetActive(true);
            
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
