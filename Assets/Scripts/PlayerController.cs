using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] private Transform visual, cannon;
    [SerializeField] private GameObject bullet;
    [Header("Specs")]
    [SerializeField] private float speed = 5f, fireDelta = 1f;
    [SerializeField] private short health = 3;
    //inputs
    private Vector2 rootMousePos;
    private float inputHorizontal = 0;

    private bool canFire = true;


    void Update()
    {
        HandleWithInput();
        Move();

        if (canFire)
            StartCoroutine(Fire());
    }

    private void HandleWithInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rootMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var dragVec = (Vector2)Input.mousePosition - rootMousePos;
            inputHorizontal = dragVec.x;
            rootMousePos = Input.mousePosition;
        }
        else
        {
            inputHorizontal = 0;
        }
    }

    private void Move()
    {
        var newPlayerPos = visual.transform.localPosition;
        newPlayerPos.x = Mathf.Clamp(newPlayerPos.x + inputHorizontal * speed * Time.smoothDeltaTime, -2f, 2f);
        visual.transform.localPosition = newPlayerPos;
    }

    private IEnumerator Fire()
    {
        canFire = false;
        Instantiate(bullet, cannon.position, Quaternion.identity);

        yield return new WaitForSeconds(fireDelta);

        canFire = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
            health--;

            if(health == 0)
            {
                //game over
            }
        }
    }
}
