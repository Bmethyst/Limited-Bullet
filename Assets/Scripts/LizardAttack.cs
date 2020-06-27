using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardAttack : MonoBehaviour
{
    Animator anim;
    GameObject traceTarget;
    GameObject Player;
    RaycastHit2D ray1;
    RaycastHit2D ray2;
    RaycastHit2D hitRay;

    public Transform pos;
    public Vector2 boxSize;

    
    public float atkCoolTime; // 물리 공격 쿨타임
    public float curAtkCoolTime; // 이전 공격 간 간격

    void Start()
    {
        
    }
    private void Awake() {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player");

    }
    void Update()
    {
        Vector2 ray_pos = transform.position;
        Vector2 ray1_direction = transform.TransformDirection(1, 0, 0);
        Vector2 ray2_direction = transform.TransformDirection(-1, 0, 0);
        Debug.DrawRay(ray_pos, ray1_direction, new Color(0, 0, 1));
        Debug.DrawRay(ray_pos, ray2_direction, new Color(1, 0, 1));
        ray1 = Physics2D.Raycast(ray_pos, ray1_direction, 6f, LayerMask.GetMask("Player"));
        ray2 = Physics2D.Raycast(ray_pos, ray2_direction, 6f, LayerMask.GetMask("Player"));
        if (ray1.collider!=null || ray2.collider != null) { 
            if (ray1.distance < 5 || ray2.distance < 5) {
                GetComponent<EnemyMove>().isTracing = true;
            }
        }
        else GetComponent<EnemyMove>().isTracing = false;

        Attack();
        CoolDown();
    }

    void Attack() {
        Vector2 ray_pos = transform.position;
        int tracingDir = Player.GetComponent<Transform>().position.x > transform.position.x ? 1 : -1;

        Vector2 ray_direction = transform.TransformDirection((-1)*transform.localScale.x, 0, 0);
        Debug.DrawRay(ray_pos, ray_direction, new Color(0, 1, 1));
        hitRay = Physics2D.Raycast(ray_pos, ray_direction, 1.5f, LayerMask.GetMask("Player"));

        if(hitRay.collider != null) {
            if (curAtkCoolTime < atkCoolTime)
                return;

            anim.SetTrigger("Attack");
            GetComponent<EnemyMove>().StopTrigger();
            Invoke("attackDelayed", 0.2f);
            Invoke("delayedHitmotionOff", 0.7f);
            curAtkCoolTime = 0;
        }
    }
    void CoolDown() { // 쿨타임 재는 함수
        curAtkCoolTime += Time.deltaTime;
    }

    private void OnDrawGizmos() // 물리공격 범위 보여줌(디버그 전용)
{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            traceTarget = collision.gameObject;
            GetComponent<EnemyMove>().isTracing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GetComponent<EnemyMove>().isTracing = false;
            traceTarget = null;
        }

    }

    private void attackDelayed() {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds) {
            if (collider.gameObject.layer.Equals(9)) {
                collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
            }
        }
    }
    private void delayedHitmotionOff() {
        GetComponent<EnemyMove>().StopTrigger();
    }
}
