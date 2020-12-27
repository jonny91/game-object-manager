//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-27 16:09:27
//Description: 
//=========================================

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawnZone : SpawnZone
{
	[SerializeField]
	private bool SurfaceOnly;

	public override Vector3 SpawnPoint
	{
		get
		{
			Vector3 p;
			p.x = p.y = p.z = Random.Range(-0.5f, 0.5f);
			if (SurfaceOnly)
			{
				int axis = Random.Range(0, 3);
				p[axis] = p[axis] < 0f ? -0.5f : 0.5f;
			}

			return transform.TransformPoint(p);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
	}
}