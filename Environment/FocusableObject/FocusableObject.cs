using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Sirenix.OdinInspector;
public abstract class FocusableObject : SerializedMonoBehaviour
{

    [SerializeField] private Collider focusCollider;
    // フォーカス対象の正面を定義する
    [SerializeField] private Vector3 _lookTestVector = new Vector3(0, 0, 1);
    public bool hasFocus
    {
        get;
        private set;
    } = false;
    public Vector3 lookTestVector
    {
        get { return _lookTestVector; }
        private set { _lookTestVector = value; }
    }
    private FocusableObjectsContainer m_Container;
    virtual protected void Start()
    {
        // thisがdestoryされたらフォーカス対象から外す
        this.OnDestroyAsObservable()
        .Subscribe(_ =>
        {
            if (!m_Container) return;
            m_Container.Remove(this);
        });
        // thisがFocusableObjectsContainerを持つオブジェクトに当たったらフォーカス対象に加える
        focusCollider.OnTriggerEnterAsObservable()
        .Subscribe(it =>
        {
            var container = it.gameObject.GetComponent<FocusableObjectsContainer>();
            if (!container) return;
            m_Container = container;
            container.Add(this);
        });
        // thisがFocusableObjectsContainerを持つオブジェクトから離れたらフォーカス対象から外す
        focusCollider.OnTriggerExitAsObservable()
        .Subscribe(it =>
        {
            var container = it.gameObject.GetComponent<FocusableObjectsContainer>();
            if (!container) return;
            m_Container = null;
            container.Remove(this);
        });
    }
    virtual public void OnFocusChange(bool hasFocus)
    {
        this.hasFocus = hasFocus;
    }
    abstract public void OnEAction();
    virtual public void OnFAction()
    {

    }
    virtual public void OnPlayerLeft()
    {

    }
}
