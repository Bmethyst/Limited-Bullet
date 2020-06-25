using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float offset;

    public GameObject Bullet;
    GameObject Player;
    public Transform shotPoint;

    public float maxShotDelay; // 총알을 쏜 뒤 시작되는 타이머
    float curShotDelay; // 총알을 쏘는 간격
    float rotateDegree;
    private float dy;
    private float dx;
    public float shotPower;
    private void Awake() {
        Player = GameObject.Find("Player");
    }
    // Update is called once per frame
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

        //if (timeBulletShots <= 0) {
        //    if (Input.GetMouseButton(1)) {
        //        Instantiate(Bullet, shotPoint.position, transform.rotation);
        //        timeBulletShots = startTimeBulletShots;
        //    }
        //    else {
        //        timeBulletShots -= Time.deltaTime;
        //    }
        //}
        Fire();
        Reload();
    }

    void Fire() {
        if (!Input.GetMouseButton(1))
            return;
        if (curShotDelay < maxShotDelay)
            return;
		GameObject bullet = Instantiate(Bullet, shotPoint.position, Quaternion.Euler(0f, 0f, rotateDegree / 2));
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		rigid.AddForce(/*Vector2.up*/ new Vector2(dx * shotPower, dy * shotPower), ForceMode2D.Impulse);
		curShotDelay = 0;
    }

    void Reload() {
        curShotDelay += Time.deltaTime;
    }
}
