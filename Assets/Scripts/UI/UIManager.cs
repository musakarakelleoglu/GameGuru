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
