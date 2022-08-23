using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Destructible2D
{
	public class MakeStamp : MonoBehaviour
	{

		public LayerMask Layers = -1;
		public D2dDestructible.PaintType Paint;
		public Texture2D Shape;
		public Color Color = Color.white;
		public Vector2 Size = Vector2.one;
		public float Angle;

		public void makeStamp()
		{
			//Debug.Log("MADE STAMP!");
			D2dStamp.All(Paint, gameObject.transform.position, Size, Angle, Shape, Color, Layers);
		}
	}
}
