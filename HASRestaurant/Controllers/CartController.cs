using HASRestaurant.Context;
using HASRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HASRestaurant.Controllers
{
    public class CartController : Controller
    {
        PRN292_HASRestaurantEntities db = new PRN292_HASRestaurantEntities();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ApplyCoupon(FormCollection formCollection)
        {
            string coup = formCollection["coupon"];
            if (coup == "HAS1" && Session["HAS1"] == null)
            {
                Session["HAS1"] = "1";
                List<CartModel> list = (List<CartModel>)Session["cart"];
                foreach (var item in list)
                {
                    item.Product.Price = item.Product.Price - (item.Product.Price / 100) * 10;
                }

                Session["cart"] = list;
                return View("Index");
            }
            else if(coup == "HAS2" && Session["HAS2"] == null)
            {
                Session["HAS2"] = "2";
                List<CartModel> list = (List<CartModel>)Session["cart"];
                foreach (var item in list)
                {
                    item.Product.Price = item.Product.Price - (item.Product.Price / 100) * 20;
                }
                Session["cart"] = list;
                return View("Index");
            }
            else if (coup == "HAS3" && Session["HAS3"] == null)
            {
                Session["HAS3"] = "3";
                List<CartModel> list = (List<CartModel>)Session["cart"];
                foreach (var item in list)
                {
                    item.Product.Price = item.Product.Price - (item.Product.Price / 100) * 30;
                }
                Session["cart"] = list;
                return View("Index");
            }
            else if (coup == "HAS4" && Session["HAS4"] == null)
            {
                Session["HAS4"] = "4";
                List<CartModel> list = (List<CartModel>)Session["cart"];
                foreach (var item in list)
                {
                    item.Product.Price = item.Product.Price - (item.Product.Price / 100) * 40;
                }
                Session["cart"] = list;
                return View("Index");
            }
            return View("Index");
        }
        public ActionResult OrderNow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Product p = db.Products.Find(id);
            if (p.Quantity != 0)
            {
                if (Session["cart"] == null)
                {
                    List<CartModel> lstCart = new List<CartModel>
                {
                    new CartModel(db.Products.Find(id),1)
                };
                    Session["cart"] = lstCart;
                }
                else
                {
                    List<CartModel> lstCart = (List<CartModel>)Session["cart"];
                    int checkExist = isExistCheck(id);
                    if (checkExist == -1)
                    {
                        if (isValidProduct(id) != -1)
                        {
                            lstCart.Add(new CartModel(db.Products.Find(id), 1));
                        }
                    }
                    else
                    {
                        if (lstCart[checkExist].Quantity < db.Products.Find(id).Quantity)
                        {
                            lstCart[checkExist].Quantity++;
                        }
                    }
                    Session["cart"] = lstCart;
                }
                return View("Index");
            }
            else
            {
                TempData["mess"] = "Product Out Stock!!!";
                return RedirectToAction("Index","Menu");
            }
        }
        private int isExistCheck(int? id)
        {
            List<CartModel> lstCart = (List<CartModel>)Session["cart"];
            for (int i = 0; i < lstCart.Count; i++)
            {
                if (lstCart[i].Product.ProductID == id) return i;
            }
            return -1;
        }
        private int isValidProduct(int? id)
        {
            var lstAllProduct = db.Products.ToList();
            for (int i = 0; i < lstAllProduct.Count; i++)
            {
                if (lstAllProduct[i].ProductID == id) return lstAllProduct[i].ProductID;
            }
            return -1;
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int check = isExistCheck(id);
            List<CartModel> lstCart = (List<CartModel>)Session["cart"];
            if (check != -1)
            {
                lstCart.RemoveAt(check);
            }
            Session.Remove("HAS1");
            Session.Remove("HAS2");
            Session.Remove("HAS3");
            Session.Remove("HAS4");
            Session["cart"] = lstCart;
            return View("Index");
        }
        public ActionResult AddQuantity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            int check = isExistCheck(id);
            List<CartModel> lstCart = (List<CartModel>)Session["cart"];
            if (check != -1)
            {
                if (lstCart[check].Quantity < db.Products.Find(id).Quantity)
                {
                    lstCart[check].Quantity++;
                }
            }
            Session["cart"] = lstCart;
            return View("Index");
        }
        public ActionResult SubQuantity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            int check = isExistCheck(id);
            List<CartModel> lstCart = (List<CartModel>)Session["cart"];
            if (check != -1)
            {
                if (lstCart[check].Quantity > 1)
                {
                    lstCart[check].Quantity--;
                }
            }
            Session["cart"] = lstCart;
            return View("Index");
        }
    }
}