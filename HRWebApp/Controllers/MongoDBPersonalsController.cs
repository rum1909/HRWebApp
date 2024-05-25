using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver; // Thêm namespace này để sử dụng MongoDB.Driver
using HRWebApp.Models;
using MongoDB.Bson;
using System.Data.Entity;
using System.Configuration;

namespace HRWebApp.Controllers
{
    public class MongoDBPersonalsController : Controller
    {
        private HRDB db = new HRDB();
        private IMongoCollection<MongoDBPersonal> collection; // Sử dụng IMongoCollection thay vì HRDB

        public MongoDBPersonalsController()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MongoDBConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["apicompany"];
                var client = new MongoClient(connectionString); // Kết nối đến MongoDB
                var database = client.GetDatabase(databaseName); // Chọn database trong MongoDB
                collection = database.GetCollection<MongoDBPersonal>("employees"); // Chọn collection trong database
            }
            catch (MongoException mongoEx)
            {
                Console.WriteLine("MongoDB connection failed. Exception: " + mongoEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred. Exception: " + ex.Message);
            }

        }

        // GET: Personals
        public ActionResult Index()
        {
            //var personals = collection.Find(_ => true); // Truy vấn tất cả các tài liệu trong collection
            //return View(personals.ToList());
            var personals = collection.Find(_ => true).ToList();

            var personalViewModels = personals.Select(p => new PersonalViewModel
            {
                // Map MongoDBPersonal to PersonalViewModel properties here
                MongoDBPersonal = p,
                Personal = new Personal
                {
                    Employee_ID = p.employeeId
                }
            }).ToList();
            return View(personalViewModels);
        }
        //public ActionResult Index()
        //{
        //    var personalsFromMongoDB = collection.Find(_ => true).ToList(); // Lấy danh sách từ MongoDB
        //    var personalViewModels = personalsFromMongoDB.Select(personal => new PersonalViewModel
        //    {
        //        // Thực hiện ánh xạ từ các thuộc tính của đối tượng MongoDBPersonal sang PersonalViewModel
        //        MongoDBPersonal = personal
        //        // Các thuộc tính khác tương tự
        //    }).ToList();

        //    return View(personalViewModels);
        //}


        //GET: Personals/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personal = collection.Find(p => p.employeeId.Equals(id)).FirstOrDefault(); // Truy vấn tài liệu dựa trên ID
            if (personal == null)
            {
                return HttpNotFound();
            }
            return View(personal);
        }

        // GET: Personals/Create
        public ActionResult Create()
        {
            // Phương thức này vẫn giữ nguyên vì không cần truy vấn từ MongoDB để tạo form
            return View();
        }

        // POST: Personals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MongoDBPersonal MongoDBPersonal)
        {
            if (ModelState.IsValid)
            {
                collection.InsertOne(MongoDBPersonal); // Thêm một tài liệu mới vào collection
                return RedirectToAction("Index");
            }
            return View(MongoDBPersonal);
        }

        // GET: Personals/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var personal = collection.Find(p => p.employeeId == id).FirstOrDefault(); // Truy vấn tài liệu dựa trên ID
        //    if (personal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(personal);
        //}
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string idString = id.ToString();
            var filter = Builders<MongoDBPersonal>.Filter.Eq("employeeId", idString);
            var projection = Builders<MongoDBPersonal>.Projection
                .Include(p => p.vacationDays)
                .Include(p => p.paidToDate)
                .Include(p => p.paidLastYear)
                .Include(p => p.payRate)
                .Include(p => p.payRateId);
            var personal = collection.Find(filter).Project<MongoDBPersonal>(projection).FirstOrDefault();

            if (personal == null)
            {
                return HttpNotFound();
            }
            var personalViewModel = new PersonalViewModel
            {
                MongoDBPersonal = personal
            };
            // Find the document in the MongoDB collection with the specified employeeId
            //var personal = collection.Find(id);
            //var personal = collection.Find(_ => true).ToList();
            //var personalViewModels = personal.Select(p => new PersonalViewModel
            //{
            //    // Map MongoDBPersonal to PersonalViewModel properties here
            //    MongoDBPersonal = p,
            //    Personal = new Personal
            //    {
            //        Employee_ID = p.employeeId
            //    }
            //}).ToList();
            //MongoDBPersonal personal = collection.Find(Builders<MongoDBPersonal>.Filter.Eq("employeeId", id.ToString())).FirstOrDefault();


            // Create a PersonalViewModel object and populate it with data from the retrieved personal object
            //PersonalViewModel personalViewModel1 = new PersonalViewModel

            //{
            //    // Populate the properties of PersonalViewModel based on the retrieved personal object
            //    MongoDBPersonal = personal
            //    // Populate other properties as needed                      
            //};

            // Pass the PersonalViewModel object to the view
            return View(personalViewModel);
        }



        // POST: Personals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "employeeId,vacationDays,paidToDate,paidLastYear,payRate,payRateId")] MongoDBPersonal MongoDBPersonal)
        {
            if (ModelState.IsValid)
            {
                collection.ReplaceOne(p => p.employeeId == MongoDBPersonal.employeeId, MongoDBPersonal); // Thay thế tài liệu cũ bằng tài liệu mới
                return RedirectToAction("Index");
            }
            return View(MongoDBPersonal);
        }

        // GET: Personals/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personal = collection.Find(p => p.employeeId.Equals(id)).FirstOrDefault(); // Truy vấn tài liệu dựa trên ID
            if (personal == null)
            {
                return HttpNotFound();
            }
            return View(personal);
        }

        // POST: Personals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            collection.DeleteOne(p => p.employeeId.Equals(id)); // Xóa tài liệu dựa trên ID
            return RedirectToAction("Index");
        }
    }
}
