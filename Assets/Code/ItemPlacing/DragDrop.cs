using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    // The prefab to instantiate
    public bool isDragging;
    public ApiClient apiClient;
    public GameObject CodeMenu;
    public float finalPositionX;
    public float finalPositionY;

    void Start()
    {
        CodeMenu = GameObject.Find("CodeMenu");
        if (CodeMenu != null)
        {
            apiClient = CodeMenu.GetComponent<ApiClient>();
        }
    }

    void OnMouseDown()
    {
        isDragging = !isDragging;
        Debug.Log(isDragging);
        if (!isDragging)
        {
            finalPositionX = transform.position.x;
            finalPositionY = transform.position.y;
            apiClient.SaveObject2D(finalPositionX, finalPositionY);
        }
    }

    void Update()
    { 
        if (isDragging)
        { 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
        }
    }
}