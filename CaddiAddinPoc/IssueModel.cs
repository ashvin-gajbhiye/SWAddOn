using System.Collections.Generic;

namespace CaddiAddinPoc
{
    public class IssueModel
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Status { get; set; }

        public string Timestamp { get; set; }

        public List<string> Comments { get; set; }

        // NEW
        public double X { get; set; }

        public double Y { get; set; }

        public IssueModel(
            string code,
            string title,
            string description,
            string url,
            string status,
            string timestamp,
            List<string> comments,
            double x,
            double y)
        {
            Code = code;
            Title = title;
            Description = description;
            Url = url;
            Status = status;
            Timestamp = timestamp;
            Comments = comments;

            X = x;
            Y = y;
        }
    }
}