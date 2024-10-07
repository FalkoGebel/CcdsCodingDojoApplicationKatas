namespace ComparativeEstimationLibrary
{
    public class Project
    {
        public int Id { get; }
        public string Title { get; set; } = string.Empty;  // optional
        public Item[] Items { get; } = [];
        public Comparision[] Comparisions { get; } = [];
        public Dictionary<string, Item[]> Comparations { get; } = [];

        public void AddItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateComparation(string email, Item[] items)
        {
            throw new NotImplementedException();
        }

        public Item[] GetCummulatedComparation()
        {
            throw new NotImplementedException();
        }

        public void SaveToFile()
        {
            throw new NotImplementedException();
        }
    }
}