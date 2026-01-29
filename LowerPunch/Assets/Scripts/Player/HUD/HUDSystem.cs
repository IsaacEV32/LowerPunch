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
        fillHealthPlayer.fillAmount = character.health/maxHealth;
        if (character.increaseSpecialBar)
        {
            fillSpecialPlayer.fillAmount = character.specialPoints / maxSpecial;
            character.increaseSpecialBar = false;
        }
        //fillChronometerPlayer.fillAmount -= (10 * Time.deltaTime); 
    }
}
