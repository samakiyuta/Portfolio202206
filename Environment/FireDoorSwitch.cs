using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class FireDoorSwitch : FocusableObject, Powerable
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    [SerializeField] private Animator switchAnimator;

    [SerializeField] private DoorState doorState;
    [SerializeField] private DoorState fireDoorState;

    [SerializeField] private Renderer lampRenderer;

    public bool isPowered = false;
    override protected void Start()
    {

        base.Start();
    }
    private void LoadUi()
    {
        if (hasFocus)
        {
            if (animator.GetBool("IsOpen"))
            {
                if (isPowered)
                {
                    ActionButton.Instance.Display("Switch");
                }
                else
                {
                    ActionButton.Instance.Display("Electricity", true);
                }
                return;
            }
            if (!doorState.isLocked.Value)
            {
                ActionButton.Instance.Display("hand");
            }
            else
            {
                // 対象の鍵をプレイヤーが持っているならそのカギをアクションボタンに表示
                var isHolding = ItemHolderController.Instance.GetItemFromHolder(doorState.doorKey);
                ActionButton.Instance.Display(doorState.doorKey, !isHolding);
            }


        }
        else
        {

            ActionButton.Instance.Hide();

        }
    }

    // Update is called once per frame
    override public void OnFocusChange(bool hasFocus)
    {
        base.OnFocusChange(hasFocus);
        if (switchAnimator.GetBool("IsOpen")) return;
        LoadUi();
    }
    override public void OnEAction()
    {
        if (switchAnimator.GetBool("IsOpen")) return;
        if (!animator.GetBool("IsOpen"))
        {
            if (!doorState.isLocked.Value)
            {
                animator.SetBool("IsOpen", true);
                if (!animator.GetBool("IsReady"))
                {
                    animator.SetBool("IsReady", true);
                }
                LoadUi();

            }
            else
            {
                // 対象の鍵をプレイヤーが持っているなら、鍵を消費して鍵を開ける
                var isConsumed = ItemHolderController.Instance.ConsumeItem(doorState.doorKey);
                if (isConsumed)
                {
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
            if (isPowered)
            {
                // スイッチをオンにして防火戸の鍵を解除する
                switchAnimator.SetBool("IsOpen", true);
                if (!switchAnimator.GetBool("IsReady"))
                {
                    switchAnimator.SetBool("IsReady", true);
                }
                fireDoorState.isLocked.Value = false;
            }
            else
            {
                ActionButton.Instance.Shake();
            }
        }


    }
    public void OnPowerChange(bool isPowered)
    {
        this.isPowered = isPowered;
        if (isPowered)
        {
            lampRenderer.material.EnableKeyword("_EMISSION");
        }
        else
        {
            lampRenderer.material.DisableKeyword("_EMISSION");
        }
    }
}
