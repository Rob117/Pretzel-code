using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AnimationFXDesignerTool", menuName = "Programmer/createAnimDesignerTool", order =101)]
public class AnimationTool : ScriptableObject {
    public List<ClipFXPair> ClipFXPairs;
}
