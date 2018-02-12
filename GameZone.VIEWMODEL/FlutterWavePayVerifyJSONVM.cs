using System;
using System.Collections.Generic;

namespace GameZone.VIEWMODEL
{

    public class CardToken
    {
        public string shortcode { get; set; }
        public string embedtoken { get; set; }
    }

    public class Card
    {
        public string expirymonth { get; set; }
        public string expiryyear { get; set; }
        public string cardBIN { get; set; }
        public string last4digits { get; set; }
        public string brand { get; set; }
        public List<CardToken> card_tokens { get; set; }
    }

    public class Meta
    {
        public int id { get; set; }
        public string metaname { get; set; }
        public string metavalue { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int getpaidTransactionId { get; set; }
    }

    public class FlwMeta
    {
        public string chargeResponse { get; set; }
        public string chargeResponseMessage { get; set; }
        public string VBVRESPONSEMESSAGE { get; set; }
        public string VBVRESPONSECODE { get; set; }
        public object ACCOUNTVALIDATIONRESPMESSAGE { get; set; }
        public object ACCOUNTVALIDATIONRESPONSECODE { get; set; }
    }

    public class Recurr
    {
        public int id { get; set; }
        public string type { get; set; }
        public int retryTxId { get; set; }
        public int retries { get; set; }
        public string status { get; set; }
        public DateTime next_due { get; set; }
        public DateTime start { get; set; }
        public object stop { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int TransactionId { get; set; }
    }

    public class verifyData
    {
        public int id { get; set; }
        public string tx_ref { get; set; }
        public string order_ref { get; set; }
        public string flw_ref { get; set; }
        public string transaction_type { get; set; }
        public object settlement_token { get; set; }
        public string rave_ref { get; set; }
        public string transaction_processor { get; set; }
        public string status { get; set; }
        public object chargeback_status { get; set; }
        public string ip { get; set; }
        public string device_fingerprint { get; set; }
        public string cycle { get; set; }
        public string narration { get; set; }
        public int amount { get; set; }
        public double appfee { get; set; }
        public int merchantfee { get; set; }
        public object markupFee { get; set; }
        public int merchantbearsfee { get; set; }
        public int charged_amount { get; set; }
        public string transaction_currency { get; set; }
        public object system_type { get; set; }
        public string payment_entity { get; set; }
        public string payment_id { get; set; }
        public string fraud_status { get; set; }
        public string charge_type { get; set; }
        public int is_live { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public object deletedAt { get; set; }
        public int merchant_id { get; set; }
        public int addon_id { get; set; }
        public Customer customer { get; set; }
        public Card card { get; set; }
        public List<Meta> meta { get; set; }
        public FlwMeta flwMeta { get; set; }
        public Recurr recurr { get; set; }
    }

    public class FlutterWavePayVerifyJSONVM
    {
        public string status { get; set; }
        public string message { get; set; }
        public verifyData data { get; set; }
    }
}
