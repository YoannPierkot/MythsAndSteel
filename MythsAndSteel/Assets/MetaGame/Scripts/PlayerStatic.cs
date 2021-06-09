using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class PlayerStatic{
    /// <summary>
    /// Est ce que la valeur du joueur est supérieur ou égale au cout
    /// </summary>
    /// <param name="Cout"></param>
    /// <param name="ValeurJoueur"></param>
    /// <returns></returns>
    public static bool IsBGreaterThanA(int Cout, int ValeurJoueur){
        //Retourne true si la valeur est plus grande que le cout
        return ValeurJoueur > Cout ? true : false;
    }

    /// <summary>
    /// Est ce que l'unité appartient bien à cette armée ? On doit y insérer notre scriptable.
    /// </summary>
    /// <param name="unite"></param>
    /// <param name="J1"></param>
    /// <returns></returns>
    public static bool CheckIsUnitArmy(UnitScript unite, bool RedPlayer)
    {
        //Permet de dire si l'unité fait partie de l'armée, par défault il est null (ni oui, ni non).
        bool IsUnitInArmy;
        //Si la valeur de l'armée est égale au cout alors..
        if (unite.UnitSO.IsInRedArmy == RedPlayer)
        {
            //..Oui l'unité fait partie de l'armée.
            IsUnitInArmy = true;
        }
        else{
            //..Non l'unité fait partie de l'armée.
            IsUnitInArmy = false;
        }
        return IsUnitInArmy;
    }

    /// <summary>
    /// Remplacer le Unite par Tile
    /// </summary>
    /// <param name="idTile"></param>
    /// <param name="ligne"></param>
    /// <param name="diag"></param>
    /// <returns></returns>
    public static List<int> GetNeighbourDiag(int idTile, int ligne, bool diag)
    {
        //Je prend la position du raycast, a partir de cette position je prends son ID.
        //De cette position, j'ajoute +9 pour la tile au nord, -9 pour la tile au Sud, +1 pour la tile à l'est et -1 pour la tile à l'ouest.
        //Pour la diagonale, j'ajoute +10 pour la tile au Nord Est, +8 pour la tile au Nord Ouest, -10 pour le Sud Ouest et -8 pour le Sud Est.
        //De la je return la valeur de ces ID.
        List<int> currentList = new List<int>();

        int witchLeftLine = 9 * (ligne - 1);
        int witchRightLine = (9 * ligne) - 1;

        //Définit la position sur le quadrillage
        int topTile = idTile + 9;
        int downTile = idTile - 9;
        int leftTile = idTile - 1;
        int rightTile = idTile + 1;

        if (diag){
            int lTopTile = idTile + 8;
            int rTopTile = idTile + 10;

            int lDownTile = idTile - 10;
            int rDownTile = idTile - 8;

            currentList.Add(lTopTile);
            currentList.Add(rTopTile);
            currentList.Add(lDownTile);
            currentList.Add(rDownTile);

            if (topTile > 80 || idTile == witchLeftLine){
                currentList.Remove(lTopTile);
            }

            if (topTile > 80 || idTile == witchRightLine){
                currentList.Remove(rTopTile);
            }

            if (downTile < 0 || idTile == witchLeftLine){
                currentList.Remove(lDownTile);
            }

            if (downTile < 0 || idTile == witchRightLine){
                currentList.Remove(rDownTile);
            }
        }

        currentList.Add(topTile);
        currentList.Add(downTile);
        currentList.Add(leftTile);
        currentList.Add(rightTile);

        if (idTile == witchLeftLine){
            currentList.Remove(leftTile);
        }

        if (idTile == witchRightLine){
            currentList.Remove(rightTile);
        }
        if (downTile < 0){
            currentList.Remove(downTile);
        }

        if (topTile > 80){
            Debug.Log("bonsoir à tous");
            currentList.Remove(topTile);
        }
        return currentList;
    }

    /// <summary>
    /// Permet de savoir si une tile est voisine d'une autre tile
    /// </summary>
    /// <param name="Id1"></param>
    /// <param name="Id2"></param>
    /// <param name="LineId1"></param>
    /// <param name="Diag"></param>
    /// <returns></returns>
    public static bool IsNeighbour(int Id1, int Id2, int LineId1, bool Diag){
        if(GetNeighbourDiag(Id1, LineId1, Diag).Contains(Id2)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// Cherche si la tile en id possède l'effet de terrain demandé
    /// </summary>
    /// <param name="effectToCheck"></param>
    /// <param name="idTiles"></param>
    /// <returns></returns>

    public static bool CheckTiles(MYthsAndSteel_Enum.TerrainType effectToCheck, int idTiles){
        bool hasTiles = false;
        hasTiles = TilesManager.Instance.TileList[idTiles].GetComponent<TileScript>().TerrainEffectList.Contains(effectToCheck);
        return hasTiles;
    }

    /// <summary>
    /// obtiens la direction de la tile
    /// </summary>
    public static MYthsAndSteel_Enum.Direction CheckDirection(int OriginTile, int NextTile)
    {
        int Result = NextTile - OriginTile;
        switch (Result)
        {
            case 9: return MYthsAndSteel_Enum.Direction.Nord;
            case -9: return MYthsAndSteel_Enum.Direction.Sud; 
            case -1: return MYthsAndSteel_Enum.Direction.Ouest;
            case 1: return MYthsAndSteel_Enum.Direction.Est;
        }
        return MYthsAndSteel_Enum.Direction.None;
    }
}

