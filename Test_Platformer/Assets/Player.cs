using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Animator animator;
    GameObject gfx;

    public float radius = 2f;

    public float exlpodeForce = 5;

    void Start()
    {
        gfx = GameObject.Find("GFX");

        animator = gfx.GetComponent<Animator>();
    }

    void Update () {


        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("qwe");
            animator.Play("Pinkie_Attack");

            StartCoroutine(PinkieAttack());
        }
	}

    IEnumerator PinkieAttack()
    {
        yield return new WaitForSeconds(1);
        Explode();
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D item in colliders)
        {
            AddExpolsionForce(item.gameObject);
        }
    }

    void AddExpolsionForce(GameObject _target)
    {
        Rigidbody2D rb = _target.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce((Vector2)(_target.transform.position - transform.position) * exlpodeForce, ForceMode2D.Impulse);
        }
    }
}
