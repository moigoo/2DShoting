﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour {

    Spaceship spaceship;

    BackGround background;

    Vector2 max;
    Vector2 startPos;
    Vector2 direction;
    Vector2 endPos;

    private Vector2 screenPoint;
    private Vector2 offset;
    private bool isDragging = false;
    // Use this for initialization
    IEnumerator Start () {
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        spaceship = GetComponent<Spaceship>();

        background = FindObjectOfType<BackGround>();
        while (true)
        {

            //spaceship.Shot(transform);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(spaceship.shotDelay);
        }

	}
    
    // Update is called once per frame
    void Update () {

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        //Vector2 direction = new Vector2(x, y).normalized;
        Vector2 direction = new Vector2(x, y);
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
        Move(direction);
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            //ドラッグ中を真.
            isDragging = true;
            startPos = mousePoint;
        }
        //左マウスボタンが離されたとき.
        if (Input.GetMouseButtonUp(0))
        {
            //ドラッグ中を偽.
            isDragging = false;
        }
        if (isDragging)
        {
            direction = mousePoint - startPos;


        }
        direction.x = direction.x / max.x;
        direction.y = direction.y / max.y;
        if (direction.magnitude > 1)
        {
            direction.Normalize();
            Debug.Log("over");
        }
        Move(direction);
#else
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            //ドラッグ中を真.
            isDragging = true;
            startPos = mousePoint;
        }
        //左マウスボタンが離されたとき.
        if (Input.GetMouseButtonUp(0))
        {
            //ドラッグ中を偽.
            isDragging = false;
        }
        if (isDragging)
        {
            direction = mousePoint - startPos;

            Move(direction);
        }
        //direction.x = direction.x / (max.x * 10.0f);
        //direction.y = direction.y / (max.y * 10.0f);
        //Debug.Log(max.x.ToString()+"," + max.y.ToString());
        //Debug.Log(direction.x.ToString() + "," + direction.y.ToString() + "->" +direction.magnitude.ToString());
        //Move(direction);
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    switch (touch.phase)
        //    {
        //        case TouchPhase.Began:
        //            startPos = touch.position;
        //            break;

        //        case TouchPhase.Moved:
        //        case TouchPhase.Stationary:
        //            direction = touch.position - startPos;

        //            break;
        //        case TouchPhase.Ended:
        //            endPos = touch.position;
        //            break;

        //    }
        //    //float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        //    //float y = CrossPlatformInputManager.GetAxisRaw("Vertical");
        //    ////Vector2 direction = new Vector2(x, y).normalized;
        //    //Vector2 direction = new Vector2(x, y);
        //    //direction.x *= 0.01f;
        //    //direction.y *= 0.01f;
        //    Debug.Log(direction.x.ToString() + ","+ direction.y.ToString());
        //    direction.x = direction.x / (max.x *10.0f);
        //    direction.y = direction.y / (max.y *10.0f);
        //    if (direction.magnitude > 2.0f)
        //    {
        //        direction.Normalize();
        //    }
        //    Move(direction);
        //}
#endif
    }

    void Move(Vector2 direction)
    {
        Vector2 scale = background.transform.localScale;


        Vector2 min = scale * -0.5f; 

        Vector2 max = scale * 0.5f;

        Vector2 pos = transform.position;

        pos += direction * spaceship.speed * Time.deltaTime;
        //pos += direction * spaceship.speed;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        string layerName = LayerMask.LayerToName(c.gameObject.layer);

        //if (layerName == ("Bullet(Enemy)"))
        //{

        //    Destroy(c.gameObject);
        //}
        if (layerName == ("Bullet(Enemy)") || layerName == ("Enemy"))
        {
            FindObjectOfType<Manager>().GameOver();

            spaceship.Explosion();

            Destroy(gameObject);
        }
    
    }
}
