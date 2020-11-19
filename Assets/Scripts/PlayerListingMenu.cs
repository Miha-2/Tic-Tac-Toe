using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;

public class PlayerListingMenu : MonoBehaviour
{
    [SerializeField] private PlayerListing playerListing = null;
    [SerializeField] private Transform contentTransform = null;
    private List<PlayerListing> _availableRooms = new List<PlayerListing>();
    public void UpdateListings(List<RoomInfo> roomList)
    {
        foreach (var updatedRoom in roomList)
        {
            bool isFind = false;
            for (int j = 0; j < _availableRooms.Count; j++)
            {
                if (updatedRoom.Name == _availableRooms[j].Name)
                {
                    isFind = true;
                    if (updatedRoom.RemovedFromList)
                    {
                        RemoveListing(j);
                        break;
                    }
                }
            }
            
            if (!isFind && !updatedRoom.RemovedFromList)
            {
                PlayerListing newListing =
                    Instantiate(playerListing, Vector3.zero, quaternion.identity, contentTransform);
                newListing.Name = updatedRoom.Name;
                _availableRooms.Add(newListing);
            }
        }
    }

    public void ResetListings()
    {
        for (int i = 0; i < _availableRooms.Count; i++)
        {
            RemoveListing(i);
        }
    }

    private void RemoveListing(int index)
    {
        PlayerListing toRemove = _availableRooms[index];
        _availableRooms.RemoveAt(index);
        Destroy(toRemove.gameObject);
    }
}
