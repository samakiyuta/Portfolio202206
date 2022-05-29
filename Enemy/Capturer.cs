using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityStandardAssets.Characters.ThirdPerson;

public class Capturer : MonoBehaviour
{
    [SerializeField] private AICharacterControl aICharacterControl;
    [SerializeField] private LayerMask obstacleLayer;
    void Start()
    {
        this.OnTriggerStayAsObservable()
            .Subscribe(collider =>
            {
                // プレイヤーがCapturerの内に存在し、Linecastが壁などにぶつからなければプレイヤーを追跡する
                var hitGameObject = collider.gameObject;
                var playCore = hitGameObject.GetComponent<PlayerCore>();
                if (playCore == null) return;
                Debug.DrawLine(transform.position + Vector3.up, collider.transform.position + Vector3.up, Color.blue);
                if(!Physics.Linecast(transform.position + Vector3.up, collider.transform.position + Vector3.up, obstacleLayer)){
                    aICharacterControl.target = hitGameObject.transform;
                }
                
            });
    }
}
