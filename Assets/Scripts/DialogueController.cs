using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMODUnity;

public class DialogueController : MonoBehaviour
{

	public TMP_Text nameText;
	public TMP_Text dialogueText;
	public GameObject firstDialogue;
	public bool isDialogueStarted = false;

	public SetFMODParams fmdoparams;

	// public Animator animator;

	private Queue<DialoguePart> dialogueParts;

	public LevelLoader loader;

	public Image bgImage;
	public Sprite[] bgImages;

	// Use this for initialization
	void Start()
	{
		dialogueParts = new Queue<DialoguePart>();
		firstDialogue.GetComponent<DialogueTrigger>().TriggerDialogue();
	}

    public void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isDialogueStarted)
            {
				DisplayNextSentence();
            }
            else
            {
				isDialogueStarted = true;
			}

			if (dialogueParts.Count == 0)
            {
				loader.LoadNextLevel();
            }
		}
    }

    public void StartDialogue(Dialogue dialogue)
	{
		// animator.SetBool("IsOpen", true);

		dialogueParts.Clear();

		foreach (DialoguePart part in dialogue.dialogueParts)
		{
			dialogueParts.Enqueue(part);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (dialogueParts.Count == 0)
		{
			EndDialogue();
			return;
		}

		DialoguePart part = dialogueParts.Dequeue();
		nameText.text = part.npcName;
		nameText.font = part.NPCfontAsset;

		if (part.SFXeventName is not null && part.SFXeventName.Length > 0)
        {
			RuntimeManager.PlayOneShot(part.SFXeventName);
        }

		bgImage.sprite = GetBgImage(part.bgImageIndex);

		fmdoparams.isScary = part.isScary;
		fmdoparams.isChill = part.isChill;

		dialogueText.font = part.dialogueFontAsset;
		StopAllCoroutines();
		StartCoroutine(TypeSentence(part.dialogueSentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		// animator.SetBool("IsOpen", false);
	}

	public Sprite GetBgImage(int imageIndex)
    {
		return bgImages[imageIndex];
    }

}