using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject m_CameraRig;

    GameObject m_StraightRoad;
    GameObject m_CurvedRoad;
    GameObject m_Car;

    Vector3 m_InitialCenter;
    Quaternion m_InitialRotation;
    
    string m_CurrentRoadDirection;

    int[] m_ThreeRoadsBefore;

    void Start()
    {
        m_ThreeRoadsBefore = new int[2] { 0, 0};

        DataScript.turningPoints = new List<Transform>();
        DataScript.inputLock = true;
        DataScript.passedRoadCount = 0;
        DataScript.totalRoadCount = 0;

        m_CurvedRoad = Resources.Load<GameObject>("UsedPrefabs/CurvedRoad");
        m_StraightRoad = Resources.Load<GameObject>("UsedPrefabs/StraightRoad");
        m_Car = Resources.Load<GameObject>("UsedPrefabs/Car");

        GameObject firstRoad = Instantiate(m_StraightRoad, Vector3.zero, Quaternion.identity);
        m_CurrentRoadDirection = "up";
        firstRoad.GetComponent<StraightRoadDataHolder>().direction = m_CurrentRoadDirection;
     

        m_InitialCenter = Vector3.zero;
        m_InitialRotation = Quaternion.identity;
        
        GenerateRoads(10);
        PutTheCarInStartPoint();
    }
    
    public void GenerateRoads(int roadCount)
    {

        for (int i = 0; i < roadCount; i++)
        {

            // To select the position of the next road placed, calculations made to prevent looping
            #region nextRoadPositionSelector
            int randomPosSelector = Random.Range(1, 3);
           
            if (m_ThreeRoadsBefore[0] == m_ThreeRoadsBefore[1])
            {
                if(m_ThreeRoadsBefore[0] == 1)
                {
                    randomPosSelector = 2;
                }
                else if(m_ThreeRoadsBefore[0] == 2)
                {
                    randomPosSelector = 1;
                }
            }

            m_ThreeRoadsBefore[0] = m_ThreeRoadsBefore[1];
            m_ThreeRoadsBefore[1] = randomPosSelector;
            #endregion



            Quaternion rot = new Quaternion();
            Quaternion curveRot = new Quaternion();
            Vector3 pos = new Vector3();
            Vector3 curvePos = new Vector3();

            //put a road to right
            if (randomPosSelector == 1)
            {
                if (m_CurrentRoadDirection == "up")
                {
                    pos = m_InitialCenter + new Vector3(45f, 0, 20f);
                    rot.eulerAngles = new Vector3(0, 90f, 0);

                    curvePos = pos + new Vector3(-25f, 0, -20f);
                    curveRot.eulerAngles = new Vector3(0, 180f, 0);

                    m_CurrentRoadDirection = "right";
                }
                else if (m_CurrentRoadDirection == "right")
                {
                    pos = m_InitialCenter + new Vector3(20f, 0, -45f);
                    rot.eulerAngles = new Vector3(0, 180f, 0);

                    curvePos = pos + new Vector3(-20f, 0, 25f);
                    curveRot.eulerAngles = new Vector3(0, 270f, 0);
                    
                    m_CurrentRoadDirection = "down";
                }
                else if (m_CurrentRoadDirection == "left")
                {
                    pos = m_InitialCenter + new Vector3(-20f, 0, 45f);
                    rot.eulerAngles = new Vector3(0, 0, 0);

                    curvePos = pos + new Vector3(20f, 0, -25f);
                    curveRot.eulerAngles = new Vector3(0, 90f, 0);
                    
                    m_CurrentRoadDirection = "up";
                }
                else
                {
                    pos = m_InitialCenter + new Vector3(-45f, 0, -20f);
                    rot.eulerAngles = new Vector3(0, -90f, 0);

                    curvePos = pos + new Vector3(25f, 0, 20f);
                    curveRot.eulerAngles = Vector3.zero;
                    
                    m_CurrentRoadDirection = "left";
                }
            }

            //put a road to left
            else
            {
                if (m_CurrentRoadDirection == "up")
                {
                    pos = m_InitialCenter + new Vector3(-57f, 0, 32.5f);
                    rot.eulerAngles = new Vector3(0, 270f, 0);

                    curvePos = pos + new Vector3(24.5f, 0, -32.5f);
                    curveRot.eulerAngles = new Vector3(0, -90f, 0);

                   
                    m_CurrentRoadDirection = "left";
                }
                else if (m_CurrentRoadDirection == "right")
                {
                    pos = m_InitialCenter + new Vector3(32.5f, 0, 57f);
                    rot.eulerAngles = new Vector3(0, 0, 0);

                    curvePos = pos + new Vector3(-32.5f, 0, -24.5f);
                    curveRot.eulerAngles = new Vector3(0, 0, 0);

                    m_CurrentRoadDirection = "up";
                }
                else if (m_CurrentRoadDirection == "left")
                {
                    pos = m_InitialCenter + new Vector3(-32.5f, 0, -57f);
                    rot.eulerAngles = new Vector3(0, 180f, 0);

                    curvePos = pos + new Vector3(32.5f, 0, 24.5f);
                    curveRot.eulerAngles = new Vector3(0, 180f, 0);

                    m_CurrentRoadDirection = "down";
                }
                else
                {
                    pos = m_InitialCenter + new Vector3(57f, 0, -32.5f);
                    rot.eulerAngles = new Vector3(0, 90f, 0);

                    curvePos = pos + new Vector3(-24.5f, 0, 32.5f);
                    curveRot.eulerAngles = new Vector3(0, 90f, 0);

                    m_CurrentRoadDirection = "right";
                }
            }

            //initialize straight and curved roads
            GameObject roadToInitialize = Instantiate(m_StraightRoad, pos, rot);
            GameObject curvedRoadToInitialize = Instantiate(m_CurvedRoad, curvePos, curveRot);
            DataScript.turningPoints.Add(curvedRoadToInitialize.GetComponentsInChildren<Transform>()[1]);

            roadToInitialize.GetComponent<StraightRoadDataHolder>().direction = m_CurrentRoadDirection;
            m_InitialCenter = pos;
            DataScript.totalRoadCount++;
        }
        

    }


    void PutTheCarInStartPoint()
    {
        //car starts slightly above the surface to prevent unwanted collisions
        Vector3 carStartPoint = new Vector3(-6f, 0.51f, -20f);
        Quaternion carRotation = new Quaternion();
        carRotation.eulerAngles = new Vector3(0, 0, 0);

        GameObject instantiatedCar = Instantiate(m_Car, carStartPoint, carRotation);
        instantiatedCar.GetComponent<CarControllerScript>().m_CameraRig = m_CameraRig;
    }
}
