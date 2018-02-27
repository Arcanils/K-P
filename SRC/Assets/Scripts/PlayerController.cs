using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float Speed;

	public AnimComponent Anim;


	public void Update()
	{
		Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (dir != Vector2.zero)
			dir = dir.normalized;
		Move(dir);
	}

	public void Move(Vector3 Direction)
	{
		transform.position += Direction * Speed * Time.deltaTime;
		
		if (Direction.x == 0f && Direction.y == 0f)
		{
			Anim.PlayAnim("Idle");
		}
		else
		{
			if (Mathf.Abs(Direction.x) >= Mathf.Abs(Direction.y))
			{
				Anim.PlayAnim(Direction.x > 0f ? "Right" : "Left");
			}
			else
			{
				Anim.PlayAnim(Direction.y > 0f ? "Top" : "Bottom");
			}
		}
	}
}
