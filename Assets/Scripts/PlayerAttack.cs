using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator anim;
    GameObject Player;

    public GameObject Bullet;
    public Transform shotPoint;
    public Transform pos;
    public Vector2 boxSize;

    public float shotCoolTime; // 마법 공격 쿨타임
    public float atkCoolTime; // 물리 공격 쿨타임
    float curAtkCoolTime; // 이전 공격 간 간격
    float rotateDegree;
    private float dy;
    private float dx;
   
    public float shotPower; // 마법공격의 발사 속도
    public float atkDamage; // 물리 공격의 데미지
    private void Awake() {
        Player = GameObject.Find("Player");
        anim = Player.GetComponent<Animator>();

    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objPos = transform.position;

        mousePos.z = objPos.z - Camera.main.transform.position.z;

        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);


        dy = target.y - objPos.y;
        dx = target.x - objPos.x;
        rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        if (Player.transform.localScale.x == -1)
			transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree + 180);
		else
			transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree);

        Attack();
        CoolDown();
    }

    void Attack()
    {
        Fire();
        Smash();
    }

    void Smash() //물리공격
    {

        if (!Input.GetMouseButton(0))
            return;
        if (curAtkCoolTime < atkCoolTime)
            return;
        anim.SetTrigger("PhysicAttack");//PhysicAttack 모션 추가하고 바꿀 것
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer.Equals(10)) {
                collider.GetComponent<EnemyDamage>().TakeDamage(atkDamage);
            }
        }

        curAtkCoolTime = 0;

    }

    void Fire() { //마법공격
        if (!Input.GetMouseButton(1))
            return;
        if (curAtkCoolTime < shotCoolTime)
            return;
        if ((Player.GetComponent<Transform>().localScale.x == -1 && dx > 0) ||
            (Player.GetComponent<Transform>().localScale.x == 1 && dx < 0))
            return;
	    anim.SetTrigger("MagicAttack");
		GameObject bullet = Instantiate(Bullet, shotPoint.position, Quaternion.Euler(0f, 0f, rotateDegree / 2));
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(dx / (Mathf.Abs(dx) + Mathf.Abs(dy)) * shotPower, dy / (Mathf.Abs(dx) + Mathf.Abs(dy)) * shotPower), ForceMode2D.Impulse);
        curAtkCoolTime = 0;
    }

    void CoolDown() { // 쿨타임 재는 함수
        curAtkCoolTime += Time.deltaTime;
    }

    private void OnDrawGizmos() // 물리공격 범위 보여줌(디버그 전용)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }
}
