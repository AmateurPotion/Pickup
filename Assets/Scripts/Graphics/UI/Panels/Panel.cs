using Pickup.Utils.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup.Graphics.UI.Panels
{
    public class Panel : MonoBehaviour
    {
        public UnityEvent onOpen = new ();
        [SerializeField, GetSet("open")] protected bool _open = false;
        public virtual bool open
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