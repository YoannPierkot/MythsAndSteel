using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luch : Capacity
{
    [SerializeField] private int LineCount = 2;
    GameObject finaltile;

    public override void StartCpty()
    {
        int ressourcePlayer = GetComponent<UnitScript>().UnitSO.IsInRedArmy ? PlayerScript.Instance.RedPlayerInfos.Ressource : PlayerScript.Instance.BluePlayerInfos.Ressource;
        if (ressourcePlayer >= Capacity1Cost)
        {
            List<GameObject> unit = new List<GameObject>();
            foreach (int T in PlayerStatic.GetNeighbourDiag(GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false))
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null) unit.Add(TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit);
            }
            GameManager.Instance._eventCall += EndCpty;
            GameManager.Instance._eventCallCancel += StopCpty;
            GameManager.Instance.StartEventModeUnit(1, GetComponent<UnitScript>().UnitSO.IsInRedArmy, unit, Capacity1Name , "Inflige 1 point de dégât et faire reculer l'unité jusqu'à 2 cases. Voulez-vous vraiment effectuer cette action ?");
        }
    }

    public override void StopCpty()
    {
        GameManager.Instance.StopEventModeTile();
        GameManager.Instance.TileChooseList.Clear();
        GetComponent<UnitScript>().StopCapacity(true);
    }

    public override void EndCpty()
    {
        GameManager.Instance.TileChooseList.Add(TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId]);
        if (GetComponent<UnitScript>().UnitSO.IsInRedArmy)
        {
            PlayerScript.Instance.RedPlayerInfos.Ressource -= Capacity1Cost;
        }
        else
        {
            PlayerScript.Instance.BluePlayerInfos.Ressource -= Capacity1Cost;
        }
        
        GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().TakeDamage(1);

        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        MYthsAndSteel_Enum.Direction Direct = PlayerStatic.CheckDirection(GetComponent<UnitScript>().ActualTiledId, GameManager.Instance.TileChooseList[GameManager.Instance.TileChooseList.Count - 1].GetComponent<TileScript>().TileId);
        

        for (int i = 0; i <= LineCount; i++)
        {
            TileScript LastTile = GameManager.Instance.TileChooseList[GameManager.Instance.TileChooseList.Count - 1].GetComponent<TileScript>();
            bool check = false;
            foreach (int D in PlayerStatic.GetNeighbourDiag(LastTile.TileId, LastTile.Line, false))
            {
                if (PlayerStatic.CheckDirection(LastTile.TileId, TilesManager.Instance.TileList[D].GetComponent<TileScript>().TileId) == Direct)
                {
                    check = true;
                    GameManager.Instance.TileChooseList.Add(TilesManager.Instance.TileList[D]);
                    finaltile = TilesManager.Instance.TileList[D];
                }
            }
            if (check == false)
            {

                break;
            }
        }

        Deplacementcapacity();


    }


    public void Deplacementcapacity()
    {
        GameManager.Instance._eventCall = null;

        while (GameManager.Instance.UnitChooseList[0].transform.position != finaltile.transform.position)
        {
            GameManager.Instance.UnitChooseList[0].transform.position = Vector3.MoveTowards(GameManager.Instance.UnitChooseList[0].transform.position, finaltile.transform.position, .7f);
            GameManager.Instance._waitEvent -= Deplacementcapacity;
            GameManager.Instance._waitEvent += Deplacementcapacity;
            GameManager.Instance.WaitToMove(.025f);
            return;
        }

        GameManager.Instance._waitEvent -= Deplacementcapacity;
        TilesManager.Instance.TileList[GameManager.Instance.UnitChooseList[0].GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().RemoveUnitFromTile();
        finaltile.GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[0]);

        GameManager.Instance.TileChooseList.Clear();
        GameManager.Instance.UnitChooseList.Clear();
    } 
} 