using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Ledger.Public.Models
{
    public interface IReceiptRepository
    {
        void Save(ReceiptViewModel model);
        int GetCountTransactionsWaiting();
        List<ReceiptViewModel> GetAllReceipts();
        void DeleteReceipt(Guid id);
    }

    public class ReceiptRepository : IReceiptRepository
    {
        public void Save(ReceiptViewModel model)
        {
            model.Id = Guid.NewGuid();
            var targetFolder = HttpContext.Current.Server.MapPath("~/Transactions");
            var filename = string.Format("Transaction{0}.json", model.Id);
            var targetPath = Path.Combine(targetFolder, filename);

            var json = JsonConvert.SerializeObject(model);

            File.WriteAllText(targetPath, json);
        }

        public int GetCountTransactionsWaiting()
        {
            var targetFolder = HttpContext.Current.Server.MapPath("~/Transactions");
            return Directory.GetFiles(targetFolder).Count();
        }

        public List<ReceiptViewModel> GetAllReceipts()
        {
            var targetFolder = HttpContext.Current.Server.MapPath("~/Transactions");
            var fullFilenamePaths = Directory.GetFiles(targetFolder);
            var filecontents = fullFilenamePaths.Select(File.ReadAllText);
            return filecontents.Select(GetReceiptObject).ToList();
        }

        public void DeleteReceipt(Guid id)
        {
            var targetFolder = HttpContext.Current.Server.MapPath("~/Transactions");
            var filename = string.Format("Transaction{0}.json", id);
            var targetPath = Path.Combine(targetFolder, filename);
            if(!File.Exists(targetPath))
                throw new FileNotFoundException("Unable to delete because file wasn't found");
            File.Delete(targetPath);
        }

        ReceiptViewModel GetReceiptObject(string json)
        {
            return (ReceiptViewModel)JsonConvert.DeserializeObject(json, typeof(ReceiptViewModel));
        }
    }
}