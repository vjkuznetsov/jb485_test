namespace MdParser.models
{
    public enum PreviousLine
    {
        Row = 0,
        TableHeader = 1,
        TableDelimiterOrTableRow = 2,
        EndOfTable = 3,
        Image = 4
    }
}
