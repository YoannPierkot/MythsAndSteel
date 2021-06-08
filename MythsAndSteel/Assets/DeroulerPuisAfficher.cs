using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DeroulerPuisAfficher : MonoBehaviour
{
    [SerializeField] private GameObject Bouton;

    private void Start()
    {
        Bouton.SetActive(false);
        StartCoroutine(DisplayButton(40f));
    }
    IEnumerator DisplayButton(float WaitToTime)
    {
        yield return new WaitForSeconds(WaitToTime);
        Bouton.SetActive(true);
    }
}
