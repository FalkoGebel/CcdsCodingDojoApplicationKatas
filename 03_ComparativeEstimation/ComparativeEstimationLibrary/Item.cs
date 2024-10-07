namespace ComparativeEstimationLibrary
{
    public class Item
    {
        public char Id { get; }
        public string Description { get; set; } = string.Empty;
        public string Output { get; } = string.Empty;  // e.g. "D: Add persistence"
    }
}