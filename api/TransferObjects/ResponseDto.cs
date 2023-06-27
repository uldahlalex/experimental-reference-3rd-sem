namespace api.TransferObjects;

public class ResponseDto
{
    public ResponseDto(string messageToClient)
    {
        MessageToClient = messageToClient;
    }

    public string MessageToClient { get; set; }
    public Object? ResponseData { get; set; }
}