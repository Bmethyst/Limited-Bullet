using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {

	public float health;
	public GameObject deathEffect;
	Animator anim;
	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Start() {

	}
	private void Awake() {
		rigid = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

	}
	// Update is called once per frame
	void Update() {
		if (health <= 0) {
			if (deathEffect != null)
				Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	public void TakeDamage(float damage) {
		anim.SetTrigger("Damaged");
		enemyStop();
		Invoke("enemyStop", 0.5f);

		health -= damage;
	}
	void enemyStop() {
		GetComponent<EnemyMove>().StopTrigger();
	}
}
