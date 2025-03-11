using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.Rendering;

public class ApiClient : MonoBehaviour
{
    Login login;
    MenuPanel menuPanel;
    public List<TMP_Text> WorldList;
    public TMP_InputField NameInput;
    public TMP_InputField PasswordInput;
    public TMP_InputField WorldNameInput;
    public GameObject ErrorTxt;
    public Tilemap tilemapGround;
    public TileBase grassTile;
    private List<PostEnvironmentSaveDto> environmentsList;
    private List<PostObjectSaveDto> objectsList;
    private bool isLoggingIn;
    private bool isRegistering;
    private bool isRegisteredSuccusfully;
    private bool isSuccesfullyLoggedIn;
    private string email;
    private string accesTokenString;
    private string environmentId;


    public void Start()
    {
        login = FindFirstObjectByType<Login>();
        menuPanel = FindFirstObjectByType<MenuPanel>();
        environmentId = PlayerPrefs.GetString("environmentId");
        accesTokenString = PlayerPrefs.GetString("accesTokenString");
    }

    public void WaitForRegistering()
    {
        login.PlayerRegister();
        isRegistering = true;
    }

    public void WaitForLoggingIn()
    {
        login.PlayerLogin();
        isRegistering = false;
    }

    
    //Register
    public async void Register()
    {
        if (isRegistering == true)
        {
            Debug.Log("registreren");
            PostRegisterRequestDto registerRequest = new PostRegisterRequestDto()
            {
                email = NameInput.text,
                password = PasswordInput.text
            };
            var jsondata = JsonUtility.ToJson(registerRequest);
            Debug.Log(jsondata);
            email = NameInput.text;
            isRegisteredSuccusfully = true;
            await PerformApiCall("https://localhost:7239/account/register", "POST", jsondata); // Register for user.
        }
        else
        {
            Login();
        }
        
    }


    //Login
    public async void Login()
    {
        Debug.Log("Inloggen");
        isLoggingIn = true;
        var registerRequest = new PostLoginDto()
        {
            email = NameInput.text,
            password = PasswordInput.text
        };
        var jsondata = JsonUtility.ToJson(registerRequest);
        Debug.Log(jsondata);
        email = NameInput.text;
        isSuccesfullyLoggedIn = true;
        var response = await PerformApiCall("https://localhost:7239/account/login", "POST", jsondata); // Login for user.
        Debug.Log(response);
        var accesToken = JsonUtility.FromJson<PostLoginResponseDto>(response);

        PlayerPrefs.SetString("accesTokenString", accesToken.accessToken);
        accesTokenString = accesToken.accessToken;
        PlayerPrefs.Save();
        isLoggingIn = false;
    }


    // Creates a new environment.
    public async void CreateEnvironment()
    {
        Debug.Log("Saving environment");
        // Retrieves the id of the worldowner.
        var ownerId = await PerformApiCall($"https://localhost:7239/api/user/id?email={Uri.EscapeDataString(email)}", "GET", null, accesTokenString);


        // Retrieves all the worlds that the logged in user created.
        var allEnvironments = await PerformApiCall($"https://localhost:7239/api/environment/get?owner={Uri.EscapeDataString(ownerId)}", "GET", null, accesTokenString);
        // Retrieves each individual world and puts it in a list.
        List<PostEnvironmentSaveDto> environmentsList = JsonConvert.DeserializeObject<List<PostEnvironmentSaveDto>>(allEnvironments);

        if (environmentsList.Count < 5)
        {
            // Saves the created environment to a database.
            Guid guid = Guid.NewGuid();
            var saveRequest = new PostEnvironmentSaveDto()
            {
                id = guid.ToString(),
                name = WorldNameInput.text,
                maxHeight = 100,
                maxLength = 100,
                owner = ownerId
            };
            PlayerPrefs.SetString("environmentId", saveRequest.id);
            environmentId = saveRequest.id;
            PlayerPrefs.Save();
            Debug.Log("Id is: " + environmentId);
            var jsondata = JsonUtility.ToJson(saveRequest);
            Debug.Log("Data: " + jsondata);
            await PerformApiCall("https://localhost:7239/api/environment/create", "POST", jsondata, accesTokenString);
            SceneManager.LoadScene(0);
        }    
    }
    
    public async void DeleteEnvironment(int index)
    {
        var environmentToDelete = environmentsList[index].id;
        var data = JsonUtility.ToJson(environmentToDelete);
        await PerformApiCall($"https://localhost:7239/api/environment/delete?id={Uri.EscapeDataString(environmentToDelete)}", "DELETE", null, accesTokenString);
        LoadEnvironment();
    }


    // Loads all the environments beloging to a certain account.
    public async void LoadEnvironment()
    {
        Debug.Log("Saving environment");

        foreach (var world in WorldList)
        {
            world.SetText("...");
        }

        // Retrieves the id of the worldowner
        var data = JsonUtility.ToJson(email);
        var ownerId = await PerformApiCall($"https://localhost:7239/api/user/id?email={Uri.EscapeDataString(email)}", "GET", data, accesTokenString);
        Debug.Log("Het gevonden Id is: " + ownerId);

        // Retrieves all the worlds that the logged in user created.
        var allWorlds = await PerformApiCall($"https://localhost:7239/api/environment/get?owner={Uri.EscapeDataString(ownerId)}", "GET", null, accesTokenString);

        // Wraps each individual world.
        environmentsList = JsonConvert.DeserializeObject<List<PostEnvironmentSaveDto>>(allWorlds);
       

        for (int i = 0; i < environmentsList.Count; i++)
        {
            WorldList[i].SetText(environmentsList[i].name);
        }
    }


    // Searches the correct environment that is binded to a button.
    public async void SearchEnvironment(int index)
    {
        Debug.Log("Searching Environment");
        SceneManager.LoadScene(0);
        var data = JsonUtility.ToJson(email);
        var ownerId = await PerformApiCall($"https://localhost:7239/api/user/id?email={Uri.EscapeDataString(email)}", "GET", data, accesTokenString);
        Debug.Log("Het gevonden Id is: " + ownerId);

        var allWorlds = await PerformApiCall($"https://localhost:7239/api/environment/get?owner={Uri.EscapeDataString(ownerId)}", "GET", null, accesTokenString);

        environmentsList = JsonConvert.DeserializeObject<List<PostEnvironmentSaveDto>>(allWorlds);;
        PlayerPrefs.SetString("environmentId", environmentsList[index].id);
        environmentId = environmentsList[index].id;
        PlayerPrefs.Save();
        Debug.Log("Environment Id is: " + environmentId);
        var allObjects = await PerformApiCall($"https://localhost:7239/api/object2d/get?id={Uri.EscapeDataString(environmentId)}", "GET", null, accesTokenString);

        objectsList = JsonConvert.DeserializeObject<List<PostObjectSaveDto>>(allObjects);

        foreach (var obj in objectsList)
        {
            string prefabName = GetRightPrefab(Convert.ToInt32(obj.PrefabId));
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");

            GameObject newObject = Instantiate(prefab, new Vector3(obj.PositionX, obj.PositionY, 0), Quaternion.Euler(0, 0, obj.RotationZ));
            newObject.transform.localScale = new Vector3(obj.ScaleX, obj.ScaleY, 1);
            SpriteRenderer renderer = newObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = obj.SortingLayer;
            }
        }

    }

    public string GetRightPrefab(int prefabId)
    {
        switch (prefabId)
        {
            case 0:
                return "Billboard";
            case 1:
                return "Campfire";
            case 2:
                return "Lantern";
            case 3:
                return "Sign";
            default:
                return "Billboard";
        }
    }

    // Saves 2D objects as soon as they are placed down.
    public async void SaveObject2D(float modifiedPositionX, float modifiedPositionY)
    {
        Debug.Log("Saving 2D Object");
        Debug.Log(menuPanel.placedObjects.Count);

        for (int i = menuPanel.placedObjects.Count - 1; i < menuPanel.placedObjects.Count; i++)
        {
            var objectPlaced = menuPanel.placedObjects[i];
            var saveRequest = new PostObjectSaveDto()
            {
                Id = environmentId,
                PrefabId = objectPlaced.PrefabId,
                PositionX = modifiedPositionX,
                PositionY = modifiedPositionY,
                ScaleX = objectPlaced.ScaleX,
                ScaleY = objectPlaced.ScaleY,
                RotationZ = objectPlaced.RotationZ,
                SortingLayer = objectPlaced.SortingLayer,
            };
            var data = JsonUtility.ToJson(saveRequest);
            Debug.Log(data);
            await PerformApiCall("https://localhost:7239/api/object2d/create", "POST", data, accesTokenString);
        }
    }





    // Pre-made API call method.
    private async Task<string> PerformApiCall(string url, string method, string jsonData = null, string token = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            if (!string.IsNullOrEmpty(jsonData))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API-aanroep is successvol: " + request.downloadHandler.text);
                if (isRegisteredSuccusfully || isSuccesfullyLoggedIn)
                {
                    login.PickEnvironment();
                    isRegisteredSuccusfully = false;
                    isSuccesfullyLoggedIn = false;
                }
                return request.downloadHandler.text;

            }
            else
            {
                Debug.Log("Fout bij API-aanroep: " + request.error);
                if (isLoggingIn)
                {
                    ErrorTxt.SetActive(true);
                }
                return null;
            }
        }
    }
}

