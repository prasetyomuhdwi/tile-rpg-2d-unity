using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerTransition()
    {
       animator.SetTrigger("Start");
    }
}
