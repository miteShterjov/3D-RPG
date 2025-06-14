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


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(TransitionToScene());
            }
        }

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
