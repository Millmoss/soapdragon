using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPhysics : MonoBehaviour
{
	public PolyTerrain terrain;
	public float weight;
	public float friction = .5f;
	public float bounce = .3f;
	public float gravity = -9.81f;
	public float xVelocity = 0;
	public float yVelocity = 0;
	public float zVelocity = 0;
	public float terminalVelocity = -96;
	private Vector3 normal = Vector3.zero;
	private bool onGround = false;

	public float tempFric = 100;

	void Start ()
	{

	}
	
	void Update ()
	{
		//x axis handling

		if (onGround)
		{
			xVelocity = Mathf.Lerp(xVelocity, 0, friction * Time.deltaTime * tempFric) + normal.x * Time.deltaTime;
			zVelocity = Mathf.Lerp(zVelocity, 0, friction * Time.deltaTime * tempFric) + normal.z * Time.deltaTime;
			if (Vector3.Magnitude(new Vector3(xVelocity, 0, zVelocity)) < .05f)
			{
				xVelocity = 0;
				zVelocity = 0;
			}
		}
		else
		{
			if (normal != Vector3.zero)
			{
				xVelocity += normal.x * yVelocity;
				zVelocity += normal.z * yVelocity;
			}
		}

		transform.position += new Vector3(xVelocity * Time.deltaTime, 0, zVelocity * Time.deltaTime);

		//y axis handling

		float groundHeight = terrain.getHeight(transform.position.x, transform.position.z);

		transform.position += new Vector3(0, yVelocity * Time.deltaTime, 0);

		if (groundHeight < transform.position.y)
		{
			if (onGround && yVelocity == 0)
			{
				transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
				normal = terrain.getNormal(transform.position.x, transform.position.z);
			}
			else
			{
				normal = Vector3.zero;
				onGround = false;
			}
		}

		if (groundHeight >= transform.position.y)
		{
			if (yVelocity == 0 || Mathf.Abs(yVelocity) <= .5f)
			{
				yVelocity = 0;
				transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
				onGround = true;
			}
			else
			{
				yVelocity = -yVelocity;
				transform.position += new Vector3(0, yVelocity * Time.deltaTime, 0);
				if (groundHeight > transform.position.y)
				{
					transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
				}
				yVelocity *= bounce;
				onGround = false;
			}
			normal = terrain.getNormal(transform.position.x, transform.position.z);
		}

		if (!onGround)
			yVelocity += gravity * Time.deltaTime;
	}
}
