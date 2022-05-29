using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class AICharacterControl : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public ThirdPersonCharacter character { get; private set; }
    public Transform target;

    [SerializeField] private float speed = 0.8f;
    [SerializeField] private Collider enemyCollider;

    private bool isCanGoThroughOffMeshLink = true;
    private GameObject objectToOperate;
    private bool isKilledPlayer = false;
    private void Start()
    {
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;
        agent.autoTraverseOffMeshLink = true;
        enemyCollider.OnTriggerEnterAsObservable().Subscribe(it =>
        {
            // ドアに当たったらそのドアを保持しておく
            var enemyOperableObj = it.gameObject.GetComponent<EnemyOperable>();
            if (enemyOperableObj == null) return;
            objectToOperate = it.gameObject;

        });
        enemyCollider.OnTriggerExitAsObservable().Subscribe(it =>
        {
            var hitObj = it.gameObject.GetComponent<EnemyOperable>();
            if (hitObj == null) return;
            // 保持しているドアから離れたらそのドアを保持しない
            if (objectToOperate == it.gameObject)
            {
                objectToOperate = null;
            }
        });
    }


    private void Update()
    {
        if (isKilledPlayer)
        {
            agent.isStopped = true;
            character.Move(Vector3.zero, false, false);
            return;
        }
        if (target != null)
        {
            agent.SetDestination(target.position);

        }
        
        if (agent.isOnOffMeshLink)
        {
            // OffMeshLink上でドアがあれば開ける。開かなければひとまず追跡を止める
            if (objectToOperate != null)
            {
                isCanGoThroughOffMeshLink = objectToOperate.GetComponent<EnemyOperable>().OntryingToOpen();
            }
            if (isCanGoThroughOffMeshLink)
            {
                agent.isStopped = false;
                // targetとの距離がstoppingDistanceより大きい場合は追跡を続ける
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    character.Move(agent.desiredVelocity, false, false);
                }
                else
                {
                    character.Move(Vector3.zero, false, false);
                }
            }
            else
            {
                agent.isStopped = true;
                character.Move(Vector3.zero, false, false);
            }

        }
        else
        {
            if (!isCanGoThroughOffMeshLink)
            {
                isCanGoThroughOffMeshLink = true;
            }
            agent.isStopped = false;
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
    }
    public void OnKilledPlayer()
    {
        isKilledPlayer = true;

    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
