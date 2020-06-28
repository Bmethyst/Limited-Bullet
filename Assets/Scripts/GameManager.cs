using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public float Health;
    public float Mana;
    public bool isescTriggered;
    public GameObject player;
    public bool bossEntered;
    
    public GameObject BosssBar;
    public Image BossHPImage;
    public Image[] PlayerStatusPoint;
    public GameObject clear;

    public GameObject UIExitBtn;
    CapsuleCollider2D capsuleCollider;
    PolygonCollider2D polygonCollider;
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;
    private CameraControl  u_camera;

    AudioSource audioSource;
    public AudioClip audioBoss;
    public AudioClip audioClear;


    private void Awake() {
        Screen.SetResolution(1366, 768, false);
        capsuleCollider = player.GetComponent<CapsuleCollider2D>();
        polygonCollider = player.GetComponent<PolygonCollider2D>();
        circleCollider = player.GetComponent<CircleCollider2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        u_camera = GameObject.Find("Main Camera").GetComponent< CameraControl>();
        isescTriggered = false;
        BossHPImage.rectTransform.localScale = new Vector2(0.0001f, 3f);
        bossEntered = false;
        audioSource = GetComponent<AudioSource>();

    }
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isescTriggered) {
                isescTriggered = false;
                UIExitBtn.SetActive(false);
                Time.timeScale = 1;
            }
            else {
                isescTriggered = true;
                UIExitBtn.SetActive(true);
                Time.timeScale = 0;
            }
        }

        if (player.transform.position.x > 170 && player.transform.position.y < -25 && !bossEntered) { // when player enter to boss zone transform
            BosssBar.SetActive(true);
            BossHPImage.rectTransform.localScale = new Vector2(3f, 3f);
            bossEntered = true;

            //bgm 변경
			
            audioSource.clip = audioBoss;
            audioSource.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            HealthDown();
            if (Health<=0) {
                UIExitBtn.SetActive(true);
                Time.timeScale = 0;

                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.transform.position = new Vector3(4, 0, -3);
                Health = 10;
                spriteRenderer.color = new Color(1, 1, 1, 1);

                spriteRenderer.flipY = false;

                capsuleCollider.enabled = true;
                polygonCollider.enabled = true;
                circleCollider.enabled = false;

            }
            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector3(4, 0, -3);

            u_camera.ResetCamera();
        }
    }

    public void HealthDown() {
		if (Health > 1) {
			Health--;
            PlayerStatusPoint[0].rectTransform.localScale = new Vector2(Health / 10f * 1.3f, 3);

        }


		else {
            //player Die Effect
            Health--;
            PlayerStatusPoint[0].rectTransform.localScale = new Vector2(Health / 10f * 1.3f, 3);
            player.GetComponent<PlayerMove>().OnDie();
        }
    }

    public void ManaDown() {
        Mana -= 1;
        PlayerStatusPoint[1].rectTransform.localScale = new Vector2(Mana / 10f * 1.3f, 3);
    }
    public void HealthUp() {
        Health += 2.5f;
        if (Health > 10) Health = 10;
        PlayerStatusPoint[0].rectTransform.localScale = new Vector2(Health / 10f * 1.3f, 3);
        PlayerStatusPoint[0].rectTransform.localScale = new Vector2(Health / 10f * 1.3f, 3);

    }

    public void ManaUp() {
        Mana += 4;
        if (Mana > 10) Mana = 10;
        PlayerStatusPoint[1].rectTransform.localScale = new Vector2(Mana / 10f * 1.3f, 3);
    }


    public void exitgame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void bossDamage(float health) { // 18f는 보스 체력 바꾸면 바꿀 것
        BossHPImage.rectTransform.localScale = new Vector2(health / 18f * 3f, 3);
        if (health <= 0) {
            BossHPImage.rectTransform.localScale = new Vector2(0, 3);

            BosssBar.SetActive(false);
            clear.SetActive(true);

            audioSource.clip = audioClear;
            audioSource.Play();
			audioSource.loop=false;
            //승리 bgm 재생
        }
    }
}
