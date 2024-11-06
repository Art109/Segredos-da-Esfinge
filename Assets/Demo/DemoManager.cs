using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    
   [SerializeField] RoomData roomData;
   [SerializeField] GameObject playerPrefab;
   List<DemoPlayer> players = new List<DemoPlayer>();
   int numberOfPlayers = 2;
   int roomIndex = 0;


  void OnEnable(){
    Room.OnRoomEndend += ChangeRoom;
  }

  void OnDisable(){
    Room.OnRoomEndend -= ChangeRoom;
  }

   void Start(){
        PlayerInitializer();
        RoomInitializer();
   }


   void RoomInitializer(){
        if(roomIndex == roomData.Rooms.Count)
        {
          Debug.Log("O jogo acabou");
        }
        else
        {
          GameObject room = Instantiate(roomData.Rooms[roomIndex]);

          Transform spawnPosition = room.transform.Find("SpawnPlayerPositions");

          Debug.Log(spawnPosition);
          
          if(spawnPosition != null)
          {
              int playerIndex = 0;
              
              foreach(var player in players)
              {
                Debug.Log(player);
                Debug.Log(players.Count);
                Debug.Log(playerIndex);
                if(playerIndex < spawnPosition.childCount)
                {
                  Debug.Log(spawnPosition.GetChild(playerIndex));
                  player.transform.position = spawnPosition.GetChild(playerIndex).position;
                  playerIndex++;
                }
              }
              
              
          }
          
          roomIndex++;
        }
        
   }

   void PlayerInitializer(){
      for(int i = 0 ; i < numberOfPlayers ; i++)
      {
          GameObject player = Instantiate(playerPrefab);
          players.Add(player.GetComponent<DemoPlayer>());
      }
      
   }

   void ChangeRoom(){
    foreach(var player in players)
      player.CompletedObjective = false;
    
     RoomInitializer();
   }


   
   
   
   
}
