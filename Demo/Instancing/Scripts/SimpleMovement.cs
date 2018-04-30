using UE.Common;
using UnityEngine;

namespace UE.Demo.Instancing
{
    public class SimpleMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed = 3;
        
        void Update()
        {
            transform.position += new Vector2(
                                      - Input.GetAxis("Vertical"), 
                                      Input.GetAxis("Horizontal")).X0Y() 
                                  * speed
                                  * Time.deltaTime;
        }
    }
}