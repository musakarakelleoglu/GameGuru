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
    

    private void OnEnable()
    {
        uIManager = FindObjectOfType(typeof(UIManager)) as UIManager;
        mapGenerator = FindObjectOfType(typeof(MapGenerator)) as MapGenerator;

        m_ParticleSystems = GetComponentsInChildren<ParticleSystem>();
        m_Animator = GetComponentInChildren<Animator>();

        m_Rb = GetComponent<Rigidbody>();
        m_Speed = 13f;

        m_InputX = new InputX();

        m_TurningAxis = new Vector3();
        m_LineRenderer = new LineRenderer();
    }


    void Update()
    {
        m_CameraRig.transform.position = transform.position;

        if (!DataScript.inputLock)
        {
            m_Rb.velocity = transform.up * m_Speed * -1f;

            if (m_InputX.IsInput())
            {

                GeneralInput gInput = m_InputX.GetInput(0);


                //Find Turning axes and direction
                if (gInput.phase == IPhase.Began)
                {
                    m_ClosestTurningPoint = FindClosestTurningPoint();
                    
                    m_LineRenderer = m_ClosestTurningPoint.gameObject.GetComponent<LineRenderer>();
                    DrawTireTrails(true);
                    FindTurningAxes();
                }
                else if(gInput.phase == IPhase.Ended)
                {
                    m_LineRenderer.SetPosition(1, m_LineRenderer.gameObject.transform.position);
                    DrawTireTrails(false);
                    StartCoroutine(CarCorrectionAnimator());
                }
                else
                {

                    transform.RotateAround(m_ClosestTurningPoint.position, m_TurningAxis, 1f);
                    m_LineRenderer.SetPosition(0, m_LineRenderer.gameObject.transform.position);
                    m_LineRenderer.SetPosition(1, transform.position);
                }

            }
        }
    }

    //car correction after turning
    IEnumerator CarCorrectionAnimator()
    {
        m_Animator.SetBool("isCarCorrection", true);
        yield return new WaitForSecondsRealtime(0.2f);
        m_Animator.SetBool("isCarCorrection", false);
        StopCoroutine(CarCorrectionAnimator());
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

            if (m_ClosestTurningPoint.position.x > transform.position.x)
            {
                m_TurningAxis = Vector3.up;

            }
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
            Debug.Log("passed: " + DataScript.passedRoadCount);
            Debug.Log("total "+ DataScript.totalRoadCount);

            //create new roads when the player are close enough to the last road
            if(DataScript.passedRoadCount >= DataScript.totalRoadCount - 5)
            {
                mapGenerator.GenerateRoads(10);
            }

            string direction = collision.gameObject.GetComponent<StraightRoadDataHolder>().direction;

            //set car direction here for when the car does not go straight
            if(direction == "up")
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
            
        }
    }
}
