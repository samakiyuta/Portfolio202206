using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DeathCamera : MonoBehaviour
{

    [SerializeField] private AudioClip neckSqueakSound;
    [SerializeField] private AudioClip neckBreakSound;
    [SerializeField] private ScenesData scenesData;

    private Camera deathCamera;
    private AudioSource audioSource;
    private MaskController maskController;
    void Awake()
    {
        DOTween.Init();
        DOTween.defaultAutoPlay = AutoPlay.None;
    }
    void Start()
    {
        // éÒÇÃÁaÇ›Å®éÒÇÃçúê‹Å®éÄñS
        maskController = GetComponent<MaskController>();
        audioSource = GetComponent<AudioSource>();
        Sequence neckBreakSequence = DOTween.Sequence();
        var neckBreakRotate = new Vector3(15.525f, 161.674f, -95.066f);
        neckBreakSequence.Append(this.transform.DORotate(neckBreakRotate, 0.5f)).SetRelative(false);
        neckBreakSequence.OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                scenesData.LoadGameOverScene();
                maskController.height = MaskController.HEIGHT_MAX;
            }).Play();
        });
        Sequence neckSqueakSequence = DOTween.Sequence();
        neckSqueakSequence.Append(this.transform.DOMoveZ(0.05f, 0.1f)).SetRelative(true);
        neckSqueakSequence.Append(this.transform.DOMoveZ(-0.05f, 0.1f)).SetRelative(true);
        neckSqueakSequence.Append(this.transform.DOMoveZ(0.05f, 0.1f)).SetRelative(true);
        neckSqueakSequence.SetLoops(8, LoopType.Restart);
        neckSqueakSequence.OnComplete(() =>
        {
            audioSource.clip = neckBreakSound;
            audioSource.Play();
            neckBreakSequence.Play();
            maskController.height = 0.7f;
        });
        audioSource.clip = neckSqueakSound;
        neckSqueakSequence.Play();
        audioSource.Play();
    }
}
