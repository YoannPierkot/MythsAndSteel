using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BouttonCredit : MonoBehaviour
{
   public void OnSceneChange(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
