using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private string nextSceneName = "Dashboard";

    private void Start()
    {
        if (loginButton == null)
        {
            Debug.LogError("Login button is not assigned in LoginUI.");
            return;
        }

        loginButton.onClick.AddListener(HandleLogin);

        if (feedbackText != null)
            feedbackText.text = "";
    }

    private void HandleLogin()
    {
        Debug.Log("HandleLogin called");

        if (AccountManager.Instance == null)
        {
            Debug.LogError("AccountManager.Instance is null. Add AccountManager to the Login scene.");
            if (feedbackText != null)
                feedbackText.text = "Account manager missing.";
            return;
        }

        if (usernameInput == null)
        {
            Debug.LogError("Username input is not assigned in LoginUI.");
            if (feedbackText != null)
                feedbackText.text = "Username input missing.";
            return;
        }

        string username = usernameInput.text;

        bool success = AccountManager.Instance.Login(username);

        if (!success)
        {
            if (feedbackText != null)
                feedbackText.text = "Enter a valid username.";
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}