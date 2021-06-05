using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttaqueUI1 : MonoBehaviour
{
    [SerializeField] private List<Image> Borne;
    [SerializeField] private Sprite Minimum;
    [SerializeField] private Sprite Maximum;
    [SerializeField] private Sprite None;
    [SerializeField] private List<int> Temp;

    [SerializeField] private Animator Animation;
    [SerializeField] private Animator Impactor;
    [SerializeField] private GameObject MinSlider;
    [SerializeField] private GameObject MaxSlider;

    [SerializeField] private TextMeshProUGUI BonusTxt;

    public
        List<int> Min;
    public 
        List<int> Max;

    private void Start()
    {
        MinSlider.SetActive(false);
        MaxSlider.SetActive(false);
    }

    public int StartMin;
    public int EndMin;
    public int StartMax;
    public int EndMax;
    public int DiceBonus;

    public void SynchAttackBorne(UnitScript Unit, int Bonus = 0)
    {
        bool Done = false;

        Min = new List<int>();
        Max = new List<int>();

        BonusTxt.text = (Unit.DiceBonus+Bonus).ToString();
        StartMin = (int)Unit.NumberRangeMin.x;
        EndMin = (int)Unit.NumberRangeMin.y;
        StartMax = (int)Unit.NumberRangeMax.x;
        EndMax = (int)Unit.NumberRangeMax.y;
        DiceBonus = Unit.DiceBonus + Bonus;
        Temp = new List<int>();
        for (int u = 0; u < Borne.Count; u++)
        {
            Temp.Add(u + 2);
        }
        if (StartMin >= 2 && EndMin <= 12)
        {
            for (int i = StartMin - DiceBonus; i <= EndMin - DiceBonus; i++)
            {
                int u = i;                
                if (i < 2)
                {
                    continue;
                }
                if (i > 12)
                {
                    continue;
                }                
                if (!Done)
                {
                    Done = true;

                    MinSlider.SetActive(true);
                    MinSlider.transform.position = new Vector3(Borne[u - 2].transform.position.x, MinSlider.transform.position.y, MinSlider.transform.position.z);
                    MinSlider.GetComponentInChildren<TextMeshProUGUI>().text = (Unit.DamageMinimum + Unit.DamageBonus).ToString();
                }
                Borne[u - 2].sprite = Minimum;
                Min.Add(u);
                Temp.Remove(u);
            }
        }
        Done = false;
        if (StartMax >= 2 && EndMax <= 12)
        {
            for (int i = StartMax - DiceBonus; i <= EndMax; i++)
            {
                int u = i;               
                if(i < 2)
                {
                    continue;
                }
                if (i > 12)
                {
                    continue;
                }
                if (!Done)
                {
                    Done = true;
                    MaxSlider.SetActive(true);
                    MaxSlider.transform.position = new Vector3(Borne[u - 2].transform.position.x, MaxSlider.transform.position.y, MaxSlider.transform.position.z); 
                    MaxSlider.GetComponentInChildren<TextMeshProUGUI>().text = (Unit.DamageMaximum + Unit.DamageBonus).ToString();
                }
                Borne[u - 2].sprite = Maximum;
                Max.Add(u);
                Temp.Remove(u);
            }
        }
        foreach (int I in Temp)
        {
            Borne[I - 2].sprite = None;
        }

        if(Max.Count == 0)
        {
            MaxSlider.SetActive(false);
        }
        if(Min.Count == 0)
        {
            MinSlider.SetActive(false);
        }
    }

    private int Dice = 0;
    public void Attack(int DiceResult)
    {
        Dice = DiceResult;
        if (Min.Contains(DiceResult))
        {
            Animation.SetTrigger("Normal"); 
        }
        else if (Max.Contains(DiceResult) || DiceResult > 12)
        {
            Animation.SetTrigger("Normal");
        }
        else
        {
            Animation.SetTrigger("Fail");
        }
    }

    public void Impact()
    {
        Vector3 I = Impactor.gameObject.transform.position;
        if (Dice > 12)
        {
            Dice = 12;
        }                        
        if(Dice -2 >= 0 && Dice -2 < Borne.Count)
        { 
        Impactor.gameObject.transform.position = new Vector3(Borne[Dice - 2].transform.position.x, Borne[Dice - 2].transform.position.y, I.z);
        }
        if(Dice == 2)
        {
            Debug.Log("low");
            Impactor.SetTrigger("ImpactLeft");
        }
        else if (Dice == 12)
        {
            Impactor.SetTrigger("ImpactRight");
        }
        else
        {
            Impactor.SetTrigger("Impact");
        }
        Dice = 0;
    }
}
