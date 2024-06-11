using System;

[Serializable]
public class AuthRequest
{
    public string username;
    public string password;
}

[Serializable]
public class AuthResponse
{
    public string status;
    public string message;
    public string token;
}
