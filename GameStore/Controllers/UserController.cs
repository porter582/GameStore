﻿using System;
using GameStore.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.ApplicationUser.GetAll() });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking / Unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked out, unlock
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                //lock user
                objFromDb.LockoutEnd = DateTime.Now.AddYears(100);
            }
            //_unitOfWork.ApplicationUser.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Lock/Unlock Successful" });

        }
    }
}