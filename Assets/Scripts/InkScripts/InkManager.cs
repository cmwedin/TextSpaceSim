using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;



// This is a super bare bones example of how to play and display a ink story in Unity.
public class InkManager : MonoBehaviour {
    public delegate void TurnAction();
    public static event Action<Story> OnCreateStory;
	public static event TurnAction OnTurn;

	//other monobehavior refs //? assigned in awake
	public InkCharacterLayer CharacterLayer; 
	public InkItemLayer ItemLayer; 
	
	[SerializeField] private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField] private Canvas canvas = null;

	// UI Prefabs
	[SerializeField] private Text textPrefab = null;
	[SerializeField] private Button buttonPrefab = null;
	
    void Awake () {
		// Remove the default message
		RemoveChildren();
		// ? assigns references for communicating data, all done here to avoid race conditions
		// * gets the other ink layers first
		CharacterLayer = gameObject.GetComponent<InkCharacterLayer>();
		ItemLayer = gameObject.GetComponent<InkItemLayer>();
		// *connects them to itself
		CharacterLayer.Manager = this;
		ItemLayer.Manager = this;
		// * create the story object here so other layers can use it for initializing
		story = new Story (inkJSONAsset.text);
		CharacterLayer.Init();
		ItemLayer.Init();
		BindInkExternals();
		//runs the main function
		StartStory();
	}
	private void BindInkExternals() {
        //binds ink external functions relevant to the class
        story.BindExternalFunction("LoadCharacter", (string name) => CharacterLayer.LoadCharacterSO(name));
        story.BindExternalFunction("GetTargetAttrib", (string name) => CharacterLayer.GetAttrib(name));
        story.BindExternalFunction("GetPlayerAttrib", (string name) => CharacterLayer.GetAttrib(name, CharacterLayer.CurrentPlayer));
        story.BindExternalFunction("GetTargetHealth", () => CharacterLayer.CurrentTarget.Health);
        story.BindExternalFunction("GetPlayerHealth", () => CharacterLayer.CurrentPlayer.Health);
        story.BindExternalFunction("GetTargetName", () => CharacterLayer.CurrentTarget.name);
        story.BindExternalFunction("GetPlayerName", () => CharacterLayer.CurrentPlayer.name);
        story.BindExternalFunction("DamageTarget", (float dmg) => CharacterLayer.CurrentTarget.Damage(dmg));
        story.BindExternalFunction("DamagePlayer", (float dmg) => CharacterLayer.CurrentTarget.Damage(dmg));
        story.BindExternalFunction("TargetIsDead", () => CharacterLayer.CurrentTarget.isDead);
		story.BindExternalFunction("TakeFromTarget", (string itemName) => CharacterLayer.TakeFrom(itemName));
		story.BindExternalFunction("PickUp", (string itemName) => CharacterLayer.GiveItem(itemName));

    }

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory () {
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}
	// Saves the current story state to a json stored in the current PlayerSO
	void SaveStory () {
		string saveData = story.state.ToJson();
		CharacterLayer.CurrentPlayer.SaveStory(saveData);
	}
	
	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView () {
		// Remove all the UI on screen
		RemoveChildren ();
		
		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue ();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		OnTurn?.Invoke();
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

}
