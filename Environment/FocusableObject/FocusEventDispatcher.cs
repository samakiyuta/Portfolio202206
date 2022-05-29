
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
public class FocusEventDispatcher : MonoBehaviour
{

    [SerializeField] private Collider collider;
    [SerializeField] private GameObject operatorObject;
    [SerializeField] private FocusableObjectsContainer m_Container;

    private IInputEventProvider inputEvent;
    private const float DotMin = 0.1;
    private const float DotMax = 0.9;

    void Start()
    {
        inputEvent = GetComponent<IInputEventProvider>();
        m_Container.addObservable.Subscribe(it =>
        {
            var count = m_Container.valueList.Count;
            // フォーカス対象のオブジェクトが1つ以上ある場合は、
            // 最新のフォーカス対象以外は都度フォーカスを解除する
            if (count > 1)
            {
                m_Container.valueList[count - 2].OnFocusChange(false);
            }
            // プレイヤーがフォーカス対象の正面に向いていればフォーカスする
            var dot = Vector3.Dot(operatorObject.transform.forward, m_Container.valueList.Last().lookTestVector);
            if (dot < DotMin && Mathf.Abs(dot) > DotMax)
            {
                m_Container.valueList.Last().OnFocusChange(true);
            }

        });
        m_Container.removeObservable.Subscribe(it =>
        {
            var count = m_Container.valueList.Count;
            // プレイヤーが最新のフォーカス対象の正面に向いていればフォーカスする
            if (count > 0)
            {
                var dot = Vector3.Dot(operatorObject.transform.forward, m_Container.valueList.Last().lookTestVector);
                if (dot < DotMin && Mathf.Abs(dot) > DotMax)
                {
                    m_Container.valueList.Last().OnFocusChange(true);
                }
            }
            // フォーカス対象から外れたオブジェクトはフォーカスを解除する
            it.Value.OnPlayerLeft();
            it.Value.OnFocusChange(false);
        });
        operatorObject.GetComponent<FirstPersonController>().playerDirection.DistinctUntilChanged().Subscribe(it =>
        {
            if (m_Container.valueList.Count == 0) return;
            // プレイヤーの向いている向きが変化したらフォーカス対象にフォーカスしているかチェックする
            var lastFocusableObject = m_Container.valueList.Last();
            var dot = Vector3.Dot(operatorObject.transform.forward, lastFocusableObject.lookTestVector);
            if (dot < DotMin && Mathf.Abs(dot) > DotMax)
            {
                if (!lastFocusableObject.hasFocus)
                    lastFocusableObject.OnFocusChange(true);
            }
            else
            {
                if (lastFocusableObject.hasFocus)
                    lastFocusableObject.OnFocusChange(false);
            }
        });
    }
    public void OnEAction()
    {
        if (m_Container.valueList.Count == 0)
        {
            return;
        }
        // 最新のフォーカス対象がフォーカスされていればイベントを通知する
        var obj = m_Container.GetLasted();
        if (!obj || !obj.hasFocus) return;
        obj.OnEAction();
    }
    public void OnFAction()
    {
        if (m_Container.valueList.Count == 0)
        {
            return;
        }
        //最新のフォーカス対象がフォーカスされていればイベントを通知する
        var obj = m_Container.GetLasted();
        if (!obj || !obj.hasFocus) return;
        obj.OnFAction();
    }
}
