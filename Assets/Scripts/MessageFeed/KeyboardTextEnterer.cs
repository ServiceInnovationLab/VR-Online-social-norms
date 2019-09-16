using UnityEngine;
using System.Collections;
using VRTK;

public class KeyboardTextEnterer : TextEnterer
{
    [SerializeField] bool typeByWord = true;
    [SerializeField] int wordsPerClick = 3;
    [SerializeField] VRTK_InteractableObject keyboard;

    string[] words;
    int currentWord = 0;

    bool typeMoreWords = false;

    FlashUntilNear flash;

    protected override void Start()
    {
        base.Start();

        words = textToEnter.Split(' ');

        flash = keyboard.GetComponent<FlashUntilNear>();

        keyboard.InteractableObjectUsed += Keyboard_Used;
        keyboard.InteractableObjectUntouched += Keyboard_Untouched;
    }

    private void Keyboard_Untouched(object sender, InteractableObjectEventArgs e)
    {
        if (!isTypingCompleted && typeByWord)
        {
            flash.enabled = true;
        }
    }

    private void Keyboard_Used(object sender, InteractableObjectEventArgs e)
    {
        typeMoreWords = true;

        StartTypeText();
    }

    protected override IEnumerator TypeText()
    {
        if (!typeByWord)
        {
            // Quick hack at the moment... this will disable the highlighting
            keyboard.GetComponent<TwoUseObject>()?.AllowSecondUse();

            yield return base.TypeText();
            yield break;
        }

        int typedLength = 0;
        while (currentWord < words.Length)
        {
            yield return new WaitUntil(() => typeMoreWords);

            typeMoreWords = false;

            for (int word = 1; word <= wordsPerClick; word++)
            {
                // Need the <= as we want to type the space as well!
                for (int wordLength = 0; wordLength <= words[currentWord].Length; wordLength++)
                {
                    typedLength++;

                    TypeCharacter(typedLength);
                    yield return new WaitForSeconds(timeBetweenCharacters.GetValue());
                }

                currentWord++;
            }
        }

        OnTypingFinished();

        flash.enabled = false;
    }
}
