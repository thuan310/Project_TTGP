using UnityEngine;
using UnityEngine.Playables;

public class MoveAsset : PlayableAsset
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    public ExposedReference<Transform> target;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();

        behaviour.startPosition = startPosition;
        behaviour.endPosition = endPosition;

        behaviour.SetTarget(target.Resolve(graph.GetResolver()));

        return playable;
    }
}
