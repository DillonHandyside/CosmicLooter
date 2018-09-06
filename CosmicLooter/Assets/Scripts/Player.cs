﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float m_speed = 10.0f;
    public float m_fireDelay = 1.0f;

    private float m_timer = 0.0f;

    public float m_bulletSpeed = 10.0f;

    public Vector2 m_bulletOffset;

    private Rigidbody2D rb;

    public GameObject m_scoreManager;

    //Player internal object pool
    public GameObject m_bullet;
    public int m_poolAmount;

    private GameObject[] m_pool;
    private Stack<GameObject> m_poolReady = new Stack<GameObject>();

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        m_pool = new GameObject[m_poolAmount];
        for (int i = 0; i < m_poolAmount; ++i)
        {
            m_pool[i] = Instantiate<GameObject>(m_bullet);
            m_pool[i].GetComponent<PlayerBullet>().m_creator = this;
            m_pool[i].SetActive(false);
            m_poolReady.Push(m_pool[i]);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Horizontal Movement
        float h = Input.GetAxisRaw("Horizontal");

        Vector2 vel = rb.velocity;
        vel.x = h * m_speed;
        rb.velocity = vel;
        //Shooting
        if (Input.GetAxisRaw("Fire1") == 1 && m_timer >= m_fireDelay)
        {
            m_timer = 0.0f;
            //fire bullet
            Debug.Log("pew");
            Fire(0.0f, m_bulletSpeed);
        }


        m_timer += Time.deltaTime;
	}

    private void Fire(float _x, float _y)
    {
        GameObject top = m_poolReady.Pop();
        Rigidbody2D topRigid = top.GetComponent<Rigidbody2D>();

        top.transform.position = transform.position + new Vector3(m_bulletOffset.x, m_bulletOffset.y);
        topRigid.velocity = new Vector2(_x, _y);

        top.GetComponent<PlayerBullet>().m_speed = new Vector2(_x, _y);

        top.SetActive(true);
    }

    public void DestroyBullet(GameObject _bullet)
    {
        _bullet.SetActive(false);

        m_poolReady.Push(_bullet);
    }
}
