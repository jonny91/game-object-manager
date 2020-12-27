//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-27 16:05:50
//Description: 
//=========================================

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereSpawnZone : SpawnZone
{
	[SerializeField]
	private bool SurfaceOnly;

	public override Vector3 SpawnPoint =>
		transform.TransformPoint(SurfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
	}
}