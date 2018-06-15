namespace NBluem.Structure.Net.Response
{
    public interface IBluemResponse
    {
        bool Status { get; set; }

        string Message { get; set; }
    }
}