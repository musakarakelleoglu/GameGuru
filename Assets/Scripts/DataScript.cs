using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScript
{

    public static List<Transform> turningPoints;

    public static bool inputLock; // to lock input, input is locked if true

    //to create new roads while playing
    public static int passedRoadCount;  
    public static int totalRoadCount;

}





/*
 IEnumerator CarCorrectionAnimator()
    {
        Vector3 currentLookPos = transform.rotation.eulerAngles;
        
        if (currentLookPos.y <= 45f)
        {
            while(Mathf.Abs(transform.rotation.eulerAngles.y) >= 0f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 355f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 359f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if(currentLookPos.y <= 90f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 95f) >= 1f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 90f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 135f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 85f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 90f) >= 1f)
            {
                transform.Rotate(new Vector3(0,1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 180f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 185f) >= 1f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 225f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 175f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180f) >= 1f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 270f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 275f) >= 1f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 27f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 315f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 265f) >= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 270f) >= 1f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 359f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 5f)
            {
                transform.Rotate(new Vector3(0, 1f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 1f)
            {
                transform.Rotate(new Vector3(0, -1f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        
        StopCoroutine(CarCorrectionAnimator());
    }
*/

