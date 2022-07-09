using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : Collectable
{
    public Sprite emptyChest;
    private int[] coinArr = { 1, 2, 3, 4, 5, 7, 9 };
    public int coinAmount;
    
    private void Awake()
    {
        if(SceneManager.GetActiveScene().name != "level0")
            coinAmount = coinArr[Random.Range(0, coinArr.Length)];
    }

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.coin += coinAmount;
            GameManager.instance.ShowText("+ " + coinAmount + " Coin!",35,Color.yellow,transform.position,Vector3.up * 50,1.5f);
        }
    }
}
