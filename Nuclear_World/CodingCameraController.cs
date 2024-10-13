using UnityEngine;
using UnityEngine.EventSystems;

namespace CodingSystem_HJH
{
    public class CodingCameraController : MonoBehaviour
    {
        Vector2 clickPoint;
        public float dragSpeed = 30.0f;
        public float maxX;
        public float minX;
        public float maxY;
        public float minY;
        bool goMove = false;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

                if (Input.GetMouseButtonDown(0))
                {
                    clickPoint = Input.mousePosition;
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        goMove = true;
                    }
                    else
                    {
                        goMove = false;
                    }
                }
                if (Input.GetMouseButton(0) && goMove)
                {
                    Vector3 position = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - clickPoint);
                    position.z = position.y;
                    position.y = .0f;
                    Vector3 move = position * (Time.deltaTime * dragSpeed) * -1;
                    transform.position = transform.position + move;
                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp(transform.position.z, minY, maxY));
                }
           


        }
    }
}