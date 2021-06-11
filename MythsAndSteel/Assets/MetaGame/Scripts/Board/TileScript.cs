using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour
{
    private bool _firstunit = true;
    public bool firstunit => _firstunit;

    [SerializeField] private GameObject _unit;
    public GameObject Unit
    {
        get
        {
            return _unit;
        }
        set
        {
            LastUnit = _unit;
            _unit = value;
            if (value != null && _firstunit)
            {
                _firstunit = false;
                foreach (MYthsAndSteel_Enum.TerrainType T1 in TerrainEffectList)
                {
                    foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                    {
                        foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                        {
                            if (T1 == T2)
                            {
                                if (Type.Child != null)
                                {
                                    if (Type.MustBeInstantiate)
                                    {
                                        foreach (GameObject G in _Child)
                                        {
                                            if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                            {
                                                if (Try2.Type == T1)
                                                {
                                                    if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                    {
                                                        Try3.FirstUnitOnCase(_unit.GetComponent<UnitScript>());
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                        {
                                            Try.FirstUnitOnCase(_unit.GetComponent<UnitScript>());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (MYthsAndSteel_Enum.TerrainType T1 in TerrainEffectList)
            {
                foreach (TerrainType Type in GameManager.Instance.Terrain.EffetDeTerrain)
                {
                    foreach (MYthsAndSteel_Enum.TerrainType T2 in Type._eventType)
                    {
                        if (T1 == T2)
                        {
                            if (Type.Child != null)
                            {
                                if (Type.MustBeInstantiate)
                                {
                                    foreach (GameObject G in _Child)
                                    {
                                        if (G.TryGetComponent<ChildEffect>(out ChildEffect Try2))
                                        {
                                            if (Try2.Type == T1)
                                            {
                                                if (Try2.TryGetComponent<TerrainParent>(out TerrainParent Try3))
                                                {
                                                    TerrainGestion.Instance.UnitModification(Try3, this);
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    if (Type.Child.TryGetComponent<TerrainParent>(out TerrainParent Try))
                                    {
                                        TerrainGestion.Instance.UnitModification(Try, this);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public GameObject LastUnit;

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
    public List<MYthsAndSteel_Enum.TerrainType> TerrainEffectList
    {
        get
        {
            return _terrainEffectList;
        }
        set
        {
            Debug.Log("Attention, vous venez d'ajouter manuellement un effet de terrain. Ce n'est clairement pas conseillé. Call un méta ou c'est la mort.");
            _terrainEffectList = value;
        }
    }


    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.EffetProg> _effetProg = new List<MYthsAndSteel_Enum.EffetProg>();
    public List<MYthsAndSteel_Enum.EffetProg> EffetProg => _effetProg;


    [Header("VARIABLES EFFET DE TERRAIN")]
    [SerializeField] MYthsAndSteel_Enum.Owner _ownerObjectiv = MYthsAndSteel_Enum.Owner.neutral;
    public MYthsAndSteel_Enum.Owner OwnerObjectiv => _ownerObjectiv;

    [SerializeField] int _resourcesCounter = 0;
    public int ResourcesCounter
    {
        get { return _resourcesCounter; }
        set 
        { 
            if(value < _resourcesCounter)
            {
                foreach(GameObject G in Child)
                {
                    if(G.TryGetComponent<ChildEffect>(out ChildEffect CH))
                    {
                        if (CH.Type == MYthsAndSteel_Enum.TerrainType.Point_de_ressource)
                        {
                            CH.gameObject.GetComponentInChildren<Animator>().SetTrigger("More");
                        }
                    }
                }
            }
            if(value > _resourcesCounter)
            {
                foreach (GameObject G in Child)
                {
                    if (G.TryGetComponent<ChildEffect>(out ChildEffect CH))
                    {
                        if (CH.Type == MYthsAndSteel_Enum.TerrainType.Point_de_ressource)
                        {
                            CH.gameObject.GetComponentInChildren<Animator>().SetTrigger("More");
                        }
                    }
                }
            }
            if(value > 999)
            {
                value = 999;
            }
            if(value < 0)
            {
                value = 0;
            }
            _resourcesCounter = value; 
        
        }
    }

    private void Start()
    {
        //Met l'unit� � la bonne position
        if (_unit != null)
        {
            LastUnit = _unit;
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
        if (!inEditor) unit.GetComponent<UnitScript>().ActualTiledId = TileId;
        Unit = unit;
        Unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);

    }

    /// <summary>
    /// Clear unit without effect.
    /// </summary>
    public void ClearUnitInfo()
    {
        _unit = null;
        LastUnit = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InfoLastAndActualUnit"> K<K<Actual Unit, Last Unit>, Tile> </param>
    public void AddUnitInfo(UnitScript Actual, UnitScript Last)
    {
        if (Actual != null) _unit = Actual.gameObject;
        if (Last != null)
        {
            LastUnit = Last.gameObject;
        }
        else
        {
            LastUnit = null;
        }
    }
    /// <summary>
    /// Enleve l'unit� qui se trouve sur cette case
    /// </summary>
    public void RemoveUnitFromTile()
    {
        Unit = null;
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
        if (ResourcesCounter - value >= 0)
        {
            
            ResourcesCounter -= value;
            if (player == 1)
            {
              
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else
            {
                
                PlayerScript.Instance.BluePlayerInfos.Ressource += value;
            }
        }
        else
        {
            int ressourceToGice = value - (ResourcesCounter - value);

            if (player == 1)
            {
              
                PlayerScript.Instance.RedPlayerInfos.Ressource += value;
            }
            else
            {
                
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
                    Debug.Log(T._terrainName);
                    GameObject Child = Instantiate(T.Child, transform.position, Quaternion.identity);
               
                    
                    

                    Child.transform.parent = this.transform;
                        Child.transform.localScale = new Vector3(.5f, .5f, .5f);
                    
                  
                    _Child.Add(Child);
                }
            }
        }
    }
    /// <summary>
    /// Ajoute un effet � la case sans instantier l'enfant.
    /// </summary>
    public void AddEffectToList(MYthsAndSteel_Enum.TerrainType Type)
    {
        if (!TerrainEffectList.Contains(Type))
        {
            TerrainEffectList.Add(Type);
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
                    if (Type == MYthsAndSteel_Enum.TerrainType.Maison || Type == MYthsAndSteel_Enum.TerrainType.Immeuble)
                    {
                        Debug.Log("bonsoirrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                            if((T.Type == MYthsAndSteel_Enum.TerrainType.Ruines))
                        {
                            Debug.Log("djfkqlmsdjlfdqsl:kjfqmze:jlskjfqeislldfjqozieljfoqmze");
                            C.GetComponent<SpriteRenderer>().enabled = true;

                            }



                    }
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
