using UnityEngine;

public class SaveManager : ISaveManager
{
    private const string SAVE_KEY = "save_key_1";
    
    public SaveData CurrentSave { get; set; }

    public void GetSavedData()
    {
        CurrentSave = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY, 
            JsonUtility.ToJson(new SaveData())));
    }

    public void ClearSaveData()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }

    public void MarkDirty()
    {
        PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(CurrentSave));
    }
}
