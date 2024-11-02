namespace BookOfHousholdAccountsLibrary
{
    internal class BookEntryModel
    {
        public BookEntryType EntryType { get; set; }
        public DateTime PostingDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public string MemoText { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}