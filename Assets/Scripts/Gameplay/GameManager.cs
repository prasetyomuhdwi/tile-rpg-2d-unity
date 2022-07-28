using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(ui);
            return;
        }
        
        gm = gameObject;

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
    public PlayerInput playerInput;
    public FloatingTextManager FloatingTextManager;
    public Transition transition;
    public HealthBar healthBar;
    public Animator deathAnimator;
    public Text deathText;
    public Text coinText;
    public GameObject ui;
    
    private GameObject cam;
    private GameObject gm;

    // Score
    public Text scoreTxt;
    private int scoreVal;
    public bool checkComplete = false;

    // transition
    public float transitionTime = 1f;

    private void Start()
    {
        healthBar.SetMaxHealth(GameManager.instance.player.maxHitPoint);
        scoreTxt.text = scoreVal.ToString();
        cam = GameObject.Find("Main Camera");
        AudioManager.instance.PlayMusic("Explore");
    }

    // Logic
    public int coin;
    public int experience;

    private void Update()
    {

        if(int.Parse(coinText.text) != coin)
        {
            coinText.text = coin.ToString();
        }
    }

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

        if(player.maxHitPoint != healthBar.slider.maxValue)
        {
            healthBar.SetMaxHealth(player.maxHitPoint);
        }
    }

    // Score
    public void GrantScore(string typeOfEnemy)
    {
        switch (typeOfEnemy)
        {
            case "medium":
                scoreVal += Random.Range(4,10);
                break;
            case "boss":
                scoreVal += Random.Range(10,20);
                break;
            case "special":
                scoreVal += Random.Range(20,30);
                break;
            default:
                scoreVal += Random.Range(2,4);
                break;
        }

        scoreTxt.text = scoreVal.ToString();
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

    public void ChangeScene(bool isBoss, bool isEnd)
    {   
        StartCoroutine(LoadLevel(isBoss,isEnd));
    }

    IEnumerator LoadLevel(bool isBoss,bool isEnd)
    {
        transition.TriggerTransition();

        yield return new WaitForSeconds(transitionTime);

        if (!isBoss)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            checkComplete = true;
            
            if (isEnd)
            {
                Time.timeScale = 1f;

                Destroy(ui);
                Destroy(gameObject);
                Destroy(cam);
                Destroy(player);

                AudioManager.instance.PlayMusic("Theme");
                SceneManager.LoadScene(0);
            }
            else
            {
                AudioManager.instance.PlayMusic("Boss");
                SceneManager.LoadScene(3);
            }
        }
    }

    // ON Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        if(player != null)
            player.transform.position = GameObject.FindGameObjectWithTag("Spawnpoint").transform.position;
    }

    //Death Menu And Respawn
    public void Respawn()
    {
        deathAnimator.SetTrigger("hide");
        
        Destroy(cam);
        Destroy(ui);
        Destroy(player.gameObject);
        Destroy(gm);

        SceneManager.LoadScene(1);
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

        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;
        
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            checkComplete = true;
        }

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
