using System;
using System.Collections;
using System.Collections.Generic;
using Pickup.Utils.Attributes;
using UnityEngine;

namespace Pickup.Graphics.UI.Panels
{
    public class DrawerPanel : Panel
    {
        [SerializeField, GetSet("isMove")] private bool _isMove = false;
        public bool isMove { get => _isMove; protected set => _isMove = value; }
        
        public float moveSpeed = 1;
        public List<Vector2> positions = new();
        [SerializeField, GetSet("positionIndex")] private int _positionIndex;
        public int positionIndex
        {
            get => _positionIndex;
            set
            {
                if(value < 0 || value > positions.Count || isMove) return;
                _positionIndex = value;

                if (Application.isPlaying)
                {
                    StartCoroutine(Move());
                    isMove = true;
                }
                else
                {
                    transform.localPosition = positions[value];
                }
                
            }
        }

        private IEnumerator Move()
        {
            var target = positions[positionIndex];
            var current = (Vector2)transform.localPosition;

            while (true)
            {
                var speed = Time.deltaTime * moveSpeed;
                var breaker = true;

                if (Math.Abs(current.x - target.x) > speed)
                {
                    current.x = target.x > current.x ? current.x + speed : current.x - speed;
                    breaker = false;
                } 
                
                if (Math.Abs(current.y - target.y) > speed)
                {
                    current.y = target.y > current.y ? current.y + speed : current.y - speed;
                    breaker = false;
                }

                transform.localPosition = current;


                yield return new WaitForFixedUpdate();
                
                if (!breaker) continue;

                transform.localPosition = target;
                isMove = false;
                onOpen.Invoke();
                yield break;
            }
            
        }
    }
}