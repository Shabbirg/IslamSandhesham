using IslamSandhesham.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IslamSandhesham.Controllers
{
    public class HomeController : Controller
    {
        private MyContext db = new MyContext();
        public ActionResult Index()
        {
            ViewBag.SectionNames = db.Sections.ToList();
           // ViewBag.SectionsList = new SelectList(db.Sections.ToList(), "id", "Description");
            ViewBag.Qna = db.Questions_Answers.Where(w=>w.Type==1).Take(25).OrderByDescending(w=>w.Key).ToList();
            ViewBag.Images = db.Images.ToList();
            return View(db.Buttons.ToList());
        }

        public ActionResult Admin()
        {
            bool res = isLoggedIn();
            if (!res)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Master", "Home", new { isLogin = false });
            }
        }
        public JsonResult Login(Admin logData)
        {
            Admin admin = db.Admin.OrderBy(w => w.id).FirstOrDefault();
           
            bool isValidUser = false;
            if (admin != null)
            {
                if (logData.UserId == admin.UserId && logData.Password == admin.Password)
                {
                    isValidUser = true;
                    admin.IsLoggedIn = true;
                    admin.LastLoginTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return Json(isValidUser, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Master()
        {
            bool res = isLoggedIn();
            ViewBag.isLogin = res;
            if (res)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
        }
        public bool isLoggedIn()
        {
            Admin admin = db.Admin.OrderBy(w => w.id).FirstOrDefault();
            bool isLoggedIn = false;
            isLoggedIn = admin.IsLoggedIn;
            if (isLoggedIn)
            {
                TimeSpan ts = DateTime.Now- (DateTime)admin.LastLoginTime ;
                if (ts.Days > 1 ||ts.Hours >1 ||ts.Minutes>45)
                {
                    isLoggedIn = false;
                    admin.IsLoggedIn = false;
                    db.SaveChanges();
                }
            }            
            return isLoggedIn;
        }
        public ActionResult Logout()
        {
            Admin admin = db.Admin.OrderBy(w => w.id).FirstOrDefault();
            if(admin  != null)
            {
                admin.IsLoggedIn = false;
                db.SaveChanges();
            }
            return RedirectToAction("Admin", "Home", new { isLogin = "***" });
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult QnA()
        {            
            return View(db.Questions_Answers.Where(w=>w.Type==1).ToList());
        }
        [HttpPost]
        public ActionResult QnA(Questions_Answers cnt)
        {
            Questions_Answers qna = new Questions_Answers();
            qna.Name = cnt.Name;
            qna.Email = cnt.Email;
            qna.Question = cnt.Question;
            qna.Date = DateTime.Now;
            qna.Type = cnt.Type;
            db.Questions_Answers.Add(qna);
            db.SaveChanges();
            return Json(JsonRequestBehavior.AllowGet, "");
        }
        public ActionResult Questions()
        {
            bool res = isLoggedIn();
            if (res)
            {
                return View(db.Questions_Answers.ToList().OrderByDescending(w => w.Key));
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
           
        }
        public JsonResult getQuestionData(int? id)
        {
            Questions_Answers qdata = db.Questions_Answers.Where(w => w.Key == id).FirstOrDefault();
            return Json(qdata,JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveAnswer(Questions_Answers data)
        {
            if (data.Key == 0)
            {
                return Json(new { Message = "Failed", JsonRequestBehavior.AllowGet });
            }
            Questions_Answers qn = db.Questions_Answers.Where(w => w.Key == data.Key).FirstOrDefault();
            qn.Answer = data.Answer;
            db.SaveChanges();
            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }
        #region Buttons
        public ActionResult Buttons(int? id)
        {
            bool res = isLoggedIn();
            if (res)
            {
                List<Buttons> btnList = db.Buttons.ToList();
                if (id != null)
                {
                    btnList = db.Buttons.Where(w => w.SectionId == id).ToList();
                }
                ViewBag.SectionsList = new SelectList(db.Sections.ToList(), "id", "Description");
                return View(btnList);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
        }
        [HttpPost]
        public JsonResult SaveButtonName(Buttons btn)
        {
            if (btn.Type == 1)
            {
                Buttons buttons = new Buttons()
                {
                    Name = btn.Name,
                    SectionId = btn.SectionId ==0?null:btn.SectionId,
                    ImageId=btn.ImageId
                };
                db.Buttons.Add(buttons);
                db.SaveChanges();
            }
            else if (btn.Type == 2)
            {
                Buttons button = db.Buttons.Where(w => w.id == btn.id).FirstOrDefault();
                button.Name = btn.Name;
                button.SectionId = btn.SectionId == 0 ? null : btn.SectionId;
                button.ImageId = btn.ImageId;
                // db.Buttons.AddOrUpdate(button);
                db.SaveChanges();

            }
            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }
        public JsonResult getBtnValues(int id)
        {
            Buttons btnObj = db.Buttons.Where(w => w.id == id).FirstOrDefault();
            return Json(btnObj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteButton(Buttons btn)
        {
            Buttons btnObj = db.Buttons.Where(w => w.id == btn.id).FirstOrDefault();
            db.Buttons.Remove(btnObj);
            db.SaveChanges();
            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }
        #endregion

        #region content
        public ActionResult Content(int? id)
        {
            bool res = isLoggedIn();
            if (res)
            {
                List<Content> contentList = db.Contents.ToList();
                if (id != null)
                {
                    contentList = db.Contents.Where(w => w.SectionId == id).ToList();
                }
                ViewBag.SectionsList = new SelectList(db.Sections.ToList(), "id", "Description");
                ViewBag.ButtonsList = new SelectList(db.Buttons.ToList(), "id", "Name");
                return View(contentList);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }            
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveContent(Content cnt)
        {
            if (cnt.Type == 1)
            {
                Content content = new Content()
                {
                    Button_Id = cnt.Button_Id,
                    Heading = cnt.Heading,
                    Description = cnt.Description,
                    SectionId = cnt.SectionId,
                    ImageId=cnt.ImageId,
                    Info = cnt.Info,
                    ArabicText=cnt.ArabicText
                };
                db.Contents.Add(cnt);
                db.SaveChanges();
            }
            else if (cnt.Type == 2)
            {
                Content content = db.Contents.Where(w => w.id == cnt.id).FirstOrDefault();
                content.Heading = cnt.Heading;
                content.Description = cnt.Description;
                content.Button_Id = cnt.Button_Id;
                content.Info = cnt.Info;
                content.SectionId = cnt.SectionId;
                content.ArabicText = cnt.ArabicText;
                content.ImageId = cnt.ImageId;
                db.SaveChanges();

            }
            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }

        public JsonResult getContentData(int id)
        {
            Content cnt = db.Contents.Where(w => w.id == id).FirstOrDefault();
            ViewBag.SectionsList = new SelectList(db.Sections.ToList(), "id", "Description");
            ViewBag.ButtonsList = new SelectList(db.Buttons.Where(w => w.SectionId == cnt.SectionId).ToList(), "id", "Name");
            return Json(cnt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteContent(Content content)
        {

            Content cnt = db.Contents.Where(w => w.id == content.id).FirstOrDefault();
            db.Contents.Remove(cnt);
            db.SaveChanges();
            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }
        #endregion

        public JsonResult getSectionBasedButton(int id)
        {
            IEnumerable<SelectListItem> btns = db.Buttons.Where(w => w.SectionId == id).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.Name
            }).ToList();
            return Json(btns, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getImageBasedButton(int id)
        {
            IEnumerable<SelectListItem> btns = db.Buttons.Where(w => w.ImageId == id).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.Name
            }).ToList();
            return Json(btns, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Info(int id)
        {
            List<Content> content = db.Contents.Where(w => w.Button_Id == id).ToList();
            return View(content);
        }

        public ActionResult Cards(int id)
        {
            List<Buttons> buttons = db.Buttons.Where(w => w.ImageId == id).ToList();
            if (id == 12)
            {
                return RedirectToAction("QnA");
            }
            return View(buttons);
        }

        public ActionResult CreateImage(int? id)
        {            
            return View();
        }
        public ActionResult UploadImage(Images img, HttpPostedFileBase file)
        {
            string date= DateTime.Now.ToString("ddMMyy_hhmmss"); 
            string fileName = "";
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    fileName = date + "_" + file.FileName;
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + fileName);
                    img.Path = fileName;
                }
                if(img.id > 0)
                {
                    Images im = db.Images.Where(w => w.id == img.id).FirstOrDefault();
                    if(im != null)
                    {
                        string imgPath = Request.MapPath("~/Images/" + im.Path);
                        if ((System.IO.File.Exists(imgPath)))
                        {
                            System.IO.File.Delete(imgPath);
                        }


                        im.Path = fileName;
                        im.Description = img.Description;
                        im.Type = img.Type;
                        db.SaveChanges();
                    }
                   
                }
                else
                {
                    db.Images.Add(img);
                    db.SaveChanges();
                }
                return RedirectToAction("Images");
            }
            return View(img);
        }
        public ActionResult Images()
        {
            return View(db.Images.ToList());
        }
        public JsonResult GetImageData(int id)
        {
            Images img = db.Images.Where(w => w.id == id).FirstOrDefault();
            return Json(img, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteImage(Images img)
        {
            Images imgObj = db.Images.Where(w => w.id == img.id).FirstOrDefault();
            string filename = imgObj.Path;
            db.Images.Remove(imgObj);
            db.SaveChanges();

            string imgPath = Request.MapPath("~/Images/" + filename);
            if ((System.IO.File.Exists(imgPath)))
            {
                System.IO.File.Delete(imgPath);
            }


            return Json(new { Message = "Success", JsonRequestBehavior.AllowGet });
        }

    }
}