using System;
using UnityEngine;

[Serializable]
public class SoldierAttack
{
    private Transform target;
    private Soldier soldier;

    [SerializeField] private CannonBall projectilePrefab;
    [SerializeField] private float rate;
    private float timer;
    private int tilesBroken;

    private bool attacking;
    public bool IsAttacking => attacking;
    public bool BrokenEnoughTiles => tilesBroken >= 3;

    public void Initialise(Soldier soldier) {
        this.soldier = soldier;
        rate += UnityEngine.Random.Range(-0.15f, 0.15f);
    }

    public void Countdown()
    {
        // While the soldier is attacking, countdown the timer+
        if (attacking) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                // When the timer has reached 0, fire a projectile and reset the timer
                Attack();
                timer = rate;
            }
        }
    }

    public void Begin() {
        // Initialise attacking and timer
        if (!attacking) {
            attacking = true;
            timer = rate;
        }
    }

    public void Stop() {
        // End attacking and register that the target tile has been broken
        if (attacking) {
            attacking = false;
            target = null;
            tilesBroken++;
        }
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }

    private void Attack() {
        // Instantiate a new projectile and fire it towards the target game object
        CannonBall newProjectile = soldier.CreateAttackProjectile(projectilePrefab);
        newProjectile.SetTarget(target, soldier.gameObject);
    }
}
