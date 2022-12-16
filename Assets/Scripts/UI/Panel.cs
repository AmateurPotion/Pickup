using System;
using Pickup.Utils.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Pickup.UI
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField, GetSet("open")] private bool _open = false;
        public bool open
        {
            get => _open;
            set
            {
                _open = value;
                gameObject.SetActive(value);
            }
        }
    }
}