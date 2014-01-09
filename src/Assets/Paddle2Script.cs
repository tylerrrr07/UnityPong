using UnityEngine;
using System.Collections;

public class Paddle2Script : MonoBehaviour {

	public float Speed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Updating");
		if (Input.GetButton ("Paddle2Down")) 
		{
			Debug.Log("Down");
			MovePaddleY(Vector3.down);
		}
		else if (Input.GetButton ("Paddle2Up")) 
		{
			Debug.Log("Up");
			MovePaddleY(Vector3.up);
		}
	}
	
	void MovePaddleY(Vector3 direction)
	{
		Vector3 positionEdge = transform.position;
		float edgeOffset = direction.y * renderer.bounds.extents.y;
		positionEdge.y += edgeOffset;
		
		Vector3 movement = Vector3.zero;
		
		int layer = 9;
		int layerMask = 1 << layer; 
		
		RaycastHit2D hit2d = Physics2D.Raycast (positionEdge, direction, Mathf.Infinity, layerMask);

		if (hit2d.fraction > Time.deltaTime * Speed)
		{
			movement = direction * Time.deltaTime * Speed;
		}
		else
		{
			movement = direction * hit2d.fraction;
		}

		transform.Translate(movement);
	}
}
