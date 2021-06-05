using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBluePlayer : ChargeOrgone
{


    public void ChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(1, 2))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);

            GameManager.Instance.StartEventModeUnit(1, true, unitList, "Charge d'orgone 1", "Êtes-vous sur d'octroyer un bonus de dés à cette unité équivalent aux unités adjacentes à celle ci lorsqu'elle attaquera lors de ce tour ?");
            GameManager.Instance._eventCall += UseChargeOrgone1;
            unitList.Clear();
        }
    }

    void UseChargeOrgone1()
    {
        UIInstance.Instance.ActivateNextPhaseButton();


        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().AddDiceToUnit(1);
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().DoingCharg1Blue = true;
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);

        GameManager.Instance.UnitChooseList.Clear();
        EndOrgoneUpdateBlue(-1);
    }

    public void ChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(3, 2))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> tileList = new List<GameObject>();
            tileList.AddRange(TilesManager.Instance.ResourcesList);

            GameManager.Instance.StartEventModeTiles(1, true, tileList, "Charge d'orgone 3", "Êtes-vous sur de vouloir voler une Ressources sur cette case?");
            GameManager.Instance._eventCall += UseChargeOrgone3;
            tileList.Clear();
        }
    }
    void UseChargeOrgone3()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach (GameObject gam in GameManager.Instance.TileChooseList)
        {
            gam.GetComponent<TileScript>().RemoveRessources(1, 2);
        }
        GameManager.Instance.TileChooseList.Clear();
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);
        EndOrgoneUpdateBlue(-3);
    }

    #region Charge 5 D'orgone
    public void ChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(5, 2))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListBluePlayer);

            GameManager.Instance.StartEventModeUnit(1, false, unitList, "Charge 5", "Êtes-vous sur de vouloir séléctionner cette unité?");
            GameManager.Instance._eventCall += MoveChargeOrgone5;
          
        }
    }

    void MoveChargeOrgone5()
    {
        List<GameObject> SelectTileList = new List<GameObject>();

        foreach (GameObject gam in TilesManager.Instance.TileList)
        {
            TileScript tilescript = gam.GetComponent<TileScript>();

            Debug.Log(tilescript.TileId);
            if (tilescript.Unit == null)
            {

                if (!tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_de_ressource) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Rouge)
                    && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Bleu) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineBleu) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineRouge))
                {
                    SelectTileList.Add(gam);
                    Debug.Log("break");

                }
            }
        }

            GameManager.Instance.StartEventModeTiles(1, false, SelectTileList, "Charge 5", "Êtes-vous sur de vouloir déplacer l'unité sur cette case?");
            
            GameManager.Instance._eventCall += DoneMoveChargeOrgone5;
          

        
    }
    void DoneMoveChargeOrgone5()
    {
        UIInstance.Instance.ActivateNextPhaseButton();
        GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0].gameObject);
        EndOrgoneUpdateBlue(-5);
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);
        GameManager.Instance.UnitChooseList.Clear();
        GameManager.Instance.TileChooseList.Clear();

        
    }
    #endregion
    public void orgonetest()
    {
        EndOrgoneUpdateBlue(-5);
    }
    public void GChargeOrgone1(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(1, 1))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);

            GameManager.Instance.StartEventModeUnit(1, true, unitList, "Charge d'orgone 1", "Êtes-vous sur d'octroyer un bonus de dés à cette unité équivalent aux unités adjacentes à celle ci lorsqu'elle attaquera lors de ce tour ?");
            GameManager.Instance._eventCall += GUseChargeOrgone1;
            unitList.Clear();
        }
    }

    void GUseChargeOrgone1()
    {
        UIInstance.Instance.ActivateNextPhaseButton();


        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().AddDiceToUnit(1);
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().DoingCharg1Blue = true;
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);

        GameManager.Instance.UnitChooseList.Clear();
        EndOrgoneUpdateRed(-1);
    }

    public void GChargeOrgone3(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(3, 1))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> tileList = new List<GameObject>();
            tileList.AddRange(TilesManager.Instance.ResourcesList);

            GameManager.Instance.StartEventModeTiles(1, true, tileList, "Charge d'orgone 3", "Êtes-vous sur de vouloir voler une Ressources sur cette case?");
            GameManager.Instance._eventCall += GUseChargeOrgone3;
            tileList.Clear();
        }
    }
    void GUseChargeOrgone3()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        foreach (GameObject gam in GameManager.Instance.TileChooseList)
        {
            gam.GetComponent<TileScript>().RemoveRessources(1, 1);
        }
        GameManager.Instance.TileChooseList.Clear();
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);
        EndOrgoneUpdateRed(-3);
    }

    #region Charge 5 D'orgone
    public void GChargeOrgone5(int cost)
    {
        if (MythsAndSteel.Orgone.OrgoneCheck.CanUseOrgonePower(5, 1))
        {
            Attaque.Instance.PanelBlockant1.SetActive(true);
            Attaque.Instance.PanelBlockant2.SetActive(true);
            List<GameObject> unitList = new List<GameObject>();

            unitList.AddRange(PlayerScript.Instance.UnitRef.UnitListRedPlayer);

            GameManager.Instance.StartEventModeUnit(1, false, unitList, "Charge 5", "Êtes-vous sur de vouloir séléctionner cette unité?");
            GameManager.Instance._eventCall += GMoveChargeOrgone5;

        }
    }

    void GMoveChargeOrgone5()
    {
        List<GameObject> SelectTileList = new List<GameObject>();

        foreach (GameObject gam in TilesManager.Instance.TileList)
        {
            TileScript tilescript = gam.GetComponent<TileScript>();

            Debug.Log(tilescript.TileId);
            if (tilescript.Unit == null)
            {

                if (!tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_de_ressource) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Rouge)
                    && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Point_Objectif_Bleu) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineBleu) && !tilescript.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.UsineRouge))
                {
                    SelectTileList.Add(gam);
                    Debug.Log("break");

                }
            }
        }

        GameManager.Instance.StartEventModeTiles(1, false, SelectTileList, "Charge 5", "Êtes-vous sur de vouloir déplacer l'unité sur cette case?");

        GameManager.Instance._eventCall += GDoneMoveChargeOrgone5;



    }
    void GDoneMoveChargeOrgone5()
    {
        UIInstance.Instance.ActivateNextPhaseButton();
        GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0].gameObject);
        EndOrgoneUpdateRed(-5);
        SoundController.Instance.PlaySound(SoundController.Instance.AudioClips[7]);
        GameManager.Instance.UnitChooseList.Clear();
        GameManager.Instance.TileChooseList.Clear();


    }
    #endregion
}
