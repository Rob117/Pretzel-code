using UnityEngine;

[Prefab("DesignerTool_PREFAB", true)]
public class DesignerTool : SingletonPrefab<DesignerTool> {

    [Range(.01f, 5)]
    public float MinionSpeed = 1f;
    [Range(10, 200)]
    public float MinionTurnSpeed = 100f;

    [Range(10, 100)]
    public int MinionHealth = 30;
    [Range(10, 30)]
    public int MinionDamagePerSecond = 10;
    [Range(.3f, 1)]
    public float MinionMinimumAttackDuration = .8f;
    [Range(0, 10)][Tooltip("Do not touch this unless you know what you are doing")]
    public float MinionAggroRange = 5f;

    [Range(.01f, 5)]
    public float HeroSpeed = 2f;
    [Range(10, 200)]
    public float HeroTurnSpeed = 100f;

    [Range(10, 300)]
    public int HeroHealth = 90;
    [Range(10, 40)]
    public int HeroDamagePerSecond = 20;
    [Range(.3f, 1)]
    public float HeroMinimumAttackDuration = .8f;


    [Range(.1f, 4)]
    public float TowerCatchupSpeed = .13f;
    [Range(0, 1f)]
    public float TowerRotationDelay = .2f;
    [Range(50, 500)]
    public int TowerHealth = 200;
    [Range(10, 100)]
    public int VictoryValue = 20;
    [Range(.1f, 1)][Tooltip("fractions of a second to wait before firing. We are the slayers")]
    public float WaitTimeBetweenShotsInSeconds = 1;
    [Range(5, 20)]
    public int TowerRange = 10;

    [Range(0, 100)][Tooltip("HigherPriority = attacked sooner")]
    public int MinionTargetPriority = 10;
    [Range(0, 100)][Tooltip("HigherPriority = attacked sooner")]
    public int HeroTargetPriority = 5;
    [Range(0, 100)][Tooltip("HigherPriority = attacked sooner")]
    public int TowerTargetPriority = 1;


    [Tooltip("Controls showing the particle systems in heirarchy")]
    public bool hideParticlesInHeirarchy = false;

    [Range(1, 50)][Tooltip("Speed that the cannonball strikes")]
    public float CannonBallSpeed = 5;
    [Range(10, 100)][Tooltip("Cannonball damage on strike")]
    public int CannonBallDamage = 20;
}
