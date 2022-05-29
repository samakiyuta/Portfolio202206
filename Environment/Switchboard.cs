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
                // �Ώۂ̌��������Ă���΁A���̌����A�N�V�����{�^���ɕ\��
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
                // �n���h�����Ȃ��ăh�A���J����
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
                // �����v���C���[�������Ă���Ȃ�A�n���h����\�����ă��b�N����������
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
            // �X�C�b�`���I���ɂ��ēd������������Ώۂɒʒm����
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
