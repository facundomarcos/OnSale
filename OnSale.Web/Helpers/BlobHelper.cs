using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    //implementa la interface IBlobHelper
    //y los metodos
    public class BlobHelper : IBlobHelper
    {
        private readonly CloudBlobClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
            //obtiene las llaves desde el json
            string keys = configuration["Blob:ConnectionString"];
            //conecta con azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            //crea el cliente blob 
            _blobClient = storageAccount.CreateCloudBlobClient();
        }
        //containerName son los contenedores en azure
        public async Task<Guid> UploadBlobAsync(byte[] file, string containerName)
        {
            MemoryStream stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containerName)
        {
            //obtenemos el stream del File
            Stream stream = File.OpenRead(image);
            //que suba el stream a ese container
            //y llama al metodo privado
            return await UploadStreamAsync(stream, containerName);
        }
       

        public async Task<Guid> UploadBlobAsync(Stream stream, string containerName)
        {
            return await UploadStreamAsync(stream, containerName);
        }
        //los metodos privados van a lo ultimo
        //este metodo es llamado por los 3 de arriba
        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName)
        {
            //Guid genera un codigo unico para que no se repitan los nombres de las imagenes
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            //y lo sube al container
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }
    }

}
