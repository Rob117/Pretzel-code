using UnityEngine;
using System.Collections;

public class ParticleDeleteOnFinish : MonoBehaviour {
    [SerializeField]
    bool deleteByTimer;
    [SerializeField]
    float timeLimit;
    [SerializeField]
    int DELETE_FRAME;

    float timer = 0;

    ParticleSystem[] particleSystems;

    void Awake() {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Start() {
        if (ParticleSystemInitializer.Instance.hideInHeirarchy) gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

    int frameCheck = 0;
    
    void Update() {
        frameCheck++;
        if (deleteByTimer) {
            timer += Time.deltaTime;
            if (timer > timeLimit)
                Destroy(gameObject);
            return;
        }
        
        if (frameCheck >= DELETE_FRAME) {
            bool aliveSystemRemaining = false;
            for (int i = 0; i < particleSystems.Length -1; i++) {
                if (particleSystems[i].IsAlive()) {
                    aliveSystemRemaining = true;
                    break;
                }
            }
            if (!aliveSystemRemaining)
                Destroy(this.gameObject);
            frameCheck = 0;
        }
    }
}
