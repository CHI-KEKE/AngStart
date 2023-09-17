using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.S3Dto;

namespace API.Domain.IService
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(S3Obj s3obj, AWSCredentials aWSCredentials);
    }
}