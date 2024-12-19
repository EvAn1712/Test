using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // N'oublie pas d'importer ce namespace pour Slider

public class ArmyManagerRed : ArmyManager
{
    private int remainingUses = 3; // Nombre d'utilisations restantes
    public TMP_Text shieldMessageText; // Référence à un TextMeshPro pour afficher "Bouclier"
    private float displayDuration = 2f; // Durée d'affichage du message en secondes
    // Références aux caméras
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera cameraPOV;
    private Camera currentCamera;

    [SerializeField] public Slider supremeDroneHealthSlider;  // Remplace TMP_Text par Slider

    public Animator healthEffectAnimator; // Référence à l'Animator pour l'effet visuel

    public override void ArmyElementHasBeenKilled(GameObject go)
    {
        base.ArmyElementHasBeenKilled(go);
        if (m_ArmyElements.Count == 0)
        {
            GUIUtility.systemCopyBuffer = "0\t" + ((int)Timer.Value).ToString() + "\t0\t0\t0";
        }
    }

    public void GreenArmyIsDead(string deadArmyTag)
    {
        int nDrones = 0, nTurrets = 0, health = 0;
        ComputeStatistics(ref nDrones, ref nTurrets, ref health);
        GUIUtility.systemCopyBuffer = "1\t" + ((int)Timer.Value).ToString() + "\t" + nDrones.ToString() + "\t" + nTurrets.ToString() + "\t" + health.ToString();

        RefreshHudDisplay(); // pour une dernière mise à jour en cas de victoire
    }

    // Méthode pour ajouter de la vie à tous les drones
    private void AddHealthToAllDrones(float healthToAdd)
    {
        DroneSupreme droneSupreme = FindObjectOfType<DroneSupreme>();
        if (droneSupreme != null)
        {
            droneSupreme.AddHealth(healthToAdd);
            Debug.Log($"{droneSupreme.name} a reçu {healthToAdd} points de vie !");
            PlayHealthEffectAnimation(); // Joue l'animation de l'effet visuel

        }
    }
    

    // Méthode pour activer l'option et afficher "Bouclier"
    private void ActivateShield()
    {
        if (remainingUses > 0)
        {
            remainingUses--;
            AddHealthToAllDrones(100f); // Ajoute 100 points de vie à tous les drones
            Debug.Log($"Bouclier activé ! Utilisations restantes : {remainingUses}");

            // Affiche "Bouclier" sur l'interface
            StartCoroutine(DisplayShieldMessage());
        }
        else
        {
            Debug.Log("Aucune utilisation restante pour activer le bouclier.");
        }
    }

    private IEnumerator DisableAfterAnimation()
    {
        // Durée de l'animation (remplacez 1f par la durée réelle de votre animation)
        yield return new WaitForSeconds(1f);

        // Désactiver le GameObject
        healthEffectAnimator.gameObject.SetActive(false);
    }

    private void PlayHealthEffectAnimation()
    {
        if (healthEffectAnimator != null)
        {
            // Active le GameObject qui contient l'Animator
            healthEffectAnimator.gameObject.SetActive(true);

            // Joue l'animation
            healthEffectAnimator.Play("HealthEffectAnimation");

            // Désactive le GameObject après l'animation
            StartCoroutine(DisableAfterAnimation());
        }
        else
        {
            Debug.LogWarning("Aucun Animator assigné pour l'effet visuel !");
        }
    }

    private IEnumerator DisplayShieldMessage()
    {
        // Affiche le texte "Bouclier"
        if (shieldMessageText != null)
        {
            shieldMessageText.text = "Bouclier";
            shieldMessageText.gameObject.SetActive(true);
        }

        // Attend quelques secondes
        yield return new WaitForSeconds(displayDuration);

        // Cache le texte
        if (shieldMessageText != null)
        {
            shieldMessageText.gameObject.SetActive(false);
        }
    }

    private void UpdateSupremeDroneHealth()
    {
        DroneSupreme droneSupreme = FindObjectOfType<DroneSupreme>();
        if (droneSupreme != null && supremeDroneHealthSlider != null)
        {
            // Met à jour la valeur du slider en fonction de la santé du drone suprême
            supremeDroneHealthSlider.value = droneSupreme.Health / 50;  // On suppose que "MaxHealth" est la santé maximale
        }
    }

    protected virtual void RefreshHudDisplay()
    {
        base.RefreshHudDisplay();
        UpdateSupremeDroneHealth();
    }

    private void Update()
    {
        // Vérifie si la touche Espace est pressée
        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateShield(); // Active le bouclier
        }
        UpdateSupremeDroneHealth();
    }
}