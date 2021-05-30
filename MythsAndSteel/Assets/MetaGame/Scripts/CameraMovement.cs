using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    public float camSpeed = 20f;
    public float margin = 10f;
    public float camPosMax = 1.5625f;
    [SerializeField]
    Animator AnimatorMainBordure;
        bool LoadAnimHaut = true;
        bool LoadAnimBas = true;
    
    private void Start()
    {
        

        UIInstance.Instance.DesactivateNextPhaseButton();
        UIInstance.Instance.RenfortBlockant.SetActive(true);
        StartCoroutine(Delay());

    }
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 camPos = this.transform.position;
        
       

        if (mousePos.y > 1080f - margin && camPos.y < camPosMax)
        {
            this.transform.position = camPos + Vector3.up * camSpeed * Time.deltaTime;

        }
        else if(mousePos.y > 1080f - margin && camPos.y >= camPosMax)
        {
            AnimatorMainBordure.SetBool("Haut", true);
        }
        else
        {
            AnimatorMainBordure.SetBool("Haut", false);
        }
        if (mousePos.y < margin && camPos.y > -camPosMax)
        {
            this.transform.position = camPos + Vector3.down * camSpeed * Time.deltaTime;
        }
        else if (mousePos.y < margin && camPos.y <= -camPosMax)
        {
            AnimatorMainBordure.SetBool("Bas", true);
        }
        else
        {
            AnimatorMainBordure.SetBool("Bas", false);
        }
    }
   IEnumerator Delay()
    {
        yield return new WaitForSeconds(7f);
        UIInstance.Instance.ActivateNextPhaseButton();
        UIInstance.Instance.RenfortBlockant.SetActive(false);
        GetComponent<Animator>().enabled = false;
    }
}