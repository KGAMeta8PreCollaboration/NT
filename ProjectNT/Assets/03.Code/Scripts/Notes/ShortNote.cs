using UnityEngine;

public class ShortNote : Note
{
    public override void Hit(NoteType noteType)
    {
        Destroy();
        isHit = true;
        this.noteType = noteType;

        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        OnHit?.Invoke(this);
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
        noteType = NoteType.Bad;
        OnHit?.Invoke(this);
    }
}
