using MyFirstARGame;
using UnityEngine.SceneManagement;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class SceneUtility : MonoBehaviour
    {
        public static SceneUtility Singleton = null;

        private void Awake()
        {
            InitSingleton();
        }

        private void InitSingleton()
        {
            if (SceneUtility.Singleton == null)
            {
                SceneUtility.Singleton = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        void OnEnable()
        {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        void OnSceneUnloaded(Scene current)
        {
            if (current == SceneManager.GetActiveScene())
            {
                LoaderUtility.Deinitialize();
                LoaderUtility.Initialize();
            }
        }

        void OnDisable()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}
