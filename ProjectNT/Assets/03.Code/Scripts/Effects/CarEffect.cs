using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using UniRan = UnityEngine.Random;
public class CarEffect : MapEffect<GameObject>
{
    [SerializeField] private List<Material> carMaterials;
    [SerializeField] private Transform leftStartTrans;
    [SerializeField] private Transform leftEndTrans;
    [SerializeField] private Transform rightStartTrans;
    [SerializeField] private Transform rightEndTrans;
    public float targetDuration;

    private void OnEnable()
    {

    }

    public override void LeftEffectInvoke()
    {
        CarDoTween(leftStartTrans.position, leftEndTrans.position);
    }

    public override void RightEffectInvoke()
    {
        CarDoTween(rightStartTrans.position, rightEndTrans.position);
    }

    protected override void Init(List<GameObject> list, ref Sequence sequence) { }
    private void CarDoTween(Vector3 startPos, Vector3 endPos)
    {
        CarObject obj = PoolManager.Instance.carEffectPool.Pop();
        obj.transform.position = startPos;
        obj.transform.DOMove(endPos, targetDuration).SetEase(Ease.OutQuart);
    }
}
