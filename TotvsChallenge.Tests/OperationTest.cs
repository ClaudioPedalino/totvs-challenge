﻿using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TotvsChallengePoC.Core.Request.Buy;
using TotvsChallengePoC.Core.Request.Buy.Model;
using TotvsChallengePoC.Data.Contracts;
using TotvsChallengePoC.Data.Repositories;
using TotvsChallengePoC.Tests.MockRepository;

namespace TotvsChallenge.Tests
{
    public class OperationTest
    {
        Mock<IOperationRepository> operationRepository;
        Mock<IClientRepository> clientRepository;
        CancellationToken cancellation;
        BuyRequest request;
        Mock<ILogger> logservice;

        [SetUp]
        public void Setup()
        {
            operationRepository = new OperationRepositoryMock().operationRepository;
            clientRepository = new ClientRepositoryMock().clientRepository;
            cancellation = new CancellationToken();
            request = new BuyRequest() { ClientId = "3FA85F64-5717-4562-B3FC-2C963F66AFA2", ClientPaymentAmount = 763.24M, PaymentType = 1, Products = new List<Product>() { new Product() { Amount = 33.09M, Id = 1 } } };
            logservice = new Mock<ILogger>();
        }

        [Test]
        public async Task holi()
        {
            BuyRequestHandler service = new BuyRequestHandler(operationRepository.Object, clientRepository.Object, logservice.Object);
            var response = await service.Handle(request, cancellation);

            var model = new ChangeModelResponse();

            Assert.Equals(response, model);
        }

    }
}

