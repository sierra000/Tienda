using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tienda.Models.Order;
using Newtonsoft.Json;

namespace Tienda.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            var model = new OrderViewModel();
            model.Item = new ItemViewModel();
            model.sessionId = Guid.NewGuid();

            return View(model);
        }

        public IActionResult ListOrder()
        {
            var model = new List<ListOrderViewModel>();
            var sessionKeys = HttpContext.Session.Keys.Where(x => x.Contains("Order_"));

            if (sessionKeys != null && sessionKeys.Any())
            {
                foreach (var key in sessionKeys)
                {
                    var order = HttpContext.Session.GetString(key);
                    var orderObj = JsonConvert.DeserializeObject<OrderViewModel>(order);

                    if (orderObj != null)
                    {
                        if(model.Any(x => x.Order.Name == orderObj.Name)){
                            model.Where(x => x.Order.Name == orderObj.Name).First().Order.ItemsSelected.AddRange(orderObj.ItemsSelected);
                        }
                        else
                        {
                            model.Add(new ListOrderViewModel
                            {
                                Order = new OrderViewModel
                                {
                                    ItemsSelected = orderObj.ItemsSelected,
                                    Name = orderObj.Name
                                }
                            });
                        }

                        var sum = model.Where(y => y.Order.Name == orderObj.Name).SelectMany(x => x.Order.ItemsSelected.Select(p => p.Price)).ToList();
                        var priceAcumulate = sum.Sum();
                        model.Where(x => x.Order.Name == orderObj.Name).First().TotalPrice = priceAcumulate;

                    }                      
                }


            }

            model = model.OrderByDescending(x => x.TotalPrice).ToList();

            return View(model);
        }

        public IActionResult Create(OrderViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name)){
                return Json(false);
            }
            else
            {
                var items = HttpContext.Session.GetString("Items_" + model.sessionId.ToString());

                if (!string.IsNullOrEmpty(items))
                {
                    var itemList = JsonConvert.DeserializeObject<List<ItemViewModel>>(items);
                    model.ItemsSelected = itemList;
                    HttpContext.Session.SetString("Order_" + model.sessionId.ToString(), JsonConvert.SerializeObject(model));
                }
            }


            return Json(true);
        }

        public IActionResult AddItems(OrderViewModel model)
        {
            var ok = true;
            try
            {
                var items = HttpContext.Session.GetString("Items_" + model.sessionId.ToString());
                if (string.IsNullOrEmpty(items))
                {
                    HttpContext.Session.SetString("Items_" + model.sessionId.ToString(), JsonConvert.SerializeObject(new List<ItemViewModel> { model.Item }));
                }
                else
                {
                    var itemList = JsonConvert.DeserializeObject<List<ItemViewModel>>(items);
                    itemList.Add(model.Item);
                    HttpContext.Session.SetString("Items_" + model.sessionId.ToString(), JsonConvert.SerializeObject(itemList));
                }
            }
            catch (Exception _e)
            {
                ok = false;
            }

            return Json(ok);
        }
    }
}