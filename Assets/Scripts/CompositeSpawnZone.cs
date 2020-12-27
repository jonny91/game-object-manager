//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-27 16:15:00
//Description: 
//=========================================

using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
	[SerializeField]
	private SpawnZone[] SpawnZones;

	public override Vector3 SpawnPoint
	{
		get
		{
			var index = Random.Range(0, SpawnZones.Length);
			return SpawnZones[index].SpawnPoint;
		}
	}
}