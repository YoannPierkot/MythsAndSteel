using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour
{
    [SerializeField] private GameObject _unit;
    public GameObject Unit
    {
        get
        {
            return _unit;
        }
        set
        {
            _unit = value;
        }
    }

    //Liste des enfants de la case
    [SerializeField] private List<GameObject> Child;
    public List<GameObject> _Child
    {
        get
        {
            return Child;
        }
        set
        {
            Child = value;
        }
    }

    //Ligne sur laquelle se trouve la case
    [SerializeField] private int _line;
    public int Line => _line;

    //Id de la case
    [SerializeField] private int _tileId;
    public int TileId => _tileId;

    [Space(20)]
    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.TerrainType> _terrainEffectList = new List<MYthsAndSteel_Enum.TerrainType>();
    public List<MYthsAndSteel_Enum.TerrainType> TerrainEffectList => _terrainEffectList;


    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.EffetProg> _effetProg = new List<MYthsAndSteel_Enum.EffetProg>();
    public List<MYthsAndSteel_Enum.EffetProg> EffetProg => _effetProg;


    [Header("VARIABLES EFFET DE TERRAIN")]
    [SerializeField] MYthsAndSteel_Enum.Owner _ownerObjectiv = MYthsAndSteel_Enum.Owner.neutral;
    public MYthsAndSteel_Enum.Owner OwnerObjectiv => _ownerObjectiv;

    [SerializeField] int _resourcesCounter = 0;
    public int ResourcesCounter => _resourcesCounter;

    private void Start()
    {
        //Met l'unit� � la bonne position
        if (_unit != null)
        {
            _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            _Child.Add(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Ajoute une unit� � cette case
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToTile(GameObject unit, bool inEditor = false)
    {
        _unit = unit;
        _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);

        if (!inEditor) _unit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(this.gameObject);
    }

    /// <summary>
    /// Enleve l'unit� qui se trouve sur cette case
    /// </summary>
    public void RemoveUnitFromTile()
    {
        _unit = null;
    }

    /// <summary>
    /// Active un enfant � l'unit�
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject ActiveChildObj(MYthsAndSteel_Enum.ChildTileType type, Sprite sprite = null, float alpha = 1f)
    {
        GameObject child = null;

        foreach (GameObject gam in _Child)
        {
            string tag = "";
            if (gam.GetComponent<SpriteRenderer>() != null)
            {
                switch (type)
                {
                    case MYthsAndSteel_Enum.ChildTileType.MoveSelect:
                        tag = "MoveSelectable";
                        if (gam.tag == tag)
                        {
                            child = gam;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            if (sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.AttackSelect:
                        tag = "AttackSelectable";
                        if (gam.tag == tag)
                        {
                            child = gam;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            if (sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.EventSelect:
                        tag = "SelectableTile";
                        if (gam.tag == tag)
                        {
                            child = gam;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            if (sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.MoveArrow:
                        tag = "DisplayArrowForMove";
                        if (gam.tag == tag)
                        {
                            child = gam;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            if (sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.MovePath:
                        tag = "DisplayMovePath";
                        if (gam.tag == tag)
                        {
                            child = gam;
                            child.GetComponent<SpriteRenderer>().enabled = true;
                            if (sprite != null) child.GetComponent<SpriteRenderer>().sprite = sprite;
                        }
                        break;
                }
            }
        }
        child.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
        return child;
    }

    /// <summary>
    /// D�sactive un enfant � l'unit�
    /// </summary>
    /// <param name="type"></param>
    /// <param name="destroy"></param>
    /// <returns></returns>
    public GameObject DesActiveChildObj(MYthsAndSteel_Enum.ChildTileType type, bool destroy = false)
    {
        GameObject child = null;

        foreach (GameObject gam in _Child)
        {
            string tag = "";
            if (gam.GetComponent<SpriteRenderer>() != null)
            {
                switch (type)
                {
                    case MYthsAndSteel_Enum.ChildTileType.MoveSelect:
                        tag = "MoveSelectable";
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.AttackSelect:
                        tag = "AttackSelectable";
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.EventSelect:
                        tag = "SelectableTile";
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.MoveArrow:
                        tag = "DisplayArrowForMove";
                        break;
                    case MYthsAndSteel_Enum.ChildTileType.MovePath:
                        tag = "DisplayMovePath";
                        break;
                }

                if (gam.tag == tag)
                {
                    child = gam;
                    Debug.Log("desactive " + child.name);
                    child.GetComponent<SpriteRenderer>().enabled = false;
                    if (destroy)
                    {
                        _Child.Remove(child);
                        Destroy(child);
                        child = null;
                    }
                }
            }
        }
        return child;
    }

    /// <summary>
    /// Change le joueur qui poss�de le drapeau
    /// </summary>
    /// <param name="own"></param>
    public void ChangePlayerObj(MYthsAndSteel_Enum.Owner own)
    {
        _ownerObjectiv = own;
    }

    /// <summary>
    /// Enleve des ressources � la case
    /// </summary>
    /// <param name="value"></param>
    public void RemoveRessources(int value, int player)
    {
        if (_resourcesCounter - value >= 0)
        {
            _resourcesCounter -= value;
            if (player == 1)
            {
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else
            {
                PlayerScript.Instance.BluePlayerInfos.Ressource += value;
            }
        }
         else{
            int ressourceToGice = value - (_resourcesCounter - value);

            if(player == 1){
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else{
                PlayerScript.Instance.BluePlayerInfos.Ressource += value;
            }
         }
    }

    /// <summary>
    /// Ajoute un effet � la case
    /// </summary>
    /// <param name="Type"></param>
    public void CreateEffect(MYthsAndSteel_Enum.TerrainType Type)
    {
        foreach (TerrainType T in GameManager.Instance.Terrain.EffetDeTerrain)
        {
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T._eventType)
            {
                if (T1 == Type)
                {
                    if (!TerrainEffectList.Contains(Type))
                    {
                        TerrainEffectList.Add(Type);
                    }
                    GameObject Child = Instantiate(T.Child, transform.position, Quaternion.identity);
                    Child.transform.parent = this.transform;
                    Child.transform.localScale = new Vector3(.5f, .5f, .5f);
                    _Child.Add(Child);
                }
            }
        }
    }

    /// <summary>
    /// enleve un effet de la case
    /// </summary>
    /// <param name="Type"></param>
    public void RemoveEffect(MYthsAndSteel_Enum.TerrainType Type)
    {
        if (TerrainEffectList.Contains(Type))
        {                       
            TerrainEffectList.Remove(Type);
            foreach (GameObject C in Child)
            {
                if (C.TryGetComponent<ChildEffect>(out ChildEffect T))
                {
                    if (T.Type == Type)
                    {
                        GameObject G = C;
                        Child.Remove(C);
                        Destroy(G);
                        break;
                    }
                }
            }
        }
    }
}
