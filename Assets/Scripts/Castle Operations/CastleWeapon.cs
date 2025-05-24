using UnityEngine;

public class CastleWeapon : MonoBehaviour
{
    private Transform thisTransform;

    [SerializeField] private float range;
    [SerializeField] private LayerMask detectableLayers;
    [SerializeField] private float attackRate;
    private float timer;

    [Space(10)]
    [SerializeField] private CannonBall projectilePrefab;
    [SerializeField] private Transform projectileSpawnpoint;
    [SerializeField] private Transform firingObject;
    private Transform target;

    private void Start() {
        thisTransform = transform;
        timer = 0;
    }

    private void Update() {
        CheckForNearbyObjects();
    }

    private void CheckForNearbyObjects() {
        // Check for any objects within range that are on the target layer
        Collider[] nearbyTargets = (Physics.OverlapSphere(thisTransform.position, range, detectableLayers));
        if(nearbyTargets.Length == 1) {
            // If there is only 1 nearby object, set that to be the target
            target = nearbyTargets[0].transform;
            HandleAttack();
        }
        else if (nearbyTargets.Length > 1) {
            // If there is more than 1 nearby object, calculate the closest one and set that to be the target
            GameObject closestTarget = GetClosestTarget(nearbyTargets);
            if (closestTarget != null) {
                target = closestTarget.transform;
                HandleAttack();
            }
        }
    }

    private void HandleAttack() {
        // Look towards the target object and countdown the firing timer
        Vector3 lookPosition = new Vector3(target.transform.position.x, firingObject.position.y, target.transform.position.z);
        firingObject.LookAt(lookPosition);
        Countdown();
    }

    private GameObject GetClosestTarget(Collider[] targets) {
        // Out of all the found targets, check the distance to determine which one is closest to this game object
        GameObject bestTarget = null;
        float minDistance = range;
        for(int i = 0; i < targets.Length; i++) {
            float distance = Vector3.Distance(targets[i].transform.position, thisTransform.position);
            if (distance < minDistance) {
                bestTarget = targets[i].gameObject;
                minDistance = distance;
            }
        }
        return bestTarget;
    }

    private void Countdown() {
        // Countdown the timer, then reset it and fire a projectile every time it reaches 0
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Fire();
            timer = attackRate;
        }
    }

    private void Fire() {
        CannonBall projectile = Instantiate(projectilePrefab, projectileSpawnpoint.position, thisTransform.rotation);
        projectile.SetTarget(target, this.gameObject);
    }

    private void OnDrawGizmosSelected() {
        // Visualise the range of the tile while it is selected
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(thisTransform.position, range);
    }
}
