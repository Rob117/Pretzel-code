using UnityEngine;
using System.Collections;

public class Tags : MonoBehaviour {

}

public static class RewiredActions {
    public const string fire = "fire";
    public const string horizontal = "horizontal";
    public const string vertical = "vertical";
    public const string change_actor = "change_actor";
    public const string change_team = "change_team";
    public const string change_to_player_character = "change_to_player_character";
    public const string change_actor_minus = "change_actor_minus";
}

public static class ObjectTags {
    public const string DesignerTool = "DesignerTool";
}

public static class AnimTags {
    public static int speedHash = Animator.StringToHash("Speed");
    public static int attackingHash = Animator.StringToHash("Attacking");
    public static int attackStartedHash = Animator.StringToHash("AttackStarted");
    public static int deadHash = Animator.StringToHash("Dead");
}