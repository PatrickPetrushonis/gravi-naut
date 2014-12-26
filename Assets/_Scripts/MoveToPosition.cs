using UnityEngine;
using System.Collections;

public class MoveToPosition : MonoBehaviour 
{
	private Vector3 start;
	private Vector3 end;
	private float speed = 10f;

	void Start()
	{
		end = transform.position;
	}

	void Update()
	{
		start = transform.position;

		if(SystemInfo.deviceType == DeviceType.Desktop)
		{
			//Set target movement position to last mouse position
			if(Input.GetMouseButtonUp(0))
			{
				end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else if(SystemInfo.deviceType == DeviceType.Handheld)
		{
			//Set target movement position to last touch position
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				end = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			}
		}
		//Permit only horizontal movement
		end.z = transform.position.z;		
		//if gravity is vertical
		end.y = transform.position.y;
		//else end.x = transform.position.x;

		transform.position = Vector3.MoveTowards(start, end, speed * Time.deltaTime);
	}
}
