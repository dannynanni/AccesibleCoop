namespace MultiplayerWithBindingsExample
{
	using UnityEngine;
    using UnityEngine.SceneManagement;


	// This is just a simple "player" script that rotates and colors a cube
	// based on input read from the actions field.
	//
	// See comments in PlayerManager.cs for more details.
	//
	public class Player : MonoBehaviour
	{
		public PlayerActions Actions { get; set; }

		Renderer cachedRenderer;

        private delegate void runningControlIO();
        private runningControlIO controlIO;

        public void setControlScheme(controlScheme scheme)
        {
            switch (scheme)
            {
                case (controlScheme.joinScreenIO):
                    //join screen
                    controlIO = joinScreenIO;
                    break;
                case (controlScheme.menuIO):
                    //menu screen
                    controlIO = menuIO;
                    break;
                case (controlScheme.playIO):
                    //play input
                    controlIO = playIO;
                    break;
            }
        }

        void OnDisable()
		{
			if (Actions != null)
			{
				Actions.Destroy();
			}
		}

        void Awake ()
        {
            SceneManager.sceneLoaded += OnSceneChangeInit;
        }

		void Start()
		{
			cachedRenderer = GetComponent<Renderer>();
		}

		void Update()
		{
            runningControlIO();
		}

        
        // OnSceneChangeInit is a function passed to the SceneManager as a delegate 
        // Uses the passed scene.buildIndex as an int to change control scheme IO
        private void OnSceneChangeInit(Scene scene, LoadSceneMode m)
        {
            if (scene.buildIndex == 0)
            {
                setControlScheme(controlScheme.joinScreenIO);
            }

            else if (scene.buildIndex == 1)
            {
                setControlScheme(controlScheme.playIO);
            }
        }

        // The following three functions are the control schemes depending on three scene types
        // Type 1 - Controller connection and assignment
        // Type 2 - Non-play space menu or screen navigation
        // Type 3 - In-game player actions

        private void joinScreenIO ()
        {
            if (Actions == null)
            {
                // If no controller exists for this cube, just make it translucent.
                cachedRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            }
            else
            {
                // Rotate target object.
                transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * Actions.Rotate.X, Space.World);
                transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * Actions.Rotate.Y, Space.World);
                if (Actions.Start)
                {
                    SceneManager.LoadScene(1);
                }
            }
        }

        private void menuIO ()
        {
            if (Actions.AButton)
            {
                Debug.Log("AButton");
            }

        }

        private void playIO ()
        {

        }
    }
}
public enum controlScheme { joinScreenIO, menuIO, playIO }

