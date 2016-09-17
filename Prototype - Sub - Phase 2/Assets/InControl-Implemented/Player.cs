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

        private float stickThresh = .25f;

        private ActionsOutputTarget AOT;

		Renderer cachedRenderer;

        private delegate void runningControlIO();
        private runningControlIO controlIO;

        public void assignControlIO (controlSchemes scheme)
        {
            switch (scheme)
            {
                case (controlSchemes.joinIO):
                    //join screen
                    controlIO = joinIOFunc;
                    break;
                case (controlSchemes.menuIO):
                    //menu screen
                    controlIO = menuIOFunc;
                    break;
                case (controlSchemes.playIO):
                    //play input
                    controlIO = playIOFunc;
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

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += newSceneLoaded;
            assignControlIO(controlSchemes.joinIO);
            AOT = GetComponent<ActionsOutputTarget>();
        }


		void Start()
		{
			cachedRenderer = GetComponent<Renderer>();
		}


		void Update()
		{
            controlIO();
		}


        private void joinIOFunc ()
        {
            if (Actions == null)
            {
                // If no controller exists for this cube, just make it translucent.
                cachedRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            }
            else
            {
                // Set object material color.
                cachedRenderer.material.color = GetColorFromInput();

                // Rotate target object.
                transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * Actions.Rotate.X, Space.World);
                transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * Actions.Rotate.Y, Space.World);
            }

            if (Actions.Start)
            {
                SceneManager.LoadScene(1);
            }
        }

        private void menuIOFunc()
        {

        }

        private void playIOFunc()
        {
            if (Mathf.Abs(Actions.Rotate.X) >= stickThresh || Mathf.Abs(Actions.Rotate.Y) >= stickThresh)
            AOT.passLS(Actions.Rotate.X, Actions.Rotate.Y);
            if (Actions.AButton.WasPressed)
            {
                AOT.passAButton(true);
            }
            else if (Actions.AButton.WasReleased)
            {
                AOT.passAButton(false);
            }
        }


        private void newSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex ==0)
            {
                assignControlIO(controlSchemes.joinIO);
            }
            else if (scene.buildIndex == 1)
            {
                assignControlIO(controlSchemes.playIO);
                if (GetComponent<MeshRenderer>().enabled)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                }
                
            }
        }

        Color GetColorFromInput()
        {
            if (Actions.Green)
            {
                return Color.green;
            }

            if (Actions.Red)
            {
                return Color.red;
            }

            if (Actions.Blue)
            {
                return Color.blue;
            }

            if (Actions.Yellow)
            {
                return Color.yellow;
            }

            return Color.white;
        }
    }
}

public enum controlSchemes { joinIO, menuIO, playIO }


