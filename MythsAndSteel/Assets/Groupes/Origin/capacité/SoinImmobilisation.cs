using System.Collections.Generic;
using UnityEngine;

public class SoinImmobilisation : Capacity
{
    public override void StartCpty()
    {
        int tileId = RaycastManager.Instance.ActualUnitSelected.GetComponent<UnitScript>().ActualTiledId;
        List<GameObject> tile = new List<GameObject>();
        
        foreach (int T in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
        {
            if (TilesManager.Instance.TileList[T] != null)
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected && TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    tile.Add(TilesManager.Instance.TileList[T]);
                }
            }
        }

        foreach (int T in PlayerStatic.GetNeighbourDiag(tileId + 1, TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>().Line, false))
        {
            if (TilesManager.Instance.TileList[T] != null)
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected && TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    tile.Add(TilesManager.Instance.TileList[T]);
                }
            }
        }

        foreach (int T in PlayerStatic.GetNeighbourDiag(tileId - 1, TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>().Line, false))
        {
            if (TilesManager.Instance.TileList[T] != null)
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected && TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    tile.Add(TilesManager.Instance.TileList[T]);
                }
            }
        }

        foreach (int T in PlayerStatic.GetNeighbourDiag(tileId - 9, TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>().Line, false))
        {
            if (TilesManager.Instance.TileList[T] != null)
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected && TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    tile.Add(TilesManager.Instance.TileList[T]);
                }
            }
        }

        foreach (int T in PlayerStatic.GetNeighbourDiag(tileId + 9, TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>().Line, false))
        {
            if (TilesManager.Instance.TileList[T] != null)
            {
                if (TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != RaycastManager.Instance.ActualUnitSelected && TilesManager.Instance.TileList[T].GetComponent<TileScript>().Unit != null)
                {
                    tile.Add(TilesManager.Instance.TileList[T]);
                }
            }
        }
        
        tile.Remove(TilesManager.Instance.TileList[GetComponent<UnitScript>().ActualTiledId].GetComponent<GameObject>());

        GameManager.Instance._eventCall += EndCpty;
        GameManager.Instance._eventCallCancel += StopCpty;
        GameManager.Instance.StartEventModeTiles(1, false, tile, "Soin/Immobilisation", "Voulez-vous vraiment soigner/immobiliser cette unitée ?");
        base.StartCpty();
        
    }


    public override void StopCpty()
    {
        GameManager.Instance.StopEventModeTile();
        GameManager.Instance.TileChooseList.Clear();
        GetComponent<UnitScript>().StopCapacity(true);
        base.StopCpty();
    }

    public override void EndCpty()
    {
        if (GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy != GameManager.Instance.IsPlayerRedTurn)
        {
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().AddStatutToUnit(MYthsAndSteel_Enum.UnitStatut.Immobilisation);
           if(GameManager.Instance.IsPlayerRedTurn)
           {
                GameManager.Instance.statetImmobilisation = 1;
           }
           else
           {
                GameManager.Instance.statetImmobilisation = 2;
           }
        }
        else if (GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().UnitSO.IsInRedArmy == GameManager.Instance.IsPlayerRedTurn)   
        {
            GameManager.Instance.TileChooseList[0].GetComponent<TileScript>().Unit.GetComponent<UnitScript>().GiveLife(2);
        }
        
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        base.EndCpty();
        GameManager.Instance.TileChooseList.Clear();
        
    }
}
