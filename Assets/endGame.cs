using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGame : MonoBehaviour
{
    private string m_PlayerTag = "Player";
    public string m_LevelToLoad;


    void OnTriggerEnter(Collider other)
    {
        LoadLevelNotAdditive(other.gameObject);
    }
    void LoadLevelNotAdditive(GameObject go)
    {
        if (go.tag == m_PlayerTag)
        {

            GameObject gogm = GameObject.FindGameObjectWithTag("GameManager");
            GameManager gm = gogm.GetComponent<GameManager>();
            gm.TriggerLoadNotAdditive(m_LevelToLoad);
            GetComponent<Collider>().enabled = false;

        }
    }
}
