using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class RopeLinkingScript : MonoBehaviour {

    private GameObject[] players = new GameObject[4];
	private bool[][] face;
    private bool[] thisFace = new bool[4];

    public float linkDistMax;

    private List<VectorLine> lines = new List<VectorLine>();

    public int segmentNumber;
    public float lineWidth;

    // Use this for initialization
    void Start () {
		//initialize the array used to track the players' positions
		face = new bool[4][];
		face[0] = new bool[4];
		face[1] = new bool[4];
		face[2] = new bool[4];
		face[3] = new bool[4];


        // Initializes the players into an array.
        // Players must be named Player0, Player1, Player2, and Player3
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = GameObject.Find("Player" + i);
			for (int j = 0; j < players.Length; j++)
            {
                face[i][j] = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // start by clearing out the lsit
        resetListAndLines();

        // checks for each face each frame
        for (int faceNum = 0; faceNum < 4; faceNum++)
        {
            // goes through and looks through each player for each face
            for (int playerNum = 0; playerNum < players.Length; playerNum++)
            {
                // if a player is on a given face...
                if (face[faceNum][playerNum])
                {
                    // and that player isn't the last player
                    if (playerNum < players.Length)
                    {
                        // check all other players of a greater number for that face
                        for (int nextPlayer = playerNum + 1; nextPlayer < players.Length; nextPlayer++)
                        {
                            // if another player is also on that face, then call the tryDrawFromTo function
                            if (face[faceNum][nextPlayer])
                            {
                                queryDrawFromTo(playerNum, nextPlayer);
                            }
                        }
                    }
                }
            }
        }
	}

    public void PlayerFaceUpdate(int faceNum, int playerNum)
    {
        // Players must pass the face that they are on
        // Intention is that players will only be able to link if they're on the same face
        for (int i = 0; i < players.Length; i++)
        {
            face[i][playerNum] = false;
        }
        face[faceNum][playerNum] = true;
    }

    private void resetListAndLines()
    {
        foreach (VectorLine thisLine in lines)
        {
//            VectorLine bob = thisLine;
//            VectorLine.Destroy(ref bob);
        }
        lines.Clear();
    }

    private void queryDrawFromTo(int fromPlayer, int toPlayer)
    {
        if (Vector3.Distance(players[fromPlayer].transform.position, players[toPlayer].transform.position) < linkDistMax)
        {
            addLineToDraw(fromPlayer, toPlayer);
        }
    }

    private void addLineToDraw(int fromPlayer, int toPlayer)
    {
		Debug.Log("addLineToDraw() called");
        VectorLine myLine = new VectorLine("aLine", new List<Vector3>(segmentNumber * 2), null, lineWidth, LineType.Discrete, Joins.Weld);
        Vector3[] startFinish = new Vector3[2];
        startFinish[0] = players[fromPlayer].transform.position;
        startFinish[1] = players[fromPlayer].transform.position;
        myLine.MakeSpline(startFinish, false);
        lines.Add(myLine);
    }
}
