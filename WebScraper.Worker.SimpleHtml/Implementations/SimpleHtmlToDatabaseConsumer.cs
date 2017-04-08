namespace CodeFrom.WebScraper.Worker.Implementations
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Common;
    using NLog;
    using Interfaces.TaskElements;
    using System.IO;

    public class SimpleHtmlToDatabaseConsumer : IConsumer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string ConnectionString { get; set; }

        public string Command { get; set; }

        public CommandType CommandType { get; set; }

        public void Consume(IPayload payload)
        {
            var html = payload as SimpleHtmlPayload;
            if (html != null)
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    var insertCmd = con.CreateCommand();
                    insertCmd.CommandText = this.Command;
                    insertCmd.CommandType = this.CommandType;
                    insertCmd.Parameters.Add("content", SqlDbType.Text);
                    insertCmd.Parameters["content"].SqlValue = html.Content.RenderSelection();
                    insertCmd.ExecuteNonQuery();
                }
            }
            else
            {
                throw new ArgumentNullException("payload");
            }
        }
    }
}
