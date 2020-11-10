using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecommendationApi.Recommendations;
using static RecommendationApi.Recommendations.DTO;

namespace RecommendationApi.Controllers
{
    [Route("api/Recommendation")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        [Route("GetRecommendation"), HttpPost]
        public string GetRecommendations(RecommendationInput obj)
        {
            try
            {
                var recommendationObj = RecommendationManager.Recommendation(obj.Scenarios, obj.Trace);
                var output = JsonConvert.SerializeObject(recommendationObj);
                return output;
            }
            catch (Exception ex)
            {
                
                throw ex; 
            }
        }


    }

 
}
