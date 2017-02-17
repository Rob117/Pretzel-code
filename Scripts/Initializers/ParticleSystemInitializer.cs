using UnityEngine;
using System.Collections;

public class ParticleSystemInitializer : Singleton<ParticleSystemInitializer> {
    public bool hideInHeirarchy = false;
    DesignerTool designerTool;

    public void Start() {
        designerTool = GameObject.FindObjectOfType<DesignerTool>();
        hideInHeirarchy = designerTool.hideParticlesInHeirarchy;
    }
}
