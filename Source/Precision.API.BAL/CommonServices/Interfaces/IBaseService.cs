﻿using Precision.API.Model.Common;
using Precision.API.Model.Enums;
using Precision.API.Model.LabInfo;

namespace Precision.API.BAL.CommonServices.Interfaces
{
    public interface IBaseService
    {
        Task<HttpResponseMessage> SavePharmacy(Precision.API.Model.PharmacyInfo.PrescriptionOrder order, string processedFilePath, LabCredential credential, Actions action, string id = "");
        Task<HttpResponseMessage> Get(string processedFilePath, LabCredential credential, string id, Actions action);
    }
}
