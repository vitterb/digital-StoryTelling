using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueFiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartConversation();
    }

    // Update is called once per frame
    void StartConversation()
    {
        List<string> lines = FileManager.ReadTextAsset("testFile");
        
        DialogueSystem.Instance.Say(lines);
    }
}
