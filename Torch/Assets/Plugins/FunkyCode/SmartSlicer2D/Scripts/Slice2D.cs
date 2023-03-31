using System.Collections.Generic;
using UnityEngine;
using Utilities2D.Extensions;
using Utilities2D;

namespace Slicer2D {
		
	///装载着 切割 后产生的子物体信息
	public class Slice2D
	{

		public Slice2DType sliceType = Slice2DType.Undefined;

		///将生成的所有的slice
		public List<Vector2D> slice = new List<Vector2D>();
		public List<List<Vector2D>> slices = new List<List<Vector2D>>();
		///切割前的原GameObject
		public GameObject originGameObject = null;

		///切割后产生的不同物体的碰撞体
		private List<Vector2D> collisions = null;
		///切割后产生的不同物体的GameObject
		public List<GameObject> gameObjects = null;
		///切割后产生的不同物体的Polygon2D
		private List<Polygon2D> polygons = null;


		/// <summary>
		/// 返回切割后将生成的collisions列表，内容暂时为null 
		/// 但为什么是Vector2D类型？？
		/// </summary>
		/// <returns></returns>
		public List<Vector2D> GetCollisions()
		{
			if (collisions == null)
			{
				collisions = new List<Vector2D>();
			}
			return(collisions);
		}

		/// <summary>
		/// 返回切割后将生成的GameObject列表，内容暂时为null
		/// </summary>
		/// <returns></returns>
		public List<GameObject> GetGameObjects()
		{
			if (gameObjects == null) 
			{
				gameObjects = new List<GameObject>();
			}
			return(gameObjects);
		}

		public List<Polygon2D> GetPolygons() 
		{
			if (polygons == null) {
				polygons = new List<Polygon2D>();
			}
			return(polygons);
		}


		// Complex Slicer
		public void AddSlice(List<Vector2D> list)
		{
			slices.Add(list);
		}

		/////

		public void AddCollision(Vector2D point) 
		{
			if (collisions == null) 
			{
				collisions = new List<Vector2D>();
			}
			collisions.Add (point);
		}


		/////
		
		public void AddGameObject(GameObject gameObject)
		{
			if (gameObjects == null) 
			{
				gameObjects = new List<GameObject>();
			}
			gameObjects.Add (gameObject);
		}

		public void AddGameObjects(List<GameObject> newGameObjects) 
		{
			if (newGameObjects.Count < 1) 
			{
				return;
			}
			
			if (gameObjects == null) {
				gameObjects = new List<GameObject>();
			}

			foreach (GameObject gameObject in new List<GameObject>(newGameObjects))
			{
				AddGameObject (gameObject);
			}
		}

		public void SetGameObjects(List<GameObject> newGameObjects)
		{
			if (newGameObjects.Count < 1) 
			{
				return;
			}

			gameObjects = newGameObjects;
		}

		///

		
		public void SetPolygons(List<Polygon2D> newPolygons) 
		{
			if (newPolygons.Count < 1) 
			{
				return;
			}

			polygons = newPolygons;
		}


		public void AddPolygon(Polygon2D polygon)
		{
			if (polygons == null) {
				polygons = new List<Polygon2D>();
			}
			polygons.Add (polygon);
		}

		public void RemovePolygon(Polygon2D polygon)
		{
			if (polygons == null) {
				return;
			}
			polygons.Remove (polygon);
		}

		////



		///// CONSTRUCTOR /////

		// Linear Slice
		public void AddSlice(Pair2D slice)
		{
			List<Vector2D> list = new List<Vector2D>();
			list.Add(slice.A);
			list.Add(slice.B);
			slices.Add(list);
		}

		// Complex Slice
		public static Slice2D Create(GameObject originGameObject, List<Vector2D> newSlice) 
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.Complex);
			slice2D.slice = new List<Vector2D>(newSlice);

			return(slice2D);
		}

		// Linear Slice
		///Sliceable2D 调用 Slice2D的 Create 方法来切割自己
		public static Slice2D Create(GameObject originGameObject, Pair2D newSlice) 
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.Linear);
			slice2D.slice = new List<Vector2D>();
			slice2D.slice.Add(newSlice.A);
			slice2D.slice.Add(newSlice.B);

			return(slice2D);
		}

		// Linear Cut Slice
		public static Slice2D Create(GameObject originGameObject, LinearCut newSlice)
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.LinearCut);
			slice2D.slice = new List<Vector2D>();
			

			slice2D.slice.Add(newSlice.pairCut.a.ToVector2D());
			slice2D.slice.Add(newSlice.pairCut.b.ToVector2D());

			
			return(slice2D);
		}

		//Complex Cut Slice
		public static Slice2D Create(GameObject originGameObject, ComplexCut newSlice)
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.ComplexCut);
			slice2D.slice = new List<Vector2D>(newSlice.pointsList.ToVector2DList());
		
			return(slice2D);
		}

		// Point Slice
		public static Slice2D Create(GameObject originGameObject, Vector2D point, float rotation) 
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.Point);
			slice2D.slice = new List<Vector2D>();
			slice2D.slice.Add(point);
			return(slice2D);
		}

		// Polygon Slice
		public static Slice2D Create(GameObject originGameObject, Polygon2D slice)
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.Polygon);
			slice2D.slice = new List<Vector2D>(slice.pointsList);
			return(slice2D);
		}

		// Exploding Point Slice
		public static Slice2D Create(GameObject originGameObject, Vector2D point)
		{
			Slice2D slice2D = Create(originGameObject, Slice2DType.ExplodeByPoint);
			slice2D.slice = new List<Vector2D>();
			slice2D.slice.Add(point);
			return(slice2D);
		}

		public static Slice2D Create(GameObject originGameObject, Slice2DType sliceType) {
			Slice2D slice2D = new Slice2D ();
			slice2D.sliceType = sliceType;
			slice2D.originGameObject = originGameObject;
			return(slice2D);
		}
	}

}