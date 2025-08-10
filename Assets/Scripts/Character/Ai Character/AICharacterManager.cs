using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";

    [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AiCharacterLocomotionManager aiCharacterLocomotionManager;


    [Header("Navmesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header(" Current State")]
    [SerializeField] protected AIState currentState;

    [Header("States")]
    public IdleState idle;
    public SearchingTargetState searchingTarget;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;          // Combat stance
    public AttackState attack;                      // Attack
    public TalkingState talking;

    [Header("Animation Action")]
    public string idleAction;
    public string talkingAction;
    public string searchingAction;

    [Header("Flags")]
    public bool isTalking;
    public bool isSearching;

    protected override void Awake()
    {
        base.Awake();

        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterLocomotionManager = GetComponent<AiCharacterLocomotionManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // use a copy of the scriptableobjects, so the originals are not modified
    }

    protected override void Start()
    {
        base.Start();

        idle = Instantiate(idle);
        searchingTarget = Instantiate(searchingTarget);
        pursueTarget = Instantiate(pursueTarget);
        combatStance = Instantiate(combatStance);
        attack = Instantiate(attack);
        talking = Instantiate(talking);


        // Try to snap it onto the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(this.gameObject.transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            navMeshAgent.Warp(hit.position);
            Debug.Log("✅ Enemy agent warped to NavMesh.");
        }
        //else
        //{
        //    Debug.LogError("❌ Could not place agent on NavMesh at spawn position.");
        //}

        if (currentState == null)
        {
            currentState = searchingTarget;
        }

        currentHealth.OnValueChanged += CheckHp;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        currentHealth.OnValueChanged -= CheckHp;
    }
    protected override void Update()
    {
        base.Update();

        aiCharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ProcessStateMachine();
    }

    // option 1
    private void ProcessStateMachine()
    {
        //print("ds");
        AIState nextState = null;

        if (currentState != null)
        {
            nextState = currentState.Tick(this);
        }

        ControlAIStopOrContinueMoving();

        if (nextState != null)
        {
            currentState = nextState;
        }

    }
    private void ControlAIStopOrContinueMoving()
    {
        // the position/ rotation should be reset only after the state machine has processed it's tick
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if (aiCharacterCombatManager.currentTarget != null)
        {
            aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
            aiCharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
            aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
        }


        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                isMoving.Value = true;
            }
            else
            {
                isMoving.Value = false;
            }
        }
        else
        {
            isMoving.Value = false;
        }
    }
}
