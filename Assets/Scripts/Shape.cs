//=========================================
//Author: 洪金敏
//Email: jonny.hong91@gmail.com
//Create Date: 2020-12-23 17:21:30
//Description: 
//=========================================

using System;
using UnityEngine;

public class Shape : PersistableObject
{
	public int ShapeId { get; set; }
	public int MaterialId { get; private set; }
	public Color Color { get; private set; }

	private MeshRenderer _meshRenderer;
	private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
	private static MaterialPropertyBlock SharedPropertyBlock;

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
	}

	public void SetMaterial(Material material, int materialId)
	{
		_meshRenderer.material = material;
		MaterialId = materialId;
	}

	public void SetColor(Color color)
	{
		Color = color;
//		_meshRenderer.material.color = color;

		if (SharedPropertyBlock == null)
		{
			SharedPropertyBlock = new MaterialPropertyBlock();
		}

		SharedPropertyBlock.SetColor(ColorPropertyId, color);
		_meshRenderer.SetPropertyBlock(SharedPropertyBlock);
	}

	public override void Save(GameDataWriter writer)
	{
		writer.Write(Color);
		base.Save(writer);
	}

	public override void Load(GameDataReader reader)
	{
		base.Load(reader);
		SetColor(reader.ReadColor());
	}
}