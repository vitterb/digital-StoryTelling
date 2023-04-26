using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class TestParser : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SendFileToParse();
        }

        // Update is called once per frame
        void SendFileToParse()
        {
            List<string> lines = FileManager.ReadTextAsset("testFile");
            foreach (string  line in lines)
            {
                if (line == string.Empty)
                {
                    continue;
                }
                DIALOGUE_LINE dl = DialogueParser.Parse(line);
            }
        }
    }
}
