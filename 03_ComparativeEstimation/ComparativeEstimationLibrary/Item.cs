namespace ComparativeEstimationLibrary
{
    internal class Item(char id)
    {
        public char Id { get; private set; } = id;
        public string Description { get; set; } = string.Empty;
        internal string Output { get; } = string.Empty;  // e.g. "D: Add persistence"
    }
}