using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayService.PayPing;
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


            // ساخت کد پرداخت به منظور هدایت به درگاه بانکی
            var payCode = await RequestHelper.CallApi<PayPing_PaymentResult>(_PAYPING_API["PAYMENT_URL"], model, _PAYPING_API["Key"]);

            _repositoryManager.ITransactionRepository.Create(new() { 
                Amount = dto.Amount, 
                BillAmount = dto.BillingAmount, 
                Id = dto.TransactionId,
                CreatedDate = DateTime.Now,
                refId = payCode.Code,
                ServiceType = Entities.Enum.PayServiceType.PayPing,
                Status = Entities.Enum.PayStatusType.InProgress ,
                
            });


            //هدایت کاربر به درگاه بانک
            return _PAYPING_API["GOTOIPG_URL"] + payCode.Code;



        }

    }
}
