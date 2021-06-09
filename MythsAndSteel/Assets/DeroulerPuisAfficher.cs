using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DeroulerPuisAfficher : MonoBehaviour
{
    [SerializeField] private GameObject Bouton;
    [SerializeField] private GameObject animator;
    private void Start()
    {
        Bouton.SetActive(false);
        StartCoroutine(DisplayButton(61f));
    }
    IEnumerator DisplayButton(float WaitToTime)
    {
        yield return new WaitForSeconds(WaitToTime);
        animator.transform.position = new Vector3(-0.2f, 71.422f, -432.55f);
        Bouton.SetActive(true);
    }
}
