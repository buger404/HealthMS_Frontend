using System;

[Serializable]
public class CommentModel
{
    public string id;
    public string comment;
    public string username;
    public DateTime sendTime;
    public int likes;
    public bool praised;
    public bool liked;
}
