//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020/12/23 15:35:33
//Description: 
//=========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
	public PersistentStorage Storage;
	public KeyCode CreateKey = KeyCode.C;
	public KeyCode NewGameKey = KeyCode.N;
	public KeyCode SaveKey = KeyCode.S;
	public KeyCode LoadKey = KeyCode.L;
	public KeyCode DestroyKey = KeyCode.X;
	public int LevelCount;
	[SerializeField]
	private ShapeFactory ShapeFactory;
	private List<Shape> _shapes;
	private int _loadedLevelBuildIndex;
	private float _creationProgress, _destructionProgress;

	public float CreationSpeed { get; set; }
	public float DestructionSpeed { get; set; }

	public SpawnZone SpawnZoneOfLevel { get; set; }

	public static Game Instance { get; set; }

	private void Start()
	{
		Instance = this;
		_shapes = new List<Shape>();
		if (Application.isEditor)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene loadedLevel = SceneManager.GetSceneAt(i);
				if (loadedLevel.name.Contains("Level "))
				{
					SceneManager.SetActiveScene(loadedLevel);
					_loadedLevelBuildIndex = loadedLevel.buildIndex;
					return;
				}
			}
		}

		StartCoroutine(LoadLevel(1));
	}

	private void Update()
	{
		if (Input.GetKeyDown(CreateKey))
		{
			CreateShape();
		}

		if (Input.GetKeyDown(NewGameKey))
		{
			BeginNewGame();
		}

		if (Input.GetKeyDown(SaveKey))
		{
			Storage.Save(this);
		}

		if (Input.GetKeyDown(LoadKey))
		{
			BeginNewGame();
			Storage.Load(this);
		}

		if (Input.GetKeyDown(DestroyKey))
		{
			DestroyShape();
		}

		for (int i = 0; i <= LevelCount; i++)
		{
			if (Input.GetKeyDown(KeyCode.Alpha0 + i))
			{
				BeginNewGame();
				StartCoroutine(LoadLevel(i));
				return;
			}
		}

		_creationProgress += Time.deltaTime * CreationSpeed;
		while (_creationProgress >= 1f)
		{
			_creationProgress -= 1f;
			CreateShape();
		}

		_destructionProgress += Time.deltaTime * DestructionSpeed;
		while (_destructionProgress >= 1f)
		{
			_destructionProgress -= 1f;
			DestroyShape();
		}
	}

	public override void Save(GameDataWriter writer)
	{
		writer.Write(_shapes.Count);
		writer.Write(_loadedLevelBuildIndex);
		for (int i = 0; i < _shapes.Count; i++)
		{
			writer.Write(_shapes[i].ShapeId);
			writer.Write(_shapes[i].MaterialId);
			_shapes[i].Save(writer);
		}
	}

	public override void Load(GameDataReader reader)
	{
		var count = reader.ReadInt();
		StartCoroutine(LoadLevel(reader.ReadInt()));
		for (int i = 0; i < count; i++)
		{
			var shapeId = reader.ReadInt();
			var materialId = reader.ReadInt();
			Shape o = ShapeFactory.Get(shapeId, materialId);
			o.Load(reader);
			_shapes.Add(o);
		}
	}

	private void CreateShape()
	{
		var o = ShapeFactory.GetRandom();
		var t = o.transform;
		t.localPosition = SpawnZoneOfLevel.SpawnPoint;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		o.SetColor(Random.ColorHSV(
			0, 1,
			0.5f, 1,
			0.25f, 1,
			1, 1));
		_shapes.Add(o);
	}

	private void BeginNewGame()
	{
		for (int i = 0; i < _shapes.Count; i++)
		{
			ShapeFactory.Reclaim(_shapes[i]);
		}

		_shapes.Clear();
	}

	private void DestroyShape()
	{
		if (_shapes.Count > 0)
		{
			var index = Random.Range(0, _shapes.Count);

			ShapeFactory.Reclaim(_shapes[index]);

			var lastIndex = _shapes.Count - 1;
			_shapes[index] = _shapes[lastIndex];
			_shapes.RemoveAt(lastIndex);
		}
	}

	private IEnumerator LoadLevel(int levelBuildIndex)
	{
		enabled = false;
		if (_loadedLevelBuildIndex > 0)
		{
			yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);
		}

//		SceneManager.LoadScene("Level 1", LoadSceneMode.Additive);
//		yield return null;
		yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
		_loadedLevelBuildIndex = levelBuildIndex;
		enabled = true;
	}
}