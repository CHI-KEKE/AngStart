using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.S3Dto
{
    public class AWSCredentials
    {
        public string AwsKey { get; set; } ="";
        public string AwsSecretKey { get; set; } ="";
    }
}