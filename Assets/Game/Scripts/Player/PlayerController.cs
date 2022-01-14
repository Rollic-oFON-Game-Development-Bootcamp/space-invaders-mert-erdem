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
    [SerializeField] private float speed = 5f;
    [SerializeField] private float fireDelta = 1f;
    [SerializeField] private int health = 3;
    //inputs
    private Vector2 rootMousePos;
    private float inputHorizontal = 0;

    private bool canFire = true, gameStarted = false;

    private void Start()
    {
        GameManager.Instance.ActionGameStart += ActivateThePlayer;
    }

    void Update()
    {
        if (!gameStarted) return;

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
        newPlayerPos.x = Mathf.Clamp(newPlayerPos.x + inputHorizontal * speed * Time.smoothDeltaTime, -2.3f, 2.3f);
        visual.transform.localPosition = newPlayerPos;
    }

    private IEnumerator Fire()
    {
        canFire = false;
        
        yield return new WaitForSeconds(fireDelta);

        Instantiate(bullet, cannon.position, Quaternion.identity);
        canFire = true;
    }

    private void ActivateThePlayer()
    {
        gameStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletEnemy"))
        {
            Destroy(collision.gameObject);
            health--;
            CanvasController.Instance.UpdateHealthText(health);

            if(health == 0)
            {
                //game over
                GameManager.Instance.ActionGameOver?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.ActionGameStart -= ActivateThePlayer;
    }
}
