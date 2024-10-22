namespace ComparativeEstimationLibrary
{
    internal class ProjectModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<ItemModel> Items { get; set; } = [];
        //public Dictionary<string, Item[]> Comparations { get; private set; } = [];
    }
}