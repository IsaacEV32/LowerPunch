using UnityEngine;
using UnityEngine.UI;
public class HUDSystem : MonoBehaviour
{
    public Image fillHealthPlayer;
    public Image fillSpecialPlayer;
    public Image fillChronometerPlayer;
    public MainCharacter character;
    public int maxHealth = 100;
    public int maxSpecial = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetReferencePlayer(MainCharacter c)
    {
        character = c;
        Debug.Log(character);
    }
    void Start()
    {
        maxHealth = character.health;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(maxHealth);
        fillHealthPlayer.fillAmount = character.health/maxHealth;
        fillSpecialPlayer.fillAmount = character.specialPoints / maxSpecial;
        //fillChronometerPlayer.fillAmount -= (10 * Time.deltaTime); 
    }
}
