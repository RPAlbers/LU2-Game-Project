using UnityEngine;

public class Login : MonoBehaviour
{
    //Homescreen
    public GameObject GameTitle;
    public GameObject LoginButton;
    public GameObject RegisterButton;

    //Input screen for registering and logging in
    public GameObject NameInput;
    public GameObject PasswordInput;
    public GameObject Background;
    public GameObject LoginTitle;
    public GameObject RegisterTitle;
    public GameObject ReturnButton;
    public GameObject SubmitButton;
    public GameObject ErrorTxt;

    //Buttons for loading or creating world
    public GameObject CreateWorldButton;
    public GameObject LoadWorldButton;
    public GameObject CreateWorldTxt;
    public GameObject CreateWorldSubmitButton;
    public GameObject WorldNameInput;
    public GameObject LoadWorldTxt;
    public GameObject WorldOne;
    public GameObject WorldTwo;
    public GameObject WorldThree;
    public GameObject WorldFour;
    public GameObject WorldFive;



    public void Start()
    {
        ShowAtStart();
    }


    public void ShowAtStart()
    {
        NameInput.SetActive(false);
        PasswordInput.SetActive(false);
        Background.SetActive(false);
        LoginTitle.SetActive(false);
        RegisterTitle.SetActive(false);
        ReturnButton.SetActive(false);
        CreateWorldButton.SetActive(false);
        LoadWorldButton.SetActive(false);
        WorldNameInput.SetActive(false);
        CreateWorldTxt.SetActive(false);
        CreateWorldSubmitButton.SetActive(false);
        WorldOne.SetActive(false);
        WorldTwo.SetActive(false);
        WorldThree.SetActive(false);
        WorldFour.SetActive(false);
        WorldFive.SetActive(false);
        LoadWorldTxt.SetActive(false);
        ErrorTxt.SetActive(false);

        GameTitle.SetActive(true);
        RegisterButton.SetActive(true);
        LoginButton.SetActive(true);
    }


    public void PlayerLogin()
    {
        GameTitle.SetActive(false);
        LoginButton.SetActive(false);
        RegisterButton.SetActive(false);
        
        NameInput.SetActive(true);
        PasswordInput.SetActive(true);
        Background.SetActive(true);
        LoginTitle.SetActive(true);
        ReturnButton.SetActive(true);
        SubmitButton.SetActive(true);
    }    
    

    public void PlayerRegister()
    {
        GameTitle.SetActive(false);
        LoginButton.SetActive(false);
        RegisterButton.SetActive(false);

        NameInput.SetActive(true);
        PasswordInput.SetActive(true);
        Background.SetActive(true);
        RegisterTitle.SetActive(true);
        ReturnButton.SetActive(true);
        SubmitButton.SetActive(true);
    }

    public void PickEnvironment()
    {
        NameInput.SetActive(false);
        PasswordInput.SetActive(false);
        Background.SetActive(false);
        LoginTitle.SetActive(false);
        ReturnButton.SetActive(false);
        RegisterButton.SetActive (false);

        GameTitle.SetActive(true);
        LoadWorldButton.SetActive(true);
        CreateWorldButton.SetActive(true);
    }

    public void CreateNewWorld()
    {
        GameTitle.SetActive(false);
        LoadWorldButton.SetActive(false);
        CreateWorldButton.SetActive(false);
        SubmitButton.SetActive(false);

        Background.SetActive(true);
        WorldNameInput.SetActive(true);
        CreateWorldTxt.SetActive(true);
        CreateWorldSubmitButton.SetActive(true);
    }

    public void LoadWorld()
    {
        GameTitle.SetActive(false);
        LoadWorldButton.SetActive(false);
        CreateWorldButton.SetActive(false);
        SubmitButton.SetActive(false);

        Background.SetActive (true);
        LoadWorldTxt.SetActive(true);
        WorldOne.SetActive(true);
        WorldTwo.SetActive(true);
        WorldThree.SetActive(true);
        WorldFour.SetActive(true);
        WorldFive.SetActive(true);
    }
}
