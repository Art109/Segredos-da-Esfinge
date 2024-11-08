using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    
   [SerializeField] RoomData roomData;
   [SerializeField] GameObject playerPrefab;
   List<DemoPlayer> players = new List<DemoPlayer>();
   int numberOfPlayers = 2;
   int roomIndex = 0;

   private int initialTime;
   [SerializeField]private float timeRemaining;
   private bool timerIsRunning = false;


  void OnEnable(){
    Room.OnRoomEndend += ChangeRoom;
    DemoPlayer.OnPlayerDeath += HandlePlayerDeath;
  }

  void OnDisable(){
    Room.OnRoomEndend -= ChangeRoom;
    DemoPlayer.OnPlayerDeath -= HandlePlayerDeath;
  }

   void Start(){
        PlayerInitializer();
        RoomInitializer();
   }

   void Update(){
    if(timerIsRunning)
      Timer();
   }

#region Initializers
   void RoomInitializer(){
        if(roomIndex == roomData.Rooms.Count)
        {
          GameOver();
        }
        else
        {
          GameObject room = Instantiate(roomData.Rooms[roomIndex]);

          initialTime = room.GetComponent<Room>().ConclusionTime;

          StartTimer(initialTime);

          Transform spawnPosition = room.transform.Find("SpawnPlayerPositions");

          
          if(spawnPosition != null)
          {
              int playerIndex = 0;
              
              foreach(var player in players)
              {
                if(playerIndex < spawnPosition.childCount)
                {
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
  #endregion

   void ChangeRoom(){
      StopTimer(); 
      RoomInitializer();
   }

   void HandlePlayerDeath(DemoPlayer player){
      int aux = 0;
      foreach(var i in players){
          if(!i.IsAlive)
            aux++;
      }
      if(aux == players.Count){
        GameOver();
      }
   }

  #region timer
   void StartTimer(int time){
      timeRemaining = time;
      timerIsRunning = true;
   }

   void StopTimer(){
      timerIsRunning = false;
   }

   void Timer(){
      timeRemaining -= Time.deltaTime;

      if(timeRemaining <= 0){
        DemoBalance.TriggerDamagePlayersEvent();
        StartTimer(initialTime); // Repete o timer
        Debug.Log("Reiniciando o Timer");        
      }

   }

   #endregion


   void GameOver(){
      StopTimer();
   }
   
   
   
}
