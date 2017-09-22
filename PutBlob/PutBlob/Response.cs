namespace PutBlob
{
    public class Response
    {
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
    }
}
