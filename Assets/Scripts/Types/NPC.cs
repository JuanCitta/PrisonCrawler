[System.Serializable]
public class QuestData
{
    public string npcId;
    public int currentProgress;
    public int requiredProgress;

    public bool IsComplete()
    {
        return currentProgress >= requiredProgress;
    }
}