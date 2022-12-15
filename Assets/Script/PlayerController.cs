using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Coroutine count;
    private Rigidbody rb;
    public Pokemon feedingPokemon;
    private float size = 1;

    public float speed;
    public UltimateJoystick joystick;

    public Animator anim;
    public GameObject startingText;
    public GameObject pokemon;
    public GameObject canvas;
    public GameObject Bar;
    public FoodBar foodBar;
    public GameObject particle;

    private bool isStarted = false;
    public bool isFinished = false;
    private bool goFeed = false;
    private bool isFeeding = false;

    private float height;
    private int row = 1;
    public Stack<Transform> foodList = new Stack<Transform>();
    public Transform GioXe;

    private float y = 4;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        UltimateJoystick.DisableJoystick("Feed");
        Bar.SetActive(false);
        anim.SetFloat("velocity", 1f);
    }

    

    void Update()
    {
        //Start game
        if (Input.GetKeyDown(KeyCode.Mouse0) || (Input.touchCount > 0))
        {
            isStarted = true;
            startingText.SetActive(false);
        }

        //Foodbar
        if (isFeeding)
        {
            Bar.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (!isStarted)
        {
            return;
        }
        if(!isFinished)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
            rb.velocity = new Vector3(UltimateJoystick.GetHorizontalAxis("Road") * speed * 0.7f, 0, 0);
            Rotation();
        }
        if(goFeed)
        {
            anim.SetFloat("velocity", rb.velocity.magnitude);
            rb.velocity = new Vector3(UltimateJoystick.GetHorizontalAxis("Feed") * speed, 0, UltimateJoystick.GetVerticalAxis("Feed") * speed);
            Rotation2();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Stack food
        if (other.CompareTag("Food"))
        {
            other.transform.SetParent(GioXe);
            other.gameObject.GetComponent<Food>().enabled= false;
            foodList.Push(other.transform);
            other.enabled = false;
            other.transform.DORotate(Vector3.zero, 0.5f, RotateMode.Fast);
            if (row == 1)
            {
                other.transform.DOLocalMove(new Vector3(-0.5f, height, 0), 0.5f);
                row = 2;
            }
            else
            {
                other.transform.DOLocalMove(new Vector3(0.5f, height, 0), 0.5f);
                row = 1;
                height += 0.5f;
            }
        }

        //Finish line
        if (other.CompareTag("Finish"))
        {
            foodBar.SetMaxFood(foodList.Count);
            UltimateJoystick.DisableJoystick("Road");
            isFinished = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(Vector3.zero);
            transform.DOLocalMove(new Vector3(0, 0, 175), 0.5f).OnComplete(() =>
            {
                StartCoroutine(PassBrigde());
            });
        }

        //Feed pet
        if(other.CompareTag("Pokemon"))
        {
            count = StartCoroutine(feeding(other.transform));
            isFeeding = true;
        }
    }

    //Rotate
    public void Rotation()
    {
        if (UltimateJoystick.GetHorizontalAxis("Road") != 0 && isFinished == false)
        {
            Vector3 moveDir = new Vector3(UltimateJoystick.GetHorizontalAxis("Road") / 4, 0, 1);
            transform.rotation = Quaternion.LookRotation(moveDir).normalized;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.zero);
        }
    }

    //Rotate 2
    public void Rotation2()
    {
        if (UltimateJoystick.GetHorizontalAxis("Feed") != 0 || UltimateJoystick.GetVerticalAxis("Feed") != 0)
        {
            Vector3 moveDir = new Vector3(UltimateJoystick.GetHorizontalAxis("Feed"), 0, UltimateJoystick.GetVerticalAxis("Feed"));
            transform.rotation = Quaternion.LookRotation(moveDir).normalized;
        }
        
    }

    //Pass Bridge
    private IEnumerator PassBrigde()
    {
        transform.DOLocalMove(new Vector3(0, 0, 198), 2);
        yield return new WaitForSeconds(2);

        UltimateJoystick.EnableJoystick("Feed");
        goFeed = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            other.isTrigger = false;
        }
        if (other.CompareTag("Pokemon"))
        {
            StopCoroutine(count);
            count = null;
        }
    }

    //Feed pet
    private IEnumerator feeding(Transform parent)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.35f);
            if (foodList.Count > 0)
            {
                Transform food = foodList.Pop();
                food.SetParent(parent, true);
                food.DOMove(new Vector3(0.5f, y += 0.2f, 250), 0.3f).OnComplete(() =>
                {
                    feedingPokemon.feed.SetBool("isFeeded", true);
                    Instantiate(particle, new Vector3(pokemon.transform.position.x, pokemon.transform.position.y, pokemon.transform.position.z), Quaternion.identity);
                    food.gameObject.SetActive(false);
                    foodBar.IncreaseFood();
                    pokemon.transform.DOScale(size += 0.05f, 0.01f).OnComplete(() =>
                    {
                        feedingPokemon.feed.SetBool("isFeeded", false);
                    });
                    canvas.transform.position = new Vector3(canvas.transform.position.x, transform.position.y + (size - 1) * 5f, canvas.transform.position.z);
                });
            }
        }
    }
}
