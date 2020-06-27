using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rigid;
    Vector3 moveVelocity;
    RaycastHit2D ray;
    
    public Transform pos;
    public Vector2 hitBoxSize;
    public GameObject FireBall;
    public float maxSpeed;
    public int movementflag = 0;
    public GameObject deathEffect;
    public GameManager gameManager;

    // 0: Idle / 1: towardLeftRush / 2: towardRightRush / 3: 제자리 공격
    // 4: RapidRush L / 5: RapidRush R 
    public float health;
    float movespeed = 0;
    public int bossPhase = 1;
    float AtkStopTime; // 한번 제자리에서 공격하면 이 숫자동안 못움직임
    float AnimTime = 1.2f;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();

        StartCoroutine("ChangeMovement");
        StartCoroutine("ShootFireBall");
    }

    void Update() {
        if (health <= 0) {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Move();
        CoolDown();
    }
    public void Move() {
        moveVelocity = Vector3.zero;
        if (AtkStopTime < AnimTime)
            return;

        switch (movementflag) {
            case 0:     //완전 정지
                moveVelocity = Vector3.zero;
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                //idle 애니메이션
                break;
            case 1:     //왼쪽으로 걸어감
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(1, 1, 1);
                movespeed = 1f;
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                //walk 애니메이션
                break;
            case 2:     //오른쪽으로 걸어감
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(-1, 1, 1);
                movespeed = 1f;
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                //walk 애니메이션
                break;
            case 3:     //제자리에서 공격
                moveVelocity = Vector3.zero;
                //공격 애니메이션
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", true);
                Invoke("biteSmash", 0.5f);
                AtkStopTime = 0;
                break;
            case 4:     //왼쪽으로 뛰어감
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(1, 1, 1);
                movespeed = 1.7f;
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                break;
            case 5:     //오른쪽으로 뛰어감
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(-1, 1, 1);
                movespeed = 1.7f;
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);
                break;
            default:
                Debug.Log("에러 발생: 보스 움직임 에러");
                break;
        }

        
        
        rigid.AddForce(moveVelocity * movespeed * Time.deltaTime * 50, ForceMode2D.Impulse);
		if (rigid.velocity.x > maxSpeed) {
			rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
		} else if (rigid.velocity.x < (-1) * maxSpeed) {
			rigid.velocity = new Vector2((-1) * maxSpeed, rigid.velocity.y);
        }

        Vector2 ray_pos = new Vector2(transform.position.x, transform.position.y-2.5f);
        Debug.DrawRay(ray_pos, Vector2.left, new Color(1, 1, 0));
        Debug.DrawRay(ray_pos, Vector2.right, new Color(0, 1, 1));
        if(transform.localScale.x == 1)
            ray = Physics2D.Raycast(ray_pos, Vector2.left, 2f, LayerMask.GetMask("Platform"));
        else
            ray = Physics2D.Raycast(ray_pos, Vector2.right, 2f, LayerMask.GetMask("Platform"));

        if (ray.collider != null) {
            if(ray.distance < 1.5f) {
                if (movementflag == 1) movementflag = 2;
                else if (movementflag == 2) movementflag = 1;
                else if (movementflag == 4) movementflag = 5;
                else if (movementflag == 5) movementflag = 4;
                else if (movementflag == 3 && transform.localScale.x == 1) movementflag = 2;
                else if (movementflag == 3 && transform.localScale.x != 1) movementflag = 1;
            }
        }

    }

    void CoolDown() {
        AtkStopTime += Time.deltaTime;
    }

    IEnumerator ChangeMovement() {
        if (bossPhase != 3)
            movementflag = Random.Range(0, 4); //1~2페이즈는 패턴 4개
        else
            movementflag = Random.Range(0, 6); //3페이즈는 패턴 6개

        yield return new WaitForSeconds(Random.Range(3f, 5f));

        StartCoroutine("ChangeMovement");
    }

	private void OnDrawGizmos() {// 물리공격 범위 보여줌(디버그 전용)
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(pos.position, hitBoxSize);
	}

    void biteSmash() {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, hitBoxSize, 0);
        foreach (Collider2D collider in collider2Ds) {
            if (collider.gameObject.layer.Equals(9)) {
                collider.GetComponent<PlayerMove>().OnDamaged(transform.position);
            }
        }
    }

    IEnumerator ShootFireBall() {
        if (moveVelocity != Vector3.zero) { // 0.5초마다 탄성을 가진 화염구 던지기. 방향은 진행방향 반대 사선 위로
            GameObject fireball = Instantiate(FireBall, pos.position, Quaternion.Euler(0f, 0f, 120f));
            Rigidbody2D rigid = fireball.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * 13, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.3f);
        StartCoroutine("ShootFireBall");
    }

    public void Damaged(float damage) {
        health -= damage;
        gameManager.bossDamage(health);
    }
}
