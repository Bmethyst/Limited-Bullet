using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldAttack : MonoBehaviour {
    Animator anim;
    GameObject traceTarget;
    GameObject Player;
    RaycastHit2D ray1;
    RaycastHit2D ray2;
    RaycastHit2D hitRay;

    public Transform pos;
    public Vector2 boxSize;
    public GameObject Arrow;
    public Transform shotPoint;

    public float atkCoolTime; // 물리 공격 쿨타임
    public float curAtkCoolTime; // 이전 공격 간 간격
    
    
    float rotateDegree;
    private float dy;
    private float dx;
    
    float shotPower; // 마법공격의 발사 속도

    void Start()
    {
        
    }
    private void Awake() {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Player");

    }
    void Update()
    {
        Vector3 playerPos = Player.GetComponent<Transform>().position;
        Vector3 objPos = transform.GetChild(0).position;

        playerPos.z = objPos.z - Camera.main.transform.position.z;


        dy = playerPos.y - objPos.y;
        dx = playerPos.x - objPos.x;
        rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        if (transform.localScale.x == -1)
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, rotateDegree + 180);
        else
            transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, rotateDegree);




        Vector2 ray_pos = transform.position;
        Vector2 ray1_direction = transform.TransformDirection(1, 0, 0);
        Vector2 ray2_direction = transform.TransformDirection(-1, 0, 0);
        Debug.DrawRay(ray_pos, ray1_direction, new Color(0, 0, 1));
        Debug.DrawRay(ray_pos, ray2_direction, new Color(1, 0, 1));
        ray1 = Physics2D.Raycast(ray_pos, ray1_direction, 15, LayerMask.GetMask("Player"));
        ray2 = Physics2D.Raycast(ray_pos, ray2_direction, 15, LayerMask.GetMask("Player"));
        if (ray1.collider != null || ray2.collider != null) {
            if (ray1.distance <= 15 || ray2.distance <= 15) {
                shotPower = (ray1.distance > ray2.distance ? ray1.distance : ray2.distance)*0.5f;
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

        Vector2 ray_direction = transform.TransformDirection((-1) * transform.localScale.x, 0, 0);
        Debug.DrawRay(ray_pos, ray_direction, new Color(0, 1, 1));
        hitRay = Physics2D.Raycast(ray_pos, ray_direction, 15f, LayerMask.GetMask("Player"));

        if (hitRay.collider != null) {
            if (curAtkCoolTime < atkCoolTime)
                return;

            anim.SetTrigger("Attack");
            GetComponent<EnemyMove>().StopTrigger();
            Invoke("attackDelayed", 0.4f);
            Invoke("delayedHitmotionOff", 1f);
            curAtkCoolTime = 0;
        }
    }

    void CoolDown() { // 쿨타임 재는 함수
        curAtkCoolTime += Time.deltaTime;
    }

    private void attackDelayed() {
        float degree = 0f;
        if (transform.localScale.x == -1) degree = 180f;
        GameObject arrow = Instantiate(Arrow, shotPoint.position, Quaternion.Euler(0f, 0f, degree));
        Rigidbody2D rigid = arrow.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(dx / (Mathf.Abs(dx) + Mathf.Abs(dy)) * shotPower, Mathf.Abs(dy) / (Mathf.Abs(dx) + Mathf.Abs(dy)) * shotPower*3), ForceMode2D.Impulse);

    }
    private void delayedHitmotionOff() {
        GetComponent<EnemyMove>().StopTrigger();
    }


}
