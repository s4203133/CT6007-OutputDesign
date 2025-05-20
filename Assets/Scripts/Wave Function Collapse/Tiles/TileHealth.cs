using System;
using UnityEngine;

public class TileHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private LayerMask castleTileLayers;
    private int health;

    GameObject attacker;
    public static Action<GameObject> OnTileDestroyed;

    private void Start() {
        castleTileLayers = 0;
        castleTileLayers = 9;
        health = maxHealth;
    }

    public void DealDamage(int damage, GameObject attacker) {
        health -= damage;
        if(health <= 0) {
            DestroyTile();
        }
    }

    private void DestroyTile() {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.up, 9, castleTileLayers);
        if (hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                if (hits[i].transform != transform) {
                    Destroy(hits[i].transform.gameObject);
                }
            }
        }
        OnTileDestroyed?.Invoke(attacker);
        Destroy(gameObject);
    }
}
