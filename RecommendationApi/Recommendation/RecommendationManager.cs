using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using static RecommendationApi.Recommendations.DTO;

namespace RecommendationApi.Recommendations
{
    public static class RecommendationManager
    {
        /*Obtains a list of recommendations and orders them by their probability (most likely element first)*/
        public static List<Recommendation> Recommendation(string scenarios, string trace)
        {
            List<Recommendation> lst = new List<Recommendation>();
            lst = RecommendationFunctions.GetRecommendations(scenarios, trace);
            lst = lst.OrderByDescending(x => x.Probability).ToList();
            return lst;
        }
    }
}
