namespace ComparativeEstimationLibrary
{
    public class Item(char id)
    {
        public char Id { get; private set; } = id;
        public string Description { get; set; } = string.Empty;
        public string Output
        {
            get => $"{Id}: \"{Description}\"";
        }
    }
}