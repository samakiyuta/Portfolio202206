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
            // �h�A�ɓ��������炻�̃h�A��ێ����Ă���
            var enemyOperableObj = it.gameObject.GetComponent<EnemyOperable>();
            if (enemyOperableObj == null) return;
            objectToOperate = it.gameObject;

        });
        enemyCollider.OnTriggerExitAsObservable().Subscribe(it =>
        {
            var hitObj = it.gameObject.GetComponent<EnemyOperable>();
            if (hitObj == null) return;
            // �ێ����Ă���h�A���痣�ꂽ�炻�̃h�A��ێ����Ȃ�
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
            // OffMeshLink��Ńh�A������ΊJ����B�J���Ȃ���΂ЂƂ܂��ǐՂ��~�߂�
            if (objectToOperate != null)
            {
                isCanGoThroughOffMeshLink = objectToOperate.GetComponent<EnemyOperable>().OntryingToOpen();
            }
            if (isCanGoThroughOffMeshLink)
            {
                agent.isStopped = false;
                // target�Ƃ̋�����stoppingDistance���傫���ꍇ�͒ǐՂ𑱂���
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
