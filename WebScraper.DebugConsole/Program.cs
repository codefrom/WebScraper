namespace WebScraper.DebugConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CodeFrom.WebScraper.Common;
    using CodeFrom.WebScraper.Worker.Implementations;
    using CodeFrom.WebScraper.Worker.Interfaces.TaskElements;
    using System.IO;

    public class DummyProvider : IProvider<DummyPayload>
    {
        public IEnumerable<DummyPayload> Provide()
        {
            return new List<DummyPayload>() {
                new DummyPayload() { Value = "This,is,a,test!" }
            };
        }
    }

    public class DummyTransformer : ITransformer<DummyPayload>
    {
        private static int a = 0;
        public DummyPayload Transform(IPayload payload)
        {
            var dpl = payload as DummyPayload;
            if (dpl == null)
                throw new ArgumentNullException("Wrong parameter!");
            var newval = string.Format("{0} + {1}", dpl?.Value, a++);
            return new DummyPayload() { Value = newval, Index = dpl.Index };
        }
    }


    public class DummyPayload : IPayload
    {
        public int Index { get; set; } 
        public string Value { get; set; }
    }

    public class DummySplittor : ISplitter<DummyPayload>
    {
        public IEnumerable<DummyPayload> Split(IPayload payload)
        {
            var dpl = payload as DummyPayload;
            if (dpl == null)
                throw new ArgumentNullException("Wrong parameter!");

            return dpl.Value.Split(',').Select((s, i) => new DummyPayload() { Value = s, Index = i });
        }
    }

    public class DummyAggregator : IAggregator<DummyPayload>
    {
        public DummyPayload Aggregate(IEnumerable<IPayload> payloads)
        {
            string a = string.Empty;
            var dploads = payloads.Select(p => p as DummyPayload).OrderBy(p => p.Index);
            foreach (var pl in dploads)
            {
                a += pl?.Value + " ";
            }
            return new DummyPayload() { Value = a };
        }
    }

    public class DummyConsumer : IConsumer
    {
        public void Consume(IPayload payload)
        {
            var dpl = payload as DummyPayload;
            Console.WriteLine("Got value : " + dpl?.Value);
        }
    }

    public class DummyHtmlPrinter : IConsumer
    {
        public void Consume(IPayload payload)
        {
            var html = payload as HtmlPayload;
            Console.WriteLine(new StreamReader(html?.Content).ReadToEnd());
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            TaskParallelProcessor a = new TaskParallelProcessor() { DegreeOfParallelism = 10 };
            var l = new List<ITaskElement>();
            l.Add(new SimpleHtmlProvider() { Address = "https://www.google.com" });
            l.Add(new DummyHtmlPrinter());
            a.TaskElements = l;
            a.DoTask();
        }
    }
}
