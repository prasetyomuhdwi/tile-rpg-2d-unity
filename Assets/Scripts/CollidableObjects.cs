using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObjects : MonoBehaviour
{
    private Collider2D[] hits = new Collider2D[10];
    private BoxCollider2D boxCollider;
    public ContactFilter2D filter;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        boxCollider.OverlapCollider(filter,hits);
        for(int i = 0; i< hits.Length;i++)
        {
            if (hits[i] == null)
                continue;

            OnCollided(hits[i]);

            hits[i] = null;
        }
    }

    protected virtual void OnCollided(Collider2D collided)
    {
        Debug.Log(collided.name);
    }
}
