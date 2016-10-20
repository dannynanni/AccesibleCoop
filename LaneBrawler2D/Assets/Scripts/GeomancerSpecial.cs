using UnityEngine;
using System.Collections;

public class GeomancerSpecial : MonoBehaviour, SpecialAbilityInterface {

    private int brickCount = 3;
    private float timer;

    private RaycastHit2D myRCH;
    private bool ability2 = true;

    public LayerMask myLM;

    public void SpecialAbility1(int leftRight)
    {
        myRCH = Physics2D.Raycast(transform.position + Vector3.right * leftRight + Vector3.up * 1.64f, Vector2.down, 5, myLM);
        if (myRCH.transform.gameObject.name == "Gnd - Flat - Grass(Clone)")
        {
            myRCH.transform.position = myRCH.transform.position + Vector3.up * .06f;
            myRCH.transform.gameObject.GetComponent<GroundBehaviour>().isMoving = true;
        }
        //Debug.Log(myRCH.transform.gameObject.name);
        //Destroy(myRCH.transform.gameObject);
    }

    public void SpecialAbility2(int leftRight)
    {
        if (ability2 && brickCount > 0)
        {
            Debug.Log("ability 2");
            myRCH = Physics2D.Raycast(transform.position + (Vector3.right * leftRight), Vector2.down, 5, myLM);
            Instantiate(Resources.Load("DirtPatch"), myRCH.transform.position + Vector3.up * 2, Quaternion.identity, myRCH.transform);
            ability2 = false;
            brickCount--;
        }

    }

    public void Reset1()
    {

    }

    public void Reset2()
    {
        ability2 = true;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer -= 5;
            brickCount ++;
            if (brickCount >= 3)
                brickCount = 3;
        }
    }

}
