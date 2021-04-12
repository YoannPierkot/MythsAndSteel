using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
Ce Script permet d'afficher ou d'enlever les Options en appuyant sur Echap. 
Il fait apparaitre une autre scène qui va se superposer à la scène principal.
 */
public class MenuOption : MonoBehaviour
{
   bool menuOptionActivé = false;

    void Update()
    {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuOptionActivé == true)
                {
                    Resume();

                }
                else
                {
                    ActiveMenu();
                }
            }
        }
   
    void ActiveMenu()
    {

      
        menuOptionActivé = true;
       SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

    }
    public void Resume()
    {
       

        menuOptionActivé = false;
        SceneManager.UnloadSceneAsync(1);
    }
}
