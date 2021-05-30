using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guérison : Capacity
{
    [SerializeField] private int HealValue = 2;


    public override void StartCpty()
    {

        int ressourcePlayer = GetComponent<UnitScript>().UnitSO.IsInRedArmy ? PlayerScript.Instance.RedPlayerInfos.Ressource : PlayerScript.Instance.BluePlayerInfos.Ressource;
        if (ressourcePlayer >= Capacity1Cost)
        {
            List<GameObject> tile = new List<GameObject>();
            foreach (int T in PlayerStatic.GetNeighbourDiag(GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false))
            {
                if(TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    if(TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy == GameManager.Instance.IsPlayerRedTurn)
                    {
                    tile.Add(TilesManager.Instance.TileList[T]);
                    }

                }
            }
            GameManager.Instance._eventCall += EndCpty;
            GameManager.Instance._eventCallCancel += StopCpty;
            GameManager.Instance.StartEventModeTiles(1, GetComponent<UnitScript>().UnitSO.IsInRedArmy, tile, "Guérison", "Donne " + HealValue + " PV une unité adjacente. Voulez-vous vraiment effectuer cette action ?");
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
        if (GetComponent<UnitScript>().UnitSO.IsInRedArmy)
        {
            PlayerScript.Instance.RedPlayerInfos.Ressource -= Capacity1Cost;
        }
        else
        {
            PlayerScript.Instance.BluePlayerInfos.Ressource -= Capacity1Cost;
        }
        
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        
        if (GameManager.Instance.TileChooseList.Count > 0)
        {
            foreach(GameObject id in GameManager.Instance.TileChooseList)
            {
                GameObject U = TilesManager.Instance.TileList[id.GetComponent<TileScript>().TileId].GetComponent<TileScript>().Unit;
                if (U != null)
                {
                    U.GetComponent<UnitScript>().GiveLife(HealValue);
                }
            }            
        }
        GameManager.Instance.StopEventModeTile();

        GameManager.Instance.TileChooseList.Clear();
    }
}
