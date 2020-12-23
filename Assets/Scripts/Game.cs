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
using Random = UnityEngine.Random;

public class Game : PersistableObject
{
	public PersistentStorage Storage;
	public PersistableObject Prefab;
	public KeyCode CreateKey = KeyCode.C;
	public KeyCode NewGameKey = KeyCode.N;
	public KeyCode SaveKey = KeyCode.S;
	public KeyCode LoadKey = KeyCode.L;

	private List<PersistableObject> _objects;

	private void Awake()
	{
		_objects = new List<PersistableObject>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(CreateKey))
		{
			CreateObject();
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
	}

	public override void Save(GameDataWriter writer)
	{
		writer.Write(_objects.Count);
		for (int i = 0; i < _objects.Count; i++)
		{
			_objects[i].Save(writer);
		}
	}

	public override void Load(GameDataReader reader)
	{
		var count = reader.ReadInt();
		for (int i = 0; i < count; i++)
		{
			PersistableObject o = Instantiate(Prefab);
			o.Load(reader);
			_objects.Add(o);
		}
	}

	private void CreateObject()
	{
		var o = Instantiate(Prefab);
		var t = o.transform;
		t.localPosition = Random.insideUnitSphere * 5f;
		t.localRotation = Random.rotation;
		t.localScale = Vector3.one * Random.Range(0.1f, 1f);
		_objects.Add(o);
	}

	private void BeginNewGame()
	{
		for (int i = 0; i < _objects.Count; i++)
		{
			Destroy(_objects[i].gameObject);
		}

		_objects.Clear();
	}
}