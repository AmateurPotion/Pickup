using System;
using Pickup.Utils.Attributes;
using UnityEngine;

namespace Pickup.Net.FieldBuilder
{
    public sealed class CircleMenu : MonoBehaviour
    {
        public bool OpenMenu(Vector3 position)
        {
            if (gameObject.activeSelf) return false;
                
            transform.position = position;
            gameObject.SetActive(true);
            return true;
        }
    }
}