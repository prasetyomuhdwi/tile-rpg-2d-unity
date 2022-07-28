using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Movement
{
    private Animator playerAnimator;

    private SpriteRenderer spriteRenderer;
    public SkinChange skinChange;
    
    private bool isAlive = true;

    private PlayerInput playerInput;
    private PlayerControls playerInputControls;

    private int currlevel;

    public Weapon weapon;
    public CharacterMenu characterMenu;
    public PauseMenu pauseMenu;
    public GameObject deathmenu;

    public HighScore highScore;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        highScore = GetComponent<HighScore>();

        playerInputControls = new PlayerControls();
        playerInputControls.Player.Attack.performed += context => Swing();
        playerInputControls.Player.Menu.performed += context => ShowMenu();
        playerInputControls.Player.Pause.performed += context => ShowPauseMenu();
    }

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        currlevel = GameManager.instance.GetCurrentLevel();
    }

    private void OnEnable()
    {
        playerInputControls.Player.Enable();
    }
    private void OnDisable()
    {
        playerInputControls.Player.Disable();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        if (Time.time - lastImmune > immuneTIme)
        {
            lastImmune = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.ShowText("- " + dmg.damageAmount.ToString() + " HP", 35, Color.red, transform.position, Vector3.zero, 0.5f);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }

        playerAnimator.SetTrigger("hit");
    }
    protected override void Death()
    {
        highScore.setHighScore(int.Parse(GameManager.instance.scoreTxt.text));

        isAlive = false;
        
        maxHitPoint -= GameManager.instance.GetCurrentLevel();
        hitPoint = maxHitPoint;
        GameManager.instance.experience = 0;
        GameManager.instance.coin = 0;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;

        deathmenu.SetActive(true);
        GameManager.instance.deathAnimator.SetTrigger("show");
    }

    public void Respawn()
    {
        deathmenu.SetActive(false);
        isAlive = true;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputControls.Player.Movement.ReadValue<Vector2>();
        // Get the input X, Y
        if (isAlive)
        {
            GameManager.instance.OnHitPointChange();

            if (Mathf.Abs(inputVector.x) > 0 || Mathf.Abs(inputVector.y) > 0)
                playerAnimator.SetBool("isRun", true);
            else
                playerAnimator.SetBool("isRun", false);
            
            if (GameManager.instance.GetCurrentLevel() > 1 && currlevel != GameManager.instance.GetCurrentLevel())
            {
                xSpeed += (0.1f * GameManager.instance.GetCurrentLevel());
                ySpeed += (0.1f * GameManager.instance.GetCurrentLevel());
                currlevel = GameManager.instance.GetCurrentLevel();
            }
            
            UpdateMovement(new Vector3(inputVector.x, inputVector.y, 0));
          

            if (GameManager.instance.checkComplete)
            {
                int enemy = GameObject.FindGameObjectsWithTag("Fighter").Length - 1;
                
                if (enemy == 0)
                {
                    GameObject leader_up = GameObject.Find("leader_up");
                    leader_up.GetComponent<SpriteRenderer>().enabled = true;
                    leader_up.GetComponent<EndGame>().enabled = true;
                    leader_up.GetComponent<BoxCollider2D>().enabled = true;
                    
                    highScore.setHighScore(int.Parse(GameManager.instance.scoreTxt.text));
                    GameManager.instance.ShowText("Congratulations you have completed this dugeon!!", 50, Color.white, new Vector3(-2.37f, 20.17f), Vector3.zero, 0.5f);
                }
            }
        }

    }

    void Swing()
    {
        if (Time.time - weapon.lastSwing > weapon.cooldown)
        {
            weapon.lastSwing = Time.time;
            weapon.animator.SetTrigger("isSwing");
            AudioManager.instance.PlayEffect("swing");
        }
    }
    
    void ShowMenu()
    {
        if (!characterMenu.isMenuShowed)
        {  
            characterMenu.ShowMenu();
        }
        else
        {
            characterMenu.HideMenu();
        }
    }

    void ShowPauseMenu()
    {
        if (PauseMenu.GameisPaused)
        {
            pauseMenu.Resume();
        }
        else
        {
            pauseMenu.Pause();
        }
    }

    // Skin
    public void SwapSprite(int skinId)
    {
        switch(skinId){
            case 0:
                skinChange.KnightMSKin();
                break;
            case 1:
                skinChange.KnightFSKin();
                break;
        }
    }

    public void OnLevelUp()
    {
        maxHitPoint += GameManager.instance.GetCurrentLevel();
        hitPoint = maxHitPoint;
        GameManager.instance.ShowText("Level UP!!", 40, Color.white, transform.position, Vector3.up * 30, 4.0f);
    }

    public void SetLevel(int lvl)
    {
        for (int i = 0; i < lvl; i++)
        {
            OnLevelUp();
        }
    }

    public void Heal(float healingAmount)
    {
        if (hitPoint == maxHitPoint)
            return;

        hitPoint += healingAmount;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;
        GameManager.instance.ShowText("+ " + healingAmount.ToString() + " hp", 35, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitPointChange();
    }

}
