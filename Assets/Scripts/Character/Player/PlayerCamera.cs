using UnityEngine;
using Unity.Cinemachine;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
public class PlayerCamera : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    public PlayerManager player;

    public static PlayerCamera instance;
    public Camera cameraObject;

    public bool lockCamera;

    [Header("Lock on")]
    [SerializeField] float lockOnRadius = 20;
    [SerializeField] float minimumViewablAngle = -50;
    [SerializeField] float maximumViewableAngle = 50;
    private Coroutine cameraLockOnHeightCoroutine;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager currentLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;

    private void Awake()
    {
        cinemachineCamera = transform.parent.GetComponentInChildren<CinemachineCamera>();
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(instance.transform.parent.gameObject);
        if (!PlayerInputManager.instance.isTesting)
        {
            instance.enabled = false;
        }
        else
        {
            // Ensure the virtualCamera reference is set
            if (cinemachineCamera == null)
            {
                Debug.LogError("CinemachineVirtualCamera reference is not set!");
                return;
            }
        }
    }


    //public void SetCameraToFollowPlayer()
    //{
    //    freelookCamera.LookAt = player.transform;
    //    freelookCamera.Follow = player.transform;

    //}

    //public void SetCameraTolookAtTarget(GameObject target)
    //{
    //    freelookCamera.LookAt = target.transform;
    //    freelookCamera.Follow = player.transform;
    //}

    public void Update()
    {
        cinemachineCamera.enabled = !lockCamera;

        HandleCameraTrackingObject();
    }

    public void HandleCameraTrackingObject()
    {
        if (player.isLockedOn.Value)
        {
            Vector3 lockOnTargetsDirection = currentLockOnTarget.transform.position - player.transform.position;
            lockOnTargetsDirection = new Vector3 (lockOnTargetsDirection.x, 0, lockOnTargetsDirection.z);
            float lockAngle =  Vector3.SignedAngle(Vector3.forward, lockOnTargetsDirection, Vector3.up);

            //print(viewAngle);
            cinemachineCamera.gameObject.GetComponent<CinemachineOrbitalFollow>().HorizontalAxis.Value = lockAngle;

            if(currentLockOnTarget.isDead.Value)
            {
                SetCameraTo(player);
            }
        }
        else
        {
        }
    }    

    public void SetCameraTo(CharacterManager character)
    {
        currentLockOnTarget = character;
        if (character == null)
            return;
        cinemachineCamera.LookAt = character.gameObject.transform;
        cinemachineCamera.Follow = player.transform;
    }

    public void HandleLocatingLockOnTargets()
    {
        float shortestDistance = Mathf.Infinity; // will be used to determine the target closest to us
        float shortestDistanceOfRightTarget = Mathf.Infinity; // will be used to determine shortest distance on one axis to the right of current target (+)(closest target to the right of current target)
        float shortestDistanceOfLeftTarget = -Mathf.Infinity; // will be used to determine shortest distance on one axis to the left of current target (-)

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());
        //print("finding colliders get : " + colliders.Length);

        for (int i = 0; i < colliders.Length ; i++)
        {
           CharacterManager lockOnTarget = colliders[i].GetComponentInParent<CharacterManager>();
            //print("finding target get : " + lockOnTarget.name);
            if (lockOnTarget != null)
            {
                // check if they are within our field on view
                Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                //print(distanceFromTarget +"   "+ viewableAngle +"        "+ lockOnTarget.name);
                // if the target is dead, check the next potential target
                if (lockOnTarget.isDead.Value)
                    continue;

                // if the target is us, check the next potential target
                if (lockOnTarget.transform.root == player.transform.root)
                    continue ;

                // Lastly if the target is outside field of view or is blocked by enviro, check the next potential target
                if(viewableAngle > minimumViewablAngle && viewableAngle < maximumViewableAngle)
                {
                    RaycastHit hit;

                    //Debug.DrawLine(
                    //       player.playerCombatManager.lockOnTransform.position,
                    //       lockOnTarget.characterCombatManager.lockOnTransform.position,
                    //       Color.red, // Color of the line
                    //       10000f,        // Duration (0 = 1 frame)
                    //       false      // Depth test (set true if you want it hidden behind objects)
                    //    );

                    // toDo add layer mask for eviro layers only
                    if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, 
                        out hit, WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                      
                        // we hit something, we cannot see our lock on target
                        //print("hit somethings" + hit.collider.name);
                        continue ;
                    }
                    else
                    {
                        //Debug.Log("We can See enemy");
                        availableTargets.Add(lockOnTarget);

                    }
                }
            }
        }

        // we now sort through our potential targets to see which one we lock onto first
        for (int k = 0; k < availableTargets.Count; k++)
        {
            if (availableTargets[k] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                Vector3 lockTargetsDirection = availableTargets[k].transform.position - player.transform.position;

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                // if we are already locked on when searching for targets, search for our nearest left/right targets
                if (player.isLockedOn.Value)
                {
                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.y;

                    if (availableTargets[k] == player.playerCombatManager.currentTarget)
                        continue;

                    // check the left side for targets
                    if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }
                    // check the right side for targets
                    else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else
            {
                ClearLockOnTargets();
                player.isLockedOn.Value = false;
                SetCameraTo(player);
            }
        }
    }

    public void SetLockCameraHeight()
    {
        if(cameraLockOnHeightCoroutine != null)
        {
            StopCoroutine(cameraLockOnHeightCoroutine);
        }

        cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    public void ClearLockOnTargets()
    {
        // nếu rảnh thì làm thêm phần set camera to main nếu out of site
        leftLockOnTarget = null;
        rightLockOnTarget = null;
        nearestLockOnTarget = null;
        availableTargets.Clear();
    }

    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }

        ClearLockOnTargets();
        HandleLocatingLockOnTargets();

        if (nearestLockOnTarget != null)
        {
            player.playerCombatManager.SetTarget(currentLockOnTarget);
            player.isLockedOn.Value = true;
        }

        yield return null;
    }

    private IEnumerator SetCameraHeight()
    {
        cinemachineCamera.gameObject.GetComponent<CinemachineOrbitalFollow>().VerticalAxis.Value = cinemachineCamera.gameObject.GetComponent<CinemachineOrbitalFollow>().VerticalAxis.Center;
        yield return null;

    }
}
