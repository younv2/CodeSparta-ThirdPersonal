public enum StatType
{
    Speed
}

public class Stat
{
    private StatType statType;
    private float value;

    public float FinalValue => value;

    public Stat(StatType statType, float value)
    {
        this.statType = statType;
        this.value = value;
    }
}