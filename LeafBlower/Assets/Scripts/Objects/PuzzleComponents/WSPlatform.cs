using System.Collections.Generic;
using UnityEngine;

public class WSPlatform : MonoBehaviour
{
    private int _currentWeight;

    public int CurrentWeight => _currentWeight;

    private bool isPlayerInWeight = false;

    private PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Object obj = other.GetComponent<Object>();

        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;
        if(obj.CompareTag("Player"))
        {
            if(_player.CheckCollisions.IsGrounded)
            {
                UpdateWeight((int)obj.weight);
                isPlayerInWeight = true;
            }
        }
        else
        {
            UpdateWeight((int)obj.weight);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        Object obj = other.GetComponent<Object>();

        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf || !obj.CompareTag("Player")) return; // Si es leaf o no es player return


        if(_player.CheckCollisions.IsGrounded && !isPlayerInWeight)
        {
            UpdateWeight((int)obj.weight);
            isPlayerInWeight = true;
        }
        else if(!_player.CheckCollisions.IsGrounded && isPlayerInWeight)
        {
            UpdateWeight(-(int)obj.weight);
            isPlayerInWeight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Object obj = other.GetComponent<Object>();
        if (!obj) return;
        if (obj.weight == Enums.ObjectWeight.Leaf) return;
        if (obj.CompareTag("Player"))
        {
            if(isPlayerInWeight)
            {
                UpdateWeight(-(int)obj.weight);
                isPlayerInWeight = false;
            }
        }
        else
        {
            UpdateWeight(-(int)obj.weight);
        }        
    }

    private void UpdateWeight(int weight)
    {
        _currentWeight += weight;
    }

}











