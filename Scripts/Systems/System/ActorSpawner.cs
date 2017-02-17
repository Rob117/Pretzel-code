using UnityEngine;
using System.Collections;

public class ActorSpawner : MonoBehaviour {

    public int timeSpawnInterval;
    public Status MyActor;
    public bool OnlySpawnOnDeathOfMyAgent;
    public ActorTeamRegister register;

    Status myActorInstance;

    void Start() {
        if (!OnlySpawnOnDeathOfMyAgent)
            InvokeRepeating("SpawnActor", 5, timeSpawnInterval);
        else {
            SpawnActor();
        }

    }

    void SpawnActor() {
        myActorInstance = Instantiate(MyActor, transform.position, Quaternion.identity) as Status;
        myActorInstance.GetComponent<ActorTeamRegister>().SetMyTeam(register.GetMyTeam());
        myActorInstance.name = register.GetMyTeam().ToString() + myActorInstance.name;
        if (OnlySpawnOnDeathOfMyAgent)
            myActorInstance.OnHPZero += RespawnMyActor;
    }

    void RespawnMyActor(GameObject obj) {
        myActorInstance.OnHPZero -= RespawnMyActor;
        Invoke("SpawnActor", timeSpawnInterval);
    }
}
