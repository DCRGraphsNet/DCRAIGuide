using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static RecommendationApi.Recommendations.DTO;

namespace RecommendationApi.Recommendations
{
    public class XMLParser
    {
        public static List<LogTrace> GetLogTraces(string logXML)
        {
            XDocument document = XDocument.Parse(logXML);
            IEnumerable<XElement> lsttraces = document.Descendants("trace");
            List<LogTrace> lst = new List<LogTrace>();
            foreach (var itemTrace in lsttraces)
            {
                LogTrace LogTraces = GetLogTrace(itemTrace.ToString());
                lst.Add(LogTraces);
            }
            return lst;
        }

        public static LogTrace GetLogTrace(string traceXML)
        {
            LogTrace LogTraces = new LogTrace();
            try
            {

                XDocument document = XDocument.Parse(traceXML);
                XElement lsttraces = document.Descendants("trace").FirstOrDefault();

                LogTraces.SimulationID = lsttraces.Attribute("id").Value;
                LogTraces.Percentage = GetPercentageInDouble(lsttraces.Attribute("percentage").Value);
                LogTraces.Title = lsttraces.Attribute("title").Value;


                IEnumerable<XElement> lstevents = lsttraces.Descendants("event");
                List<TraceEvents> LstTraceEvents = new List<TraceEvents>();
                foreach (var item in lstevents)
                {
                    TraceEvents traceEvents = new TraceEvents();
                    traceEvents.EventID = item.Attribute("id").Value;
                    traceEvents.EventLabel = item.Attribute("label").Value;
                    traceEvents.EventRole = item.Attribute("role").Value;
                    LstTraceEvents.Add(traceEvents);
                }
                LogTraces.EventIDs = LstTraceEvents;
            }
            catch (Exception e)
            {
                throw e;
            }


            return LogTraces;
        }

       
        public static Double GetPercentageInDouble(string percentage)
        {
            Double percentageDouble = 0;
            try
            {
                percentage = percentage.Replace(",", ".");
                percentageDouble = Convert.ToDouble(percentage);
            }
            catch (Exception)
            {
                percentageDouble = 0;
            }
            return percentageDouble;
        }

        
        public static string GetEventLabelFromLogTrace(List<LogTrace> lstScenarios, string eventID)
        {
            string label = string.Empty;
            foreach (var itemSc in lstScenarios)
            {
                foreach (var itemTr in itemSc.EventIDs)
                {
                    if (itemTr.EventID == eventID)
                    {
                        label = itemTr.EventLabel;
                    }
                }
            }
            return label;

        }

        
        public static string GetEventRoleFromLogTrace(List<LogTrace> lstScenarios, string eventID)
        {
            string role = string.Empty;
            foreach (var itemSc in lstScenarios)
            {
                foreach (var itemTr in itemSc.EventIDs)
                {
                    if (itemTr.EventID == eventID)
                    {
                        role = itemTr.EventRole;
                    }
                }
            }
            return role;

        }
    }
}
