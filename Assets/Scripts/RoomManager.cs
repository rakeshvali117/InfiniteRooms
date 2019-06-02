/**
 * Author:  Rakesh Kumar Vali
 * Created: 01.06.2019
 * Summary: Room manager class which can generate infinite rooms with different themes.
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoomsDemo
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private GameObject     roomObj;                            // Reference to room gameobject
        [SerializeField] private GameObject     forestObj;                          // Reference to forest prefab
        [SerializeField] private GameObject     caveObj;                            // Reference to cave prefab                 
        [SerializeField] private GameObject     mountainsObj;                       // Reference to mountain prefab 
        [SerializeField] private GameObject     seasideObj;                         // Reference to seaside prefab 
        [SerializeField] private GameObject     desertObj;                          // Reference to desert prefab 
        [SerializeField] private GameObject     countryObj;                         // Reference to country prefab 
        [SerializeField] private GameObject     backwatersObj;                      // Reference to backwater prefab 
        [SerializeField] private PlayerManager  playerManager;                      // Reference to player manager class 

        [SerializeField] private Text         nextRoomText;                         // Reference to next room UI text
        [SerializeField] private Text         prevRoomText;                         // Reference to previous room UI text
        [SerializeField] private Text         currentRoomText;                      // Reference to current room UI text
        [SerializeField] private Text         currentRoomNoText;                    // Reference to current room number UI text

        private RoomThemes      currentRoomTheme;                   // Current room theme
        private RoomThemes      prevRoomThem;                       // previous room theme
        private RoomThemes      nextRoomTheme;                      // Next room theme
        private int             roomOffset = 2;                     // Room offset to calculate room theme
        private int             currentRoomNo = 1;                  // Current room number
        private readonly int    totalRoomThemes = 7;                // Total number of available room themes


        // Start is called before the first frame update
        void Start()
        {
            RandomizeRoomOffset();
            currentRoomTheme    = CalculateRoomTheme(currentRoomNo);        // Calculate current room theme
            prevRoomThem        = currentRoomTheme;                         // Assign current room theme to previous room theme as a default value
            nextRoomTheme       = CalculateRoomTheme(currentRoomNo + 1);    // Calculate next room theme for UI purpose
            UpdateUI();                                                     // Update game UI
            RefurbishRoom();                                                // Populate room with elements according to room theme
            PlayerPrefs.SetInt("RoomOffset", roomOffset);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Randomly generate room offset and check with room offset of previous run
        /// </summary>
        void RandomizeRoomOffset()
        {
            roomOffset = Random.Range(1, 6);                       // Randomly select roomoffset. cannot use 0 and 7 as it can hamper the calculation
            var prevRoomOffset = PlayerPrefs.GetInt("RoomOffset", 0);

            while (roomOffset == prevRoomOffset)
            {
                roomOffset = Random.Range(1, 6);
            }
        }

        /// <summary>
        /// Calculates theme for the particular room number
        /// </summary>
        /// <param name="roomNo">Room number</param>
        /// <returns>Theme for the specified room number</returns>
        RoomThemes CalculateRoomTheme(int roomNo)
        {
            // Calculating theme value by using right shift across ENUM values
            // Roomoffset will determine how randomly room themes will be calculated
            RoomThemes roomTheme = (RoomThemes)Mathf.Abs(((totalRoomThemes + (roomOffset * roomNo))) % totalRoomThemes);
            return roomTheme;
        }

        /// <summary>
        /// Populates room with elements based on room theme
        /// </summary>
        void RefurbishRoom()
        {
            var themeObj = GameObject.Find("ThemeObj");

            if (themeObj != null)                               // If room elements already exist. Destroy them to make way for new elements
            {
                Destroy(themeObj.gameObject);
            }
            
            switch(currentRoomTheme)                            // Instantiate room elements based on theme
            {
                case RoomThemes.MOUNTAINS:
                    themeObj = Instantiate(mountainsObj);
                    break;

                case RoomThemes.SEASIDE:
                    themeObj = Instantiate(seasideObj);
                    break;

                case RoomThemes.CAVE:
                    themeObj = Instantiate(caveObj);
                    break;

                case RoomThemes.COUNTRYSIDE:
                    themeObj = Instantiate(countryObj);
                    break;

                case RoomThemes.FOREST:
                    themeObj = Instantiate(forestObj);
                    break;

                case RoomThemes.DESERT:
                    themeObj = Instantiate(desertObj);
                    break;

                case RoomThemes.BACKWATERS:
                    themeObj = Instantiate(backwatersObj);
                    break;

            }
            themeObj.name = "ThemeObj";
            themeObj.transform.parent = roomObj.transform;
        }

        /// <summary>
        /// Updates UI elements in the scene
        /// </summary>
        void UpdateUI()
        {
            nextRoomText.text       = nextRoomTheme.ToString();
            prevRoomText.text       = prevRoomThem.ToString();
            currentRoomText.text    = currentRoomTheme.ToString();
            currentRoomNoText.text  = currentRoomNo.ToString();
        }

        

        /// <summary>
        /// Loads next room onto the scene
        /// </summary>
        public void NextRoom()
        {
            currentRoomNo++;
            prevRoomThem        = currentRoomTheme;
            currentRoomTheme    = CalculateRoomTheme(currentRoomNo);
            nextRoomTheme       = CalculateRoomTheme(currentRoomNo + 1);
            UpdateUI();
            RefurbishRoom();
            playerManager.ResetPlayerPosition();                                              // Resets player position
        }

        /// <summary>
        /// Loads previous room onto the scene
        /// </summary>
        public void PrevRoom()
        {
            if (currentRoomNo > 1)
            {
                currentRoomNo--;
                nextRoomTheme       = currentRoomTheme;
                currentRoomTheme    = CalculateRoomTheme(currentRoomNo);
                prevRoomThem        = CalculateRoomTheme(currentRoomNo - 1);
                UpdateUI();
                RefurbishRoom();
                playerManager.ResetPlayerPosition();                                          // Resets player position
            }
        }

        /// <summary>
        /// Experimental portal system. Can be used in future
        /// </summary>
        /// <param name="roomNo">Room number to be teleported to</param>
        public void JumpToRoom(int roomNo)
        {
            currentRoomNo       = roomNo;
            prevRoomThem        = CalculateRoomTheme(currentRoomNo - 1);
            currentRoomTheme    = CalculateRoomTheme(currentRoomNo);
            nextRoomTheme       = CalculateRoomTheme(currentRoomNo + 1);
            UpdateUI();
            RefurbishRoom();
            playerManager.ResetPlayerPosition();
        }

        private void Update()
        {
            //Purely experimental
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                JumpToRoom(25);
            }
        }
    }

    /// <summary>
    /// Enum for room themes
    /// </summary>
    public enum RoomThemes
    {
        MOUNTAINS = 0,
        BACKWATERS,
        FOREST,
        DESERT,
        COUNTRYSIDE,
        SEASIDE,
        CAVE,
    }
}