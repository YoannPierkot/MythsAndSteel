using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private int _line;
    public int Line => _line;

    //Liste des effets de terrain sur chaque tile
    [SerializeField] private List<MYthsAndSteel_Enum.TerrainType> _terrainEffectList = new List<MYthsAndSteel_Enum.TerrainType>();
    public List<MYthsAndSteel_Enum.TerrainType> TerrainEffectList => _terrainEffectList;

    [SerializeField] private int _ResourcesCounter = 0;
    public int ResourcesCounter
    {
        get
        {
            return _ResourcesCounter;
        }
        set
        {
            _ResourcesCounter = value;
        }
    }
    [SerializeField] private MYthsAndSteel_Enum.Owner _owner;
    public MYthsAndSteel_Enum.Owner owner
    {
        get
        {
            return _owner;
        }
        set
        {
            _owner = value;
        }
    }

    private void Start(){
        //Met l'unité à la bonne position
        if(_unit != null){
            _unit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _unit.transform.position.z);
        }
    }

    /// <summary>
    /// Ajoute une unité à cette case
    /// </summary>
    /// <param name="unit"></param>
    public void AddUnitToTile(GameObject unit){
        _unit = unit;
    }

    /// <summary>
    /// Enleve l'unité qui se trouve sur cette case
    /// </summary>
    public void RemoveUnitFromTile(){
        _unit = null;
    }

    public void AddChildRender(Sprite Rendu = null, MYthsAndSteel_Enum.TerrainType Type = MYthsAndSteel_Enum.TerrainType.Sol)
    {
        bool add = true;
        if(Rendu != null)
        {
            GameObject R = Instantiate(UIInstance.Instance.MouvementTilePrefab, transform.position, Quaternion.identity);
            R.transform.parent = this.transform;
            R.name = Rendu.name;
            R.GetComponent<SpriteRenderer>().sprite = Rendu;
            R.transform.localScale = new Vector3(1, 1, 1);
            foreach(GameObject TileRender in Child)
            {
                if(TileRender.name == R.name)
                {
                    Destroy(R);
                    add = false;
                    break;
                }

            }
            if(add)
            {       
                if(Child.Count > 0)
                {
                    GameObject temp = Child[0];
                    Child.RemoveAt(0);
                    Destroy(temp);
                }
                Child.Add(R);
            }
        }
    }
}
