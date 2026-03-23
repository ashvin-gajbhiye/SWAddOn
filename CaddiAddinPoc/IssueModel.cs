namespace CaddiAddinPoc
{
    public class IssueModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        public IssueModel(string code, string description, string url)
        {
            Code = code;
            Description = description;
            Url = url;
        }
    }
}