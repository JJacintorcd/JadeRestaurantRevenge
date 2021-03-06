﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Recodme.Rd.JadeRest.BusinessLayer.BObjects.RestaurantBO;
using Recodme.Rd.JadeRest.BusinessLayer.BObjects.UserBO;
using Recodme.Rd.JadeRest.DataAccessLayer.Seeders;
using Recodme.Rd.JadeRest.DataLayer.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JadeTests.TestsAsync
{
    [TestClass]
    public class StaffRecordAsyncTests
    {
        [TestMethod]
        public void TestCreateAndReadStaffRecordAsync()
        {
            RestaurantSeeder.SeedCountries();
            var bo = new StaffRecordBusinessObject();
            var bop = new PersonBusinessObject();
            var pe1 = bop.List().Result.First();
            var bor = new RestaurantBusinessObject();
            var rs1 = bor.List().Result.First();
            var dr = new StaffRecord(DateTime.Parse("2020/05/05"), DateTime.Parse("2020/06/06"), pe1.Id, rs1.Id);
            var resCreate = bo.CreateAsync(dr).Result;
            var resGet = bo.ReadAsync(dr.Id).Result;
            Assert.IsTrue(resCreate.Success && resGet.Success && resGet.Result != null);
        }

        [TestMethod]
        public void TestListStaffRecordAsync()
        {
            RestaurantSeeder.SeedCountries();
            var bo = new StaffRecordBusinessObject();
            var resList = bo.ListAsync().Result;
            Assert.IsTrue(resList.Success && resList.Result.Count == 1);
        }

        [TestMethod]
        public void TestUpdateStaffRecordAsync()
        {
            RestaurantSeeder.SeedCountries();
            var bo = new StaffRecordBusinessObject();
            var resList = bo.ListAsync().Result;
            var item = resList.Result.FirstOrDefault();
            item.BeginDate = DateTime.Parse("2020/06/05");
            var resUpdate = bo.UpdateAsync(item).Result;
            resList = bo.ListUnDeletedAsync().Result;
            Assert.IsTrue(resList.Success && resUpdate.Success && resList.Result.First().BeginDate == DateTime.Parse("2020/06/05"));
        }

        [TestMethod]
        public void TestDeleteStaffRecordAsync()
        {
            RestaurantSeeder.SeedCountries();
            var bo = new StaffRecordBusinessObject();
            var resList = bo.ListAsync().Result;
            var resDelete = bo.DeleteAsync(resList.Result.First().Id).Result;
            resList = bo.ListUnDeletedAsync().Result;
            Assert.IsTrue(resDelete.Success && resList.Success && resList.Result.Count == 0);
        }
    }
}
