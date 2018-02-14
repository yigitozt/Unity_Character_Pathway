//YIGIT OZTURK

using UnityEngine;
using System.Collections;


public class PedestrianControl : MonoBehaviour {

	CharacterController controller;
	public float stopTimer = 3;
	float stopTimerCD;

	public float speed = 3;

	GameObject cat;
	float distance;
	public float seekingDistance = 15;

	bool canMove;
	bool move;

	Vector3 oldpos;
	Vector3 newpos;
	Vector3 velocity;

	Vector3 firstPos;

	[HideInInspector]
	public Vector3 initialPosition;


	public Vector3[] positions;

	Vector3[] tempPositions;

	[HideInInspector]
	public int patrolNumber;
	[HideInInspector]
	public int currentPatrol;

	[HideInInspector]
	public int pressCount;
	[HideInInspector]
	public bool isDone;

	public void SetInitialPos()
	{
		initialPosition = transform.position;

		tempPositions = positions;
		patrolNumber += 1;
		positions = new Vector3[patrolNumber];
		positions [0] = initialPosition;
	}

	void OnDrawGizmosSelected()
	{
		if (patrolNumber > 0) {
			for(int i=0; i<patrolNumber;i++)
			{
				Gizmos.DrawWireSphere (positions[i] + new Vector3(0, 0.5f , 0), 0.2f);

				if(i >= 1)
				{
					Gizmos.DrawLine(positions[i - 1] + new Vector3(0, 0.5f , 0), positions[i] + new Vector3(0, 0.5f , 0));
				}

			}


		}
	}

	public void SetPosition()
	{
		tempPositions = positions;
		patrolNumber += 1;
		positions = new Vector3[patrolNumber];

		for (int i=0; i<patrolNumber; i++) {
			if(i < patrolNumber - 1)
			{
				positions[i] = tempPositions[i];
			}
			else
			{
				positions[i] = gameObject.transform.position;
			}

		}

	}

	public void ResetPositions()
	{
		patrolNumber = 0;
		tempPositions = null;
		positions = null;
		transform.position = initialPosition;
		initialPosition = Vector3.zero;
	}

	public void DonePositioning()
	{
		transform.position = initialPosition;
	}

	void Awake ()
	{
		gameObject.transform.LookAt (positions[0]);

		oldpos = transform.position;

		stopTimerCD = stopTimer;

		controller = GetComponent<CharacterController>();

	}

	bool stopNow;
	bool goBack;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		newpos = transform.position;
		Vector3 difference = newpos - oldpos;
		velocity = difference /Time.deltaTime;
		oldpos = newpos;
		newpos = transform.position;



		if (!cat) {
			cat = GameObject.FindWithTag ("Player");
		} else {
			distance = Vector3.Distance(gameObject.transform.position, cat.transform.position);


			if(!stopNow)
			{
				if(distance <= seekingDistance)
				{
					canMove = true;
				}
				else
				{
					canMove = false;
				}
			}

		}

		if (canMove) {
			//			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
			var forward = transform.TransformDirection(Vector3.forward);
			gameObject.transform.LookAt(positions[currentPatrol]);
			controller.SimpleMove(forward * speed);

		}


		if (Vector3.Distance (transform.position, positions [currentPatrol]) <= 0.75f) {
			stopNow = true;
			canMove = false;
		}

		if (stopNow) {
			stopTimer -= Time.deltaTime;
			if(stopTimer <= 0)
			{
				stopTimer = stopTimerCD;


				if(!goBack)
				{
					if(currentPatrol < patrolNumber - 1)
					{
						currentPatrol += 1;
					}
					else
					{
						goBack = true;
					}
				}
				else
				{
					if(currentPatrol > 0)
					{
						currentPatrol -= 1;
					}
					else
					{
						goBack = false;
					}
				}

				gameObject.transform.LookAt(positions[currentPatrol]);
				canMove = true;
				stopNow = false;
			}
		}

		//Here you can animate your character with velocity or something else.
	}
}
