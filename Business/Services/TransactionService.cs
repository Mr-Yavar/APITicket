using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayingDto;
using Entities.DataTransferObjects.PayService.PayPing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Security.Cryptography.X509Certificates;


namespace Services.Services
{
    public class TransactionService : ServiceBase, ITransactionService
    {


        private Dictionary<string, string?> _PAYPING_API { get; set; }

        public TransactionService(IMapper mapper, IRepositoryManager repositoryManager, IConfiguration configuration) : base(repositoryManager, mapper)
        {

            _PAYPING_API = configuration.GetSection("Configuration:PAYPAING_API").AsEnumerable().ToDictionary(x => x.Key, x => x.Value);

        }

        public async Task<string> CreatePayment(BillingDto dto)
        {
            PayPing_RequestPayment model = new PayPing_RequestPayment()
            {
                Amount = (int)dto.BillingAmount,
                PayerName = dto.FullName,
                ReturnUrl = dto.ReturnUrl,
                Description = dto.FullName + " بابت کنسرت " + DateTime.Now.ToString("yyyy/MM/dd HH:mm"),
                ClientRefId = dto.TransactionId.ToString()
            };

            var t = _PAYPING_API["Configuration:PAYPAING_API:PAYMENT_URL"];
            //در صورت وجود تراکنش های قبلی آنها را تعیین وضعیت کنیم
            var preTranactions = _repositoryManager.ITransactionRepository.FindByCondition(x => x.TempReservedId == dto.TempReservationId, true).ToList();
            foreach (var item in preTranactions)
            {
                item.Status = (item.Status == Entities.Enum.PayStatusType.InProgress) ? Entities.Enum.PayStatusType.Refuted : item.Status;

                _repositoryManager.ITransactionRepository.Update(item);
            }
            _repositoryManager.ITransactionRepository.SaveChanges();

            // ساخت کد پرداخت به منظور هدایت به درگاه بانکی
            var payCode = await RequestHelper.CallApi<PayPing_PaymentResult>(_PAYPING_API["Configuration:PAYPAING_API:PAYMENT_URL"], model, _PAYPING_API["Configuration:PAYPAING_API:Key"]);

            _repositoryManager.ITransactionRepository.Create(new() { 
                Amount = dto.Amount, 
                BillAmount = dto.BillingAmount, 
                Id = dto.TransactionId,
                CreatedDate = DateTime.Now,
                refId = payCode.Code,
                ServiceType = Entities.Enum.PayServiceType.PayPing,
                Status = Entities.Enum.PayStatusType.InProgress ,
                TempReservedId = dto.TempReservationId
            });

            _repositoryManager.ITransactionRepository.SaveChanges();
            //هدایت کاربر به درگاه بانک
            return _PAYPING_API["Configuration:PAYPAING_API:GOTOIPG_URL"] + payCode.Code;



        }

        public async Task<ResponseVerifyDto> VerifyPayment(toVerifyDto dto)
        {
            var verifyBody = new RequestVerify
            {
                Amount = ((int)dto.BillAmount.Value), // مبلغ به توامن می باشد
                RefId = dto.refId,
                
            };





            // اگر وریفای با موفقیت انجام پذیرد، تنها وضعیت 200 را برگشت می دهد و حاوی هیچ دیتایی نیست
            // اما اگر به هر دلیلی ادامه فرایند وریفای مقدور نباشد، 400 برگشت می دهد به همراه یک متن خطا
            var result = await RequestHelper.CallApi<ResponseVerifyDto>(_PAYPING_API["VERIFY_URL"], verifyBody, _PAYPING_API["Key"]);

            var transaction  = await _repositoryManager.ITransactionRepository.FindByCondition(x => x.Id == dto.clientrefid, true).FirstOrDefaultAsync();

           

            if (result.Message != null || result.Message == string.Empty)
            {
                transaction.Status = Entities.Enum.PayStatusType.Verified;
                
            }
            else
            {
                transaction.Status = Entities.Enum.PayStatusType.Refuted;
            }

            _repositoryManager.ITransactionRepository.Update(transaction);
            _repositoryManager.ITransactionRepository.SaveChanges();
            
            return result;
        }


        public async Task CancelTransactionsByTempReservation(Guid TempReservationId)
        {
            var preTranactions = _repositoryManager.ITransactionRepository.FindByCondition(x => x.TempReservedId == TempReservationId, true).ToList();
            foreach (var item in preTranactions)
            {
                item.Status = (item.Status == Entities.Enum.PayStatusType.InProgress) ? Entities.Enum.PayStatusType.Refuted : item.Status;

                _repositoryManager.ITransactionRepository.Update(item);
            }
            _repositoryManager.ITransactionRepository.SaveChanges();
        }
    }
}
