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
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) { 
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }


        // Stop Speed
        if (Input.GetButtonUp("Horizontal")) {
            //rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f,
            //    rigid.velocity.y);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        //if (Input.GetButton("Horizontal")) {
        //    //spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        //    if (Input.GetAxisRaw("Horizontal") == -1)
        //        transform.localScale.Set(-1, 1, 1);
        //}

        if (Mathf.Abs(rigid.velocity.normalized.x) < 0.3f)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);


    }

    // Start is called before the first frame update
    void FixedUpdate() {

        //move horizontal
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h * 30, ForceMode2D.Impulse);

		if (h < 0) transform.localScale = new Vector3(-1, 1, 1);
		else if (h > 0) transform.localScale = new Vector3(1, 1, 1);

		//최대 속력 제한
		if (rigid.velocity.x > maxSpeed) {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
		else if (rigid.velocity.x < maxSpeed * (-1)) {
			rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
		}
        if (rigid.velocity.y < 0) {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));
            //Debug.Log(rayHit.distance);
            if (rayHit.collider != null) {
                if (rayHit.distance < 1)
                    anim.SetBool("isJumping", false);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            OnDamaged(collision.transform.position);
        }
    }
    
    void OnDamaged(Vector2 targetPos) {
        gameObject.layer = 13;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("Damaged");

        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged() {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

}
