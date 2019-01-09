using UnityEngine;
using Pathfinding;

public class HealthOrb : AutoCollectCollectibles {

    public GameObject crossParticle;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Player>() != null) {
            collision.GetComponent<Player>().ModifyCurHealth(myBaseDamage);
            GameObject healthEffect = Instantiate(crossParticle, collision.transform.Find("Target").transform.position, Quaternion.identity);

            healthEffect.transform.parent = collision.transform;
            Destroy(healthEffect, healthEffect.GetComponent<ParticleSystem>().main.duration);
            Destroy(gameObject);
        }
    }

}
