using System;
using ServerData;
using UnityEngine;

[Obsolete("юс╫ц")]
public class GlobalScene : BaseScene<GlobalScene, GlobalUI>
{
    /// <summary>
    /// Table
    /// </summary>
    public SpawnerTable     SpawnerTable    { get { return CreateOrGetBaseTable<SpawnerTable>(); } }
    public EnemyTable       EnemyTable      { get { return CreateOrGetBaseTable<EnemyTable>(); } }
    public CharacterTable   CharacterTable  { get { return CreateOrGetBaseTable<CharacterTable>(); } }
    public StatTable        StatTable       { get { return CreateOrGetBaseTable<StatTable>(); } }
    public WeaponTable      WeaponTable     { get { return CreateOrGetBaseTable<WeaponTable>(); } }

    protected override void OnAwake()
    {
        RegisteredGlobalObjects();
    }

    protected override void OnStart() { }
    protected override void onDestroy() { }

}
