using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;


namespace Services.Services
{
    public class TransactionService : ServiceBase, ITransactionService
    {


   

        public TransactionService(IMapper mapper,IRepositoryManager repositoryManager):base( repositoryManager,mapper) {
        
      
         
        }


    
    }
}
