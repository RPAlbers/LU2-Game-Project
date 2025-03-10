using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int PrefabIndex;
    public static MenuPanel Instance { get; private set; }
    public List<Object2D> placedObjects = new List<Object2D>();

    private void Start()
    {
        
    }

    public void CreateGameObjectFromClick(int prefabIndex)
    {
        var well = Instantiate(prefabs[prefabIndex], Vector3.zero, Quaternion.identity);
        var dadWell = well.GetComponent<DragAndDrop>();
        dadWell.isDragging = true;
        PrefabIndex = prefabIndex;

        Object2D data = new Object2D();           
        data.PrefabId = prefabIndex;
        data.PositionX = well.transform.position.x;
        data.PositionY = well.transform.position.y;
        data.ScaleX = well.transform.localScale.x;
        data.ScaleY = well.transform.localScale.y;
        data.RotationZ = well.transform.eulerAngles.z;
        data.SortingLayer = 0;
        placedObjects.Add(data);
        Debug.Log("object toegevoegd aan de lijst aantal objecten: " + placedObjects.Count);


    }

    public void HideMenu(bool show)
    {
        this.gameObject.SetActive(show);
    }

}
