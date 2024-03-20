﻿using System;

public class Define
{
    public enum Spawning
    {
        None,
        Player,
        Enemy,
        Item,
    }

    public enum WorldObject
    {
        None,
        Character,
        Item,
    }

    [Obsolete("임시")]
    public enum Character
    {
        None,
        Human,
        Monster,
    }

    public enum Actor
    {
        None,
        Player,
        Enemy,
        //NPC,
    }

    public enum ExistenceState
    {
        Despawn,
        Spawn,
    }

    public enum SurvivalState
    {
        Dead,
        Alive,
    }

    public enum BaseAnim
    {
        None,
        Die,
        Idle,
        Run,
        //Attack,
    }

    public enum UpperAnim
    {
        None,
        Ready,
        Attack,
    }

    public enum WeaponType
    {
        None,
        Hand,
        Sword,
        Gun,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum CameraMode
    {
        None,
        Defualt,
        QuarterView,
    }

    //public enum Prefab
    //{
    //    None,
    //    Character,
    //    Weapon,
    //}

    //static public readonly Scene startScene = Scene.InitScene;
}
