using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetCsharp.DAL
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}
