using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/UI/Pause Menu")]
    [RequireComponent(typeof(CanvasGroup))]
    public class PauseMenu : MonoBehaviour
    {
        public bool freezOnPause = true;
        public GameObject UI;

        public UnityEvent OnPause;
        public UnityEvent OnResume;

        public static PauseMenu Instance;
        private CanvasGroup canvasGroup;

        public bool paused { get; set; }
        public Controls controls;

        private void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);

            if (!LoadingScreen.Instance)
                SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        }

        private void Start()
        {
            controls = new Controls();
            controls.UI.Enable();

            controls.UI.Pause.performed += context =>
            {
                paused = !paused;

                if (paused) OnPause?.Invoke();
                if (!paused) OnResume?.Invoke();
            };

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            UI.SetActive(paused);

            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = paused;

            if(canvasGroup)
            canvasGroup.alpha = paused ? Mathf.Lerp(canvasGroup.alpha, 1, Time.unscaledDeltaTime * 10) : 0;

            if (freezOnPause)
            {
                Time.timeScale = paused ? 0 : 1;
            }
        }

        public void Resume()
        {
            paused = false;
            OnResume?.Invoke();
        }


        public void Pause()
        {
            paused = true;
            OnPause?.Invoke();
        }
        public void Quit() => Application.Quit();
    }
}