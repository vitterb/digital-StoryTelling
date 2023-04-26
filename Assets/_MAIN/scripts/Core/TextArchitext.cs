using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TextArchitext 
{
    private TextMeshProUGUI tmpro_ui;
    private TextMeshPro tmpro_world;
    public TMP_Text tmpro => tmpro_ui != null ? tmpro_ui : tmpro_world;

    public string currentText => tmpro.text;
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int preTextLenght = 0;

    public string fullTargetText => preText + targetText;

    public enum BuildMethode {  instant, typewriter, fade}
    public BuildMethode buildMethod = BuildMethode.typewriter;

    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    public float speed { get { return basespeed + speedMultiplier; } set { speedMultiplier = value; } }
    private float basespeed = 1;
    private float speedMultiplier = 1;

    public int CharacterPerCycle { get { return speed <= 2f ? CharacterMultiplier : speed <= 2.5f ? CharacterMultiplier * 2 : CharacterMultiplier * 3; } }
    private int CharacterMultiplier = 1;

    public bool hurryUP = false;

    public TextArchitext(TextMeshProUGUI tmpro_ui)
    {
        this.tmpro_ui = tmpro_ui;
    }
    public TextArchitext(TextMeshPro tmpro_world)
    {
        this.tmpro_world = tmpro_world;
    }

    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        BuildProcess = tmpro.StartCoroutine(Building());
        return BuildProcess;
    }
    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        BuildProcess = tmpro.StartCoroutine(Building());
        return BuildProcess;
    }

    private Coroutine BuildProcess = null;
    public bool isBuilding => BuildProcess != null;

    public void Stop()
    {
        if(!isBuilding)
            return;

        tmpro.StopCoroutine(BuildProcess);
        BuildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();

        switch (buildMethod)
        {
            case BuildMethode.typewriter:
                yield return build_Typewriter();
                break;
            case BuildMethode.fade:
                yield return build_Fade();
                break;
        }

        Oncomplete();
    }

    private void Oncomplete()
    {
        BuildProcess = null;
        hurryUP = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethode.instant:
                
                break;
            case BuildMethode.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethode.fade:
                tmpro.ForceMeshUpdate();
                break;
        }
        Stop();
        Oncomplete();
    }
    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethode.instant:
                Prepare_Instant(); 
                break;
            case BuildMethode.typewriter:
                Prepare_Typewriter();
                break;
            case BuildMethode.fade:
                Prepare_Fade();
                break;
        }
    }

    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }
    private void Prepare_Typewriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if (preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }
    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if(preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLenght = tmpro.textInfo.characterCount;
        }
        else
        {
            preTextLenght = 0;
        }

        tmpro.text += targetText;
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color colorVisable = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color colorHidden = new Color(textColor.r, textColor.g, textColor.b, 0);

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            if(i < preTextLenght)
            {
                for(int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v ] = colorVisable;
                }
            }
            else
            {
                for (int v = 0; v < 4; v++)
                {
                    vertexColors[charInfo.vertexIndex + v] = colorHidden;
                }
            }
        }

        tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    private IEnumerator build_Typewriter()
    {
        while (tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUP ? CharacterPerCycle * 5 : CharacterPerCycle;

            yield return new WaitForSeconds(0.015f / speed );
        }
    }
    private IEnumerator build_Fade()
    {
        int minRange = preTextLenght;
        int maxRange = minRange + 1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color32[] vertexColors = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alphas = new float[textInfo.characterCount];

        while (true)
        {
            float fadespeed = ((hurryUP ? CharacterPerCycle * 5 : CharacterPerCycle) * speed) * 4f;

            for (int i = minRange; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadespeed);

                for (int v = 0; v < 4; v++)
                    vertexColors[charInfo.vertexIndex + v].a = (byte)alphas[i];
                
                if (alphas[i] >= 255)
                    minRange++;
                
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool lastCharacterIsVisible = !textInfo.characterInfo[maxRange - 1].isVisible;
            if (alphas[maxRange - 1] > alphaThreshold || lastCharacterIsVisible)
            {
                if (maxRange < textInfo.characterCount)
                    maxRange++;
                else if (alphas[maxRange - 1] >= 255 || lastCharacterIsVisible)
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
 