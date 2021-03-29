using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MYthsAndSteel_Enum : MonoBehaviour
{
    /// <summary>
    /// Enum pour le type des unités
    /// </summary>
    public enum TypeUnite{ 
        Infanterie, Vehicule, Artillerie, Mythe, Leader, Mecha
    }

    /// <summary>
    /// Enum pour els phases du jeu
    /// </summary>
    public enum PhaseDeJeu{
        Debut, Activation, OrgoneJ1, ActionJ1, OrgoneJ2, ActionJ2, Strategie
    }

    /// <summary>
    /// Enum pour les cartes events
    /// </summary>
    public enum EventCard
    {
        Pointeurs_laser_optimisés, Fil_barbelé, Déploiement_accéléré, Activation_de_nodus, Réapprovisionnement, Illusion_stratégique, Détonation_d_orgone, Bombardement_aérien,
        Sérum_expérimental, Transfusion_d_orgone, Optimisation_de_l_orgone, Pillage_orgone, Manoeuvre_stratégique, Reprogrammation, Paralysie, Sabotage,
        Vol_de_ravitaillement, Arme_épidémiologique, Cessez_le_feu, Armes_perforantes, Entraînement_rigoureux,
        J1Faction1, J1Faction2, J1Faction3, J2Faction1, J2Faction2, J2Faction3
    }

    /// <summary>
    /// Les différentes stats modifiables
    /// </summary>
    public enum Stat{
        Attaque, vie, portee
    }
}
