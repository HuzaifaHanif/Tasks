using AutoMapper;
using ServiceContracts;
using Services;
using Task8.Models.Employees;
using Task8.Models.Kafka;
using Task8.Models.RabbitMq;

namespace Task8.Data
{
    public class MappingProfile : Profile
    {
       public MappingProfile() 
       {
            CreateMap<UpdateEmployee, Employee>();
            CreateMap<ProducerKafka, Kafka>();
            CreateMap<ProducerRabbitMQ, RabbitMq>();
            CreateMap<AddEmployee, Employee>();
            
       }
    }
}


