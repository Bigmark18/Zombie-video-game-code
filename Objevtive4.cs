using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objevtive4 : MonoBehaviour
{
 private void OnTriggerEnter(Collider other)
 
    {
        if(other.gameObject.tag == "Vehicle")
        {
            //complete objective
            ObjectivesComplete.occurrence.GetObjectivesDone(true, true, true, true);

            SceneManager.LoadScene("MainMenu");
        }
    }
}
