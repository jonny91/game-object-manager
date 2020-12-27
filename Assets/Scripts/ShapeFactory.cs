//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-23 17:26:53
//Description: 
//=========================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
	[SerializeField]
	private Shape[] Prefabs;

	[SerializeField]
	private Material[] Materials;

	[SerializeField]
	private bool Recycle;

	private List<Shape>[] _pools;

	private Scene _poolScene;

	public void CreatePools()
	{
		_pools = new List<Shape>[Prefabs.Length];
		for (int i = 0; i < _pools.Length; i++)
		{
			_pools[i] = new List<Shape>();
		}

		if (Application.isEditor)
		{
			_poolScene = SceneManager.GetSceneByName(name);
			if (_poolScene.isLoaded)
			{
				var rootObjects = _poolScene.GetRootGameObjects();
				for (int i = 0; i < rootObjects.Length; i++)
				{
					var pooledShape = rootObjects[i].GetComponent<Shape>();
					if (pooledShape.gameObject.activeSelf)
					{
						_pools[pooledShape.ShapeId].Add(pooledShape);
					}
				}

				return;
			}
		}

		_poolScene = SceneManager.CreateScene(name);
	}

	public Shape Get(int shapeId = 0, int materialId = 0)
	{
		Shape instance;
		if (Recycle)
		{
			if (_pools == null)
			{
				CreatePools();
			}

			var pool = _pools[shapeId];
			int lastIndex = pool.Count - 1;
			if (lastIndex >= 0)
			{
				instance = pool[lastIndex];
				instance.gameObject.SetActive(true);
				pool.RemoveAt(lastIndex);
			}
			else
			{
				instance = Instantiate(Prefabs[shapeId]);
				instance.ShapeId = shapeId;

				SceneManager.MoveGameObjectToScene(instance.gameObject, _poolScene);
			}
		}
		else
		{
			instance = Instantiate(Prefabs[shapeId]);
			instance.ShapeId = shapeId;
		}


		instance.SetMaterial(Materials[materialId], materialId);
		return instance;
	}

	public void Reclaim(Shape shapeToRecycle)
	{
		if (Recycle)
		{
			if (_pools == null)
			{
				CreatePools();
			}

			_pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
			shapeToRecycle.gameObject.SetActive(false);
		}
		else
		{
			Destroy(shapeToRecycle.gameObject);
		}
	}

	public Shape GetRandom()
	{
		return Get(
			Random.Range(0, Prefabs.Length),
			Random.Range(0, Materials.Length)
		);
	}
}