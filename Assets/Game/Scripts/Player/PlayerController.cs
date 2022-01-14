using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] private Transform visual;
    [SerializeField] private Transform cannon;
    [SerializeField] private GameObject bullet;
    [Header("Specs")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float fireDelta = 1f;
    [SerializeField] private int health = 3;
    //laser audio
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioLaser;
    [SerializeField] private AudioClip audioHit;
    [SerializeField] private AudioClip audioExplosion;
    //inputs
    private Vector2 rootMousePos;
    private float inputHorizontal = 0;

    private bool canFire = true, isActive;

    


    private void Start()
    {
        isActive = false;
        GameManager.Instance.ActionGameStart += ActivateThis;
        GameManager.Instance.ActionGameOver += DestroyThis;
    }

    void Update()
    {
        if (!isActive) return;

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
        //newPlayerPos.x = Mathf.Clamp(newPlayerPos.x + inputHorizontal * speed * Time.smoothDeltaTime, -2.3f, 2.3f);
        newPlayerPos.x = newPlayerPos.x + inputHorizontal * speed * Time.smoothDeltaTime;
        visual.transform.localPosition = newPlayerPos;
    }

    private IEnumerator Fire()
    {
        canFire = false;
        
        yield return new WaitForSeconds(fireDelta);

        Instantiate(bullet, cannon.position, Quaternion.identity);
        audioSource.PlayOneShot(audioLaser);
        canFire = true;
    }

    private void ActivateThis() => isActive = true;

    private void DestroyThis() => Destroy(this);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletEnemy"))
        {
            audioSource.PlayOneShot(audioHit);
            Destroy(collision.gameObject);
            health--;
            CanvasController.Instance.UpdateHealthText(health);

            if(health == 0)
            {
                //game over
                audioSource.PlayOneShot(audioExplosion);
                GameManager.Instance.ActionGameOver?.Invoke();
            }
        }

        if (collision.CompareTag("Mirror"))
            visual.transform.localPosition *= Vector2.left + 0.01f * Vector2.one;//mirror effect
    }

    private void OnDestroy()
    {
        GameManager.Instance.ActionGameStart -= ActivateThis;
        GameManager.Instance.ActionGameOver -= DestroyThis;
    }
}
