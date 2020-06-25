using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
	Rigidbody2D rigid;
	Animator anim;
	SpriteRenderer spriteRenderer;
	public int nextMove;
	public float customMoveSpeed;
	// Start is called before the first frame update

	void Awake() {
		rigid = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		Invoke("Think", 5);
	}

	// Update is called once per frame

	void FixedUpdate() {
		rigid.velocity = new Vector2(nextMove * customMoveSpeed, rigid.velocity.y);
		Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
		Debug.DrawRay(frontVec, Vector3.down, new Color(0, 0.7f, 0.3f));
		RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 5f, LayerMask.GetMask("Platform"));
		if (rayHit.collider == null) {
			Turn();
		}
	}

	void Think() {
		nextMove = Random.Range(-1, 2);

		anim.SetInteger("WalkSpeed", nextMove);
		if (nextMove != 0) spriteRenderer.flipX = nextMove == 1;

		float nextThinkTime = Random.Range(2f, 5f);
		Invoke("Think", nextThinkTime);

	}

	void Turn() {
		nextMove *= -1;
		spriteRenderer.flipX = nextMove == 1;
		CancelInvoke();
		Invoke("Think", 3);

	}
}
