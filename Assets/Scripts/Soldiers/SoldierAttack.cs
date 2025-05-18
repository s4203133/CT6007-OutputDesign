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
    private int tilesBorken;

    private bool attacking;
    public bool IsAttacking => attacking;
    public bool BrokenEnoughTiles => tilesBorken >= 3;

    public void Initialise(Soldier soldier) {
        this.soldier = soldier;
        rate += UnityEngine.Random.Range(-0.15f, 0.15f);
        TileHealth.OnTileDestroyed += NotifyTileDestroyed;
    }

    public void Countdown()
    {
        if (attacking) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                Attack();
                timer = rate;
            }
        }
    }

    public void Begin() {
        if (!attacking) {
            attacking = true;
            timer = rate;
        }
    }

    public void Stop() {
        if (attacking) {
            attacking = false;
            target = null;
            tilesBorken++;
        }
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }

    private void Attack() {
        CannonBall newProjectile = soldier.CreateAttackProjectile(projectilePrefab);
        newProjectile.SetTarget(target, soldier.gameObject);
    }

    private void NotifyTileDestroyed(GameObject attacker) {
        //if(attacker == soldier.gameObject) {
        //    Stop();
        //    tilesBorken++;
        //}
    }
}
