using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChange : MonoBehaviour
{
    public Animator playerAnimator;
    public AnimatorOverrideController knight_m;
    public AnimatorOverrideController knight_f;

    public void KnightFSKin()
    {
        playerAnimator.runtimeAnimatorController = knight_f as RuntimeAnimatorController;
    }

    public void KnightMSKin()
    {
        playerAnimator.runtimeAnimatorController = knight_m as RuntimeAnimatorController;
    }

}
