using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderboardController : MonoBehaviour
{
	[SerializeField] private TMP_Text[] _entryTextObjects;
	[SerializeField] private TMP_InputField _usernameInputField;

	// NOTE: For now using Scripts/UI/LeaderboardShowcase.cs
	
	private void Start()
	{
		LoadEntries();
	}

	private void LoadEntries()
	{
		Leaderboards.Ricochet.GetEntries(entries =>
		{
			foreach (var t in _entryTextObjects)
				t.text = "";

			var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
			for (int i = 0; i < length; i++)
				_entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
		});
	}
	
	public void UploadEntry()
	{
		Leaderboards.Ricochet.UploadNewEntry(_usernameInputField.text, GameStateManager.GetScore(), isSuccessful =>
		{
			if (isSuccessful)
				LoadEntries();
		});
	}
}

