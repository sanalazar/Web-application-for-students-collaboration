﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Http.Validation.Validators;
using System.Web.Mvc;
using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using Microsoft.AspNet.Identity;

// ReSharper disable once CheckNamespace
namespace CocktionMVC.Controllers
{
    /// <summary>
    /// Данный контроллер содержит в себе только те методы,
    /// которые открывают непосредственно вэб страницы.
    /// 
    /// Методы, которые используются для обработки RealTime 
    /// находятся в контроллере AuctionRealTime
    /// </summary>
    public class AuctionController : Controller
    {
        /// <summary>
        /// Главная страничка, на которой отображаются
        /// все активные в данный момент аукционы.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            /*TODO Сделать страничку с бесконечной прокруткой
             * где будут отображаться все аукционы 
             * 
             * Подумать насчет фильтров (как их лучше прифигачить)
             */

            //Конвертим время
            var controlTime = DateTimeManager.GetCurrentTime();

            CocktionContext db = new CocktionContext();

            //Получаем все активные в данный момент аукционы
            var auctions = (from x in db.Auctions
                where ((x.EndTime > controlTime) && (x.IsActive))
                select x).ToList<Auction>();

            return View(auctions);
        }

        /// <summary>
        /// Выводит страничку, где нужно создать
        /// аукцион
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Create() //метод для создания находится в контроллере FileSaver
        {
            //TODO сделать фильтр по универам
            return View();
        } //end of create

        /// <summary>
        /// Выводит страничку, которая показывает
        /// все созданные пользователем аукционы
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ShowMyAuctions()
        {
            var db = new CocktionContext();
            string userId = User.Identity.GetUserId();
            var user = db.AspNetUsers.Find(userId);

            List<Auction> auctions = user.HisAuctions.ToList();

            return View("MyAuctions", auctions);
        }

        /// <summary>
        /// Выводит страничку, где показан
        /// сам торг
        /// </summary>
        /// <param name="id">Айди аукциона, который надо показать</param>
        [HttpGet]
        public ActionResult CurrentAuction(int? id)
        {
            try
            {
                var db = new CocktionContext();
                Auction auction = db.Auctions.Find(id);

                //Если не удалось найти аукцион - кидаем эксепшн
                if (auction == null && id == null) throw new Exception();
                return View(auction);
            }
            catch
            {
                return View("PageNotFound");
            }
        }
    }
}