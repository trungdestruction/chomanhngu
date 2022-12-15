using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    
    public Tween arrow1, arrow2;
    
    public int dame = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Move();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        other.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        if (other.CompareTag("X1"))
        {
            dame = 1;
        }

        if (other.CompareTag("X2"))
        {
            dame = 2;
        }

        if (other.CompareTag("X3"))
        {
            dame = 3;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.transform.localScale = new Vector3(1, 1, 1);
    }
    
    public void Move()
    {
        arrow1 = GetComponent<RectTransform>().DOLocalMoveX(144, 1.3f).SetEase(Ease.InOutQuad).OnComplete(MoveBack);
    }

    public void MoveBack()
    {
        arrow2 = GetComponent<RectTransform>().DOLocalMoveX(-143, 1.3f).SetEase(Ease.InOutQuad).OnComplete(Move);
    }
}
