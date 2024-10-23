using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    
   [SerializeField] RoomData roomData;
   int roomIndex = 0;


   void Start(){
        RoomInitializer(roomIndex);
   }

   void RoomInitializer(int index){
        Instantiate(roomData.Rooms[index]);
        roomIndex++;
   }
   
   
   
}
