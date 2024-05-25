using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRWebApp.Models
{
    [Table("MongoDBPersonal")]
    public partial class MongoDBPersonal
    {
        [BsonElement("employeeId")]
        public decimal employeeId { get; set; }

        //[BsonIgnore]
        //public string firstName { get; set; } 

        //[BsonIgnore]
        //public string lastName { get; set; }  
        
        [BsonElement("vacationDays")]
        public int vacationDays { get; set; }

        [BsonElement("paidToDate")]
        public int paidToDate { get; set; }

        [BsonElement("paidLastYear")]
        public int paidLastYear { get; set; }

        [BsonElement("payRate")]
        public int payRate { get; set; }

        [BsonElement("payRateId")]
        public int payRateId { get; set; }

        [BsonIgnore]
        public BsonDateTime createdAt {  get; set; }

        [BsonIgnore]
        public BsonDateTime updatedAt { get; set;}

    }
}