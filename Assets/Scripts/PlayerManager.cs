/**
 * Author:  Rakesh Kumar Vali
 * Created: 01.06.2019
 * Summary: Player class to perform operation based on trigger detection
 **/

using UnityEngine;

namespace RoomsDemo
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private RoomManager roomManager;   // Reference to Room manager class

        private Vector3 pInitialpos;                        // Player's initial position

        private void Start()
        {
            pInitialpos = this.transform.position;                          // Assign player's initial position
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "NextRoom")
            {
                roomManager.NextRoom();
            }
            else if (other.gameObject.tag == "PrevRoom")
            {
                roomManager.PrevRoom();
            }

        }

        /// <summary>
        /// Resets player's position to middle of room
        /// </summary>
        public void ResetPlayerPosition()
        {
            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = pInitialpos;
            this.GetComponent<CharacterController>().enabled = true;
        }
    }
}
