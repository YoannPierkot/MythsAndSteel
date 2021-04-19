using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Capacity : MonoBehaviour
{
    //----------------------------Capacité 1--------------------------------------
    [Header("-----------------Capacité 1-----------------")]
    [SerializeField] string _Capacity1Name = "";
    public string Capacity1Name => _Capacity1Name;
    [SerializeField] private Sprite render1;

    [TextArea]
    [SerializeField] string _Capacity1Description = "";
    public string Capacity1Description => _Capacity1Description;

    [SerializeField] int _Capacity1Cost;
    public int Capacity1Cost => _Capacity1Cost;

    virtual public void Capacite1()
    {
        Debug.Log("Active La capacité 1");
    }


    [Header("-----------------Capacité 2-----------------")]
    //----------------------------Capacité 2--------------------------------------
    [SerializeField] bool _isCapacity2Exist;
    public bool isCapacity2Exist => _isCapacity2Exist;

    [SerializeField] string _Capacity2Name = "";
    public string Capacity2Name => _Capacity1Name;
    [SerializeField] private Sprite render2;

    [TextArea]
    [SerializeField] string _Capacity2Description = "";
    public string Capacity2Description => _Capacity1Description;

    [SerializeField] int _Capacity2Cost;
    public int Capacity2Cost => _Capacity2Cost;
    public virtual void Capacite2()
    {
        Debug.Log("Active La capacité 2");
    }

    /// <summary>
    /// Retourne le préfab pour l'UI de l'unité.
    /// </summary>
    /// <param name="PrefabCapacity"></param>
    /// <param name="number">Capacité 0 ou capacité 1</param>
    /// <returns></returns>
    public GameObject ReturnInfo(GameObject PrefabCapacity, int number = 0)
    {
        switch (number)
        {
            case 0:
                {
                    PrefabCapacity.transform.GetChild(0).GetComponent<Image>().sprite = render1;
                    PrefabCapacity.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _Capacity1Name;
                    PrefabCapacity.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _Capacity1Description;
                    break;
                }
            case 1:
                {
                    if (_isCapacity2Exist)
                    {
                        PrefabCapacity.transform.GetChild(0).GetComponent<Image>().sprite = render2;
                        PrefabCapacity.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _Capacity2Name;
                        PrefabCapacity.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _Capacity2Description;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
        }
        return PrefabCapacity;
    }
}