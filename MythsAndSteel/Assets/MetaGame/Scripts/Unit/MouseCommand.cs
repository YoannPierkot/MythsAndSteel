using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script contenant les controles clavier+souris pour l'UI ainsi que le MouseOver quand la souris survole un élément d'UI.
/// </summary>
public class MouseCommand : MonoBehaviour
{

    #region Variables
    [Header("UNIT REFERENCE")]
    public UnitReference unitReference = null;

    public bool _checkIfPlayerAsClic;

    public bool _hasCheckUnit = false;

    [Header("UI STATIQUE UNITE")]
    //Le panneau à afficher lorsqu'on souhaite voir les statistiques de l'unité en cliquant.
    [SerializeField] private GameObject _mouseOverUI;
    public GameObject MouseOverUI => _mouseOverUI;
    //Le panneau ou les panneaux à afficher lorsqu'on souhaite le shift click sur l'unité
    [SerializeField] private List<GameObject> _shiftUI;
    public List<GameObject> ShiftUI => _shiftUI;

    [Header("DELAI ATTENTE MOUSE OVER")]
    //Paramètre de délai qui s'applique à la coroutine.
    [SerializeField] private float _timeToWait = 2f;
    public float TimeToWait => _timeToWait;

    [Header("VALEUR POSITION UI")]
    //Permet de modifier la position de l'UI dans l'espace
    [SerializeField] private float _offsetXActivationMenu;
    [SerializeField] private float _offsetYActivationMenu;
    [Space]
    [SerializeField] private float _offsetXMouseOver;
    [SerializeField] private float _offsetYMouseOver;
    [Space]
    [SerializeField] private float _offsetXStatPlus;
    [SerializeField] private float _offsetYStatPlus;
    [Space]
    [SerializeField] private Vector2 _xOffsetMin;
    [SerializeField] private Vector2 _yOffsetMin;
    [SerializeField] private Vector2 _xOffset;
    [SerializeField] private Vector2 _yOffset;
    [SerializeField] private Vector2 _xOffsetMax;
    [SerializeField] private Vector2 _yOffsetMax;

    [Header("UI RENFORT UNITE")]
    [SerializeField] private GameObject _renfortUI;
    public GameObject RenfortUI => _renfortUI;

    [SerializeField] private List<GameObject> _elementMenuRenfort = null;
    public List<GameObject> ElementOfMenuRenfort => _elementMenuRenfort;

    [Header("UNIT ICONE")]
    [SerializeField] private UnitIcon _iconeUnit = null;
    #endregion Variables

    #region UpdateStats
    void UpdateUIStats()
    {

        //Si la tile ne contient pas d'effet de terrain, on n'affiche pas d'information. Si la tile contient 1 effet, on affiche et met à jour l'effet de la case. Si la tile contient 2 effets, on affiche les 2 Effets.
        UIInstance UI = UIInstance.Instance;

        //Statistique pour le MouseOver.
        UnitScript unit = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>();

        UI.TitlePanelMouseOver.GetComponent<TextMeshProUGUI>().text = unit.UnitSO.UnitName;
        UI.MouseOverStats._lifeGam.GetComponent<TextMeshProUGUI>().text = unit.Life.ToString();
        UI.MouseOverStats._rangeGam.GetComponent<TextMeshProUGUI>().text = unit.AttackRange.ToString();
        UI.MouseOverStats._moveGam.GetComponent<TextMeshProUGUI>().text = unit.MoveSpeed.ToString();

        switch (unit.UnitSO.typeUnite)
        {
            case MYthsAndSteel_Enum.TypeUnite.Infanterie:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.infanterieSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Infanterie";
                break;
            case MYthsAndSteel_Enum.TypeUnite.Artillerie:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.ArtillerieSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Artillerie";
                break;
            case MYthsAndSteel_Enum.TypeUnite.Vehicule:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.VehiculeSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Char";
                break;
            case MYthsAndSteel_Enum.TypeUnite.Mythe:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.MytheSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Mythe";
                break;
            case MYthsAndSteel_Enum.TypeUnite.Mecha:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.MechaSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Mécha";
                break;
            case MYthsAndSteel_Enum.TypeUnite.Leader:
                UI.PageUnitStat._unitSpriteGam.GetComponent<Image>().sprite = _iconeUnit.LeaderSprite;
                UI.PageUnitStat._unitSpriteGam.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Leader";
                break;
        }

        //Statistique de la Page 1 du Carnet.
        //Synchronise le texte du titre.
        UI.TitlePanelShiftClicPage1.GetComponent<TextMeshProUGUI>().text = unit.UnitSO.UnitName;
        //Synchronise le texte de la vie avec l'emplacement d'UI.
        UI.PageUnitStat._lifeGam.GetComponent<TextMeshProUGUI>().text = unit.Life.ToString();
        //Synchronise le texte de la valeur de la distance d'attaque de l'unité avec l'emplacement d'UI.
        UI.PageUnitStat._rangeGam.GetComponent<TextMeshProUGUI>().text = unit.AttackRange.ToString();
        //Synchronise le texte de la valeur de la vitesse de l'unité avec l'emplacement d'UI.
        UI.PageUnitStat._moveGam.GetComponent<TextMeshProUGUI>().text = unit.MoveSpeed.ToString();

        //Synchronise le texte de l'UI de la avec l'emplacement d'UI.
        UI.AttackStat._rangeMinDamageGam.GetComponent<TextMeshProUGUI>().text = unit.NumberRangeMin.x.ToString() + " - " + unit.NumberRangeMin.y.ToString();
        UI.AttackStat._rangeMaxDamageGam.GetComponent<TextMeshProUGUI>().text = unit.NumberRangeMax.x.ToString() + " - " + unit.NumberRangeMax.y.ToString();
        UI.AttackStat._minDamageValueGam.GetComponent<TextMeshProUGUI>().text = unit.DamageMinimum.ToString();
        UI.AttackStat._maxDamageValueGam.GetComponent<TextMeshProUGUI>().text = unit.DamageMaximum.ToString();

        for (int i = UI.capacityList.Count - 1; i >= 0; i--)
        {
            Destroy(UI.capacityList[UI.capacityList.Count - 1]);
            UI.capacityList.RemoveAt(UI.capacityList.Count - 1);
        }

        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().Unit.TryGetComponent<Capacity>(out Capacity Capa))
        {
            int contentSize = 0;
            // CAPACITY 1.             
            if (Capa.ReturnInfo(UI.capacityPrefab, 0) != null)
            {
                UI.capacityParent.transform.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
                GameObject CAPA1 = Instantiate(Capa.ReturnInfo(UI.capacityPrefab, 0), Vector2.zero, Quaternion.identity);
                CAPA1.transform.SetParent(UI.capacityParent.transform);
                CAPA1.transform.localScale = new Vector3(.9f, .9f, .9f);
                UI.capacityList.Add(CAPA1);

                int lengthTxt = CAPA1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text.Length;
                float LengthLine = (float)lengthTxt / 21;
                int truncateLine = (int)LengthLine;
                int capaSize = 130 + (20 * truncateLine);
                contentSize += capaSize;
            }

            UI.capacityParent.GetComponent<RectTransform>().sizeDelta = new Vector2(UI.capacityParent.GetComponent<RectTransform>().sizeDelta.x, contentSize);
        }

        //Attributs
        MYthsAndSteel_Enum.Attributs[] _UnitAttributs = RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitSO.UnitAttributs;
        for (int i = 0; i < 3; i++)
        {
            if (_UnitAttributs.Length > i)
            {
                if (_UnitAttributs[i] == MYthsAndSteel_Enum.Attributs.Aucun)
                {
                    UIInstance.Instance.objectsAttributs[i].MainObjects.SetActive(false);
                    continue;
                }

                foreach (TextSpriteAttributUnit attribut in UIInstance.Instance.textSpriteAttributUnit)
                {
                    if (attribut._attributs == _UnitAttributs[i])
                    {
                        GameObject gam = UIInstance.Instance.objectsAttributs[i].MainObjects;
                        gam.SetActive(true);
                        gam.transform.GetChild(0).GetComponent<Image>().sprite = attribut.SpriteAttributUnit;

                        UIInstance.Instance.objectsAttributs[i].Description.GetComponent<TextMeshProUGUI>().text = attribut._name + " :" + attribut.TextAttributUnit;
                        continue;
                    }
                }
            }
            else
            {
                UIInstance.Instance.objectsAttributs[i].MainObjects.SetActive(false);
                continue;
            }
        }

        //Statistique de la Page 2 du Carnet.  
        //Compléter avec les Images des Tiles.

        for (int i = UI.effetDeTerrain.Count - 1; i >= 0; i--)
        {
            Destroy(UI.effetDeTerrain[UI.effetDeTerrain.Count - 1]);
            UI.effetDeTerrain.RemoveAt(UI.effetDeTerrain.Count - 1);
        }

        UI.parentSlotEffetDeTerrain.transform.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        foreach (MYthsAndSteel_Enum.TerrainType Terrain in RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList)
        {
            GameObject Effet = Instantiate(UI.Terrain.ReturnInfo(UI.prefabSlotEffetDeTerrain, Terrain), UI.parentSlotEffetDeTerrain.transform.position, Quaternion.identity);
            Effet.transform.SetParent(UI.parentSlotEffetDeTerrain.transform);
            Effet.transform.localScale = new Vector3(.9f, .9f, .9f);
            UI.effetDeTerrain.Add(Effet);

            UI.parentSlotEffetDeTerrain.GetComponent<RectTransform>().sizeDelta = new Vector2(UI.parentSlotEffetDeTerrain.GetComponent<RectTransform>().sizeDelta.x, 212 * UI.effetDeTerrain.Count);
        }

        if (RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Count == 0)
        {
            GameObject Effet = Instantiate(UI.prefabSlotEffetDeTerrain, UI.parentSlotEffetDeTerrain.transform.position, Quaternion.identity);
            Effet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Liste vide.";
            Effet.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Cette unité n'a actuellement aucun pouvoir.";
            Effet.transform.SetParent(UI.parentSlotEffetDeTerrain.transform);
            Effet.transform.localScale = new Vector3(.9f, .9f, .9f);
            UI.parentSlotEffetDeTerrain.GetComponent<RectTransform>().sizeDelta = new Vector2(UI.parentSlotEffetDeTerrain.GetComponent<RectTransform>().sizeDelta.x, 212 * UI.Statuts.Count);
        }

        for (int i = UI.Statuts.Count - 1; i >= 0; i--)
        {
            Destroy(UI.Statuts[UI.Statuts.Count - 1]);
            UI.Statuts.RemoveAt(UI.Statuts.Count - 1);
        }

        UI.parentSlotStatuts.transform.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        if(RaycastManager.Instance.UnitInTile != null)
        {
            foreach (MYthsAndSteel_Enum.UnitStatut status in RaycastManager.Instance.UnitInTile.GetComponent<UnitScript>().UnitStatuts)
            {
                GameObject Effet = Instantiate(UI.StatusSc.ReturnInfo(UI.prefabSlotStatuts, status), UI.parentSlotStatuts.transform.position, Quaternion.identity);
                Effet.transform.SetParent(UI.parentSlotStatuts.transform);
                Effet.transform.localScale = new Vector3(.9f, .9f, .9f);
                UI.Statuts.Add(Effet);

                UI.parentSlotStatuts.GetComponent<RectTransform>().sizeDelta = new Vector2(UI.parentSlotStatuts.GetComponent<RectTransform>().sizeDelta.x, 212 * UI.Statuts.Count);
            }
        }
    }

    #endregion UpdateStats

    private void Update()
    {
        UIInstance UI = UIInstance.Instance;
        GameObject Tile = RaycastManager.Instance.Tile;

        //Update des info de la tile sur le pannel du bas quand
        UI.CallUpdateUI(Tile);
    }

    #region ActivateUI
    /// <summary>
    /// Permet d'activer un élément de l'UI en utilisant un Raycast distint de la position et d'assigner une position custom par rapport au Canvas (Conflit avec le Canvas).
    /// </summary>
    /// <param name="uiElements"></param>
    /// <param name="offSetX"></param>
    /// <param name="offSetY"></param>
    public void ActivateUI(GameObject uiElements, float lastPosX = 0, float lastPosY = 0, bool switchPage = false, bool activationMenu = false, bool mouseOver = false, bool bigStat = false)
    {
        //Reprendre la position du raycast qui a sélectionné la tile
        RaycastHit2D hit = RaycastManager.Instance.GetRaycastHit();

        //Je stop l'ensemble des coroutines en cours.
        Vector3 pos = Vector3.zero;
        StopAllCoroutines();

        //Menu d'activation d'une unité
        if (activationMenu)
        {
            if (hit.transform.position.x >= _xOffset.y)
            {
                if (hit.transform.position.x >= _xOffsetMax.y)
                {
                    if (hit.transform.position.y >= _yOffset.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffset.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffset.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffset.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
            }

            else if (hit.transform.position.x <= _xOffset.x)
            {
                if (hit.transform.position.x <= _xOffsetMax.x)
                {
                    if (hit.transform.position.y >= _yOffset.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffset.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffset.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffset.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if (hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y - _offsetYActivationMenu, hit.transform.position.z));
                }
                else if (hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y + _offsetYActivationMenu, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXActivationMenu, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }

        //Menu mouseOver
        else if (mouseOver)
        {
            if (hit.transform.position.x >= _xOffset.y)
            {
                if (hit.transform.position.x >= _xOffsetMax.y)
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y <= _yOffsetMax.y && hit.transform.position.y >= _yOffset.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
            }

            else if (hit.transform.position.x <= _xOffset.x)
            {
                if (hit.transform.position.x <= _xOffsetMax.x)
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXMouseOver, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if (hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver / 2, hit.transform.position.y - _offsetYMouseOver - .5f, hit.transform.position.z));
                }
                else if (hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver / 2, hit.transform.position.y + _offsetYMouseOver + .5f, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXMouseOver * 2.5f, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }

        //Menu avec toutes les stats
        else if (bigStat)
        {
            if (hit.transform.position.x >= _xOffset.y)
            {
                if (hit.transform.position.x >= _xOffsetMax.y)
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y <= _yOffsetMax.y && hit.transform.position.y >= _yOffset.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
            }

            else if (hit.transform.position.x <= _xOffset.x)
            {
                if (hit.transform.position.x <= _xOffsetMax.x)
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
                else
                {
                    if (hit.transform.position.y >= _yOffsetMin.y)
                    {
                        if (hit.transform.position.y >= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffset.y && hit.transform.position.y <= _yOffsetMax.y)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                        }
                    }
                    else if (hit.transform.position.y <= _yOffsetMin.x)
                    {
                        if (hit.transform.position.y <= _yOffsetMax.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + 1, hit.transform.position.z));
                        }
                        else if (hit.transform.position.y >= _yOffsetMax.x && hit.transform.position.y <= _yOffset.x)
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                        else
                        {
                            pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x + _offsetXStatPlus, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                        }
                    }
                    else
                    {
                        pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                    }
                }
            }
            else
            {
                if (hit.transform.position.y >= _yOffsetMax.y)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus / 2, hit.transform.position.y - _offsetYStatPlus - .5f, hit.transform.position.z));
                }
                else if (hit.transform.position.y <= _yOffsetMax.x)
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus / 2, hit.transform.position.y + _offsetYStatPlus + .5f, hit.transform.position.z));
                }
                else
                {
                    pos = Camera.main.WorldToScreenPoint(new Vector3(hit.transform.position.x - _offsetXStatPlus * 2.5f, hit.transform.position.y, hit.transform.position.z));
                }
            }
        }
        else if (switchPage)
        {
            pos = new Vector3(lastPosX, lastPosY, ShiftUI[0].transform.position.z);
        }
        else
        {
            Debug.LogError("Vous essayez de positionner un objet qui ne peut pas se positionner autour de l'unité");
        }

        //Rendre l'élément visible.
        uiElements.SetActive(true);

        //Si la position de l'UI est différente de celle de la position de référence alors tu prends cette position comme référence.
        if (uiElements.transform.position != pos)
        {
            uiElements.transform.position = pos;
        }
    }
    #endregion ActivateUI

    #region ControleDesClicks
    /// <summary>
    /// Permet de déterminer quand le joueur appuie sur le Shift puis le clic Gauche de la souris.
    /// </summary>
    public void ShiftClick()
    {
        ActivateUI(ShiftUI[0], 0, 0, false, false, false, true);
        UpdateUIStats();
    }

    /// <summary>
    /// Permet de déterminer et d'afficher un élément quand la souris passe au dessus d'une tile possédant une unité.
    /// </summary>
    public void MouseOverWithoutClick()
    {
        if (GameManager.Instance.activationDone == false)
        {

        if (!_hasCheckUnit)
        {
            //Si le joueur n'a pas cliqué, alors tu lances la coroutine.
            if (_checkIfPlayerAsClic == false)
            {
                //Coroutine : Une coroutine est une fonction qui peut suspendre son exécution (yield) jusqu'à la fin de la YieldInstruction donnée.
                StartCoroutine(ShowObject(TimeToWait));
                UpdateUIStats();
                _hasCheckUnit = true;
            }
        }
        if (_checkIfPlayerAsClic)
        {
            MouseExitWithoutClick();
        }
        }
    }

    /// <summary>
    /// Fonction pour désactiver en MouseOver.
    /// </summary>
    public void MouseExitWithoutClick()
    {
        if(GameManager.Instance.activationDone == false)
        {
        //Arrete l'ensemble des coroutines dans la scène.
        StopAllCoroutines();
        _mouseOverUI.SetActive(false);
        _hasCheckUnit = false;

        }
    }

    /// <summary>
    /// Correspond au paramètre qu'on rentre dans la coroutine.
    /// </summary>
    /// <param name="Timer"></param>
    /// <returns></returns>
    IEnumerator ShowObject(float TimeToWait)
    {
        //J'utilise un délai pour que le boutton apparaisse après un délai.
        yield return new WaitForSeconds(TimeToWait);
        //J'active l'élément et je lui assigne des paramètres.
        ActivateUI(MouseOverUI, 0, 0, false, false, true);
    }
    #endregion ControleDesClicks

    #region SwitchPages
    /// <summary>
    /// Fonction qui permet de cacher les Pages 1 et 2 du carnet.
    /// </summary>
    public void QuitShiftPanel()
    {
        //Je retourne la valeur comme quoi il a clické à false car il a fini son action de Shift+Clic et désactive les 2 pages.
        for (int i = 0; i < 3; i++)
        {
            UIInstance.Instance.objectsAttributs[i].MainObjects.transform.GetChild(0).GetComponent<MouseOverUI>().StopOver();
        }

        _checkIfPlayerAsClic = false;
        ShiftUI[0].SetActive(false);
        ShiftUI[1].SetActive(false);
    }

    /// <summary>
    /// Permet de switch entre la page 1 et la page 2
    /// </summary>
    public void switchWindows1()
    {
        //J'active le Panneau 2 car le joueur a cliqué sur le bouton permettant de transitionner de la page 1 à la page 2. De plus, je masque la page 1.
        ActivateUI(ShiftUI[1], ShiftUI[0].transform.position.x, ShiftUI[0].transform.position.y, true);
        ShiftUI[0].SetActive(false);
    }

    /// <summary>
    /// Switch entre la page 2 et la page 1.
    /// </summary>
    public void switchWindows2()
    {
        //J'active le Panneau 1 car le joueur a cliqué sur le bouton permettant de transitionner de la page 2 à la page 1. De plus, je masque la page 2.
        ActivateUI(ShiftUI[0], ShiftUI[1].transform.position.x, ShiftUI[1].transform.position.y, true);
        ShiftUI[1].SetActive(false);
    }
    #endregion SwitchPages

    #region MenuRenfortFunction
    /// <summary>
    /// Quitte le menu renfort
    /// </summary>
    public void QuitRenfortPanel()
    {
        if (!GameManager.Instance.IsPlayerRedTurn)
        {
            GameManager.Instance.RenfortPhase.CreateTileJ2.Clear();
            GameManager.Instance.RenfortPhase.CreateLeader2.Clear();
        }
        else if (GameManager.Instance.IsPlayerRedTurn)
        {
            GameManager.Instance.RenfortPhase.CreateTileJ1.Clear();
            GameManager.Instance.RenfortPhase.CreateLeader1.Clear();
        }

        UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité1.GetComponent<RenfortBtnUI>().HideCanvas();

        UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité2.GetComponent<RenfortBtnUI>().HideCanvas();

        UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité3.GetComponent<RenfortBtnUI>().HideCanvas();

        UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité4.GetComponent<RenfortBtnUI>().HideCanvas();

        UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité5.GetComponent<RenfortBtnUI>().HideCanvas();

        UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().interactable = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<CanvasGroup>().blocksRaycasts = false;
        UIInstance.Instance.ButtonRenfort._clicSurUnité6.GetComponent<RenfortBtnUI>().HideCanvas();


        RenfortUI.SetActive(false);
        UIInstance.Instance.RenfortBlockant.SetActive(false);
    }

    /// <summary>
    /// Update les stats du menu renfort
    /// </summary>
    void UpdateStatsMenuRenforts(bool player)
    {
        if (player)
        {

            UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = PlayerScript.Instance.RedPlayerInfos.Ressource.ToString();

            //Permet de déterminer le nombre d'emplacements à mettre à jour sur le menu Renfort de l'Armée Rouge.
            for (int i = 2; i < unitReference.UnitClassCreableListRedPlayer.Count; i++)
            {
                #region UpdateTexteRenfort1a3
                //Active les différents UI des unités de 1 à 3.
                _elementMenuRenfort[0].SetActive(true);
                _elementMenuRenfort[1].SetActive(true);
                _elementMenuRenfort[2].SetActive(true);



                //Statistique pour l'unité1
                UIInstance.Instance.PageUnitéRenfort._nameUnit1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                //Statistique pour l'unité2
                UIInstance.Instance.PageUnitéRenfort._nameUnit2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                //Statistique pour l'unité3
                UIInstance.Instance.PageUnitéRenfort._nameUnit3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                #endregion UpdateTexteRenfort1a3

                #region UpdateImageRenfort1a3
                //Update Ressource en fonction du nombre.

               
                    //Si la première unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[0].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(false);
                    }

                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[1].SetActive(true);
                    //Image Ressource pour l'unité 2 de l'armée Rouge
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[1].SetActive(true);

                    //Si la deuxième unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[1].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(false);
                    }
                    //Image Ressource pour l'unité 3 de l'armée Rouge
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[1].SetActive(true);

                    //Si la troisième unité de l'armée Rouge a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListRedPlayer[2].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(false);
                    }



                    #endregion UpdateImageRenfort1a3

                    #region Update Textuelle et Image Renforts de 4 à 6 pour l'équipe Rouge
                    //Si la liste des unités créables comportent plus de 3 unités dans la liste de l'équipe Rouge.
                    if (i >= 3)
                    {
                        //Active l'UI de l'unité 4 de l'arrmée Rouge.
                        _elementMenuRenfort[3].SetActive(true);

                        //Statistique pour l'unité4
                        UIInstance.Instance.PageUnitéRenfort._nameUnit4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                        UIInstance.Instance.PageUnitéRenfort._lifeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                        UIInstance.Instance.PageUnitéRenfort._rangeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                        UIInstance.Instance.PageUnitéRenfort._moveValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                        UIInstance.Instance.PageUnitéRenfort._damageValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                        //Image Ressource pour l'unité 4 de l'armée Rouge.
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[0].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[1].SetActive(true);

                        //Si la quatrième unité de l'armée Rouge a besoin de plus de 2 ressources.
                        if (unitReference.UnitClassCreableListRedPlayer[3].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(true);
                        }
                        else
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(false);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(false);
                        }

                        //Si la liste des unités créables comportent plus de 4 unités dans la liste de l'équipe Rouge.
                        if (i >= 4)
                        {
                            //Active l'UI de l'unité 5 de l'arrmée Rouge.
                            _elementMenuRenfort[4].SetActive(true);

                            //Statistique pour l'unité 5 de l'armée Rouge.
                            UIInstance.Instance.PageUnitéRenfort._nameUnit5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                            UIInstance.Instance.PageUnitéRenfort._lifeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                            UIInstance.Instance.PageUnitéRenfort._rangeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                            UIInstance.Instance.PageUnitéRenfort._moveValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                            UIInstance.Instance.PageUnitéRenfort._damageValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                            //Image Ressource pour l'unité 5 de l'armée Rouge.
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[0].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[1].SetActive(true);

                            //Si la cinquième unité de l'armée Rouge a besoin de plus de 2 ressources.
                            if (unitReference.UnitClassCreableListRedPlayer[4].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(true);
                            }
                            else
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(false);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(false);
                            }

                            //Si la liste des unités créables comportent plus de 5 unités dans la liste de l'équipe Rouge.
                            if (i >= 5)
                            {
                                //Active l'UI de l'unité 6 de l'arrmée Rouge.
                                _elementMenuRenfort[5].SetActive(true);

                                //Statistique pour l'unité6 de l'armée Rouge.
                                UIInstance.Instance.PageUnitéRenfort._nameUnit6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                                UIInstance.Instance.PageUnitéRenfort._lifeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                                UIInstance.Instance.PageUnitéRenfort._rangeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                                UIInstance.Instance.PageUnitéRenfort._moveValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                                UIInstance.Instance.PageUnitéRenfort._damageValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                                //Image Ressource pour l'unité6 de l'armée Rouge.
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[0].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[1].SetActive(true);

                                //Si la sixième unité de l'armée Rouge a besoin de plus de 2 ressources.
                                if (unitReference.UnitClassCreableListRedPlayer[5].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(true);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(true);
                                }
                                else
                                {
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(false);
                                    UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(false);
                                }

                            }
                            else
                            {
                                _elementMenuRenfort[5].SetActive(false);
                            }
                        }
                        else
                        {
                            _elementMenuRenfort[4].SetActive(false);
                            _elementMenuRenfort[5].SetActive(false);
                        }
                    }
                    else
                    {
                        _elementMenuRenfort[3].SetActive(false);
                        _elementMenuRenfort[4].SetActive(false);
                        _elementMenuRenfort[5].SetActive(false);
                    }
                    #endregion Update Textuelle et Image Renforts de 4 à 6 pour l'équipe Rouge
                

            }
        }

        else if (!player)
        {

            UIInstance.Instance.PageUnitéRenfort._ressourceJoueur.GetComponent<TextMeshProUGUI>().text = PlayerScript.Instance.BluePlayerInfos.Ressource.ToString();
            //Permet de déterminer le nombre d'emplacements à mettre à jour sur le menu Renfort de l'Armée Bleu.
            for (int i = 2; i < unitReference.UnitClassCreableListBluePlayer.Count; i++)
            {

                #region Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Bleu
                #region Update Textuelle Renforts de 1 à 3 pour l'équipe Bleu

                //Active les différents UI des unités 1 à 3 de l'armée Bleu.
                _elementMenuRenfort[0].SetActive(true);
                _elementMenuRenfort[1].SetActive(true);
                _elementMenuRenfort[2].SetActive(true);

                //Statistique pour l'unité 1 de l'armée Bleu.
                UIInstance.Instance.PageUnitéRenfort._nameUnit1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor1.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                //Statistique pour l'unité 2 de l'armée Bleu.
                UIInstance.Instance.PageUnitéRenfort._nameUnit2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor2.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                //Statistique pour l'unité 3 de l'armée Bleu.
                UIInstance.Instance.PageUnitéRenfort._nameUnit3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                UIInstance.Instance.PageUnitéRenfort._lifeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                UIInstance.Instance.PageUnitéRenfort._rangeValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                UIInstance.Instance.PageUnitéRenfort._moveValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                UIInstance.Instance.PageUnitéRenfort._damageValor3.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();
                #endregion Update Textuelle Renforts de 1 à 3 pour l'équipe Bleu

                #region Update Image Renforts de 1 à 3 pour l'équipe Bleu
                //Update Ressource en fonction du nombre.
                //Image Ressource pour l'unité 1 de l'armée Bleu.
                UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[0].SetActive(true);
                UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[1].SetActive(true);

                //Si la première unité de l'armée Bleu a besoin de plus de 2 ressources.
                if (unitReference.UnitClassCreableListBluePlayer[0].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(true);
                }
                else
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[2].SetActive(false);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité1Ressource[3].SetActive(false);
                }

                //Image Ressource pour l'unité 2 de l'armée Bleu.
                UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[0].SetActive(true);
                UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[1].SetActive(true);

                //Si la deuxième unité de l'armée Bleu a besoin de plus de 2 ressources.
                if (unitReference.UnitClassCreableListBluePlayer[1].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(true);
                }
                else
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[2].SetActive(false);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité2Ressource[3].SetActive(false);
                }


                //Image Ressource pour l'unité 3 de l'armée Bleu.
                UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[0].SetActive(true);
                UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[1].SetActive(true);

                //Si la troisième unité de l'armée Bleu a besoin de plus de 2 ressources.
                if (unitReference.UnitClassCreableListBluePlayer[2].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(true);
                }
                else
                {
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[2].SetActive(false);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité3Ressource[3].SetActive(false);
                }

                #endregion Update Image Renforts de 1 à 3 pour l'équipe Bleu
                #endregion Update Textuelle et Image Renforts de 1 à 3 pour l'équipe Bleu

                #region Update Image Textuelle et Image de 4 à 6 pour l'équipe Bleu
                //Update Ressource en fonction du nombre.
                //Si la liste des unités créables comportent plus de 3 unités dans la liste de l'équipe Rouge.
                if (i >= 3)
                {
                    //Active l'UI de l'unité 4 (oui 4 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                    _elementMenuRenfort[3].SetActive(true);

                    //Statistique pour l'unité 4 de l'armée Bleu.
                    UIInstance.Instance.PageUnitéRenfort._nameUnit4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                    UIInstance.Instance.PageUnitéRenfort._lifeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                    UIInstance.Instance.PageUnitéRenfort._rangeValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                    UIInstance.Instance.PageUnitéRenfort._moveValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                    UIInstance.Instance.PageUnitéRenfort._damageValor4.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                    //Image Ressource pour l'unité 4 de l'armée Bleu.
                    UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[0].SetActive(true);
                    UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[1].SetActive(true);

                    //Si la quatrième unité de l'armée Bleu a besoin de plus de 2 ressources.
                    if (unitReference.UnitClassCreableListBluePlayer[3].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(true);
                    }
                    else
                    {
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[2].SetActive(false);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité4Ressource[3].SetActive(false);
                    }

                    //Si la liste des unités créables comportent plus de 4 unités dans la liste de l'équipe Bleu.
                    if (i >= 4)
                    {
                        //Active l'UI de l'unité 5 (oui 5 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                        _elementMenuRenfort[4].SetActive(true);

                        //Statistique pour l'unité5
                        UIInstance.Instance.PageUnitéRenfort._nameUnit5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                        UIInstance.Instance.PageUnitéRenfort._lifeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                        UIInstance.Instance.PageUnitéRenfort._rangeValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                        UIInstance.Instance.PageUnitéRenfort._moveValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                        UIInstance.Instance.PageUnitéRenfort._damageValor5.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();


                        //Image Ressource pour l'unité 5 de l'armée Bleu.
                        UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[0].SetActive(true);
                        UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[1].SetActive(true);

                        //Si la cinquième unité de l'armée Bleu a besoin de plus de 2 ressources.
                        if (unitReference.UnitClassCreableListBluePlayer[4].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(true);
                        }
                        else
                        {
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[2].SetActive(false);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité5Ressource[3].SetActive(false);
                        }

                        //Si la liste des unités créables comportent plus de 5 unités dans la liste de l'équipe Bleu.
                        if (i >= 5)
                        {
                            //Active l'UI de l'unité 6 (oui 6 car dans une liste, le 0 est pris en compte comme l'emplacement 1).
                            _elementMenuRenfort[5].SetActive(true);

                            //Statistique pour l'unité6
                            UIInstance.Instance.PageUnitéRenfort._nameUnit6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.UnitName.ToString();
                            UIInstance.Instance.PageUnitéRenfort._lifeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.LifeMax.ToString();
                            UIInstance.Instance.PageUnitéRenfort._rangeValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.AttackRange.ToString();
                            UIInstance.Instance.PageUnitéRenfort._moveValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.MoveSpeed.ToString();
                            UIInstance.Instance.PageUnitéRenfort._damageValor6.GetComponent<TextMeshProUGUI>().text = unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.DamageMinimum.ToString();

                            //Image Ressource pour l'unité 6 de l'armée Bleu.
                            UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[0].SetActive(true);
                            UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[1].SetActive(true);

                            //Si la sixième unité de l'armée Bleu a besoin de plus de 2 ressources.
                            if (unitReference.UnitClassCreableListBluePlayer[5].GetComponent<UnitScript>().UnitSO.CreationCost > 2)
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(true);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(true);
                            }
                            else
                            {
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[2].SetActive(false);
                                UIInstance.Instance.RessourceUnit_PasTouche._unité6Ressource[3].SetActive(false);
                            }
                        }
                        else
                        {
                            _elementMenuRenfort[5].SetActive(false);
                        }
                    }
                    else
                    {
                        _elementMenuRenfort[4].SetActive(false);
                        _elementMenuRenfort[5].SetActive(false);
                    }
                  
                }
                else
                {
                    _elementMenuRenfort[3].SetActive(false);
                    _elementMenuRenfort[4].SetActive(false);
                    _elementMenuRenfort[5].SetActive(false);
                }

                #endregion Update Image Textuelle et Image de 4 à 6 pour l'équipe Bleu
            }

        }
    }

    /// <summary>
    /// Actives le menu renfort
    /// </summary>
    public void MenuRenfortUI(bool player)
    {
        RenfortUI.SetActive(true);
        UIInstance.Instance.RenfortBlockant.SetActive(true);
        UpdateStatsMenuRenforts(player);
    }
    #endregion MenuRenfortFunction
}

[System.Serializable]
public class UnitIcon
{
    public Sprite infanterieSprite;
    public Sprite ArtillerieSprite;
    public Sprite VehiculeSprite;
    public Sprite LeaderSprite;
    public Sprite MytheSprite;
    public Sprite MechaSprite;
}