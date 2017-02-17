using UnityEngine;
using System.Collections.Generic;
using Team;
using System.Linq;

[Prefab("VictoryConditions_PREFAB", true)]
public class VictoryConditions : SingletonPrefab<VictoryConditions> {

    [SerializeField][Tooltip("Think CAREFULLY before touching this. Set visible only for runtime debugging")]
    List<VictoryObject> victoryObjects = new List<VictoryObject>();

    public static bool canCallEndOfGame = true;

    public void RegisterAsVictoryObject(VictoryObject obj) {
        if (obj != null)
            victoryObjects.Add(obj);
    }

    public void DeregisterVictoryObject(VictoryObject obj) {
        if (obj != null) {
            obj.SetVictoryObjectStatusToFalseOnDeregister();
            victoryObjects.Remove(obj);
            if (canCallEndOfGame && victoryObjects.GroupBy(vo => vo.GetTeam()).Select(g => g.First()).ToList().Count == 1) {
                var winningTeam = victoryObjects.First().GetTeam();
                if (winningTeam == Teams.teamOne)
                    FindObjectOfType<MainSceneController>().TeamOneWin();
                else
                    FindObjectOfType<MainSceneController>().TeamTwoWin();
            }
        }
    }

    public VictoryObject GetHighestPriorityUnalignedVictoryObject(Teams myTeam) {
        return victoryObjects.Where(x => x.GetTeam() != myTeam).OrderByDescending(x => x.VictoryConditionPriority).First();
    }

    public Damageable GetHighestPriorityDamageableVictoryObject(Teams myTeam) {
        var damageableObjects = victoryObjects.Where(x => x.GetTeam() != myTeam && x.GetComponent<Damageable>() != null);
        if (damageableObjects.Count() > 0) 
            return damageableObjects.OrderByDescending(x => x.VictoryConditionPriority).First() as Damageable;
        return null;
    }
}
