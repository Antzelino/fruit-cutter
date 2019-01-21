using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nuitrack;
using UnityEngine.UI;

public class Blade : MonoBehaviour {

	public GameObject bladeTrailPrefab;
	public float minCuttingVelocity = .001f;
	public enum Hands { left = 0, right = 1 };

	[SerializeField]
	Hands currentHand;

	bool isCutting = false;

    bool camera_connected = true;

	Vector2 previousPosition;

	GameObject currentBladeTrail;

	Rigidbody2D rb;

	Camera cam;

	Vector2 righthand;
	Vector2 lefthand;
    
	CircleCollider2D circleCollider;

	// Update is called once per frame
	void Start ()
	{
		cam = Camera.main;

		rb = GetComponent<Rigidbody2D> ();

		circleCollider = GetComponent<CircleCollider2D> ();
        circleCollider.enabled = true;

        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;

    }

	void NuitrackManager_onHandsTrackerUpdate (HandTrackerData handTrackerData)
	{
		//Debug.Log ("on hands tracker update");
		if (handTrackerData != null)
		{
            
			nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

			if (userHands != null)
			{
				if (currentHand == Hands.right && userHands.RightHand != null)
				{
                    camera_connected = true;
                    Debug.Log ("update right hand");

					righthand.x = userHands.RightHand.Value.X * Screen.width;
					righthand.y = (1f - userHands.RightHand.Value.Y) * Screen.height;
				}
                else
                {
                    if(currentHand == Hands.right)
                    {
                        Debug.Log("null RightHand");
                        //camera_connected = false;
                    }
                }

				if (currentHand == Hands.left && userHands.LeftHand != null)
				{
                    camera_connected = true;
                    Debug.Log ("update left hand");

					lefthand.x = userHands.LeftHand.Value.X * Screen.width;
					lefthand.y = (1f - userHands.LeftHand.Value.Y) * Screen.height;
				}
                else
                {
                    if (currentHand == Hands.left)
                    {
                        Debug.Log("null LeftHand");
                        //camera_connected = false;
                    }
                }
            }
            else
            {
                Debug.Log("null userHands");
                camera_connected = false;
            }
        }
    }


	void Update () {
        StartCutting();
        UpdateCut();
    }

	void UpdateCut() {
        Vector2 newPosition;
        if (camera_connected)
        {
            if (currentHand == Hands.left)
            {
                newPosition = cam.ScreenToWorldPoint(lefthand);
                //Debug.Log("pos x,y: " + lefthand.x + " , " + lefthand.y);
            }
            else
            {
                newPosition = cam.ScreenToWorldPoint(righthand);
                //Debug.Log("pos x,y: " + righthand.x + " , " + righthand.y);
            }
        }
        else
        {
            newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("pos x,y: " + Input.mousePosition.x + " , " + Input.mousePosition.y);
        }
        
		rb.position = newPosition;

		previousPosition = newPosition;
	}

	void StartCutting ()
	{
		isCutting = true;
		currentBladeTrail = Instantiate (bladeTrailPrefab, transform);
        if (!camera_connected)
        {
            previousPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (currentHand == Hands.left)
                previousPosition = cam.ScreenToWorldPoint(lefthand);
            else
                previousPosition = cam.ScreenToWorldPoint(righthand);
        }
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }
}
