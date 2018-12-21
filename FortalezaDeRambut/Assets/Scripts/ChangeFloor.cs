using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeFloor : MonoBehaviour {

    public int changeFloorNumber;

    private void OnTriggerStay()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.instance.LoadScene(changeFloorNumber);
        }
    }
}
