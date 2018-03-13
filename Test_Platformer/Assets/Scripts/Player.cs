using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit {
    Animator animator;
    GameObject gfx;

    public float exlpodeForce = 5;

    public Collider2D connonCollider;

    void Start()
    {
        gfx = GameObject.Find("GFX");

        animator = gfx.GetComponent<Animator>();
    }

    void Update ()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger("attack");
        }*/
	}

    //施加力
    public void Explode()
    {
        foreach (GameObject item in list)
        {
            AddExpolsionForce(item);
        }
    }
    //施加爆炸斥力
    void AddExpolsionForce(GameObject _target)
    {
        Rigidbody2D rb = _target.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce((Vector2)(_target.transform.position - transform.position) * exlpodeForce, ForceMode2D.Impulse);
        }
    }

    #region 攻击列表
    List<GameObject> list = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        list.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        list.Remove(collision.gameObject);
    }
#endregion
}
