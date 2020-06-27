using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMove : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;



    public GameObject destroyEffect; // 발사체가 사라질 때 효과

    private void Awake() {
    }
    private void Start() {
        Invoke("DestroyProjectile", lifeTime);
    }
    void Update() {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, 0.5f);

        if (hitInfo.collider != null) {
            if (!tag.Equals("Web")) {
                if (hitInfo.collider.gameObject.layer.Equals(8)) {
                    Debug.Log("바닥 맞았음");
                    DestroyProjectile();
                }
                else if (hitInfo.collider.gameObject.layer.Equals(9)) {
                    hitInfo.collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
                    DestroyProjectile();

                }
            }
            else if (hitInfo.collider.gameObject.layer.Equals(9)) {
                hitInfo.collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
                DestroyProjectile();

            }
        }
    //    RaycastHit2D hitInfoPlatform = Physics2D.Raycast(transform.position, transform.up, 0.5f, 8);
    //    RaycastHit2D hitInfoPlayer = Physics2D.Raycast(transform.position, transform.up, 0.5f, 9);
    //    RaycastHit2D hitInfoEnemy = Physics2D.Raycast(transform.position, transform.up, 0.5f, 10);
    //    Debug.Log(hitInfoPlatform);
    //    Debug.Log(hitInfoPlayer);

    //    if (!tag.Equals("Web")) {
				//if (hitInfoPlatform.collider != null) {
				//	Debug.Log(hitInfoPlatform);
				//	Debug.Log(hitInfoPlayer);
				//	DestroyProjectile();
				//}
				//else if (hitInfoPlayer.collider != null) {
    //                hitInfoPlayer.collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
    //                DestroyProjectile();

    //            }
    //        }
    //        else if(hitInfoPlayer.collider != null) {
    //            hitInfoPlayer.collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
    //            DestroyProjectile();
    //        }
    }
    void DestroyProjectile() {
        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
