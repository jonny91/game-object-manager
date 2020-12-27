//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020/12/27 15:41:26
//Description: 
//=========================================

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SpawnZone : MonoBehaviour
{
	public abstract Vector3 SpawnPoint { get; }
}