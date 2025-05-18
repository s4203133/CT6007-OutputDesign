using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask hittableLayers;

    private float destroyTimer;

    private Transform thisTransform;
    private Transform targetTransform;
    public GameObject attacker { get; private set; }    

    public void SetTarget(Transform tartget, GameObject attacker) {
        targetTransform = tartget;
        thisTransform = transform;
        destroyTimer = 1;
        this.attacker = attacker;
    }

    private void Update() {
        if (targetTransform != null)
        {
            thisTransform.LookAt(targetTransform, Vector3.up);
            thisTransform.Translate(thisTransform.forward * speed * Time.deltaTime, Space.World);
        }
        else {
            thisTransform.Translate(thisTransform.forward * speed * Time.deltaTime, Space.World);
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0) {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(hittableLayers == (hittableLayers | (1 << other.gameObject.layer))) {
            other.gameObject.GetComponent<TileHealth>().DealDamage(5, attacker);
            Destroy(gameObject);
        }
    }
}
