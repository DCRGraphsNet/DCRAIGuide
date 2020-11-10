using System;
using System.Collections.Generic;
using System.Xml;
using static RecommendationApi.Recommendations.DTO;

namespace RecommendationApi.Recommendations
{
    public static class RecommendationFunctions
    {
        /*returns list of recommendations for the next event, with the associated probability of that event being the next.*/
        public static List<Recommendation> GetRecommendations(string scenarios, string trace)
        {
            List<LogTrace> lstScenarios = XMLParser.GetLogTraces(scenarios);
            LogTrace lstsTrace = XMLParser.GetLogTrace(trace);
            List<Recommendation> lst = Recommend(lstScenarios, lstsTrace);
            return lst;
        }

        /*Based on a list of scenarios and a trace, find the scenarios that match the trace, 
         * adjust percentages, create Recommendations and return them in a list.*/
        public static List<Recommendation> Recommend(List<LogTrace> lstScenarios, LogTrace trace)
        {
            List<Recommendation> lst = new List<Recommendation>();
            List<LogTrace> matches = SelectScenariosForTrace(lstScenarios, trace);
            matches = AdjustPercentage(matches);
            string[] traceEventIds = GetEventIDArray(trace.EventIDs);

            foreach (LogTrace item in matches)
            {
                string[] matchEventIds = GetEventIDArray(item.EventIDs);
                string nxt = GetNextEvent(traceEventIds, matchEventIds);

                Recommendation recommendation = new Recommendation();
                recommendation.SimulationId = item.SimulationID;
                recommendation.SimulationTitle = item.Title;

                if (nxt != null)
                {
                    recommendation.EventId = nxt;
                    recommendation.EventLabel = XMLParser.GetEventLabelFromLogTrace(lstScenarios, nxt);
                    recommendation.EventRole = XMLParser.GetEventRoleFromLogTrace(lstScenarios, nxt);

                }
                recommendation.Probability = item.Percentage;

                lst.Add(recommendation);


            }
            return lst;
        }

        /*Returns list of the scenarios in lstScenarios that have eventIDs matching the eventIDs of the trace lstTrace*/
        public static List<LogTrace> SelectScenariosForTrace(List<LogTrace> scenarios, LogTrace trace)
        {
            List<LogTrace> matches = new List<LogTrace>();
            
            foreach (LogTrace scenario in scenarios)
            {
                bool isMatch = trace.IsPrefixOf(scenario);
                if (isMatch)
                { 
                    matches.Add(scenario);
                }
            }
            return matches;
        }

        /*Function for updating the percentage of each match with their relative influence on the total amount.
         (Users assign percentages that aren't necessarily reflective and may exceed 100% total for all scenarios for a graph)*/
        private static List<LogTrace> AdjustPercentage(List<LogTrace> matches)
        {
            Double totalSumOfRemaining = 0;
            foreach (LogTrace item in matches)
            {
                //Adjust for scenarios that don't have a percentage estimate included to ensure they are included in calculation.
                Double per = item.Percentage;
                if (per == 0)
                {
                    item.Percentage = 1;
                    per = 1;
                }
                totalSumOfRemaining += per;

            }

            foreach (LogTrace item in matches)
            {
                Double oldValue = item.Percentage;
                Double newValue = AdjustPercentageValue(oldValue, totalSumOfRemaining);
                item.Percentage = newValue;
            }
            return matches;
        }

       

        /*Given two arrays of eventIDs, picks the next event from the second array*/
        public static string GetNextEvent(string[] str, string[] str2)
        {
            string nextEvent = null;
            try
            {
                nextEvent = str2[str.Length];
            }
            catch (Exception e)
            {
                throw e;
            }
            return nextEvent;
        }

        

        /*Adjusts the percentage of the total amount for a single logtrace*/
        private static Double AdjustPercentageValue(Double oldValue, Double sumOfRemaining)
        {
            if (sumOfRemaining == 0)
            {
                return 0;
            }

            Double adjustedValue = (oldValue / sumOfRemaining) * 100;
            return Math.Round(adjustedValue);

        }

        public static Boolean IsPrefixOf(this LogTrace lt, LogTrace t)
        {
            string[] traceEventIDs = GetEventIDArray(lt.EventIDs);
            string[] scenarioEventIDs = GetEventIDArray(t.EventIDs);

            return TraceEqual(traceEventIDs, scenarioEventIDs);
        }

        /*returns array of eventIDs for a given trace.*/
        public static string[] GetEventIDArray(List<TraceEvents> trace)
        {
            List<string> arr = new List<string>();
            foreach (TraceEvents item in trace)
            {
                string eventID = item.EventID;
                arr.Add(eventID);
            }
            return arr.ToArray();

        }

        /*Function for testing equality between two arrays of eventIDs.*/
        public static bool TraceEqual(string[] eventIDs1, string[] eventIDs2)
        {
            if (eventIDs1.Length > eventIDs2.Length)
            {
                return false;
            }
            for (int i = 0; i < eventIDs1.Length; i++)
            {
                if (eventIDs1[i] != eventIDs2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

