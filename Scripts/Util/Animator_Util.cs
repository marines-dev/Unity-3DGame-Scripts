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
            if (index != -1) { this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value); }
        }
    }
}

public class Animator_Util : MonoBehaviour
{
    Animator anim = null;
    Animator Anim => anim ?? (anim = gameObject.GetOrAddComponent<Animator>());
    AnimatorOverrideController  animatorOverrideController  = null;
    AnimationClipOverrides      clipOverrides               = null;


    void Start()
    {
        anim = gameObject.GetOrAddComponent<Animator>();
        Anim.applyRootMotion = true;
    }
    
    public void SetAnimatorController(string pAnimatorController, string pAnimatorAvatar)
    {
        string path = string.Empty;

        // RuntimeAnimatorController
        if (Anim.runtimeAnimatorController == null)
        {
            path = $"Animators/{pAnimatorController}";
            RuntimeAnimatorController runtimeAnimatorController = ResourceLoader.Load<RuntimeAnimatorController>(path);
            if (runtimeAnimatorController == null)
            {
                Util.LogWarning();
                return;
            }

            Anim.runtimeAnimatorController = runtimeAnimatorController;
        }

        // Avatar
        if (Anim.avatar == null)
        {
            path = $"Animators/{pAnimatorAvatar}";
            Avatar avatar = ResourceLoader.Load<Avatar>(path);
            if (avatar == null)
            {
                Util.LogWarning();
                return;
            }

            Anim.avatar = avatar;
        }

        // AnimationClip
        animatorOverrideController          = new AnimatorOverrideController(Anim.runtimeAnimatorController);
        Anim.runtimeAnimatorController      = animatorOverrideController;

        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        //foreach (KeyValuePair<AnimationClip, AnimationClip> clipOverride in clipOverrides)
        //{
        //    Util.LogSuccess("clipOverride.Key : " + clipOverride.Key.name);
        //}
        //animatorOverrideController.ApplyOverrides(clipOverrides);

        // Layer
        int     upperLayerIndex = Anim.GetLayerIndex("Upper Layer");
        float   layerWeight     = 1f;

        Anim.SetLayerWeight(upperLayerIndex, layerWeight);
    }

    public void SetCrossFade(string pLayerName, string pAnimStateName, float pNormalizedTransitionDuration, float pSpeed)
    {
        Anim.speed          = pSpeed;
        int upperLayerIndex = Anim.GetLayerIndex(pLayerName);

        Anim.CrossFade(pAnimStateName, pNormalizedTransitionDuration, upperLayerIndex);
    }

    public void SetRebind()
    {
        Anim.Rebind();
    }

    public bool IsCurrentAnimatorStateName(string pLayerName, string pAnimName)
    {
        int upperLayerIndex = Anim.GetLayerIndex(pLayerName);
        return Anim.GetCurrentAnimatorStateInfo(upperLayerIndex).IsName(pAnimName);
    }

    public float GetAnimatorStateNormalizedTime(string pLayerName)
    {
        int upperLayerIndex = Anim.GetLayerIndex(pLayerName);
        return Anim.GetCurrentAnimatorStateInfo(upperLayerIndex).normalizedTime;
    }

    [Obsolete("임시")]
    public void SwapAnimationClip(string pBeforAnimationClip, string pAfterAnimationClip)
    {
        if (animatorOverrideController == null)
        {
            Util.LogWarning();
            return;
        }

        AnimationClip animationClip = Resources.Load<AnimationClip>($"Animations/{pAfterAnimationClip}"); //임시
        if (animationClip == null)
        {
            Util.LogWarning();
            return;
        }

        clipOverrides[pBeforAnimationClip] = animationClip;
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }
}
