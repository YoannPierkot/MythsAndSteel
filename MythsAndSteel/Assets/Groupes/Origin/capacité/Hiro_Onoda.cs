using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiro_Onoda : Capacity
{
    public override void StartCpty()
    {
        List<GameObject> unitlist = new List<GameObject>();

        foreach (GameObject gam in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
        {
            if (gam.GetComponent<UnitScript>().UnitSO.typeUnite == MYthsAndSteel_Enum.TypeUnite.Infanterie && gam != gameObject)
            {
                unitlist.Add(gam);
            }
        }
  

        EventUnit(2, GameManager.Instance.IsPlayerRedTurn ? true : false, unitlist, "Regroupement Instantané", "Êtes-vous sûr de vouloir déplacer ces deux unités à côté de Hiro Onoda");
        GameManager.Instance._eventCall += UseCpty;
    }

    void UseCpty()
    {
        List<GameObject> tileList = new List<GameObject>();
        List<int> unitNeigh = PlayerStatic.GetNeighbourDiag(GetComponent<UnitScript>().ActualTiledId, TilesManager.Instance.TileList[GetComponent<UnitScript>().ActualTiledId].GetComponent<TileScript>().Line, false);
        foreach (int i in unitNeigh)
        {
            if(TilesManager.Instance.TileList[i].GetComponent<TileScript>().Unit == null)
            {

            tileList.Add(TilesManager.Instance.TileList[i]);
            }
        }

        EventTile(2, GameManager.Instance.IsPlayerRedTurn, tileList, "Regroupement Instantané", "Êtes-vous sur de vouloir déplacer les unités sur ces cases?", false);
        GameManager.Instance._eventCall += EndCpty;

    }

    public override void EndCpty()
    {
        for (int i = 0; i < GameManager.Instance.UnitChooseList.Count; i++)
        {
            GameManager.Instance.TileChooseList[i].GetComponent<TileScript>().AddUnitToTile(GameManager.Instance.UnitChooseList[i].gameObject);
        }
        GameManager.Instance._eventCall -= EndCpty;
        GetComponent<UnitScript>().EndCapacity();
        base.EndCpty();
        GameManager.Instance.TileChooseList.Clear();
    }
}