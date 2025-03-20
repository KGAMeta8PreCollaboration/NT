using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShortNote : Note
{
    // TODO: GameManager.Scoremanager로 변경할듯?
    private ScoreManager _scoreManager;

    public override void Hit(JudgementType noteType)
    {
        Destroy();
        isHit = true;
        this.judgementType = noteType;

        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        OnHit?.Invoke(this);
    }


    protected override void PostJudgement()
    {
        if (judgementType == JudgementType.Bad)
            _scoreManager.ResetCombo();
        else
            _scoreManager.IncreaseCombo();
        _scoreManager.AddScore(judgementType);
        _scoreManager.ShowJudgementType(judgementType);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Woofer"))
            Miss();
    }

    private void Miss()
    {
        Destroy();
        isHit = true;
        judgementType = JudgementType.Bad;
        OnHit?.Invoke(this);
    }
}
