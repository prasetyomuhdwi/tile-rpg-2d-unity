using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(FloatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            Destroy(transition.gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager FloatingTextManager;
    public Transition transition;
    public HealthBar healthBar;
    public Animator deathAnimator;
    public GameObject hud;
    public GameObject menu;

    // transition
    public float transitionTime = 1f;
    public string[] sceneNames;

    private void Start()
    {
        healthBar.SetMaxHealth(GameManager.instance.player.maxHitPoint);
    }

    // Logic
    public int coin;
    public int experience;

    
    // floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Weapon Upgrade
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;

        if (coin >= weaponPrices[weapon.weaponLevel])
        {
            coin -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }
        return false;   
    }

    // Hitpoint Bar
    public void OnHitPointChange()
    {
        healthBar.SetHealth(player.hitPoint);
    }

    // Experience System
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if(r == xpTable.Count) // Max Lavel
            {
                return r;
            }
        }
        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if(currLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    public void OnLevelUp()
    {
        player.OnLevelUp();
        OnHitPointChange();
    }

    public void ChangeScene()
    {   
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.TriggerTransition();

        yield return new WaitForSeconds(transitionTime);
        
        string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
        SceneManager.LoadScene(sceneName);
    }

    // ON Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    //Death Menu And Respawn
    public void Respawn()
    {
        deathAnimator.SetTrigger("hide");
        SceneManager.LoadScene(sceneNames[0]);
        player.Respawn();
    }

    /* Save State
     *
     * Int preferedSkin
     * Int Coin
     * Int Experience
     * Int WeaponLevel
     */
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += coin.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        
        // Coin
        coin = int.Parse(data[1]);
        
        // Exp & Level
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() == 1)
            player.SetLevel(GetCurrentLevel());
        
        // Weapon
        weapon.weaponLevel = int.Parse(data[3]);
    }

}
