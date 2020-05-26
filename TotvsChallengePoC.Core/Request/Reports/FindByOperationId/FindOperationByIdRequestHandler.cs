﻿using EasyCaching.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using TotvsChallengePoC.Data.Contracts;
using TotvsChallengePoC.Data.Models;

namespace TotvsChallengePoC.Core.Request.Reports.FindByOperationId
{
    public class FindOperationByIdRequestHandler : IRequestHandler<FindOperationByIdRequest, OperationInfoModelResponse>
    {
        private readonly IReportRepository reportRepository;
        private readonly IEasyCachingProvider cachingProvider;
        private readonly IEasyCachingProviderFactory cachingProviderFactory;
        private readonly ILogger logService;

        public FindOperationByIdRequestHandler(IReportRepository reportRepository, IEasyCachingProviderFactory cachingProviderFactory, ILogger logService, IConfiguration _config)
        {
            this.reportRepository = reportRepository ?? throw new System.ArgumentNullException(nameof(reportRepository));
            this.cachingProviderFactory = cachingProviderFactory ?? throw new ArgumentNullException(nameof(cachingProviderFactory));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
            this.cachingProvider = this.cachingProviderFactory.GetCachingProvider(_config["Redis:Name"]);
        }

        public async Task<OperationInfoModelResponse> Handle(FindOperationByIdRequest request, CancellationToken cancellationToken)
        {
            OperationInfoModelResponse result = new OperationInfoModelResponse();
            try
            {
                result = await reportRepository.FindOperationInfoById(request.OperationId);
                await SetIntoRedis(request, result);
                return result;
            }
            catch (Exception ex)
            {
                logService.Error(ex, $"Operation Id failed querying database with parameters: {request.OperationId}");
                throw;
            }
        }

        private async Task SetIntoRedis(FindOperationByIdRequest request, OperationInfoModelResponse result)
        {
            try
            {
                await RegisterCache(request, result);
            }
            catch (Exception ex)
            {
                await Task.Run(() => logService.Error(ex, "Redis isnt avaiable"));
            }
        }

        private Task RegisterCache(FindOperationByIdRequest request, OperationInfoModelResponse result)
        {
            return cachingProvider.SetAsync
                            ("OperationId :" + request.OperationId,
                            JsonConvert.SerializeObject(result),
                            TimeSpan.FromDays(7));
        }

    }
}
