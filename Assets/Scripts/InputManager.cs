using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private bool gyroEnabled;
    private bool OGRotSet = false;
    private Gyroscope gyro;

    private Quaternion OGRot;
    private Quaternion CurRot;
    private Quaternion CurRot1;

    private Quaternion LeftRot = new Quaternion(0, -.4f, 0, 0);
    private Quaternion RightRot = new Quaternion(0, .4f, 0, 0);
    private Quaternion BackRot = new Quaternion(.2f, 0, 0, 0);
    private Quaternion FrontRot = new Quaternion(-.6f, 0, 0, 0);

    public static int PlayerInput;

    private void Awake()
    {
        gyroEnabled = EnableGyro();
    }
    void Start()
    {
        print("Has gyro: " + gyroEnabled);
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroEnabled = true;
            return true;
        }
        else
        {
            //Say device does not support GyroScope
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!OGRotSet)
        {
            if (gyroEnabled)
            {
                OGRot = gyro.attitude;
                OGRotSet = true;
            }
        }
        if (gyroEnabled && OGRotSet)
        {
            CurRot = gyro.attitude;
            CurRot1 = CurRot * Quaternion.Inverse(OGRot);

            if (CurRot1.x <= .3f && CurRot1.x >= -.3f && CurRot1.y <= .3f && CurRot1.y >= -.3f) //Checks if rotation hasn't changed too much, meaning phone is in the middle
            {
                //print("Middle");
                PlayerInput = 0;
            }
            else if (CurRot1.x <= .4f && CurRot1.x >= -.4f && CurRot1.y <= -.4f)  //Checks X value to make sure not rotating back or forward, then checks if Y is passed the right value
            {
                //print("Left");
                PlayerInput = 1;
            }
            else if (CurRot1.x <= .4f && CurRot1.x >= -.4f && CurRot1.y >= .4f)
            {
                //print("Right");
                PlayerInput = 2;
            }
            else if (CurRot1.x <= -.4f && CurRot1.y >= -.4f && CurRot1.y <= .4f)
            {
                //print("Front");
                PlayerInput = 3;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlayerInput = 4;
            }

            if (gyro.userAcceleration.x <= -1 || gyro.userAcceleration.x >= 1 || gyro.userAcceleration.y <= -1.5f || gyro.userAcceleration.y >= 1.5f)
            {
                print("Shake");
                PlayerInput = 5;
            }


        }
    }
}


/*         //TO ROTATE BACK, NOT WORKING.
 * else if (CurRot1.x >= .4f && CurRot1.y >= -.4f && CurRot1.y <= .4f)  //if X is higher than .2 and the Y values aren't changing too much
            {
                print("Back" + CurRot1);
            }
 */
