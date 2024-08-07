using AutoMapper;
using Contracts.IPaymentGateway;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.DataTransferObjects.PayingDto;
using Entities.DataTransferObjects.PayService.PayPing;
using Entities.Enum;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Security.Cryptography.X509Certificates;


namespace Services.Services
{
    public class TransactionService : ServiceBase, ITransactionService
    {


        private Dictionary<string, string?> _PAYPING_API { get; set; }
        private IPaymentFactory _paymentFactory { get; set; }
        public TransactionService(IMapper mapper, IRepositoryManager repositoryManager, IConfiguration configuration,IPaymentFactory paymentFactory) : base(repositoryManager, mapper)
        {

            _PAYPING_API = configuration.GetSection("Configuration:PAYPAING_API").AsEnumerable().ToDictionary(x => x.Key, x => x.Value);
            _paymentFactory = paymentFactory;
        }

        public async Task<string> CreatePayment(BillingDto dto)
        {


         

            //در صورت وجود تراکنش های قبلی آنها را تعیین وضعیت کنیم
            var preTranactions = _repositoryManager.ITransactionRepository.FindByCondition(x => x.TempReservedId == dto.TempReservationId, true).ToList();
            foreach (var item in preTranactions)
            {
                item.Status = (item.Status == Entities.Enum.PayStatusType.InProgress) ? Entities.Enum.PayStatusType.Refuted : item.Status;

                _repositoryManager.ITransactionRepository.Update(item);
            }
            _repositoryManager.ITransactionRepository.SaveChanges();

            
            toPayResponse NewPaymentResponse = new()
            {
                payCode = "-",
                PayUrl = "",
                Amount = dto.BillingAmount,
            };

            if (dto.PayService != PayServiceType.NoService)
            {
                IPaymentGateway paymentService = _paymentFactory.GetInstanceByType(dto.PayService);
                NewPaymentResponse = await paymentService.InitilizePayment(dto);
            }

            if (NewPaymentResponse == null)
                return "";

            _repositoryManager.ITransactionRepository.Create(new() { 
                Amount = dto.Amount, 
                BillAmount = dto.BillingAmount, 
                Id = dto.TransactionId,
                CreatedDate = DateTime.Now,
                refId = NewPaymentResponse.payCode,
                ServiceType = dto.PayService,
                Status = Entities.Enum.PayStatusType.InProgress ,
                TempReservedId = dto.TempReservationId
            });

            _repositoryManager.ITransactionRepository.SaveChanges();
            //هدایت کاربر به درگاه بانک
            return NewPaymentResponse.PayUrl;



        }

        public async Task<PayStatusType> VerifyPayment(toVerifyDto dto, Transactions transaction)
        {

            if (transaction.Id != dto.clientrefid)
                return PayStatusType.Refuted;
             

            bool verifyResponse = true;
            if (dto.PayService != PayServiceType.NoService)
            {
                IPaymentGateway paymentService = _paymentFactory.GetInstanceByType(dto.PayService);
                 verifyResponse = await paymentService.VerifyPayment(dto);
            }


            if (verifyResponse)
            {
                transaction.Status = Entities.Enum.PayStatusType.Verified;
                
            }
            else
            {
                transaction.Status = Entities.Enum.PayStatusType.Refuted;
            }

            _repositoryManager.ITransactionRepository.Update(transaction);
            _repositoryManager.ITransactionRepository.SaveChanges();

            return transaction.Status;
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
