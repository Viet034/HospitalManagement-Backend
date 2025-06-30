using System;
using System.Collections.Generic;

namespace SWP391_SE1914_ManageHospital.Models.Entities
{
    public class Payment : BaseEntity
    {
        public string Code { get; set; }                  
        public string Name { get; set; }                    
        public DateTime PaymentDate { get; set; }      
        public DateTime PaidAt { get; set; }             
        public decimal Amount { get; set; }              
        public string PaymentMethod { get; set; }          
        public string Payer { get; set; }                
        public string Notes { get; set; }               
        public string TransactionId { get; set; }       
        public string Status { get; set; }               
        public string CreateBy { get; set; }               
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; } 
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<Payment_Invoice> Payment_Invoices { get; set; } = new List<Payment_Invoice>();
    }
}
