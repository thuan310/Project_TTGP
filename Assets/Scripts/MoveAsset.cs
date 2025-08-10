using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class MoveAsset : PlayableAsset
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Vector3 startRotation;
    public Vector3 endRotation;

    public bool isActive;

    //public ExposedReference<Transform> target;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        try { behaviour.SetTarget(SceneNavigationManager.instance.player.transform); } catch { return playable; }


        behaviour.startPosition = startPosition;
        behaviour.endPosition = endPosition;
        behaviour.startRotation = startRotation;
        behaviour.endRotation = endRotation;
        behaviour.isActive = isActive;


        return playable;
    }
}
