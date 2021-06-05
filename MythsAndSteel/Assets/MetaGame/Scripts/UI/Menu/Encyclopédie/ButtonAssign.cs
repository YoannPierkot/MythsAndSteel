using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonAssign : MonoBehaviour
{
    [SerializeField]
    bool isunivers = false;
    [SerializeField] bool isArmy1;
    [SerializeField] GameObject Army1;
    [SerializeField] GameObject Army2;
    [SerializeField] GameObject informationPanel;
    [SerializeField] GameObject homePanel;
    public List<Image> MiniJaugeSlot = new List<Image>();
    public Sprite Maximum;
    public Sprite Minimum;
    public Sprite None;
    public GameObject MinSlider;
    public GameObject MaxSlider;
    [SerializeField] Unit_SO UnitShown;
    [SerializeField] TextMeshProUGUI UnitName;
    [SerializeField] TextMeshProUGUI UnitLore;
    [SerializeField] GameObject UnitImage;
    [SerializeField] TextMeshProUGUI UnitLife;
    [SerializeField] TextMeshProUGUI UnitRange;
    [SerializeField] TextMeshProUGUI UnitMovement;
    [SerializeField] TextMeshProUGUI UnitType;

    public void showArmy1()
    {
        if (!isArmy1)
        {
            Army1.SetActive(true);
            Army2.SetActive(false);
        }
    }

    public void showArmy2()
    {
        if (isArmy1)
        {
            Army1.SetActive(false);
            Army2.SetActive(true);
        }
    }

    public void UnitButton()
    {
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[13]);
        /*if (informationPanel != null)
        {
            informationPanel.SetActive(false);
        }*/
        //informationPanel = EventSystem.current.currentSelectedGameObject.transform.GetChild(1).gameObject;
        informationPanel.SetActive(true);
        homePanel.SetActive(false);
        UnitShown = EventSystem.current.currentSelectedGameObject.GetComponent<Encyclopedie_Unit>().AssociatedUnit;
        if(!isunivers)
        {

        UpdateMiniJauge(UnitShown);
        UnitLife.SetText(UnitShown.LifeMax.ToString());
        UnitRange.SetText(UnitShown.AttackRange.ToString());
        UnitMovement.SetText(UnitShown.MoveSpeed.ToString());
        UnitType.SetText(UnitShown.typeUnite.ToString());
        }
        UnitName.SetText(UnitShown.UnitName);
        UnitLore.SetText(UnitShown.Description);
        UnitImage.GetComponent<Image>().sprite = UnitShown.Sprite;

    }


    public void OnEnable()
    {
        homePanel.SetActive(true);
        informationPanel.SetActive(false);
    }
    public void UpdateMiniJauge(Unit_SO Unit)
    {
        bool Done = false;
        List<int> Min = new List<int>();
        List<int> Max = new List<int>();
        List<int> Temp = new List<int>();

        int StartMin = (int)Unit.NumberRangeMin.x;
        int EndMin = (int)Unit.NumberRangeMin.y;
        int StartMax = (int)Unit.NumberRangeMax.x;
        int EndMax = (int)Unit.NumberRangeMax.y;
        

        for (int u = 0; u < MiniJaugeSlot.Count; u++)
        {
            Temp.Add(u + 2);
        }
        if (StartMin >= 2 && EndMin <= 12)
        {
            for (int i = StartMin; i <= EndMin; i++)
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
                    MinSlider.transform.position = new Vector3(MiniJaugeSlot[u - 2].transform.position.x, MinSlider.transform.position.y, MinSlider.transform.position.z);
                    MinSlider.GetComponentInChildren<TextMeshProUGUI>().text = Unit.DamageMinimum.ToString();
                }
                MiniJaugeSlot[u - 2].sprite = Minimum;
                Min.Add(u);
                Temp.Remove(u);
            }
        }
        Done = false;
        if (StartMax >= 2 && EndMax <= 12)
        {
            for (int i = StartMax; i <= EndMax; i++)
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
                    MaxSlider.SetActive(true);
                    MaxSlider.transform.position = new Vector3(MiniJaugeSlot[u - 2].transform.position.x, MaxSlider.transform.position.y, MaxSlider.transform.position.z);
                    MaxSlider.GetComponentInChildren<TextMeshProUGUI>().text = Unit.DamageMaximum.ToString();
                }
                MiniJaugeSlot[u - 2].sprite = Maximum;
                Max.Add(u);
                Temp.Remove(u);
            }
        }
        foreach (int I in Temp)
        {
            MiniJaugeSlot[I - 2].sprite = None;
        }

        if (Max.Count == 0)
        {
            MaxSlider.SetActive(false);
        }
        if (Min.Count == 0)
        {
            MinSlider.SetActive(false);
        }
    }
}
