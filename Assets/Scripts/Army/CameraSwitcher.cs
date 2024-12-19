using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;  // Camera principale
    [SerializeField] private Camera cameraPOV;  // Camera POV
    private Camera currentCamera;                // Caméra actuellement active

    private void Start()
    {
        // Vérifie que les caméras sont bien assignées
        if (mainCamera == null || cameraPOV == null)
        {
            Debug.LogError("Les deux caméras doivent être assignées dans l'inspecteur !");
            return;
        }

        // Initialiser avec la caméra principale active
        currentCamera = mainCamera;
        mainCamera.gameObject.SetActive(true);
        cameraPOV.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Appuyer sur "C" pour changer de caméra
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }

        // Si CameraPOV a été détruite, passer à MainCamera
        if (cameraPOV == null || !cameraPOV.gameObject.activeInHierarchy)
        {
            SwitchToMainCamera();
        }
    }

    private void SwitchCamera()
    {
        if (currentCamera == mainCamera)
        {
            if (cameraPOV != null)
            {
                mainCamera.gameObject.SetActive(false);
                cameraPOV.gameObject.SetActive(true);
                currentCamera = cameraPOV;
            }
        }
        else
        {
            // Désactive la caméra POV et active la caméra principale
            cameraPOV.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            currentCamera = mainCamera;
        }
    }

    private void SwitchToMainCamera()
    {
        // Si CameraPOV n'existe plus, passer directement à MainCamera
        mainCamera.gameObject.SetActive(true);
        if (currentCamera != mainCamera)
        {
            currentCamera.gameObject.SetActive(false);
        }
        currentCamera = mainCamera;
    }
}
