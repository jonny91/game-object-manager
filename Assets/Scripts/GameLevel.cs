//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020/12/27 16:01:55
//Description: 
//=========================================

using System;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
	[SerializeField]
	private SpawnZone SpawnZone;

	private void Start()
	{
		Game.Instance.SpawnZoneOfLevel = SpawnZone;
	}
}