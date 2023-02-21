using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Region[] regions;

    [Header("Runtime")]
    [SerializeField] int roomNumber = 0;
    [SerializeField] GameObject currentRoom;

    int regionIndex = 0;

    void Start () 
    {
        LoadNextRoom();
    }

    public void LoadNextRoom ()
    {
        roomNumber++;
        Debug.Log("Loading next room -> index " + roomNumber);

        if (currentRoom) 
            Destroy(currentRoom);

        Room selectedRoom = regions[regionIndex].normalRooms[Random.Range(0, regions[regionIndex].normalRooms.Length)];
        GameObject levelObj = Instantiate(selectedRoom.gameObject, Vector3.zero, Quaternion.identity);
        currentRoom = levelObj;
    }
}

[System.Serializable]
struct Region 
{
    public string regionName;
    public Room[] normalRooms;
}