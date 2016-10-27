using UnityEngine;
using System.Collections;

public class BossCreator : EnemyBaseScript {

	private GameObject bossFight;
	private const string BOSS_FIGHT_OBJ = "BossFight";
	private const string ENEMY_ORGANIZER = "Enemies";

	private void Start(){
		bossFight = Instantiate(Resources.Load(BOSS_FIGHT_OBJ), transform.position, Quaternion.identity) as GameObject;
		transform.parent = GameObject.Find(ENEMY_ORGANIZER).transform;
	}

	public override void GetDestroyed(){
		Destroy(gameObject);
	}
}