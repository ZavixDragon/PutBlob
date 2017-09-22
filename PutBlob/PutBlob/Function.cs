using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace PutBlob
{
    public class Function
    {
        public async Task<Response> FunctionHandler(Request request, ILambdaContext context)
        {
            var authorization = await new AuthorizationRequest(request.Username, request.Password).Authorize();
            if (!authorization.IsSuccess)
                return authorization;
            using (var client = new AmazonS3Client())
            {
                try
                {
                    using (var stream = new MemoryStream(request.Blob))
                    {
                        var putRequest =
                            new PutObjectRequest {BucketName = request.Bucket, Key = request.Key, InputStream = stream};
                        await client.PutObjectAsync(putRequest);
                    }
                    return new Response();
                }
                catch (AmazonS3Exception ex)
                {
                    return new Response { ErrorMessage = ex.Message };
                }
            }
        }
    }
}
