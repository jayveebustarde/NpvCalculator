using Microsoft.AspNetCore.Mvc;
using NpvCalculator.Controllers;
using NpvCalculator.Models;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NpvCalculator.Test
{
    [TestFixture]
    public class CalculationController_PostShould
    {
        private CalculationController _calculationCtrl;

        [SetUp]
        public void Setup() 
        {
            _calculationCtrl = new CalculationController();
        }

        [Test]
        public async Task Post_RequestIsNull_ReturnBadRequest()
        {
            //Act
            var result = await _calculationCtrl.Post(null);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Post_CashflowIsNull_ReturnBadRequest()
        {
            //Arrange
            NpvRequestModel model = new()
            {
                CashFlows = null
            };

            //Act
            var result = await _calculationCtrl.Post(model);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Post_CashflowIsEmpty_ReturnBadRequest()
        {
            //Arrange
            NpvRequestModel model = new()
            {
                CashFlows = Array.Empty<decimal>()
            };

            //Act
            var result = await _calculationCtrl.Post(model);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Post_UpperBoundIsLessthanLowerBound_ReturnBadRequest()
        {
            //Arrange
            NpvRequestModel model = new()
            {
                CashFlows = new decimal[1],
                UpperBoundRate = 5,
                LowerBoundRate = 6
            };

            //Act
            var result = await _calculationCtrl.Post(model);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task Post_IncrementIsZero_ReturnBadRequest()
        {
            //Arrange
            NpvRequestModel model = new()
            {
                CashFlows = new decimal[1],
                Increment = 0
            };

            //Act
            var result = await _calculationCtrl.Post(model);

            //Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}