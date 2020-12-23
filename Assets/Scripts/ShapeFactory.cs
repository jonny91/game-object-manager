//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-23 17:26:53
//Description: 
//=========================================

using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
	[SerializeField]
	private Shape[] Prefabs;

	[SerializeField]
	private Material[] Materials;

	public Shape Get(int shapeId = 0, int materialId = 0)
	{
		var instance = Instantiate(Prefabs[shapeId]);
		instance.ShapeId = shapeId;
		instance.SetMaterial(Materials[materialId], materialId);
		return instance;
	}

	public Shape GetRandom()
	{
		return Get(
			Random.Range(0, Prefabs.Length),
			Random.Range(0, Materials.Length)
		);
	}
}