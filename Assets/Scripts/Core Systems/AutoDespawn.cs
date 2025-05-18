using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [Tooltip("How many seconds before this object is destroyed")]
    public float despawnTime = 5f;

    [Tooltip("Particle effect to play before destruction")]
    public ParticleSystem despawnEffect;

    private void Start()
    {
        Invoke(nameof(Despawn), despawnTime);
    }

    private void Despawn()
    {
        if (despawnEffect != null)
        {
            ParticleSystem effect = Instantiate(despawnEffect, transform.position, transform.rotation);
            effect.Play();

            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }

        Destroy(gameObject);
    }
}
