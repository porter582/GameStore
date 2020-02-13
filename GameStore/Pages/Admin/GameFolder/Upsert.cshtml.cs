using System;
using System.IO;
using Game.Models;
using Game.Models.ViewModels;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameStore.Pages.Admin.GameFolder
{
    public class UpsertModel : PageModel
    {

        private readonly IUnitOfWork _unitofWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UpsertModel(IUnitOfWork unitofWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitofWork = unitofWork;
            _hostingEnvironment = hostingEnvironment;
        }

        //binds the model to the page
        [BindProperty]
        public GameVM GameObj { get; set; }
        public IActionResult OnGet(int? id) ///IActionResult return type is page, obj
        {
            GameObj = new GameVM()
            {
                GenreList =
                _unitofWork.Genre.GetGenreListForDropDown(),
                RatingList =
                _unitofWork.Rating.GetRatingListForDropDown(),

                Game = new GameModel()
            };

            if (id != null) //edit
            {
                GameObj.Game = _unitofWork.GameObj.GetFirstOrDefault(u => u.Id == id);
                if (GameObj == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            //find root path wwwroot
            string webRootPath = _hostingEnvironment.WebRootPath;
            //Grab the file(s) from the form
            var files = HttpContext.Request.Form.Files;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (GameObj.Game.Id == 0) //new menu item
            {
                //rename file user submits for image
                string fileName = Guid.NewGuid().ToString();
                //upload file to the path
                var uploads = Path.Combine(webRootPath, @"images\gameImages");
                //preserve our extension
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream); //files variable comes from the razor page files id
                }

                GameObj.Game.Image = @"\images\gameImages\" + fileName + extension;

                _unitofWork.GameObj.Add(GameObj.Game);
            }
            else //else we edit
            {
                var objFromDb =
                    _unitofWork.GameObj.Get(GameObj.Game.Id);
                //checks if there are files submitted
                if (files.Count > 0)
                {
                    //rename file user submits for image
                    string fileName = Guid.NewGuid().ToString();
                    //upload file to the path
                    var uploads = Path.Combine(webRootPath, @"images\gameImages");
                    //preserve our extension
                    var extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var filestream = new FileStream(Path.Combine(uploads, fileName, extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    GameObj.Game.Image = @"\images\gameImages\" + fileName + extension;
                }
                else
                {
                    GameObj.Game.Image = objFromDb.Image;
                }
                _unitofWork.GameObj.Update(GameObj.Game);
            }
            _unitofWork.Save();
            return RedirectToPage("./Index");
        }
    }
}