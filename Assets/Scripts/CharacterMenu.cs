using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text filds
    public Text levelText, hitPointText, coinText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite; 
    public Image weaponSprite;
    public RectTransform xpBar;
    private Animator animator;
    public Button hudButton;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Show Menu
    public void ShowMenu()
    {
        animator.SetTrigger("show");
        hudButton.interactable = false;
    }

    // hide menu
    public void HideMenu()
    {
        animator.SetTrigger("hide");
        hudButton.interactable = true;
    }

    // Character Selection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            // if we went too far away
            if(currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();

        }
        else
        {
            currentCharacterSelection--;

            // if we went too far away
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // Weapon Upgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    // Update the character Stats
    public void UpdateMenu()
    {
        // weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];

        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
       
        // meta
        hitPointText.text = GameManager.instance.player.hitPoint.ToString();
        coinText.text = GameManager.instance.coin.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        // XP Bar
        int currentLvl = GameManager.instance.GetCurrentLevel();
        if(currentLvl == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience point"; // display total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currentLvl - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currentLvl);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLvl = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLvl / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLvl.ToString() + " / " + diff;
        }
    }
}
