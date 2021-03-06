﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Recodme.Rd.JadeRest.BusinessLayer.BObjects.MenuBO;
using Recodme.Rd.JadeRest.WebApi.Models.MenuViewModels;

namespace Recodme.Rd.JadeRest.WebApi.Controllers.Web.MenuControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MealsController : Controller
    {
        private readonly MealBusinessObject _bo = new MealBusinessObject();
        public async Task<IActionResult> Index()
        {
            var listOperation = await _bo.ListUnDeletedAsync();
            if (!listOperation.Success) return View("Error", new ErrorViewModel() { RequestId = listOperation.Exception.Message });
            var lst = new List<MealViewModel>();
            foreach (var item in listOperation.Result)
            {
                lst.Add(MealViewModel.Parse(item));
            }
            return View(lst);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var getOperation = await _bo.ReadAsync((Guid)id);
            if (!getOperation.Success) return View("Error", new ErrorViewModel() { RequestId = getOperation.Exception.Message });
            if (getOperation.Result == null) return NotFound();
            var vm = MealViewModel.Parse(getOperation.Result);
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, StartingHours, EndingHours")]MealViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var ds = vm.ToMeal();
                var createOperation = await _bo.CreateAsync(ds);
                if (!createOperation.Success) return View("Error", new ErrorViewModel() { RequestId = createOperation.Exception.Message });
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var getOperation = await _bo.ReadAsync((Guid)id);
            if (!getOperation.Success) return View("Error", new ErrorViewModel() { RequestId = getOperation.Exception.Message });
            if (getOperation.Result == null) return NotFound();
            var vm = MealViewModel.Parse(getOperation.Result);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name, StartingHours, EndingHours")]DietaryRestrictionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var getOperation = await _bo.ReadAsync(id);
                if (!getOperation.Success) return View("Error", new ErrorViewModel() { RequestId = getOperation.Exception.Message });
                if (getOperation.Result == null) return NotFound();
                var result = getOperation.Result;
                result.Name = vm.Name;
                var updateOperation = await _bo.UpdateAsync(result);
                if (!updateOperation.Success) return View("Error", new ErrorViewModel() { RequestId = getOperation.Exception.Message });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var deleteOperation = await _bo.DeleteAsync((Guid)id);
            if (!deleteOperation.Success) return View("Error", new ErrorViewModel() { RequestId = deleteOperation.Exception.Message });
            return RedirectToAction(nameof(Index));
        }
    }
}