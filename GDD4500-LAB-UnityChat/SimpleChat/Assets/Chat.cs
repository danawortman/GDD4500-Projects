using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour {

    [SerializeField]
    private Text scrollText;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    protected InputField usernameInput;

    [SerializeField]
    protected InputField passwordInput;

    internal void AddMessage(string message)
    {
        scrollText.text += "\n" + message;

        //just a hack to jump a frame and scrolldown the chat
        Invoke("ScrollDown", .1f);
    }

    public virtual void SendMessage(InputField input) { }

    public virtual void SendMessage() { }

    private void ScrollDown()
    {
        if (scrollRect != null)
            scrollRect.verticalScrollbar.value = 0;
    }
}
