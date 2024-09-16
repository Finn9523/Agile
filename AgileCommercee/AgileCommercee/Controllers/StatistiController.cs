﻿using AgileCommercee.Data;
using AgileCommercee.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgileCommercee.Controllers
{
    public class StatistisController : Controller
    {
        private readonly AgileStoreContext _context;
        private readonly IDictionary<string, IStatisticsStrategy1> _strategies;

        public StatistisController(AgileStoreContext context)
        {
            _context = context;

            // Khởi tạo dictionary các chiến lược
            _strategies = new Dictionary<string, IStatisticsStrategy1>
        {
            { "category", new CategoryStatisticsStrategy1() },
                { "supplier", new SupplierStatisticsStrategy1() },
            { "supplierCategory", new SupplierCategoryStatisticsStrategy1() }
        };
        }

        [HttpGet("/Statistic/ByCategoryV2")]
        public IActionResult StatisticByCategory()
        {
            var strategy = _strategies["category"];
            var data = strategy.GetStatistics(_context);
            return View(data);
        }

        [HttpGet("/Statistic/BySupplierV2")]
        public IActionResult StatisticBySupplier()
        {
            var strategy = _strategies["supplier"];
            var data = strategy.GetStatistics(_context);
            return View(data);
        }

        [HttpGet("/Statistic/BySupplierCategoryV2")]
        public IActionResult StatisticBySupplierCategory()
        {
            var strategy = _strategies["supplierCategory"];
            var data = strategy.GetStatistics(_context);
            return View(data);
        }
    }
}
