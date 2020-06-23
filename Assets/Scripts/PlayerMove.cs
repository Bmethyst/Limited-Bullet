using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        //jump
        if (Input.GetButtonDown("Jump"))
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);


        // Stop Speed
        if (Input.GetButtonUp("Horizontal")) {
            //rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,
            //    rigid.velocity.y);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        if (Mathf.Abs(rigid.velocity.normalized.x) < 0.3f)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);


    }

    // Start is called before the first frame update
    void FixedUpdate() {

        //move horizontal
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * 3, ForceMode2D.Impulse);

        //최대 속력 제한
        if (rigid.velocity.x > maxSpeed) {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
		else if (rigid.velocity.x < maxSpeed * (-1)) {
			rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
		}
	}
}
