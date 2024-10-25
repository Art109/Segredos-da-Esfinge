using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    
   [SerializeField] RoomData roomData;
   int roomIndex = 0;


  void OnEnable(){
    Room.OnRoomEndend += ChangeRoom;
  }

  void OnDisable(){
    Room.OnRoomEndend -= ChangeRoom;
  }

   void Start(){
        RoomInitializer();
   }


   void RoomInitializer(){
        if(roomIndex == roomData.Rooms.Count)
        {
          Debug.Log("O jogo acabou");
        }
        else
        {
          Instantiate(roomData.Rooms[roomIndex]);
          roomIndex++;
        }
        
   }

   void ChangeRoom(){
     RoomInitializer();
   }


   
   
   
   
}
