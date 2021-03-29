using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UIManager")]
public class UIManager : ScriptableObject
{
    [Header("Cartes événements")]
    [SerializeField] EventCardClass _eventCardClass = null;
    public EventCardClass EventCardClass => _eventCardClass;

    int _lastNumberOfEventJ1 = 0;
    int _lastNumberOfEventJ2 = 0;

    #region PhaseDeJeu
    /// <summary>
    /// Appelle la fonction dans le GameManager pour passer à la phase suivante
    /// </summary>
    public void ClicToNextPhase(){
        GameManager.Instance.CallEventToSwitchPhase();
    }

    public void ActivateActivationPhase(bool activate){
        UIInstance.Instance.CanvasActivation.SetActive(activate);
    }
    #endregion PhaseDeJeu

    #region Evenement
    /// <summary>
    /// Call an event
    /// </summary>
    /// <param name="id"></param>
    public void CallEvent(int id){
        switch(id){
            case 0:
                _eventCardClass.PointeursLaserOptimisés();
                break;
        }
    }

    /// <summary>
    /// Update l'UI des cartes event
    /// </summary>
    public void UpdateEventUI(){
        if(_lastNumberOfEventJ1 != ScriptPlayerWait.Instance.EventCardJ1.Count){
            _lastNumberOfEventJ1 = ScriptPlayerWait.Instance.EventCardJ1.Count;


        }
    }

    /// <summary>
    /// Ajoute une carte event
    /// </summary>
    /// <param name="player"></param>
    public void AddEventCardObj(int player, MYthsAndSteel_Enum.EventCard card){
        if(player == 1){
            GameObject newEvent = Instantiate(UIInstance.Instance.EventCardObject,
                                  UIInstance.Instance.EventCardJ1Transform.GetChild(UIInstance.Instance.EventCardJ1Transform.childCount - 1).transform.position,
                                  Quaternion.identity,
                                  UIInstance.Instance.EventCardJ1Transform);
        }
    }
    #endregion Evenement

}