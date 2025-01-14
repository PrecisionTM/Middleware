using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Precision.API.BAL.CommonServices.Interfaces;
using Precision.API.BAL.LabServices.Interfaces;
using Precision.API.BAL.PharmacyServices.Interfaces;
using Precision.API.Model.Common;
using Precision.API.Model.Enums;
using Precision.API.Model.LabInfo;
using System.Net;

namespace Precision.API.BAL.CommonServices
{
    public class BaseService : IBaseService
    {
        private readonly IHttpService _httpService;
        private readonly ICommonMethods _commonMethods;
        private readonly IOrderService _orderService;
        private readonly IPrescriptionService _prescriptionService;

        public BaseService(IHttpService httpService, ICommonMethods commonMethods, IOrderService orderService, IPrescriptionService prescriptionService)
        {
            _httpService = httpService;
            _commonMethods = commonMethods;
            _orderService = orderService;
            _prescriptionService = prescriptionService;
        }

        public async Task<HttpResponseMessage> SavePharmacy(string processedFilePath, LabCredential credential, Actions action, object order, string id = "")
        {
            await _commonMethods.CreateOrAppendFile(processedFilePath, string.Concat("--- Save ", action.ToString(), " Started ---"));

            HttpResponseMessage? response = null;

            string _str = action switch
            {
                Actions.PharmacyCreateRequest => await _prescriptionService.GenerateJson((Precision.API.Model.PharmacyInfo.PrescriptionOrder)order),
                Actions.PharmacyCancelRequest => await _prescriptionService.GenerateCancelRequestJson(processedFilePath, id),
                Actions.PharmacyRefillRequest => await _prescriptionService.GenerateRefillJson((Precision.API.Model.PharmacyInfo.RefillOrder)order)
            };

            response = await _httpService.PostRequest(credential, action.ToString(), _str, processedFilePath);

            return response;
        }

        public async Task<HttpResponseMessage> Get(string processedFilePath, LabCredential credential, string id, Actions action)
        {
            await _commonMethods.CreateOrAppendFile(processedFilePath, string.Concat("--- Get ", action.ToString(), " Started ---"));

            HttpResponseMessage? response = await _httpService.GetRequest(credential, action.ToString(), processedFilePath, id);

            return response;
        }
    }
}