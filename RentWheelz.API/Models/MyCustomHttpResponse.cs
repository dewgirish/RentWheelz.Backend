namespace RentWheelz.API.Models
{
    public class MyCustomHttpResponse
    {
        public MyCustomHttpResponse(bool IsSuccess, string message, object? data = null)
        {
            Status = IsSuccess ? "Success" : "Fail";
            Message = message;
            this.data = data == null ? null : data;
        }

        public string Status { get; private set; }
        public string Message { get; private set; }
        public object? data { get; private set; }
    }
}