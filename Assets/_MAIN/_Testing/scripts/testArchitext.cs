using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace Testing
{
    public class testArchitext : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitext architext;

        public TextArchitext.BuildMethode bm = TextArchitext.BuildMethode.instant;

        string[] lines = new string[13]
        {
            "Dublin Tower, this is Delta 123 on final approach for runway 10, request landing clearance.",
            "Delta 123, Dublin Tower, winds are calm, runway 10 cleared to land.",
            "Runway 10 cleared to land, Delta 123.",
            "Delta 123, confirm gear down and locked.",
            "Gear down and locked, Delta 123.",
            "Delta 123, roger that, continue approach, caution wake turbulence, traffic is a heavy Boeing 747 departing runway 16.",
            "Roger that, continuing approach, Delta 123.",
            "Delta 123, traffic alert, Cessna on short final for runway 10, runway not cleared, go around, maintain altitude.",
            "Roger that, going around, maintain altitude, Delta 123.",
            "Delta 123, traffic is clear, runway 10 now cleared to land, wind calm.",
            "Runway 10 cleared to land, Delta 123.",
            "Delta 123, welcome to Dublin Airport, taxi to gate via taxiway alpha, bravo and delta.",
            "Thank you, Dublin Tower, taxi to gate via alpha, bravo and delta, Delta 123."
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.Instance;
            architext = new TextArchitext(ds.dialogueContainer.dialogueText);
            architext.buildMethod = TextArchitext.BuildMethode.fade;
        }

        // Update is called once per frame
        void Update()
        {
            if(bm != architext.buildMethod)
            {
                architext.buildMethod = bm;
                architext.Stop();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                architext.Stop();
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (architext.isBuilding)
                {
                    if (!architext.hurryUP)
                        architext.hurryUP = true;
                    else
                        architext.ForceComplete();
                }
                else
                    architext.Build(lines[Random.Range(0, lines.Length)]);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                architext.Append(lines[Random.Range(0, lines.Length)]);
            }
        }
    }
}

