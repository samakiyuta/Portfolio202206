using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
public class PeepingDoor : MonoBehaviour
{
    [SerializeField] private Animator altarPeepingDoorAnimator;
    [SerializeField] private Camera peepingDoorCamera;
    [SerializeField] private string sceneName;
    private float initialPositionZ;
    private Sequence openSequence;
    private Sequence closeSequence;
    void Start()
    {
        initialPositionZ = peepingDoorCamera.transform.localPosition.z;

    }
    public void OpenPeepingDoor()
    {
        if ((openSequence?.IsPlaying() ?? false) || (closeSequence?.IsPlaying() ?? false)) return;
        Debug.Log("OpenPeepingDoor");
        PeepingDoorPresenter.Instance.isUsingPeepingDoor.Value = true;
        peepingDoorCamera.enabled = true;
        // カメラから見て前に進むようにする
        var distanceZ = (initialPositionZ < 0) ? initialPositionZ + 0.4f : initialPositionZ - 0.4f;
        openSequence = DOTween.Sequence()
            .Append(peepingDoorCamera.transform.DOLocalMoveZ(distanceZ, 3f).SetRelative(false))
            .OnComplete(() =>
            {
                //waitしないが、戻り値を受け取らないとエラーになる
                UniTask nowait = LoadSceneInPeepingDoorAsync(sceneName, LoadSceneMode.Additive, null, () =>
                {
                    peepingDoorCamera.enabled = false;
                });

            });
        openSequence.Play();
        altarPeepingDoorAnimator.SetBool("IsOpen", true);
        if (!altarPeepingDoorAnimator.GetBool("IsReady"))
        {
            altarPeepingDoorAnimator.SetBool("IsReady", true);
        }

    }
    public void OnClosePeepingDoor()
    {
        if ((openSequence?.IsPlaying() ?? false) || (closeSequence?.IsPlaying() ?? false)) return;
        peepingDoorCamera.enabled = true;
        SceneManager.UnloadSceneAsync(sceneName);
        altarPeepingDoorAnimator.SetBool("IsOpen", false);
        if (!altarPeepingDoorAnimator.GetBool("IsReady"))
        {
            altarPeepingDoorAnimator.SetBool("IsReady", true);
        }
        closeSequence = DOTween.Sequence()
            .Append(peepingDoorCamera.transform.DOLocalMoveZ(initialPositionZ, 3f).SetRelative(false))
            .OnComplete(() =>
            {
                peepingDoorCamera.enabled = false;
                PeepingDoorPresenter.Instance.isUsingPeepingDoor.Value = false;
            });
        closeSequence.Play();
    }
    private async UniTask LoadSceneInPeepingDoorAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, Action onLoadStart = null, Action onLoadFinished = null)
    {
        onLoadStart?.Invoke();
        await SceneManager.LoadSceneAsync(sceneName, mode);
        onLoadFinished?.Invoke();
    }
}
