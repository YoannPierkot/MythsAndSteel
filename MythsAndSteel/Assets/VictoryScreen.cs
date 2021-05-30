using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    #region Variables
    // Infos globales de la partie
    [SerializeField] private TextMeshProUGUI TimeCounterM;
    [SerializeField] private TextMeshProUGUI TimeCounterS;
    [SerializeField] private TextMeshProUGUI TurnCounter;
    [SerializeField] private TextMeshProUGUI TurnCounterOverlay;
    public int turnCounter;
    // Infos du joueur rouge
    [SerializeField] private TextMeshProUGUI RedDeadUnits;
    public int redDeadUnits = 0;
    [SerializeField] private TextMeshProUGUI RedEventUsed;
    public int redEventUsed = 0;
    [SerializeField] private TextMeshProUGUI RedResourcesUsed;
    public int redRessourcesUsed = 0;
    public bool RedWin = false;

    //Infos du joueur Bleu
    [SerializeField] private TextMeshProUGUI BlueDeadUnits;
    public int blueDeadUnits = 0;
    [SerializeField] private TextMeshProUGUI BlueEventUsed;
    public int blueEventUsed = 0;
    [SerializeField] private TextMeshProUGUI BlueResourcesUsed;
    public int blueResourcesUsed = 0;
    public bool BlueWin = false;
    
    //Variables System
    private float startTime;
    public bool IsVictoryScreenActive = false;
    #endregion
    void Awake()
    {
      
        startTime = Time.time;
    }

    void Update()
    {
        Timer();
        TurnCounterOverlay.text = (turnCounter + 1).ToString();
    }

    void Timer()
    {
      
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        TimeCounterM.text = minutes;
        TimeCounterS.text = seconds;
        TurnCounter.text = (turnCounter + 1).ToString();
        RedDeadUnits.text = redDeadUnits.ToString();
        RedEventUsed.text = redEventUsed.ToString();
        RedResourcesUsed.text = redRessourcesUsed.ToString();
        BlueDeadUnits.text = blueDeadUnits.ToString();
        BlueEventUsed.text = blueEventUsed.ToString();
        BlueResourcesUsed.text = blueResourcesUsed.ToString();

        if (IsVictoryScreenActive)
        {
            DisplayVictoryScreen();
            Time.timeScale = 0;       
        }
    }

    void DisplayVictoryScreen()
    {
        UIInstance.Instance.VictoryScreen.SetActive(true);
        if (RedWin == true)
        {
            UIInstance.Instance.RedWin.SetActive(true);
            UIInstance.Instance.BlueWin.SetActive(false);
        }
        else if (BlueWin == true)
        {
            UIInstance.Instance.BlueWin.SetActive(true);
            UIInstance.Instance.RedWin.SetActive(false);
        }
    }
}