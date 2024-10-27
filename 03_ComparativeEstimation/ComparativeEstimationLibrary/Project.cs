namespace ComparativeEstimationLibrary
{
    internal class Project()
    {
        private readonly int _maxNumberOfItems = 10;
        private readonly char _firstId = 'A';

        internal int Id { get; set; }
        internal string Title { get; set; } = string.Empty;
        internal List<Item> Items { get; private set; } = [];
        internal Dictionary<string, List<char>> UsersRankedItemIds { get; private set; } = [];

        internal void AddComparation(string userAsEmail, List<char> rankedItemIds) => UsersRankedItemIds[userAsEmail] = rankedItemIds;

        internal void AddItem(char itemId, string itemDescription)
        {
            if (itemId == ' ')
                throw new ArgumentException("No item id");

            if (itemDescription == string.Empty)
                throw new ArgumentException("No item description");

            Items.Add(new(itemId) { Description = itemDescription });
        }

        internal void AddOrUpdateComparation(string email, Item[] items)
        {
            throw new NotImplementedException();
        }

        internal Item[] GetCummulatedComparation()
        {
            throw new NotImplementedException();
        }

        public override string ToString() => $"{Id}. {(Title == string.Empty ? "<UNTITLED>" : Title)}";

        internal char GetNextItemId()
        {
            if (Items.Count == _maxNumberOfItems)
                return ' ';

            if (Items.Count == 0)
                return _firstId;

            return (char)(Items.Last().Id + 1);
        }
    }
}