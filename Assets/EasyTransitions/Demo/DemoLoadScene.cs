using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace EasyTransition
{

    public  class DemoLoadScene : MonoBehaviour
    {

        [SerializeField] private GameEventChannelSO SceneCheck;
        public static DemoLoadScene instance;
        public bool IsNomarlClear = false;

        private void Awake()
        {
            if (PlayerPrefs.HasKey("Clear"))
            {
                if(PlayerPrefs.GetFloat("Clear") == 1)
                {
                    IsNomarlClear =true;
                }
                else
                {
                    IsNomarlClear = false;
                }
            }

            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else 
            { 
                Destroy(gameObject);
            }
        }

        public TransitionSettings transition;
        public float startDelay;
        
        public void LoadScene(string _sceneName)
        {
            if(PoolManagerMono.Instacne != null)
            {
                PoolManagerMono.Instacne.AllPush();
            }
            TransitionManager.instance.Transition(_sceneName, transition, startDelay);
            SceneCheck.RaiseEvent(SceneChangeEvents.SceneChangeCheck.Init(_sceneName));
        }   
    }

}


