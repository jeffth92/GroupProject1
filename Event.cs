using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GroupProject.ProgramUI;

namespace GroupProject
{
    public class Event
    {
        public enum EventType { Get, Use}
        public EventType Type;
        public string TriggerPhrase;
        public Result EventResult;

        public Event(string triggerphrase, EventType type, Result eventResult)
        {
            TriggerPhrase = triggerphrase;
            Type = type;
            EventResult = eventResult;
        }
    }
    public class Result
    {
        public enum ResultType { NewExit, GetItem, MessageOnly }
        public ResultType Type { get; }
        public string ResultExit { get; }
        public Item ResultItem { get; }
        public string ResultMessage { get; }

        public Result(string resultExit, string resultMessage)
        {
            Type = ResultType.NewExit;
            ResultExit = resultExit;
            ResultMessage = resultMessage;
        }
        public Result(Item resultItem, string resultMessage)
        {
            Type = ResultType.GetItem;
            ResultItem = resultItem;
            ResultMessage = resultMessage;
        }
        public Result(string resultMessage)
        {
            Type = ResultType.MessageOnly; //once had result.blahblah
            ResultMessage = resultMessage;
        }
    }
}
