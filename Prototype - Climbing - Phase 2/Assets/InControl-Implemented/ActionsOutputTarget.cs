namespace MultiplayerWithBindingsExample
{
    using UnityEngine;
    using System.Collections;
    using InControl;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public class ActionsOutputTarget : MonoBehaviour
    {
        private List<string> playerStations = new List<string>();

        public string stn1;
        public string stn2;
        public string stn3;
        public string stn4;

        public ControlIO myIO;


        // PLAYERNUMBER must be set in scene 0
        public int playerNumber;
        public int PLAYERNUMBER
        {
            get
            {
                return playerNumber;
            }
            set
            {
                if (value != playerNumber)
                {
                    playerNumber = value;
                }
                Debug.Log(value);
            }
        }

        private GameObject playerStation;
        private GameObject stationComponenet;

        void Awake()
        {
            SceneManager.sceneLoaded += newSceneLoaded;

            playerStations.Add(stn1);
            playerStations.Add(stn2);
            playerStations.Add(stn3);
            playerStations.Add(stn4);
        }

        private void newSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != 0)
            {
                playerStation = GameObject.Find(playerStations[playerNumber]);
                myIO = playerStation.GetComponent <ControlIO> ();
            }
            if (scene.buildIndex == 1)
            {
                myIO.playerNum = PLAYERNUMBER;
            }
        }

        public void passLS (float leftRight, float upDown)
        {
            myIO.LS (leftRight, upDown);
            //Debug.Log(leftRight + " " + upDown + " " + Mathf.Rad2Deg * Mathf.Atan2(upDown, leftRight));

        }

        public void passAButton (bool pressed)
        {
            myIO.AButton(pressed);
        }

        public void resetPlayerNum()
        {
            playerNumber = 0;
        }
    }
}
