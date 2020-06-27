using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour

{
    public Transform player;
    private Transform cameraTransform;
    private float originalY;
    Animator playerAnim;
    Rigidbody2D playerRigid;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
        cameraTransform.position = new Vector3(player.position.x, player.position.y + 1.2f, cameraTransform.position.z);

    }

    private void Awake() {
        playerAnim = GameObject.Find("Player").GetComponent<Animator>();
        playerRigid = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer() {
        originalY = this.transform.position.y;

        if (cameraTransform.position.y - player.position.y > 1.2f) { //점프를 하던 말던 캐릭터가 카메라에서 너무 아래에 위치해있으면 카메라를 내림
			cameraTransform.position = new Vector3(player.position.x, player.position.y + 1.2f, cameraTransform.position.z);
			return;
		}
        if (player.position.y - cameraTransform.position.y > 1.2f) { //점프를 하던 말던 캐릭터가 카메라에서 너무 위에 위치해있으면 카메라를 올림
            cameraTransform.position = new Vector3(player.position.x, originalY + 1.2f*Time.deltaTime, cameraTransform.position.z);
            return;
        }


        if (playerAnim.GetBool("isJumping")) { //뛰는중에는 카메라 y축 고정
            cameraTransform.position = new Vector3(player.position.x, originalY, cameraTransform.position.z);
            return;
        } 
        else { //not jumping
            if ( cameraTransform.position.y - player.position.y < 1.2f) { //뛰어서 착지했을때 플레이어가 너무 높이있으면 카메라를 올림
                cameraTransform.position = new Vector3(player.position.x, originalY + 1.2f*Time.deltaTime, cameraTransform.position.z);
                return;
            }
            else { 
                    cameraTransform.position = new Vector3(player.position.x, originalY, cameraTransform.position.z);
                    return;
            }
        }

    }

    public void ResetCamera() {
        cameraTransform.position = new Vector3(4, 2, cameraTransform.position.z);
    }
}
