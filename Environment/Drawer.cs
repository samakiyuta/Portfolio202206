using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : FocusableObject
{

    [SerializeField] private GameObject item;
    private Animator animator;
    override protected void Start()
    {
        base.Start();
        if (item)
        {
            item.SetActive(false);
        }

        animator = GetComponent<Animator>();
    }
    override public void OnFocusChange(bool hasFocus)
    {
        base.OnFocusChange(hasFocus);
        Debug.Log("OnFocusChange: Drawer" + hasFocus);
        if (hasFocus)
        {
            ActionButton.Instance.Display("hand");
        }
        else
        {
            ActionButton.Instance.Hide();

        }
    }
    override public void OnEAction()
    {
        // 引き出しを開けるときに引き出しの中のアイテムをアクティブにする
        if (!animator.GetBool("IsOpen"))
        {
            if (item)
            {
                item.SetActive(true);
            }
            animator.SetBool("IsOpen", true);
        }
        else
        {

            animator.SetBool("IsOpen", false);
            if (item)
            {
                item.SetActive(false);
            }
        }


    }
    override public void OnPlayerLeft()
    {
        if (animator.GetBool("IsOpen"))
        {
            animator.SetBool("IsOpen", false);
            if (item)
            {
                item.SetActive(false);
            }
        }

    }
}
