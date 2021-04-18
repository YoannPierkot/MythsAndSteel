using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "META/Game Manager")]
public class GameManagerSO : ScriptableObject
{
    //Event pour quand le joueur clique sur un bouton pour passer à la phase en question
    public delegate void ClickButtonSwitchPhase();
    public event ClickButtonSwitchPhase GoToDebutPhase;
    public event ClickButtonSwitchPhase GoToActivationPhase;
    public event ClickButtonSwitchPhase GoToOrgoneJ1Phase;
    public event ClickButtonSwitchPhase GoToActionJ1Phase;
    public event ClickButtonSwitchPhase GoToOrgoneJ2Phase;
    public event ClickButtonSwitchPhase GoToActionJ2Phase;
    public event ClickButtonSwitchPhase GoToStrategyPhase;

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void GoToPhase(MYthsAndSteel_Enum.PhaseDeJeu phase = MYthsAndSteel_Enum.PhaseDeJeu.Debut, bool randomPhase = false)
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;
        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = MYthsAndSteel_Enum.PhaseDeJeu.Debut;

        if(randomPhase)
        {
            nextPhase = phase;
        }
        else
        {
            phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;

            if(phaseSuivante > 6)
            {
                if(GameManager.Instance.ActualTurnNumber > 11)
                {
                    GameManager.Instance.VictoryForArmy(1);
                    return;
                }
                else
                {
                    phaseSuivante = 0;
                    GameManager.Instance.ActualTurnNumber++;
                }
            }

            nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu)phaseSuivante;
        }

        gophase(nextPhase);
    }

    /// <summary>
    /// Aller à la phase de jeu renseigner en paramètre POUR LE BOUTON
    /// </summary>
    /// <param name="phaseToGoTo"></param>
    public void GoToPhase()
    {
        UIInstance.Instance.ActivateNextPhaseButton();

        int phaseSuivante = ((int)GameManager.Instance.ActualTurnPhase) + 1;


        if(phaseSuivante > 6)
        {
            if(GameManager.Instance.ActualTurnNumber > 11)
            {
                GameManager.Instance.VictoryForArmy(1);
                return;
            }
            else
            {
                phaseSuivante = 0;
                GameManager.Instance.ActualTurnNumber++;
            }
        }

        MYthsAndSteel_Enum.PhaseDeJeu nextPhase = (MYthsAndSteel_Enum.PhaseDeJeu)phaseSuivante;

        gophase(nextPhase);
    }

    /// <summary>
    /// Va a la phase
    /// </summary>
    /// <param name="nextPhase"></param>
    void gophase(MYthsAndSteel_Enum.PhaseDeJeu nextPhase)
    {
        //Selon la phase effectue certaines actions
        switch(nextPhase)
        {
            case MYthsAndSteel_Enum.PhaseDeJeu.Debut:
                if(GoToDebutPhase != null)
                {
                    GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Debut);
                    GoToDebutPhase();
                }
                else
                {
                    GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Activation);
                    GoToActivationPhase();
                }
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Activation:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Activation);
                GoToActivationPhase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1);

                GoToOrgoneJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ1);

                foreach(GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ1Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2);

                GoToOrgoneJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.ActionJ2);

                foreach(GameObject unit in GameManager.Instance.IsPlayerRedTurn ? PlayerScript.Instance.UnitRef.UnitListRedPlayer : PlayerScript.Instance.UnitRef.UnitListBluePlayer)
                {
                    unit.GetComponent<UnitScript>().ResetTurn();
                }

                GoToActionJ2Phase();
                break;

            case MYthsAndSteel_Enum.PhaseDeJeu.Strategie:
                GameManager.Instance.GoPhase(MYthsAndSteel_Enum.PhaseDeJeu.Strategie);

                if(GoToStrategyPhase != null) GoToStrategyPhase();
                break;
        }
    }

    public bool GetDebutFunction(){
        if(GoToDebutPhase != null) return true;
        else return false;
    }
}
