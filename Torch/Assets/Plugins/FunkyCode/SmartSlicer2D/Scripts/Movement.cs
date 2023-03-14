using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slicer2D {

	//有Update，主要作用是获得最新的 Sliceable2D 的 postion，rotation等信息
    //在 Shape 类中被调用
	public class Movement
	{
		public bool update = true;

		public Vector3 updatePosition = Vector3.zero;
		public float updateRotation = 0f;

		public Sprite sprite;
		public bool spriteflipX = false;
		public bool spriteflipY = false;

		//将 update 设置为 true
		public void ForceUpdate() 
		{
			update = true;
		}

		/// <summary>
		/// 更新position，rotation，Sprite 和 Sprite xy翻转，记录在Movement中
		/// </summary>
		/// <param name="source"></param>
		public void Update(Sliceable2D source)
		{
			Transform transform = source.transform;
			
			//更新position
			if (updatePosition != transform.position) 
			{
				updatePosition = transform.position;

				update = true;
			}

			//更新rotation
			if (updateRotation != transform.rotation.eulerAngles.z)
			{
				updateRotation = transform.rotation.eulerAngles.z;

				update = true;
			}

			//更新 Sprite 和 Sprite xy翻转
			if (source.spriteRenderer != null) 
			{	
				if (sprite != source.spriteRenderer.sprite) 
				{
					sprite = source.spriteRenderer.sprite;

					update = true;
				}

				if (spriteflipX != source.spriteRenderer.flipX)
				{
					spriteflipX = source.spriteRenderer.flipX;

					update = true;
				}

				if (spriteflipY != source.spriteRenderer.flipY) 
				{
					spriteflipY = source.spriteRenderer.flipY;

					update = true;
				}
			}
		}
	}
}