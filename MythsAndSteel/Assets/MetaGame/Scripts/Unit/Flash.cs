using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private bool Activate = false;
    [SerializeField] private List<SpriteRenderer> Fx;

    private void Start()
    {
        A = minA;
    }

    void FixedUpdate()
    {
        Flashingeffect();
    }

    [Range(0, 1)] [SerializeField] private float minA = 0;
    [Range(0, 1)] [SerializeField] private float maxA = 1;
    [SerializeField] private float speed = 1;

    [SerializeField] private float A;
    private bool switched = false;
    private void Flashingeffect()
    {
        Debug.Log("flash");
        if (Activate)
        {
            if (switched)
            {
                A = Mathf.MoveTowards(A, maxA, speed);
                if (A >= maxA - .01f)
                {
                    switched = false;
                }
            }
            else if (!switched)
            {
                A = Mathf.MoveTowards(A, minA, speed);
                if (A <= minA + .01f)
                {

                    switched = true;
                }
            }
            foreach (SpriteRenderer F in Fx)
            {
                F.color = new Color(A, A, A, A);
            }
        }
        else
        {
            foreach (SpriteRenderer F in Fx)
            {
                A = Mathf.MoveTowards(A, 1, speed);
                F.color = new Color(A, A, A, A);
            }
        }
    }


    private IEnumerator Flashed(float timetoflash = 1.5f)
    {
        Activate = true;
        yield return new WaitForSeconds(timetoflash);
        Activate = false;
    }

    Coroutine Last;
    public void Callflash(float TimeToFlash = 1.5f)
    {
        if(Last != null)
        {
            StopCoroutine(Last);
        }
        Last = StartCoroutine(Flashed(TimeToFlash));
    }

}
