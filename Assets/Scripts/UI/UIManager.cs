using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject m_GameOverPanel;
    public ParticleSystem m_GameOverParticleSystem;

    
    void Start()
    {
        m_GameOverPanel.SetActive(false);
        m_GameOverParticleSystem.gameObject.SetActive(false);
    }

    
    public void GameOverAtPosition(Vector3 position)
    {
        m_GameOverPanel.SetActive(true);
        m_GameOverParticleSystem.gameObject.SetActive(true);
        m_GameOverParticleSystem.transform.position = position;
        m_GameOverParticleSystem.Play();
    }

    public void RestartLevel()
    {
        DataScript.turningPoints.Clear();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}


/*
 //car correction after turning
    IEnumerator CarCorrectionAnimator()
    {
        Vector3 currentLookPos = transform.rotation.eulerAngles;
        
        if (currentLookPos.y <= 45f)
        {
            
            
            while(Mathf.Abs(transform.rotation.eulerAngles.y) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if(currentLookPos.y <= 90f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 90f) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 135f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 90f) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 180f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180f) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 225f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 180f) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 270f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 270f) >= 0.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else if (currentLookPos.y <= 315f)
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y - 270f) >= 0.5)
            {
                transform.Rotate(new Vector3(0, -0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (Mathf.Abs(transform.rotation.eulerAngles.y) <= 359.5f)
            {
                transform.Rotate(new Vector3(0, 0.5f, 0));
                yield return new WaitForEndOfFrame();
            }
        }
        
        StopCoroutine(CarCorrectionAnimator());
    }
*/
