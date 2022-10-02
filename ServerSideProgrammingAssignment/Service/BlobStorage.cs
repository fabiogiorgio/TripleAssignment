using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ServerSideProgrammingAssignment.Service;

public class BlobStorage
{
    private string _connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
    private string _containerName;
    private BlobContainerClient _client;

    public BlobStorage(string containerName)
    {
        _containerName = containerName;
        _client = new(_connection, _containerName);
    }

    public async Task<BlobContentInfo> UploadBlobAsync(byte[] blobBytes, string blobName) =>
        await UploadBlobAsync(new MemoryStream(blobBytes), blobName);
    
    public async Task<BlobContentInfo> UploadBlobAsync(Stream blobByteStream, string blobName)
    {
        BlobClient client = _client.GetBlobClient(blobName);
        var response = await client.UploadAsync(blobByteStream);
        return response.Value;
    }

    public async Task<bool> CheckIfBlobExistsAsync(string blobName)
    {
        BlobClient client = _client.GetBlobClient(blobName);
        return (await client.ExistsAsync()).Value;
    }

    public async Task<byte[]> DownloadBlobAsync(string blobName)
    {
        BlobClient client = _client.GetBlobClient(blobName);
        var response = await client.DownloadContentAsync();
        return response.Value.Content.ToArray();
    }
}