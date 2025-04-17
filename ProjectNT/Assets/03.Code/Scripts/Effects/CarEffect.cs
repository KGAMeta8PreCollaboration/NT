using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations;
using UniRan = UnityEngine.Random;
public class CarEffect : MapEffect<GameObject>
{
    [SerializeField] private List<Material> carMaterials;
    [SerializeField] private Transform phase2Pos;
    [SerializeField] private Transform phase3Pos;
    [SerializeField] private Transform leftStartTrans;
    [SerializeField] private Transform leftEndTrans;
    [SerializeField] private Transform rightStartTrans;
    [SerializeField] private Transform rightEndTrans;
    public float targetDuration;

    public override void P1EffectInvoke()
    {
        CarDoTween(leftStartTrans.position, leftEndTrans.position);
    }

    public override void P2EffectInvoke()
    {
        CarDoTween(rightStartTrans.position, rightEndTrans.position);
    }

    protected override void Init(List<GameObject> list, ref Sequence sequence) { }
    private void CarDoTween(Vector3 startPos, Vector3 endPos)
    {
        CarObject obj = PoolManager.Instance.carEffectPool.Pop();
        obj.renderer.material = carMaterials[UniRan.Range(0, carMaterials.Count)];
        obj.transform.position = startPos;
        obj.transform.DOMove(endPos, targetDuration).SetEase(Ease.OutQuart).onComplete += () => PoolManager.Instance.carEffectPool.Push(obj);
    }

    public override void LeftEffectEnd()
    {
        StartCoroutine(CarCoroutine());
    }

    public override void RightEffectEnd()
    {
        StartCoroutine(CarCoroutine());
    }

    private IEnumerator CarCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            CarDoTween(leftStartTrans.position, leftEndTrans.position);
            CarDoTween(rightStartTrans.position, rightEndTrans.position);
        }
    }
    public void MovePhase2Pos()
    {
        transform.position = phase2Pos.position;
    }
    public void MovePhase3Pos()
    {
        transform.position = phase3Pos.position;
    }
}
