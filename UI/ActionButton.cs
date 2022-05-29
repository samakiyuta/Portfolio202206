using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using DG.Tweening;
public class ActionButton : MonoBehaviour
{

    public static ActionButton Instance { get; private set; }
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject cross;
    private Sprite sprite;
    private float initialPositionX;
    private Sequence sequence;
    void Awake()
    {
        Instance = this;

        gameObject.SetActive(false);
        cross.SetActive(false);
    }
    public void Display(string resourceName = "", bool isEnableCross = false)
    {

        cross.SetActive(isEnableCross);
        if (resourceName.Equals(""))
        {
            icon.sprite = null;
            icon.color = Color.black;
            gameObject.SetActive(true);
            return;
        }
        Addressables.LoadAssetAsync<Sprite>(resourceName).Completed += handle =>
        {
            icon.color = Color.white;
            if (icon == null) return;
            icon.sprite = handle.Result;
            if (gameObject == null) return;
            gameObject.SetActive(true);
        };
    }
    public void Hide()
    {
        gameObject?.SetActive(false);

    }

    public void Shake()
    {
        if ((sequence?.IsPlaying() ?? false)) return;
        initialPositionX = transform.localPosition.x;
        sequence = DOTween.Sequence()
            .Append(transform.DOLocalMoveX(initialPositionX + 30f, 0.1f).SetRelative(false))
            .Append(transform.DOLocalMoveX(initialPositionX - 30f, 0.1f).SetRelative(false))
            .Append(transform.DOLocalMoveX(initialPositionX + 30f, 0.1f).SetRelative(false))
            .Append(transform.DOLocalMoveX(initialPositionX, 0.1f).SetRelative(false));
    }
}
