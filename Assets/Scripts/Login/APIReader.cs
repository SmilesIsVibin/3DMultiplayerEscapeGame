using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Proyecto26;
using System;

public class APIReader : MonoBehaviour
{
    private readonly string basePath = "https://apigenerator.dronahq.com/api/8PlQXLAN/EscapeRoomData";

    public static APIReader Instance;

    [Header("User Creation")]
    public TMP_InputField tempPlayerName;
    public TMP_InputField tempUserName;
    public TMP_InputField tempPassword;

    [Header("Login UI")]
    public TMP_InputField userName;
    public TMP_InputField password;

    [Header("Main Menu UI")]
    public string currentPlayerName;
    public string currentUsername;

    public UserData[] users;

    public UserData userData;

    void Start()
    {
        Instance = this;
        Get();
    }

    #region User Login & SignUp
    //Adding new user data
    public void SetNewUserData()
    {
        userData.PlayerName = tempPlayerName.text;
        userData.Username = tempUserName.text;
        userData.Password = tempPassword.text;

        Post();
        LoginUser();
        Debug.Log(userData.Username);
    }

    //Checking User credentials
    public void CheckUserLogin()
    {
        foreach(UserData u in users)
        {
            if(u.Username == userName.text)
            {
                if (u.Password == password.text)
                {
                    userData.id = u.id;
                    userData.PlayerName = u.PlayerName;
                    userData.Username = u.Username;
                    userData.Password = u.Password;
                    LoginUser();
                }
                else
                {
                    Debug.Log("Password is incorrect");
                }
            }
            else
            {
                Debug.Log("Username does not exist");
            }
        }
    }

    //Logging in user
    public void LoginUser()
    {
        currentPlayerName = userData.PlayerName;
        currentUsername = userData.Username;
        Debug.Log("Successful Login");
        MenuManager.Instance.SetUpPlayerName(currentPlayerName);
    }
    #endregion

    public void Get()
    {
        RestClient.Get(basePath).Then(response =>
        {
            try
            {
                string jsonResponse = response.Text;
                users = JsonHelper.ArrayFromJson<UserData>(jsonResponse);

                if (users != null)
                {
                    Debug.Log("Number of users : " + users.Length);
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex + "User array is null");
            }

        }).Catch(error => {
            Debug.Log(error.Message);
        }
        );
    }

    public void Post()
    {
        RestClient.Post(basePath, userData).Then(response =>
        {
            try
            {
                if (response != null)
                {
                    Debug.Log("Succcessful");
                    Get();
                }
                else
                {
                    Debug.Log("No Response");
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex + "Error Upload User");
            }

        }).Catch(error => {
            Debug.Log(error.Message);
        }
        );
    }
    public void DeleteUser(int userID)
    {
        RestClient.Delete(basePath + "/" + userID).Then(response =>
        {
            try
            {
                if (response != null)
                {
                    Debug.Log("Deleted Succcessful");
                    Get();
                }
                else
                {
                    Debug.Log("No Response");
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex + "Error Deletion");
            }

        }).Catch(error => {
            Debug.Log(error.Message);
        }
        );
    }
    public void PatchUser()
    {
        RestClient.Patch(basePath + "/" + userData.id, userData).Then(response =>
        {
            try
            {
                if (response != null)
                {
                    Debug.Log("Patch Succcessful");
                    Get();
                }
                else
                {
                    Debug.Log("No Response");
                }

            }
            catch (Exception ex)
            {
                Debug.Log(ex + "Error Patch");
            }

        }).Catch(error => {
            Debug.Log(error.Message);
        }
        );
    }
}
