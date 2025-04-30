using System.Collections.Generic;

public class PlayerStat
{
    private Dictionary<StatType, Stat> stats;

    public Dictionary<StatType, Stat> Stats => stats;

    public PlayerStat() { }
    public PlayerStat(Dictionary<StatType, Stat> stats)
    {
       this.stats = stats;
    }
}