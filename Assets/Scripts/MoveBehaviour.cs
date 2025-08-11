using UnityEngine;
using UnityEngine.Playables;

public class MoveBehaviour : PlayableBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public Vector3 startRotation;
    public Vector3 endRotation;

    public bool isActive;

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

        target.gameObject.SetActive(isActive);

        if (!isActive)
        {
            target.position = Vector3.Lerp(startPosition, endPosition, t);
            target.rotation = Quaternion.Euler(endRotation);
        }
    }
}
