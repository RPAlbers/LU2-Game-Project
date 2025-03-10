using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    // The prefab to instantiate
    public bool isDragging;
    public ApiClient apiClient;
    public GameObject CodeMenu;

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
            apiClient.SaveObject2D();
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