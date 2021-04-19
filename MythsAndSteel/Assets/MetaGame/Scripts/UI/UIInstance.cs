using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInstance : MonoSingleton<UIInstance>
{
    #region PhaseDeJeu
    [Header("PHASE DE JEU")]
    //Le panneau à afficher lorsque l'on change de phase
    [SerializeField] private GameObject _switchPhaseObject = null;
    public GameObject SwitchPhaseObject => _switchPhaseObject;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _canvasActivation = null;
    public GameObject CanvasActivation => _canvasActivation;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private Image _skipPhaseImage = null;
    public Image SkipPhaseImage => _skipPhaseImage;

    //Le canvas en jeu pour la phase d'activation
    [SerializeField] private GameObject _backgroundActivation = null;
    public GameObject BackgroundActivation => _backgroundActivation;

    //Le canvas en jeu pour afficher des menus
    [SerializeField] private GameObject _canvasTurnPhase = null;
    public GameObject CanvasTurnPhase => _canvasTurnPhase;

    [SerializeField] private GameObject _buttonNextPhase = null;
    public GameObject ButtonNextPhase => _buttonNextPhase;
    #endregion PhaseDeJeu

    [Header("PANNEAU D'ACTIVATION D'UNE UNITE")]
    [SerializeField] private MenuActionUnite _activationUnitPanel = null;
    public MenuActionUnite ActivationUnitPanel => _activationUnitPanel;

    #region CarteEvenement
    [Header("CARTES EVENEMENTS")]
    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée pour le joueur rouge
    [SerializeField] private GameObject _eventCardObjectRed = null;
    public GameObject EventCardObjectRed => _eventCardObjectRed;

    //L'objet d'event à afficher lorsqu'une nouvelle carte event est piochée.
    [SerializeField] private GameObject _eventCardObjectBlue = null;
    public GameObject EventCardObjectBlue => _eventCardObjectBlue;

    //Sprite du bouton désactivé
    [SerializeField] private FlecheEvent _flecheSpriteRef = null;
    public FlecheEvent FlecheSpriteRef => _flecheSpriteRef;

    //Boutons des cartes events du joueur bleu
    [SerializeField] private ButtonEvent _buttonEventBluePlayer = null;
    public ButtonEvent ButtonEventBluePlayer => _buttonEventBluePlayer;

    //Boutons des cartes events du joueur bleu
    [SerializeField] private ButtonEvent _buttonEventRedPlayer = null;
    public ButtonEvent ButtonEventRedPlayer => _buttonEventRedPlayer;

    //Transform des cartes events du joueur rouge
    [SerializeField] private Transform _redPlayerEventtransf = null;
    public Transform RedPlayerEventtransf => _redPlayerEventtransf;

    //Transform des cartes events du joueur bleu
    [SerializeField] private Transform _bluePlayerEventtransf = null;
    public Transform BluePlayerEventtransf => _bluePlayerEventtransf;

    //Transform de la carte event du joueur rouge le plus en bas
    [SerializeField] private Transform _redEventDowntrans = null;
    public Transform RedEventDowntrans => _redEventDowntrans;

    //Transform de la carte event du joueur bleu le plus en bas
    [SerializeField] private Transform _blueEventDowntrans = null;
    public Transform BlueEventDowntrans => _blueEventDowntrans;
    #endregion CarteEvenement

    [Header("LISTES DES BOUTONS D ACTIONS")]
    [Tooltip("0 et  1 sont pour les boutons quitter, 2 et 3 sont pour switch entre la Page 1 et la Page 2")]
    //Les premiers chiffres (pour le moment 0 et 1 déterminent les bouttons à quitter les menus), les derniers déterminent les buttons des switch de menu.
    [SerializeField] private StatMenuButton _pageButton;
    public StatMenuButton PageButton => _pageButton;
    [Space]
    [Header("LISTES DES ELEMENTS UI POUR LE MOUSEOVER")]
    [Tooltip("Tous les éléments qui composent l'UI dans le MOUSEOVER")]
    //Texte qui indique le nom de l'unité.
    [SerializeField] private GameObject _titlePanelMouseOver;
    public GameObject TitlePanelMouseOver => _titlePanelMouseOver;
    //Ordre : 0 => Texte Vie, 1 => Texte Portée et 2 => Texte Déplacement
    [SerializeField] private TextStatUnit _mouseOverStats;
    public TextStatUnit MouseOverStats => _mouseOverStats;
    
    [Space]

    #region ShiftClicPanelP1
    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 1")]
    [Tooltip("Tous les éléments qui composent l'UI pour le Shift Clic de la Page 1")]
    
    //Texte qui indique le nom de l'unité.
    [SerializeField] private GameObject _titlePanelShiftClicPage1;
    public GameObject TitlePanelShiftClicPage1 => _titlePanelShiftClicPage1;
    
    //Ordre : 0 => Texte Vie, 1 => Texte Portée et 2 => Texte Déplacement
    [SerializeField] private TextStatUnit _pageUnitStat;
    public TextStatUnit PageUnitStat => _pageUnitStat;

    //Ordre : 0 => Valeur Min entre deux valeurs, 1 => Valeur Max entre deux valeurs, 2 => Degats Min et 3 Degats Max
    [SerializeField] private AttackStat _attackStat;
    public AttackStat AttackStat => _attackStat;

    // Parent des capacités dans la page 1.
    [SerializeField] private GameObject _capacityParent;
    public GameObject capacityParent => _capacityParent;

    // Préfab slot capacité.
    [SerializeField] private GameObject _capacityPrefab;
    public GameObject capacityPrefab => _capacityPrefab;

    // List des préfabs actuels.
    [SerializeField] private List<GameObject> _capacitylist;
    public List<GameObject> capacityList => _capacitylist;

    //Comporte les descriptions et sprites pour chaque Attribut. 
    //Si vous changez la taille ou le positionnement des élements de l'Array il faut absolument que la position dans l'Arraye de Chaque ObjectsAttributs correspondent à l'ID de son enum.
    public TextSpriteAttributUnit[] textSpriteAttributUnit = new TextSpriteAttributUnit[11];
    //Array comportant les ObjectsAttributs. A ne pas modifier !
    public ObjectsAttributs[] objectsAttributs = new ObjectsAttributs[3];
    #endregion ShiftClicPanelP1
    
    #region ShiftClicPanelP2
    [Header("LISTES DES ELEMENTS UI POUR LE SHIFT CLIC DE LA PAGE 2")]
    [Tooltip("Tous les éléments qui composent l'UI pour le Shift Clic de la Page 2")]

    [SerializeField] private GameObject _prefabSlotEffetDeTerrain;
    public GameObject prefabSlotEffetDeTerrain => _prefabSlotEffetDeTerrain;

    [SerializeField] private GameObject _parentSlotEffetDeTerrain;
    public GameObject parentSlotEffetDeTerrain => _parentSlotEffetDeTerrain;

    [SerializeField] private List<GameObject> _effetDeTerrain;
    public List<GameObject> effetDeTerrain => _effetDeTerrain;

    #endregion ShiftClicPanelP2


    [Header("ENFANTS CASE DU PLATEAU")]
    [SerializeField] private GameObject _mouvementTilePrefab;
    public GameObject MouvementTilePrefab => _mouvementTilePrefab;

    #region ValidationPanel
    [Header("PANNEAU DE VALIDATION")]
    //Le panneau de validation
    [SerializeField] private GameObject _validationPanel;
    public GameObject ValidationPanel => _validationPanel;

    //texte pour le titre de la validation
    [SerializeField] private TextMeshProUGUI _titleValidationTxt;
    public TextMeshProUGUI TitleValidationTxt => _titleValidationTxt;

    //texte pour la description de la validation
    [SerializeField] private TextMeshProUGUI _descriptionValidationTxt;
    public TextMeshProUGUI DescriptionValidationTxt => _descriptionValidationTxt;
    #endregion ValidationPanel

    [Header("STAT DU JEU")]
    [SerializeField] private TextMeshProUGUI _fpsText = null;
    public TextMeshProUGUI FpsText => _fpsText;

    private void Start(){
        QuitValidationPanel();
    }

    public void DesactivateNextPhaseButton(){
        _buttonNextPhase.SetActive(false);
    }

    /// <summary>
    /// Affiche le bouton pour passer à la phase suivante
    /// </summary>
    public void ActivateNextPhaseButton(){
        _buttonNextPhase.SetActive(true);
    }

    /// <summary>
    /// Affiche le panneau de validation
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    public void ShowValidationPanel(string title, string description){
        _validationPanel.SetActive(true);
        _titleValidationTxt.text = title;
        _descriptionValidationTxt.text = description;
        DesactivateNextPhaseButton();
    }

    /// <summary>
    /// Cache le panneau de validation
    /// </summary>
    public void QuitValidationPanel(){
        _validationPanel.SetActive(false);
    }

    #region UITile
    [Header("Tile's infos update")]
    [SerializeField] private TileTypeClass _typeTileList;

    //Variable du Scriptable.
    public TerrainTypeClass Terrain;

    public void CallUpdateUI(GameObject Tile)
    {
        if (Tile == null)
        {
            _typeTileList.UiTiles.SetBool("open", false); return;
        }
        else if (Tile.TryGetComponent(out TileScript T) && GameManager.Instance.ActualTurnPhase != MYthsAndSteel_Enum.PhaseDeJeu.Activation)
        {
            Terrain.Synch(Tile.GetComponent<TileScript>(), _typeTileList.Tile, _typeTileList.Desc, _typeTileList.Ressources, _typeTileList.Rendu); // Affiche les nouvelles informations à propos de la tile séléctionnée.  
            _typeTileList.UiTiles.SetBool("open", true);
        }
        else
        {
            _typeTileList.UiTiles.SetBool("open", false);
        }
    }
    #endregion

    [Header("PLAYER ACTIVATION")]
    //nombre d'activation restante pour le joueur rouge
    [SerializeField] private TextMeshProUGUI _activationLeftTxtRP = null;
    public TextMeshProUGUI ActivationLeftTxtRP => _activationLeftTxtRP;
    //nombre d'activation restante pour le joueur bleu
    [SerializeField] private TextMeshProUGUI _activationLeftTxtBP = null;
    public TextMeshProUGUI ActivationLeftTxtBP => _activationLeftTxtBP;

    public void UpdateActivationLeft(){
        _activationLeftTxtRP.text = PlayerScript.Instance.RedPlayerInfos.ActivationLeft.ToString();
        _activationLeftTxtBP.text = PlayerScript.Instance.BluePlayerInfos.ActivationLeft.ToString();
    }

    #region MenuRenfort
    [Header("LISTE DES ELEMENTS POUR LE MENU RENFORT")]
    [Tooltip("ensemble des textes pour le menu Statistiques des renforts")]
    [SerializeField] private TextRenfortMenu _pageUnitéRenfort;
    public TextRenfortMenu PageUnitéRenfort => _pageUnitéRenfort;

    [Tooltip("Ensemble des Emplacements pour les images du menu Statistiques des renforts")]
    [SerializeField] private EmplacementImageMenuRenfort _emplacementImageMenuRenfort;
    public EmplacementImageMenuRenfort EmplacementImageMenuRenfort => _emplacementImageMenuRenfort;

    [Tooltip("Ensemble des Images pour le menu Statistiques des renforts")]
    [SerializeField] private StorageImageForUI _stockageImage;
    public StorageImageForUI StockageImage => _stockageImage;

    [SerializeField] private AssignRessouceUnit _ressourceUnit_PasTouche;
    public AssignRessouceUnit RessourceUnit_PasTouche => _ressourceUnit_PasTouche;
    #endregion MenuRenfort

    #region VieUnité
    [SerializeField] private GameObject _lifeHeartPrefab;
    public GameObject LifeHeartPrefab => _lifeHeartPrefab;

    [SerializeField] private Sprite[] _redHeartSprite;
    public Sprite[] RedHeartSprite => _redHeartSprite;

    [SerializeField] private Sprite[] _blueHeartSprite;
    public Sprite[] BlueHeartSprite => _blueHeartSprite;

    [SerializeField] private Sprite[] _redHeartShieldSprite;
    public Sprite[] RedHeartShieldSprite => _redHeartShieldSprite;

    [SerializeField] private Sprite[] _blueHeartShieldSprite;
    public Sprite[] BlueHeartShieldSprite => _blueHeartShieldSprite;
    #endregion VieUnité
}

#region ClassToRangeList
/// <summary>
/// Class pour les boutons pour les cartes events
/// </summary>
[System.Serializable]
public class ButtonEvent
{
    public GameObject _upButton = null;
    public GameObject _downButton = null;
}

/// <summary>
/// Boutons quitter et changer de page des menus de stats
/// </summary>
[System.Serializable]
public class StatMenuButton
{
    [Space]

    public Button _rightArrowPage1 = null;
    public Button _leftArrowPage2 = null;
}

/// <summary>
/// GameObject qui comprend la vie, la portée et la vitesse de déplacement
/// </summary>
[System.Serializable]
public class TextStatUnit
{
    public GameObject _lifeGam = null;
    public GameObject _rangeGam = null;
    public GameObject _moveGam = null;
    public GameObject _unitSpriteGam = null;
}

/// <summary>
/// Liste des gameObjects pour l'attaque
/// </summary>
[System.Serializable]
public class AttackStat
{
    public GameObject _rangeMinDamageGam = null;
    public GameObject _rangeMaxDamageGam = null;

    public GameObject _minDamageValueGam = null;
    public GameObject _maxDamageValueGam = null;
}

/// <summary>
/// Liste pour tous les objets pour l'affichage du type de terrain sur chaque case
/// </summary>
[System.Serializable]
public class TileTypeClass
{
    [SerializeField] private TextMeshProUGUI _title;
    public TextMeshProUGUI Tile => _title;
    [SerializeField] private TextMeshProUGUI _desc;
    public TextMeshProUGUI Desc => _desc;
    [SerializeField] private Image _rendu;
    public Image Rendu => _rendu;
    [SerializeField] private TextMeshProUGUI _ressources;
    public TextMeshProUGUI Ressources => _ressources;
    [SerializeField] private Animator _uiTiles;
    public Animator UiTiles => _uiTiles;
}

/// <summary>
/// Liste des flèches pour les cartes events
/// </summary>
[System.Serializable]
public class FlecheEvent
{
    public Sprite _redArrowUp = null;
    public Sprite _redArrowDown = null;
    public Sprite _blueArrowUp = null;
    public Sprite _blueArrowDown = null;
    public Sprite _grisArrowUp = null;
    public Sprite _grisArrowDown = null;
}


[System.Serializable]
public class StorageImageForUI
{
    public List<Sprite> _drapeauJoueur = null;

    [Space]

    public List<Sprite> _imageUnit1 = null;
    public List<Sprite> _imageUnit2 = null;
    public List<Sprite> _imageUnit3 = null;
    public List<Sprite> _imageUnit4 = null;
    public List<Sprite> _imageUnit5 = null;
    public List<Sprite> _imageUnit6 = null;

    [Space]

    //A Compléter.
    public Sprite _terrainEffectForet = null;
    public List<Sprite> _rivièreDirection = null;
    public List<Sprite> _pont = null;

}


/// <summary>
/// GameObject qui comporte l'ensemble des données écrites correspondant aux statistiques de de vie, de porté, de déplacement et d'attque ainsi que le nom de l'unité.
/// </summary>
[System.Serializable]
public class TextRenfortMenu
{
    public GameObject _ressourceJoueur = null;
    public GameObject _ressourceText = null;

    [Space(10)]

    [Header("Unité 1")]
    public GameObject _nameUnit1 = null;
    public GameObject _lifeValor1 = null;
    public GameObject _rangeValor1 = null;
    public GameObject _moveValor1 = null;
    public GameObject _damageValor1 = null;

    [Space(10)]

    [Header("Unité 2")]
    public GameObject _nameUnit2 = null;

    public GameObject _lifeValor2 = null;
    public GameObject _rangeValor2 = null;
    public GameObject _moveValor2 = null;
    public GameObject _damageValor2 = null;

    [Space(10)]

    [Header("Unité 3")]
    public GameObject _nameUnit3 = null;

    public GameObject _lifeValor3 = null;
    public GameObject _rangeValor3 = null;
    public GameObject _moveValor3 = null;
    public GameObject _damageValor3 = null;

    [Space(10)]

    [Header("Unité 4")]
    public GameObject _nameUnit4 = null;

    public GameObject _lifeValor4 = null;
    public GameObject _rangeValor4 = null;
    public GameObject _moveValor4 = null;
    public GameObject _damageValor4 = null;

    [Space(10)]

    [Header("Unité 5")]
    public GameObject _nameUnit5 = null;

    public GameObject _lifeValor5 = null;
    public GameObject _rangeValor5 = null;
    public GameObject _moveValor5 = null;
    public GameObject _damageValor5 = null;

    [Space(10)]

    [Header("Unité 6")]
    public GameObject _nameUnit6 = null;

    public GameObject _lifeValor6 = null;
    public GameObject _rangeValor6 = null;
    public GameObject _moveValor6 = null;
    public GameObject _damageValor6 = null;


}

[System.Serializable]
public class AssignRessouceUnit
{
    public List<GameObject> _unité1Ressource;
    public List<GameObject> _unité2Ressource;
    public List<GameObject> _unité3Ressource;
    public List<GameObject> _unité4Ressource;
    public List<GameObject> _unité5Ressource;
    public List<GameObject> _unité6Ressource;
}

/// <summary>
/// GameObject qui comprend l'ensemble des images pour le menuRenfort (les icones de vie, portée et déplacement ainsi qu les illustrations d'unités).
/// </summary>
[System.Serializable]
public class EmplacementImageMenuRenfort
{
    public GameObject _drapeauDuJoueur = null;

    [Space(20)]

    public GameObject _imageUnité1 = null;
    public GameObject _imageUnité2 = null;
    public GameObject _imageUnité3 = null;
    public GameObject _imageUnité4 = null;
    public GameObject _imageUnité5 = null;
    public GameObject _imageUnité6 = null;

    [Space(20)]

    public GameObject _imageLifeValor = null;
    public GameObject _imageRangeValor = null;
    public GameObject _imageMovementValor = null;
    public GameObject _imageAttaqueValor = null;
    public GameObject _imageRessource = null;
}

[System.Serializable]
public class BouttonMenuRenfort
{
    public Button _quitMenuRenfort = null;

    public Button _clicSurUnité1 = null;
    public Button _clicSurUnité2 = null;
    public Button _clicSurUnité3 = null;
    public Button _clicSurUnité4 = null;
    public Button _clicSurUnité5 = null;
    public Button _clicSurUnité6 = null;
}

/// <summary>
/// Contient le texte et le sprite pour un attribut
/// </summary>
[System.Serializable]
public class TextSpriteAttributUnit
{
    [TextArea]
    public string TextAttributUnit;
    public Sprite SpriteAttributUnit;
}

/// <summary>
/// Objet dont le sprite va changer en fonction de l'attribut, Objet dont le texte va changer en fonction de l'attribut. 
/// </summary>

[System.Serializable]
public class ObjectsAttributs
{

    public GameObject MainObjects;
    public GameObject Description;
}
#endregion ClassToRangeList
