using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
	Rigidbody2D rigid;
	Animator anim;
	SpriteRenderer spriteRenderer;
	public int nextMove;
	public float customMoveSpeed;
	public float RushMoveSpeed;
	public bool isStop;
	public bool isTracing;
	public int enemyType;
	GameObject PlayerPos;
	// Start is called before the first frame update

	void Awake() {
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		PlayerPos = GameObject.Find("Player");

		isStop = false;
		if (enemyType == 1 || enemyType == 2) Invoke("Think", 5);
	}

	// Update is called once per frame

	void FixedUpdate() {
		if (enemyType == 1) { // 리자드 움직임
			if (isStop) {
				rigid.velocity = new Vector2(0, rigid.velocity.y);
				nextMove = 0;
			}
			else {
				if (isTracing) {
					nextMove = PlayerPos.GetComponent<Transform>().position.x > transform.position.x ? 1 : -1;
					rigid.velocity = new Vector2(nextMove * RushMoveSpeed, rigid.velocity.y);
				}
				else {
					rigid.velocity = new Vector2(nextMove * customMoveSpeed, rigid.velocity.y);
				}
			}
			//anim.SetInteger("WalkDir", nextMove);
			if (nextMove != 0) {
				transform.localScale = new Vector3(nextMove == 1 ? -1 : 1, 1, 1);
				anim.SetInteger("WalkDir", nextMove);
			}


			Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
			Debug.DrawRay(frontVec, Vector3.down, new Color(0, 0.7f, 0.3f));
			RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2.5f, LayerMask.GetMask("Platform"));
			if (rayHit.collider == null) {
				Turn();
			}
		}
		else if (enemyType == 2) { // 코볼드 움직임
			if (isStop) {
				rigid.velocity = new Vector2(0, rigid.velocity.y);
				nextMove = 0;
			}
			if (isTracing) {
				nextMove = PlayerPos.GetComponent<Transform>().position.x > transform.position.x ? 1 : -1;
				rigid.velocity = new Vector2(nextMove * 0, rigid.velocity.y);
			}
			else {
				rigid.velocity = new Vector2(nextMove * customMoveSpeed, rigid.velocity.y);
			}

			//anim.SetInteger("WalkDir", nextMove);
			if (nextMove != 0) {
				transform.localScale = new Vector3(nextMove == 1 ? -1 : 1, 1, 1);
			}

			anim.SetInteger("WalkDir", nextMove);

			Vector2 front2Vec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
			Debug.DrawRay(front2Vec, Vector3.down, new Color(0, 0.7f, 0.3f));
			RaycastHit2D rayHit = Physics2D.Raycast(front2Vec, Vector3.down, 5f, LayerMask.GetMask("Platform"));
			if (rayHit.collider == null) {
				Turn();
			}
		}
		else if (enemyType == 3) { // 거미 움직임 (좌우 방향전환만 함)
			if (isStop) {
				rigid.velocity = new Vector2(0, rigid.velocity.y);
				nextMove = 0;
			}
			if (isTracing) {
				nextMove = PlayerPos.GetComponent<Transform>().position.x > transform.position.x ? 1 : -1;
				rigid.velocity = new Vector2(nextMove * 0, rigid.velocity.y);
			}
			else {
				rigid.velocity = new Vector2(nextMove * 0, rigid.velocity.y);
			}

			//anim.SetInteger("WalkDir", nextMove);
			if (nextMove != 0) {
				transform.localScale = new Vector3(nextMove == 1 ? -1 : 1, 1, 1);
			}

			//anim.SetInteger("WalkDir", nextMove);

			//Vector2 front2Vec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
			//Debug.DrawRay(front2Vec, Vector3.down, new Color(0, 0.7f, 0.3f));
			//RaycastHit2D rayHit = Physics2D.Raycast(front2Vec, Vector3.down, 5f, LayerMask.GetMask("Platform"));
			//if (rayHit.collider == null) {
			//	Turn();
			//}
		}

	}


	void Think() {
		nextMove = Random.Range(-1, 2);

		//anim.SetInteger("WalkDir", nextMove);
		//if (nextMove != 0) spriteRenderer.flipX = nextMove == 1;

		float nextThinkTime = Random.Range(2f, 5f);
		Invoke("Think", nextThinkTime);

	}

	void Turn() {
		nextMove *= -1;
		//spriteRenderer.flipX = nextMove == 1;
		CancelInvoke();
		Invoke("Think", 3);
	}

	public void StopTrigger() {
		isStop = isStop ? false : true;
	}

	public void TracingTrigger() {
		isTracing = isTracing ? false : true;

	}

}
