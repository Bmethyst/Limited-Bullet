using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;


    public GameObject destroyEffect; // 발사체가 사라질 때 효과
    private void Start() {
        Invoke("DestroyProjectile", lifeTime);
    }
    void Update() {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up);

        if (hitInfo.collider != null) {
            if (hitInfo.collider.CompareTag("Enemy")) {
                hitInfo.collider.GetComponent<EnemyDamage>().TakeDamage(damage);
                DestroyProjectile();
            }
            else if (hitInfo.collider.CompareTag("Platform")) {
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile() {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
