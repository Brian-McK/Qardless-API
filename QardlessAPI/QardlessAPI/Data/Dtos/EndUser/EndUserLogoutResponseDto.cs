namespace QardlessAPI.Data.Dtos.EndUser;

public class EndUserLogoutResponseDto
{
    public Guid Id { get; set; }
    
    public bool IsLoggedIn { get; set; }
}