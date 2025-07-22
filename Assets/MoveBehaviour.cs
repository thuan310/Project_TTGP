using UnityEngine;
using UnityEngine.Playables;

public class MoveBehaviour : PlayableBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;

    private Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (target == null) return;

        double time = playable.GetTime();
        double duration = playable.GetDuration();
        float t = (float)(time / duration);

        target.position = Vector3.Lerp(startPosition, endPosition, t);
    }
}
