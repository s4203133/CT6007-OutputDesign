using UnityEngine;

public class CastleWeapon : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private LayerMask detectableLayers;
    [SerializeField] private float attackRate;
    private float timer;

    [Space(10)]
    [SerializeField] private CannonBall projectilePrefab;
    [SerializeField] private Transform projectileSpawnpoint;
    private Transform target;

    private Transform thisTransform;
    private Animator animator;

    private void Start() {
        thisTransform = transform;
        //animator = GetComponent<Animator>();
        timer = 0;
    }

    private void Update() {
        Collider[] nearbyTargets = (Physics.OverlapSphere(thisTransform.position, range, detectableLayers));
        if(nearbyTargets.Length > 0 ) {
            GameObject closestTarget = GetClosestTarget(nearbyTargets);
            if (closestTarget != null) {
                target = closestTarget.transform;
                HandleAttack();
            }
        }
    }

    private void HandleAttack() {
        Countdown();
    }

    private GameObject GetClosestTarget(Collider[] targets) {
        GameObject bestTarget = null;
        float minDistance = range;
        for(int i = 0; i < targets.Length; i++) {
            if (Vector3.Dot((targets[i].transform.position - thisTransform.position), thisTransform.forward) > 0.1f) {
                float distance = Vector3.Distance(targets[i].transform.position, thisTransform.position);
                if (distance < minDistance) {
                    bestTarget = targets[i].gameObject;
                    minDistance = distance;
                }
            }
        }
        return bestTarget;
    }

    private void Countdown() {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Fire();
            timer = attackRate;
        }
    }

    private void Fire() {
        //animator.SetTrigger("Attack");
        CannonBall projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, thisTransform.rotation);
        projectile.SetTarget(target, this.gameObject);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(thisTransform.position, range);
    }
}
