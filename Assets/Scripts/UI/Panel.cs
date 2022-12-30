using Pickup.Utils.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup.UI
{
    public class Panel : MonoBehaviour
    {
        public UnityEvent onOpen = new ();
        [SerializeField, GetSet("open")] private bool _open = false;
        public bool open
        {
            get => _open;
            set
            {
                _open = value;
                gameObject.SetActive(value);
                onOpen.Invoke();
            }
        }

        public void Open() => open = true;
    }
}