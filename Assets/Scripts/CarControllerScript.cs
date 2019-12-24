using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtmostInput;

public class CarControllerScript : MonoBehaviour
{
    UIManager uIManager;
    MapGenerator mapGenerator;

    Rigidbody m_Rb;
    Animator m_Animator;
    public GameObject m_CameraRig;

    public float m_Speed;

    ParticleSystem[] m_ParticleSystems;

    InputX m_InputX;

    Transform m_ClosestTurningPoint;
    float m_DiffBtwTurningPointAndCar;
    LineRenderer m_LineRenderer;

    Vector3 m_TurningAxis;
   
    string m_CarDirection;

    float m_BondBreakingAngle;

    //if rotation starts, it cannot be finished unless the input state is ended.
    bool m_IsRotationStarted;
    //bond will break after input is ended if this is true
    bool m_BondBreakPossible;

    bool m_IsRotatable;

    private void OnEnable()
    {
        
        uIManager = FindObjectOfType(typeof(UIManager)) as UIManager;
        mapGenerator = FindObjectOfType(typeof(MapGenerator)) as MapGenerator;

        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        m_Animator = GetComponentInChildren<Animator>();

        m_Rb = GetComponent<Rigidbody>();
        m_Speed = 20f;

        m_InputX = new InputX();

        m_TurningAxis = new Vector3();
        m_LineRenderer = new LineRenderer();

        m_IsRotationStarted = false;
        m_BondBreakPossible = false;
        m_IsRotatable = false;
    }
    //şeride sok
    //içeri baktır

    void Update()
    {
        m_CameraRig.transform.position = transform.position;
        CheckIfIsRotatable();
        

        if (!DataScript.inputLock)
        {
            if(!m_IsRotationStarted)
                m_Rb.velocity = transform.forward * m_Speed;

            if (m_InputX.IsInput())
            {

                GeneralInput gInput = m_InputX.GetInput(0);

                if (gInput.phase == IPhase.Began)
                {
                    DriftStarted();
                }
                else if(gInput.phase == IPhase.Ended)
                {
                    DriftFinished();
                }
                else
                {
                    TryRotateCar();
                }

            }
        }
    }

    //when input is entered this function will look at if a bond should be broken, 
    //sets the line renderer and starts tire trails, find turning axes to rotate the car around them
    void DriftStarted()
    {
        if (m_BondBreakPossible)
        {
            DataScript.turningPoints.Remove(m_ClosestTurningPoint);
        }

        m_BondBreakPossible = false;
        m_ClosestTurningPoint = FindClosestTurningPoint();
        m_LineRenderer = m_ClosestTurningPoint.gameObject.GetComponent<LineRenderer>();
        DrawTireTrails(true);
        FindTurningAxes();
    }

    //checks if the car can be rotatable, when the car enters a new road it should not be rotatable until reaches the near of a turning point
    void CheckIfIsRotatable()
    {
        if (FindClosestTurningPoint() != null)
        {
            float xDiff = transform.position.x - FindClosestTurningPoint().position.x;
            float zDiff = transform.position.z - FindClosestTurningPoint().position.z;
            if ((m_CarDirection == "up" || m_CarDirection == "down") && Mathf.Abs(zDiff) <= 1f)
                m_IsRotatable =  true;
            else if ((m_CarDirection == "right" || m_CarDirection == "left") && Mathf.Abs(xDiff) <= 1f)
            {
                m_IsRotatable = true;
            }
        }
    }

    //removes line and tire trails when drift finish
    void DriftFinished()
    {
        m_IsRotationStarted = false;
        m_LineRenderer.SetPosition(0, m_LineRenderer.gameObject.transform.position);
        m_LineRenderer.SetPosition(1, m_LineRenderer.gameObject.transform.position);
        DrawTireTrails(false);
        
    }

    //if the car is rotatable or already rotating,
    //this function rotates the car around the axis which found in DriftStarted function
    void TryRotateCar()
    {
        if(m_IsRotationStarted || m_IsRotatable)
        {
            m_IsRotationStarted = true;
            m_LineRenderer.SetPosition(0, m_LineRenderer.gameObject.transform.position);
            m_LineRenderer.SetPosition(1, transform.position);
            
            Quaternion lookRotation = Quaternion.LookRotation(m_ClosestTurningPoint.position - transform.position, Vector3.up);
            lookRotation.x = transform.rotation.x;
            lookRotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
            
            transform.RotateAround(m_ClosestTurningPoint.position, m_TurningAxis, 2f);
               
        }
        
    }

    //car correction after turning
    //corrects car according to its current rotation and the direction it should go on
    IEnumerator CarCorrectionAnimator(string roadType)
    {
        Vector3 currentLookPos = transform.rotation.eulerAngles;
        
        if (currentLookPos.y <= 90f && roadType == "up")
        {
           
            while(Mathf.Abs(transform.rotation.eulerAngles.y) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            transform.Rotate(new Vector3(0, -0.5f, 0));
            yield return new WaitForEndOfFrame();
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 355f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 359f)
            {
                
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            transform.Rotate(new Vector3(0, 0.5f, 0));
            yield return new WaitForEndOfFrame();
        }
        else if (currentLookPos.y >= 270f && roadType == "up")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 359f && Mathf.Abs(transform.rotation.eulerAngles.y) >= 270f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            transform.Rotate(new Vector3(0, 2f, 0));
            yield return new WaitForEndOfFrame();
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 180f && currentLookPos.y >90f && roadType == "right")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 85.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 89.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 90f && roadType == "right")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 95f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 90.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 180f && currentLookPos.y >= 90f && roadType == "down")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 185f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 180.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y >= 180f && currentLookPos.y <= 270f && roadType == "down")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 175)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 179.5)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y >= 180f && currentLookPos.y <= 270f && roadType == "left")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 275)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 270.5)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y >= 270f && roadType == "left")
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) >= 265f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 269.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        StopCoroutine(CarCorrectionAnimator(roadType));
    }

    //to give effect of drifting 
    void DrawTireTrails(bool isStart)
    {
        if (isStart)
        {
            for (int i = 0; i < 2; i++)
            {
                m_ParticleSystems[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                m_ParticleSystems[i].Stop();
            }
        }
        
    }


    //Find turning axes to rotate around, we can rotate around up and down axes
    void FindTurningAxes()
    {
        if (m_CarDirection == "up")
        {
            //rotate right
            if (m_ClosestTurningPoint.position.x > transform.position.x)
            {
                m_TurningAxis = Vector3.up;

            }
            //rotate left
            else
            {
                m_TurningAxis = Vector3.down;

            }
        }
        else if (m_CarDirection == "down")
        {
            if (m_ClosestTurningPoint.position.x < transform.position.x)
            {
                m_TurningAxis = Vector3.up;
            }
            else
            {
                m_TurningAxis = Vector3.down;
            }
        }
        else if (m_CarDirection == "right")
        {
            if (m_ClosestTurningPoint.position.z < transform.position.z)
            {
                m_TurningAxis = Vector3.up;
            }
            else
            {
                m_TurningAxis = Vector3.down;
            }
        }
        else if (m_CarDirection == "left")
        {
            if (m_ClosestTurningPoint.position.z > transform.position.z)
            {
                m_TurningAxis = Vector3.up;
            }
            else
            {
                m_TurningAxis = Vector3.down;
            }
        }
    }

    //to find closest turning point, closest turning points are the points that lines are drawn from
    Transform FindClosestTurningPoint()
    {
        Transform closestTurningPoint = DataScript.turningPoints[0];
        m_DiffBtwTurningPointAndCar = Vector3.SqrMagnitude(closestTurningPoint.position - transform.position);
        foreach (Transform t in DataScript.turningPoints)
        {
            if (Vector3.SqrMagnitude(t.position - transform.position) <= m_DiffBtwTurningPointAndCar)
            {
                closestTurningPoint = t;
                m_DiffBtwTurningPointAndCar = Vector3.SqrMagnitude(closestTurningPoint.position - transform.position);
            }
        }
        return closestTurningPoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        //to understand collision and game over
        if(collision.gameObject.tag == "CurvedRoadCollider" || collision.gameObject.tag == "StraightRoadCollider")
        {
            uIManager.GameOverAtPosition(transform.position);
            DataScript.inputLock = true;
            Destroy(this.gameObject);
        }

        //to find direction of a car on a straight road
        if(collision.gameObject.tag == "StraightRoad")
        {

           
            //should remove passed roads...
            DataScript.passedRoadCount++;
           
            //create new roads when the player are close enough to the last road
            if(DataScript.passedRoadCount >= DataScript.totalRoadCount - 2)
            {
                mapGenerator.GenerateRoads(10);
            }

            //when the car enters a new road, make its correction,
            //in curved roads correction is not possible just like sling drift
            string direction = collision.gameObject.GetComponent<StraightRoadDataHolder>().direction;
            StartCoroutine(CarCorrectionAnimator(direction));

            //car can not rotate when enters a new road...
            if (m_CarDirection != direction) {
                DataScript.turningPoints.Remove(m_ClosestTurningPoint);
                m_IsRotatable = false;
            }
                

            //set car direction here for when the car does not go straight
            if (direction == "up")
            {
                m_CarDirection = "up";
            }
            else if(direction == "right")
            {
                m_CarDirection = "right";
            }
            else if(direction == "left")
            {
                m_CarDirection = "left";
            }
            else if(direction == "down")
            {
                m_CarDirection = "down";
            }

            DataScript.inputLock = false;
            m_IsRotatable = false;
        }
    }
    
}
