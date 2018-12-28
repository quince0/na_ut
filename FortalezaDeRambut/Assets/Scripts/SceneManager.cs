using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    public static SceneManager instance = null;
    public GameObject player;
    public GameObject[] changeFloorArray;
    public int currentFloorNumber;
    

    void Awake () {
		if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (changeFloorArray.Length == 0)
        {
            changeFloorArray = GameObject.FindGameObjectsWithTag("Change Floor");
        }
    }

    private void OnLevelWasLoaded() /*int level*/
    {
        player = GameObject.FindGameObjectWithTag("Player");
        changeFloorArray = GameObject.FindGameObjectsWithTag("Change Floor");

        if (currentFloorNumber == 6)
        {
            currentFloorNumber = 0;
        }
        for (int i = 0; i < changeFloorArray.Length; i++)
        {
            if (changeFloorArray[i].GetComponent<ChangeFloor>().changeFloorNumber == currentFloorNumber + 1
                || changeFloorArray[i].GetComponent<ChangeFloor>().changeFloorNumber == currentFloorNumber - 1)
            {
                player.transform.position = changeFloorArray[i].transform.position;
                player.transform.position.Set(changeFloorArray[i].transform.position.x, changeFloorArray[i].transform.position.y + 1, changeFloorArray[i].transform.position.z);
            }
        }
    }


    public void LoadScene(int passedChangeFloorNumber)
    {
        currentFloorNumber = passedChangeFloorNumber;

        if (currentFloorNumber == 6)
        {
            currentFloorNumber = 0;
        }
        switch (currentFloorNumber)
        {
            case 0: 
                Application.LoadLevel(1);
                break;
            case 1:
                Application.LoadLevel(2);
                break;
            case 2:
                Application.LoadLevel(0);
                break;
            case 3:
                Application.LoadLevel(1);
                break;
            case 4:
                Application.LoadLevel(3);
                break;
            case 5:
                Application.LoadLevel(0);
                break;
            default:
                break;
        }
        //if (Application.loadedLevel == 1) /*Si nuestro nivel actual es la primera escena:*/
        //{
        //    Application.LoadLevel(2);
        //}
        //else if (Application.loadedLevel == 2) /*Si nuestro nivel actual es la primera escena:*/
        //{
        //    Application.LoadLevel(1);
        //}


    }
}
