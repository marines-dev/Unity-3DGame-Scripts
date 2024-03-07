using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int pCapacity) : base(pCapacity) { }

    public AnimationClip this[string pAnimationClipName]
    {
        get { return this.Find(x => x.Key.name.Equals(pAnimationClipName)).Value; }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(pAnimationClipName));
            if (index != -1)
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
        }
    }
}

public class Animator_Util : MonoBehaviour
{
    Animator animator_ = null;
    Animator animator
    {
        get
        {
            if (animator_ == null)
            {
                animator_ = gameObject.GetOrAddComponent<Animator>();
            }
            return animator_;
        }
    }
    AnimatorOverrideController  animatorOverrideController  = null;
    AnimationClipOverrides      clipOverrides               = null;



    void Start()
    {
        animator_ = gameObject.GetOrAddComponent<Animator>();
        animator.applyRootMotion = true;
    }
    
    public void SetAnimatorController(string pAnimatorController, string pAnimatorAvatar)
    {
        string path = string.Empty;

        // RuntimeAnimatorController
        if (animator.runtimeAnimatorController == null)
        {
            path = $"Animators/{pAnimatorController}";
            RuntimeAnimatorController runtimeAnimatorController = Managers.Resource.LoadResource<RuntimeAnimatorController>(path);
            if (runtimeAnimatorController == null)
            {
                Debug.LogWarning("Failed : ");
                return;
            }
            animator.runtimeAnimatorController = runtimeAnimatorController;
        }

        // Avatar
        if (animator.avatar == null)
        {
            path = $"Animators/{pAnimatorAvatar}";
            Avatar avatar = Managers.Resource.LoadResource<Avatar>(path);
            if (avatar == null)
            {
                Debug.LogWarning("Failed : ");
                return;
            }
            animator.avatar = avatar;
        }

        // AnimationClip
        animatorOverrideController          = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController  = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        //foreach (KeyValuePair<AnimationClip, AnimationClip> clipOverride in clipOverrides)
        //{
        //    Debug.Log("clipOverride.Key : " + clipOverride.Key.name);
        //}
        //animatorOverrideController.ApplyOverrides(clipOverrides);

        // Layer
        int     upperLayerIndex = animator.GetLayerIndex("Upper Layer");
        float   layerWeight     = 1f;
        animator.SetLayerWeight(upperLayerIndex, layerWeight);
    }

    public void SetCrossFade(string pLayerName, string pAnimStateName, float pNormalizedTransitionDuration, float pSpeed)
    {
        animator.speed = pSpeed;
        int upperLayerIndex = animator.GetLayerIndex(pLayerName);
        animator.CrossFade(pAnimStateName, pNormalizedTransitionDuration, upperLayerIndex);
    }

    public void SetRebind()
    {
        animator.Rebind();
    }

    public bool IsCurrentAnimatorStateName(string pLayerName, string pAnimName)
    {
        int upperLayerIndex = animator.GetLayerIndex(pLayerName);
        return animator.GetCurrentAnimatorStateInfo(upperLayerIndex).IsName(pAnimName);
    }

    public float GetAnimatorStateNormalizedTime(string pLayerName)
    {
        int upperLayerIndex = animator.GetLayerIndex(pLayerName);
        return animator.GetCurrentAnimatorStateInfo(upperLayerIndex).normalizedTime;
    }

    [Obsolete("임시")]
    public void SwapAnimationClip(string pBeforAnimationClip, string pAfterAnimationClip)
    {
        if (animatorOverrideController == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        AnimationClip animationClip = Resources.Load<AnimationClip>($"Animations/Upper/{pAfterAnimationClip}"); //임시
        if (animationClip == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        clipOverrides[pBeforAnimationClip] = animationClip;
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }
}
