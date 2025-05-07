public class NPCData
{
    private readonly int id;
    private readonly string name;
    private readonly string imagePath;
    
    public int Id { get => id; }
    public string Name { get => name; }
    public string ImagePath { get => imagePath; }

    public NPCData(int id, string name, string imagePath)
    {
        this.id = id;
        this.name = name;
        this.imagePath = imagePath;
    }
}
