using UnityEngine;
using System.Collections;

public class BallMovement : MonoBehaviour {


	public AudioClip sfxBounce;
	public AudioClip sfxScore;

	public Vector3 Direction;
	public float Speed;
	public bool isPlaying;
	int score1;
	int score2;

	// Use this for initialization
	void Start () 
	{
		Reset ();
		score1 = 0;
		score2 = 0;



//		Direction.y = 0;
//		Direction.x = 1;
//		Direction.Normalize();

	}


	void Reset()
	{
		transform.position = Vector3.zero;
		isPlaying = false;
		Direction = Vector3.zero;
		Direction.y = Random.Range(-5, 5);
		Direction.x = Random.Range(-6, -4);
		Direction.Normalize();
	}

	// Update is called once per frame
	void Update () 
	{	
		if (isPlaying)
		{
			MoveBallY();
			MoveBallX();
		}
	}

	void MoveBallX()
	{
		int directionModifier = Direction.x < 0 ? -1 : 1;

		Vector3 directionX = Vector3.right * directionModifier;
		
		Vector3 positionEdge = transform.position;
		float edgeOffset = directionModifier * renderer.bounds.extents.x;
		positionEdge.x += edgeOffset;
		
		Vector3 movement = Vector3.zero;

		int paddleLayer = 8;
		int wallLayer = 9;

		int paddleMask = 1 << paddleLayer;
		int wallMask = 1 << wallLayer;

		int layerMask = wallMask | paddleMask;
        
        RaycastHit2D hit2d = Physics2D.Raycast (positionEdge +  (new Vector3(Direction.x, 0, 0) * Time.deltaTime * Speed), directionX, Mathf.Infinity, layerMask);
		

		if (hit2d.fraction > Mathf.Abs(Time.deltaTime * Speed * Direction.x) || hit2d.collider == null)
		{
			movement = new Vector3(Direction.x, 0, 0) * Time.deltaTime * Speed;
		}
		else
		{
			if (hit2d.collider.name.Contains("Goal"))
		    {
				if (hit2d.collider.name == "LeftGoal")
				{
					score2++;
					GameObject scoreObject = GameObject.Find("Score2");
					GUIText score = (GUIText)scoreObject.GetComponent(typeof(GUIText));
					score.text = score2.ToString();
				}
				else if (hit2d.collider.name == "RightGoal")
				{
					score1++;
					GameObject scoreObject = GameObject.Find("Score1");
					GUIText score = (GUIText)scoreObject.GetComponent(typeof(GUIText));
					score.text = score1.ToString();
				}

				audio.PlayOneShot(sfxScore);


				Reset();
				return;
			}
			Vector2 collisionPosition = positionEdge;
			collisionPosition.x += hit2d.fraction * directionModifier;
			
			float distanceToTravel = Mathf.Abs(Time.deltaTime * Speed * Direction.x);
			float distanceToTravelAfterBounce = distanceToTravel - hit2d.fraction;
			
			Vector3 positionAfterBounce = new Vector3(transform.position.x, 0, 0);
			positionAfterBounce.x =  collisionPosition.x + (-directionModifier * distanceToTravelAfterBounce);			
			
			movement = positionAfterBounce - new Vector3(positionEdge.x, 0, 0);
			Direction = Vector3.Reflect(Direction, Vector3.right);
			audio.PlayOneShot(sfxBounce);
		}
		
		transform.Translate(movement);
	}

	void MoveBallY()
	{
		int directionModifier = Direction.y < 0 ? -1 : 1;
		Vector3 directionY = Vector3.up * directionModifier;

		Vector3 positionEdge = transform.position;
		float edgeOffset = directionModifier * renderer.bounds.extents.y;
		positionEdge.y += edgeOffset;
		
		Vector3 movement = Vector3.zero;

		int paddleLayer = 8;
		int wallLayer = 9;
		
		int paddleMask = 1 << paddleLayer;
		int wallMask = 1 << wallLayer;
		
		int layerMask = wallMask | paddleMask;

		RaycastHit2D hit2d = Physics2D.Raycast (positionEdge + (new Vector3(0, Direction.y, 0) * Time.deltaTime * Speed), directionY, Mathf.Infinity, layerMask);


		if (hit2d.fraction > Mathf.Abs(Time.deltaTime * Speed * Direction.y) || hit2d.collider == null)
		{
			movement = new Vector3(0, Direction.y, 0) * Time.deltaTime * Speed;

		}
		else
		{
			Vector2 collisionPosition = positionEdge;
			collisionPosition.y += hit2d.fraction * directionModifier;

			float distanceToTravel = Mathf.Abs(Time.deltaTime * Speed * Direction.y);
			float distanceToTravelAfterBounce = distanceToTravel - hit2d.fraction;

			Vector3 positionAfterBounce = new Vector3(0, transform.position.y, 0);
			positionAfterBounce.y =  collisionPosition.y + (-directionModifier * distanceToTravelAfterBounce);	
			movement = positionAfterBounce - new Vector3(0, positionEdge.y, 0);

			Direction = Vector3.Reflect(Direction, Vector3.up);
			audio.PlayOneShot(sfxBounce);

		}
		
		transform.Translate(movement);
	}


}
