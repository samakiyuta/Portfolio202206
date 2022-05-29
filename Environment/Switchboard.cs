using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Switchboard : FocusableObject
{
    [SerializeField] private Animator animator;

    [SerializeField] private DoorState doorState;
    [SerializeField] GameObject handlekey;
    [SerializeField] GameObject pawerSwitch;
    Sequence openDoorSequence;
    Sequence onSwitchSequence;
    public List<Powerable> poweredObjects;
    override protected void Start()
    {
        base.Start();
    }
    override public void OnFocusChange(bool hasFocus)
    {
        base.OnFocusChange(hasFocus);
        if (hasFocus)
        {
            if (!doorState.isLocked.Value)
            {
                ActionButton.Instance.Display("hand");
            }
            else
            {
                // 対象の鍵を持っていれば、その鍵をアクションボタンに表示
                var isHolding = ItemHolderController.Instance.GetItemFromHolder(doorState.doorKey);
                ActionButton.Instance.Display(doorState.doorKey, !isHolding);
            }


        }
        else
        {
            ActionButton.Instance.Hide();

        }
    }
    override public void OnEAction()
    {

        if (!animator.GetBool("IsOpen"))
        {
            if (!doorState.isLocked.Value)
            {
                // ハンドルを曲げてドアを開ける
                openDoorSequence = DOTween.Sequence()
                    .Append(handlekey.transform.DORotate(new Vector3(-90f, 0, 0), 1f)).SetRelative(true)
                    .OnComplete(() =>
                    {
                        animator.SetBool("IsOpen", true);
                        if (!animator.GetBool("IsReady"))
                        {
                            animator.SetBool("IsReady", true);
                        }
                    });
                openDoorSequence.Play();


            }
            else
            {
                // 鍵をプレイヤーが持っているなら、ハンドルを表示してロックを解除する
                var isConsumed = ItemHolderController.Instance.ConsumeItem(doorState.doorKey);
                if (isConsumed)
                {
                    handlekey.SetActive(true);
                    doorState.isLocked.Value = false;

                    ActionButton.Instance.Display("hand");
                }
                else
                {
                    ActionButton.Instance.Shake();
                }
            }

        }
        else
        {
            // スイッチをオンにして電源を供給する対象に通知する
            onSwitchSequence = DOTween.Sequence()
                .Append(pawerSwitch.transform.DORotate(new Vector3(-60f, 0, 0), 0.5f)).SetRelative(true)
                .OnComplete(() =>
                {
                    poweredObjects.ForEach(it=>{
                        it.OnPowerChange(true);
                    });
                });
            onSwitchSequence.Play();
        }


    }
}
