using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private ConversationManager conversationManager;
        private TextArchitext architext;

        public static DialogueSystem Instance;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
                DestroyImmediate(gameObject);

        }

        bool _initialized = false;

        private void Initialize()
        {
            if (_initialized)
                return;

            architext = new TextArchitext(dialogueContainer.dialogueText);
            conversationManager = new ConversationManager(architext);
        }

        public void ShowSpeakerName(string speakerName = "") => dialogueContainer.nameContainer.Show(speakerName);
        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();
        public void Say(string speaker, string dialogue)
        {
            List<string> conversation =  new List<string>() { $"{speaker} \"{dialogue}\"" };
            Say(conversation);
        }

        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }
}
