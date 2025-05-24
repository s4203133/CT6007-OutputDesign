using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask hittableLayers;

    private float destroyTimer;

    private Transform thisTransform;
    private Transform targetTransform;
    public GameObject attacker { get; private set; }

    private void Start() {
        CastleTarget.OnSimulationEnded += DestroyProjectile;
    }

    public void SetTarget(Transform tartget, GameObject attacker) {
        targetTransform = tartget;
        thisTransform = transform;
        destroyTimer = 1.5f;
        this.attacker = attacker;
    }

    private void Update() {
        // If the projectile has a target, move towards it
        if (targetTransform != null)
        {
            thisTransform.LookAt(targetTransform, Vector3.up);
            thisTransform.Translate(thisTransform.forward * speed * Time.deltaTime, Space.World);
        }
        // If the projectile has not got a target, move in the forward direction but despawn after a short time
        else {
            thisTransform.Translate(thisTransform.forward * speed * Time.deltaTime, Space.World);
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        // If the projectile hits the target layer, deal damage to the object and destroy the game object
        if(hittableLayers == (hittableLayers | (1 << other.gameObject.layer))) {
            other.gameObject.GetComponent<TileHealth>().DealDamage(5, attacker);
            DestroyProjectile();
        }
    }

    private void DestroyProjectile() {
        if (this != null) {
            Destroy(gameObject);
        }
    }
}
