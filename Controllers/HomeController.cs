using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;        // necessary to use SESSION
using Microsoft.AspNetCore.Identity;    // necessary to HASH a PASSWORD
using Microsoft.EntityFrameworkCore;
using DiveLog.Models;

namespace DiveLog.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;
        // }

        public DiveContext dbContext;

        public HomeController(DiveContext context)
        {
            dbContext = context;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(Diver Diver2Login)
        {
            Diver DiverInDB = dbContext.Divers.FirstOrDefault(u => u.Username == Diver2Login.Username);
            if (DiverInDB == null)
            {
                ModelState.AddModelError("Username", "Invalid User Name");
                return View("Index");
            }

            PasswordHasher<Diver> hasher = new PasswordHasher<Diver>();
            PasswordVerificationResult result = hasher.VerifyHashedPassword(Diver2Login, DiverInDB.Password, Diver2Login.Password);

            if (result == 0)
            {
                ModelState.AddModelError("Password", "Invalid Password");
                return View("Index");
            }
            // SESSION INSTANTIATION
            HttpContext.Session.SetInt32("sessionid", DiverInDB.DiverID);
            return RedirectToAction("DisplayLogs");
        }

        [HttpGet("register")]
        public ViewResult Register()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost("creatediver")]
        public IActionResult RegisterDiver(Diver RDiver)
        {
            ViewBag.RegOrLogin = "0";

            if (ModelState.IsValid)
            {
                // does the Username entered exist?
                if (dbContext.Divers.Any(u => u.Username == RDiver.Username))
                {
                    ModelState.AddModelError("Username", "Username already in use");
                    return View("Register");
                }
                
                // HASH the password before storing it in the DB
                PasswordHasher<Diver> Hasher = new PasswordHasher<Diver>();
                RDiver.Password = Hasher.HashPassword(RDiver, RDiver.Password);
                dbContext.Add(RDiver);
                dbContext.SaveChanges();
                //if new Diver Username address exists, create the Diver and the new logged in sessionid
                HttpContext.Session.SetInt32("sessionid", RDiver.DiverID);
                return RedirectToAction("DisplayLogs");
            }
            return View("Register");
        }

        [HttpGet("displaylogs")]
        public IActionResult DisplayLogs()
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                ViewBag.LoggedInDiver = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid);
                ViewBag.DSCount = dbContext.DiveSites.Count();
                // Diver DLogs = new Diver();
                // DLogs = dbContext.Divers.Include(s => s.DiverLogs).ThenInclude(l => l.DiveSite).FirstOrDefault(d => d.DiverID == diverid);
                List<DiverLog> AllLogs = new List<DiverLog>();
                AllLogs = dbContext.DiverLogs.Include(s => s.DiveSite).ToList();

                foreach (DiverLog dl in AllLogs)
                {
                    System.Console.WriteLine(dl.DiveNumber +", "+ dl.DiveStartTime.ToString("MMM d, yyyy"));
                    System.Console.WriteLine(dl.DiveSite);
                    // System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dl.DiveSite));
                    System.Console.WriteLine(dl.DiveSite.SiteName +", "+ dl.DiveSite.City +", "+ dl.DiveSite.StProv +", "+ dl.DiveSite.Country);
                }
                return View(AllLogs);
            }
            return Redirect("/");
        }

        [HttpGet("displaysites")]
        public IActionResult DisplaySites()
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                ViewBag.LoggedInDiver = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid);
                List<DiveSite> AllSites = dbContext.DiveSites.ToList();
                return View(AllSites);
            }
            return Redirect("/");
        }

        [HttpGet("adddive")]
        public IActionResult AddDive()
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                ViewAddLog NewDL = new ViewAddLog();
                NewDL.ThisDiver = dbContext.Divers.Include(l => l.DiverLogs).SingleOrDefault(d => d.DiverID == diverid);
                System.Console.WriteLine("Number of Logs: " + NewDL.ThisDiver.DiverLogs.Count());
                ViewBag.LogNum = NewDL.ThisDiver.DiverLogs.Count() + 1;
                NewDL.DiveLocations = dbContext.DiveSites.ToList();
                // System.Console.WriteLine(ModelState.Count);
                // System.Console.WriteLine(ModelState.ValidationState);
                // System.Console.WriteLine(ModelState.Values);
                return View(NewDL);
            }
            return Redirect("/");
        }

        [HttpPost("createdive")]
        public IActionResult CreateDive(ViewAddLog NewViewAddLog)
        {
            System.Console.WriteLine(ModelState.Count);
            System.Console.WriteLine(ModelState.ValidationState);
            System.Console.WriteLine("Diver ID: " + NewViewAddLog.NewDiverLog.DiverID);
            System.Console.WriteLine("Site ID: " + NewViewAddLog.NewDiverLog.DiveSiteID);
            System.Console.WriteLine("Dive Num: " + NewViewAddLog.NewDiverLog.DiveNumber);
            System.Console.WriteLine("Dive Date: " + NewViewAddLog.NewDiverLog.DiveDate);
            System.Console.WriteLine("Bottom Time: " + NewViewAddLog.NewDiverLog.BottomTime);
            System.Console.WriteLine("Max Depth: " + NewViewAddLog.NewDiverLog.MaxDepth);
            System.Console.WriteLine("Tank Start: " + NewViewAddLog.NewDiverLog.TankStartPSI);
            System.Console.WriteLine("Tank End: " + NewViewAddLog.NewDiverLog.TankEndPSI);
            System.Console.WriteLine("Dive Start: " + NewViewAddLog.NewDiverLog.DiveStartTime);
            System.Console.WriteLine("Dive Stop: " + NewViewAddLog.NewDiverLog.DiveStopTime);
            System.Console.WriteLine("Safety Time: " + NewViewAddLog.NewDiverLog.SafetyStopTime);
            System.Console.WriteLine("Safety Depth: " + NewViewAddLog.NewDiverLog.SafetyStopDepth);
            System.Console.WriteLine("PGEntry: " + NewViewAddLog.NewDiverLog.PressureGroupEntry);
            System.Console.WriteLine("PGExit: " + NewViewAddLog.NewDiverLog.PressureGroupExit);
            System.Console.WriteLine("SI: " + NewViewAddLog.NewDiverLog.SurfaceInterval);
            System.Console.WriteLine("Water Temp: " + NewViewAddLog.NewDiverLog.WaterTemp);
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                System.Console.WriteLine("if statement 1 - session test");
                System.Console.WriteLine(ModelState.IsValid);
                ViewAddLog NewDL = new ViewAddLog();
                if (NewViewAddLog.NewDiverLog.DiveDate > DateTime.Now)
                {
                    System.Console.WriteLine("if statement 2");
                    NewDL.ThisDiver = dbContext.Divers.Include(l => l.DiverLogs).SingleOrDefault(d => d.DiverID == diverid);
                    ViewBag.LogNum = NewDL.ThisDiver.DiverLogs.Count() + 1;
                    NewDL.DiveLocations = dbContext.DiveSites.ToList();
                    return View("AddDive", NewDL);
                }
                System.Console.WriteLine(ModelState.IsValid);
                if (ModelState.IsValid)
                {
                    System.Console.WriteLine("if statement 3 - modelstate isvalid");
                    DiverLog CreateDiverLog = new DiverLog();
                    CreateDiverLog = NewViewAddLog.NewDiverLog;
                    CreateDiverLog.DiverID = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid).DiverID;
                    CreateDiverLog.DiveSiteID = NewViewAddLog.NewDiverLog.DiveSiteID;
                    CreateDiverLog.PressureGroupEntry = Char.ToUpper(NewViewAddLog.NewDiverLog.PressureGroupEntry);
                    CreateDiverLog.PressureGroupExit = Char.ToUpper(NewViewAddLog.NewDiverLog.PressureGroupExit);
                    dbContext.Add(CreateDiverLog);
                    dbContext.SaveChanges();
                    // System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(CreateDiverLog));
                    return Redirect("/displaylogs");
                }
                NewDL.ThisDiver = dbContext.Divers.Include(l => l.DiverLogs).SingleOrDefault(d => d.DiverID == diverid);
                ViewBag.LogNum = NewDL.ThisDiver.DiverLogs.Count() + 1;
                NewDL.DiveLocations = dbContext.DiveSites.ToList();
                return View("AddDive", NewDL);
            }
            return Redirect("/");
        }

        [HttpGet("viewdive/{divenum}")]
        public IActionResult ViewDive(int divenum)
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                ViewBag.LoggedInDiver = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid);
                DiverLog CurrentDive = dbContext.DiverLogs.Include(s => s.DiveSite).FirstOrDefault(cd => cd.DiveNumber == divenum);
                return View(CurrentDive);
            }
            return Redirect("/");
        }

        [HttpGet("addsite")]
        public IActionResult AddSite()
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                ViewBag.LoggedInDiver = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid);
                return View();
            }
            return Redirect("/");
        }

        [HttpPost("createsite")]
        public IActionResult CreateSite(DiveSite NewDiveSite)
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                if (ModelState.IsValid)
                {
                    dbContext.Add(NewDiveSite);
                    dbContext.SaveChanges();
                    return RedirectToAction("AddDive");
                }
            }
            return Redirect("/");
        }

        [HttpGet("editsite/{siteid}")]
        public IActionResult EditSite(int divesiteid)
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                if (ModelState.IsValid)
                {
                    ViewBag.LoggedInDiver = dbContext.Divers.SingleOrDefault(d => d.DiverID == (int)diverid);
                    DiveSite Site2Edit = dbContext.DiveSites.SingleOrDefault(s => s.DiveSiteID == divesiteid);
                    return View(Site2Edit);
                }
            }
            return Redirect("/");
        }

        [HttpPost("updatesite")]
        public IActionResult UpdateSite(DiveSite thisdivesite)
        {
            int? diverid = HttpContext.Session.GetInt32("sessionid");
            if (diverid != null)
            {
                if (ModelState.IsValid)
                {
                    dbContext.Add(thisdivesite);
                    dbContext.SaveChanges();
                    return RedirectToAction("DisplaySites");
                }
            }
            return Redirect("/");
        }

        [HttpGet("logout")]
        public ViewResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpGet("seeddb")]
        public IActionResult SeedDB()
        {
            DiveSite DS1 = new DiveSite();
            DS1.SiteName = "Meadows";
            DS1.City = "Kaiua Kona";
            DS1.StProv = "Hawaii";
            DS1.Country = "USA";
            DS1.Latitude = 19.611935;
            DS1.Longitude = -155.995750;
            DS1.Altitude = 0;
            
            DiveSite DS2 = new DiveSite();
            DS2.SiteName = "3-Room Cave";
            DS2.City = "Kaiua Kona";
            DS2.StProv = "Hawaii";
            DS2.Country = "USA";
            DS2.Latitude = 19.611935;
            DS2.Longitude = -155.995750;
            DS2.Altitude = 0;
            
            DiveSite DS3 = new DiveSite();
            DS3.SiteName = "Paradise Pinnacle";
            DS3.City = "Captain Cook";
            DS3.StProv = "Hawaii";
            DS3.Country = "USA";
            DS3.Latitude = 19.484941;
            DS3.Longitude = -155.958732;
            DS3.Altitude = 0;
            
            DiveSite DS4 = new DiveSite();
            DS4.SiteName = "Au Au Crater";
            DS4.City = "Pu'uhonua O Honaunau Park";
            DS4.StProv = "Hawaii";
            DS4.Country = "USA";
            DS4.Latitude = 19.428509;
            DS4.Longitude = -155.931238;
            DS4.Altitude = 0;
            
            DiveSite DS5 = new DiveSite();
            DS5.SiteName = "Black Coral Forest";
            DS5.City = "Pu'uhonua O Honaunau Park";
            DS5.StProv = "Hawaii";
            DS5.Country = "USA";
            DS5.Latitude = 19.388199;
            DS5.Longitude = -155.921268;
            DS5.Altitude = 0;
            
            DiveSite DS6 = new DiveSite();
            DS6.SiteName = "Catacombs";
            DS6.City = "Pu'uhonua O Honaunau Park";
            DS6.StProv = "Hawaii";
            DS6.Country = "USA";
            DS6.Latitude = 19.379303;
            DS6.Longitude = -155.915243;
            DS6.Altitude = 0;
            
            DiveSite DS7 = new DiveSite();
            DS7.SiteName = "Blue Lake";
            DS7.City = "Cascade";
            DS7.StProv = "Idaho";
            DS7.Country = "USA";
            DS7.Latitude = 44.409112;
            DS7.Longitude = 116.134790;
            DS7.Altitude = 7322;
            
            DiveSite DS8 = new DiveSite();
            DS8.SiteName = "Lucky Peak";
            DS8.City = "Boise";
            DS8.StProv = "Idaho";
            DS8.Country = "USA";
            DS8.Latitude = 43.525157;
            DS8.Longitude = 116.054857;
            DS8.Altitude = 3054;
            
            dbContext.Add(DS1);
            dbContext.Add(DS2);
            dbContext.Add(DS3);
            dbContext.Add(DS4);
            dbContext.Add(DS5);
            dbContext.Add(DS6);
            dbContext.Add(DS7);
            dbContext.Add(DS8);
            dbContext.SaveChanges();

            return Redirect("displaylogs");
        }
        // public IActionResult Privacy()
        // {
        //     return View();
        // }

        // public JsonResult APICall(string SearchText)
        // {
        //     List<string> test = new List<string>()
        //     {
        //         "test",
        //         "testAgain"
        //     };
        //     HttpClient Http = new HtppClient()
        //     return Json(test);
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
