using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace RecommendationApi.Recommendations
{
    public class DTO
    {

        public class Recommendation
        {
            public string SimulationId { get; set; }
            public string SimulationTitle { get; set; }
            public string EventId { get; set; }
            public string EventLabel { get; set; }
            public string EventRole { get; set; }

            public Double Probability { get; set; }
        }
        public class LogTrace
        {
            public string SimulationID { get; set; }
            public Double Percentage { get; set; }
            public string Title { get; set; }

            public List<TraceEvents> EventIDs { get; set;  }

        }

        public class TraceEvents
        {
            public string EventID { get; set; }
            public string EventLabel { get; set; }
            public string EventRole { get; set; }
        }
        public class EventOccurence
        {
            public string EventID { get; set; }
            public int Occurence { get; set; }
            public double Percentage { get; set; }
        }

        public class RecommendationInput
        {
            public string Scenarios { get; set; }
            public string Trace { get; set; }

            public string GraphId { get; set; }
        }
    }

}

