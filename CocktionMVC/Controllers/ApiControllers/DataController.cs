﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CocktionMVC.Models.DAL;
using CocktionMVC.Models.JsonModels.MobileClientModels;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class DataController : ApiController
    {
        [Authorize]
        [HttpPost]
        public List<HouseInfo> GetHouses()
        {
            CocktionContext db = new CocktionContext();
            List<HouseInfo> houses = new List<HouseInfo>();
            foreach (var house in db.Houses)
            {
                houses.Add(new HouseInfo()
                {
                    Adress = house.Adress,
                    Faculty = house.Faculty,
                    Likes = house.Likes,
                    Rating = house.Rating,
                    University = house.University
                });
            }
            return houses;
        }
    }
}
