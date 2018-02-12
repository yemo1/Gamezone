using System;

namespace GameZone.VIEWMODEL
{
    public class Data2
    {
        public string responsecode { get; set; }
        public string responsemessage { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public object phone { get; set; }
        public string fullName { get; set; }
        public object customertoken { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int AccountId { get; set; }
    }

    public class ChargeToken 
    {
        public string user_token { get; set; }
        public string embed_token { get; set; }
    }

    public class Tx
    {
        public int id { get; set; }
        public string txRef { get; set; }
        public string orderRef { get; set; }
        public string flwRef { get; set; }
        public string redirectUrl { get; set; }
        public string device_fingerprint { get; set; }
        public object settlement_token { get; set; }
        public string cycle { get; set; }
        public int amount { get; set; }
        public int charged_amount { get; set; }
        public double appfee { get; set; }
        public int merchantfee { get; set; }
        public int merchantbearsfee { get; set; }
        public string chargeResponseCode { get; set; }
        public string raveRef { get; set; }
        public string chargeResponseMessage { get; set; }
        public string authModelUsed { get; set; }
        public string currency { get; set; }
        public string IP { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string vbvrespmessage { get; set; }
        public string authurl { get; set; }
        public string vbvrespcode { get; set; }
        public object acctvalrespmsg { get; set; }
        public object acctvalrespcode { get; set; }
        public string paymentType { get; set; }
        public object paymentPlan { get; set; }
        public object paymentPage { get; set; }
        public string paymentId { get; set; }
        public string fraud_status { get; set; }
        public string charge_type { get; set; }
        public int is_live { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int customerId { get; set; }
        public int AccountId { get; set; }
        public Customer customer { get; set; }
        public ChargeToken chargeToken { get; set; }
    }

    public class Data
    {
        public Data2 data { get; set; }
        public Tx tx { get; set; }
    }

    public class Customer2
    {
        public int id { get; set; }
        public object phone { get; set; }
        public string fullName { get; set; }
        public object customertoken { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int AccountId { get; set; }
    }

    public class ChargeToken2
    {
        public string user_token { get; set; }
        public string embed_token { get; set; }
    }

    public class Tx2
    {
        public int id { get; set; }
        public string txRef { get; set; }
        public string orderRef { get; set; }
        public string flwRef { get; set; }
        public string redirectUrl { get; set; }
        public string device_fingerprint { get; set; }
        public object settlement_token { get; set; }
        public string cycle { get; set; }
        public int amount { get; set; }
        public int charged_amount { get; set; }
        public double appfee { get; set; }
        public int merchantfee { get; set; }
        public int merchantbearsfee { get; set; }
        public string chargeResponseCode { get; set; }
        public string raveRef { get; set; }
        public string chargeResponseMessage { get; set; }
        public string authModelUsed { get; set; }
        public string currency { get; set; }
        public string IP { get; set; }
        public string narration { get; set; }
        public string status { get; set; }
        public string vbvrespmessage { get; set; }
        public string authurl { get; set; }
        public string vbvrespcode { get; set; }
        public object acctvalrespmsg { get; set; }
        public object acctvalrespcode { get; set; }
        public string paymentType { get; set; }
        public object paymentPlan { get; set; }
        public object paymentPage { get; set; }
        public string paymentId { get; set; }
        public string fraud_status { get; set; }
        public string charge_type { get; set; }
        public int is_live { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int customerId { get; set; }
        public int AccountId { get; set; }
        public Customer2 customer { get; set; }
        public ChargeToken2 chargeToken { get; set; }
    }

    public class FlutterWaveJSONVM
    {
        public string name { get; set; }
        public Data data { get; set; }
        public Tx2 tx { get; set; }
        public bool success { get; set; }
    }
}
