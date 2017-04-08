namespace CodeFrom.WebScraper.Worker.SimpleHtml.Implementations
{
    using Common;
    using System.Collections.Generic;
    using System.Text;

    public class VirtualObjectPayload : IPayload
    {
        private const char ObjectsSeporator = '.';
        private Dictionary<string, string> values = new Dictionary<string, string>();
        private Dictionary<string, VirtualObjectPayload> references = new Dictionary<string, VirtualObjectPayload>();

        public void SetProperty(string name, string value)
        {
            if (name.Contains(ObjectsSeporator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeporator }, 2);
                if (!references.ContainsKey(split[0]))
                {
                    references[split[0]] = new VirtualObjectPayload();
                }
                references[split[0]].SetProperty(split[1], value);
            }
            else
            {
                values[name] = value;
            }
        }

        public string GetProperty(string name)
        {
            if (name.Contains(ObjectsSeporator.ToString()))
            {
                var split = name.Split(new char[] { ObjectsSeporator }, 2);
                if (!references.ContainsKey(split[0]))
                {
                    references[split[0]] = new VirtualObjectPayload();
                }
                return references[split[0]].GetProperty(split[1]);
            }
            else
            {
                if (!values.ContainsKey(name))
                {
                    return string.Empty;
                }
                return values[name];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            foreach (var value in values)
            {
                sb.AppendLine($"{value.Key}={value.Value}");
            }
            foreach (var reference in references)
            {
                sb.AppendLine($"{reference.Key}={reference.Value.ToString()}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
