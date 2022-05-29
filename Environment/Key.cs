using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using CanaryTower.ScriptableObjects;
public class Key : FocusableObject
{
    public string keyName;
    public ScenesData scenesData;

    private FieldItem itemData;
    private Collider collider;
    private bool isCanAction = false;
    private IGameEvent gameEvent = null;
    override protected void Start()
    {
        base.Start();
        // Šù‚ÉŽæ“¾‚³‚ê‚Ä‚¢‚½‚çdestroy‚·‚é
        itemData=scenesData.GetCurrentEp().tower.items.Find(it=>it.GetName()==keyName);
        if(itemData.isAcquired){
            Destroy(gameObject);
        }
        gameEvent=GetComponent<IGameEvent>();
    }
    override public void OnFocusChange(bool hasFocus)
    {
        base.OnFocusChange(hasFocus);
        if (hasFocus){
            ActionButton.Instance.Display(keyName);
        }else{
            ActionButton.Instance.Hide();
        }
            
    }
    override public void OnEAction()
    {
        ItemHolderController.Instance.GrabbedItem(keyName);
        ItemPopup.Instance.Display(keyName);
        ActionButton.Instance.Hide();
        itemData.isAcquired=true;
        if(gameEvent!=null){
            gameEvent.dispatchEvent();
        }
        Destroy(gameObject);
        
        
    }
}
