using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
public class MoveObject : MonoBehaviour
{
    [SerializeField]
    private Vector2 touchPosition = default;
    [SerializeField]
    private Camera arCamera;
    private ARRaycastManager arRaycastManager;

    public GameObject placeObject;
    private GameObject placePrefab;

    private bool onTouch = false;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Awake() {
        arRaycastManager = GetComponent<ARRaycastManager>();    
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) //Ĳ�I�˸m
        {
            //caculate the screen and the 3d world's relation
            Touch touch = Input.GetTouch(0); //������a�Ĥ@�ӱ����I //2d on screen
            touchPosition = touch.position;
            Debug.Log("touch");
            if(touch.phase == TouchPhase.Began)
            {
                //��Began/Moved/Ended
                //began : initial touch
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;
                if(Physics.Raycast(ray,out hitObject))
                {
                    Debug.Log("onTouch"+hitObject);
                    onTouch = true;
                }
            }
            if(touch.phase == TouchPhase.Ended)
            {
                onTouch = false;
            }
        }
        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            //calculate the real 3d world
            Pose hitpose = hits[0].pose; //3d �欰
            if(placeObject == null)
            {
                placeObject = Instantiate(placePrefab, hitpose.position, Quaternion.identity);
            }
            else
            {
                if (onTouch)
                {
                    placeObject.transform.position = hitpose.position;
                }
            }
        }

    }

}
