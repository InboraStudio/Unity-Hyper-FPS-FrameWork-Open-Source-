using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Loading Screen")]
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            SetActive(false);

            DontDestroyOnLoad(gameObject);
        }

        public void SetActive(bool state)
        {
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(state);
            }
        }

        public void Enable()
        {
            SetActive(true);
        }

        public void Disable()
        {
            SetActive(false);
        }

        public IEnumerator LoadSceneAsync(string name)
        {
            SetActive(true);

            yield return new WaitForSeconds(0.2f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(name);

            yield return new WaitUntil(() => operation.progress <= 0.9f);
            Invoke(nameof(Disable), 1);
        }
    }
}