/*
 * 
 * This class is necessary to allow the ChangeEnemyType script to destroy enemies, no matter what type they are.
 * 
 * All enemies  and enemy creators MUST inherit from this script,
 * so that ChangeEnemyType can find them when it's time to destroy them.
 * 
 */

using UnityEngine;
using System.Collections;

public abstract class EnemyBaseScript : MonoBehaviour {

	public abstract void GetDestroyed();
}
