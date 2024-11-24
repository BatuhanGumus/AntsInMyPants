
public class SaveData
{
    public string username;
    public int money;
}

public interface ISaveManager
{
    SaveData CurrentSave { get; set; }

    void GetSavedData();
    void ClearSaveData();
    void MarkDirty();
}
