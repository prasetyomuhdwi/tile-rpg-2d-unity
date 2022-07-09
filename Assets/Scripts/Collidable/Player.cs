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

    public Weapon weapon;
    public CharacterMenu characterMenu;
    public PauseMenu pauseMenu;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
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
        else
        {
            // playerAnimator.SetBool("isHit",false);
        }

        playerAnimator.SetTrigger("hit");
        GameManager.instance.OnHitPointChange();
    }
    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathAnimator.SetTrigger("show");
    }

    public void Respawn()
    {
        maxHitPoint -= GameManager.instance.GetCurrentLevel();
        hitPoint = maxHitPoint;
        GameManager.instance.experience = 0;
        GameManager.instance.coin = 0;
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputControls.Player.Movement.ReadValue<Vector2>();
        // Get the input X, Y
        if (isAlive)
        {
            if (Mathf.Abs(inputVector.x) > 0 || Mathf.Abs(inputVector.y) > 0)
                playerAnimator.SetBool("isRun", true);
            else
                playerAnimator.SetBool("isRun", false);

            UpdateMovement(new Vector3(inputVector.x, inputVector.y, 0));
        }

    }

    void Swing()
    {
        if (Time.time - weapon.lastSwing > weapon.cooldown)
        {
            weapon.lastSwing = Time.time;
            weapon.animator.SetTrigger("isSwing");
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

    public void Heal(int healingAmount)
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
