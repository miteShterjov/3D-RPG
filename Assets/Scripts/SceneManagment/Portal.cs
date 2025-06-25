using System;
using System.Collections;
using RPG.SceneManagement;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagment
{

    public class Portal : MonoBehaviour
    {
        [SerializeField] private int sceneIndexToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] private float fadeOutTime = 3f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;

        // Checks if the portal is active and ready to transition
        // This method is called when the player enters the portal's trigger area
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(TransitionToScene());
            }
        }

        // Transitions to the specified scene index
        // It handles the fading out and in of the screen, loads the new scene,
        // and updates the player's position and camera
        // This method is called when the player enters the portal
        private IEnumerator TransitionToScene()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindFirstObjectByType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            UpdateCamera();

            print("Scene loaded: " + SceneManager.GetActiveScene().name);
            Destroy(gameObject); // Destroy the portal after loading the scene
        }

        // Updates the camera to follow the player after the scene transition
        // This method is called after the scene is loaded and the player is updated
        private void UpdateCamera()
        {
            GameObject camera = GameObject.Find("Third Person Aim Camera");
            if (camera == null)
            {
                Debug.LogError("Camera not found in the scene.");
                return;
            }
            camera.GetComponent<CinemachineCamera>().Follow = GameObject.FindWithTag("Player").transform;
        }

        // Checks if the player is within attack range of the portal
        // This is used to determine if the player can interact with the portal
        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found in the scene.");
                return;
            }

            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.transform.Rotate(Vector3.up * 180f); // Rotate player to face the correct direction
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsByType<Portal>(FindObjectsSortMode.None))
            {
                if (portal == this) continue;

                return portal;
            }

            throw new Exception("No other portal found for scene index: " + sceneIndexToLoad);
        }
    }
}
