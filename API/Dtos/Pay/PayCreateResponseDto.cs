using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Pay
{
    public class PayCreateResponseDto
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public string Rec_trade_id { get; set; }
        public string Bank_transaction_id { get; set; }
        public string Bank_order_number { get; set; }
        public string Auth_code { get; set; }
        public Card_Secret Card_Secret { get; set; }         ///////////////
        public int Amount { get; set; }
        public string Currency { get; set; }
        public Card_info Card_info { get; set; }
        public string Order_number { get; set; }
        public string Acquirer { get; set; }
        public long Transaction_time_millis { get; set; }
        public Bank_transaction_time Bank_transaction_time { get; set; }
        public string Bank_result_code { get; set; }
        public string Bank_result_msg { get; set; }
        public string Payment_url { get; set; }
        public Instalment_info Instalment_info { get; set; }
        public Redeem_info Redeem_info { get; set; }
        public string Card_identifier { get; set; }
        public Merchant_reference_info Merchant_reference_info { get; set; }
        public string Event_code { get; set; }
        public bool Is_rba_verified { get; set; }
        public int MyProperty { get; set; }
        public Transaction_method_details  Transaction_method_details { get; set; }
    }


    public class Card_Secret
    {
        public string Card_token { get; set; }
        public string Card_key { get; set; }
    }

    public class Card_info
    {
        public string Bin_code { get; set; }
        public string Last_four { get; set; }
        public string Issuer { get; set; }
        public string Issuer_zh_tw { get; set; }
        public string Bank_id { get; set; }
        public int Funding { get; set; }
        public int Type { get; set; }
        public string Level { get; set; }
        public string Country { get; set; }
        public string Country_code { get; set; }
        public string Expiry_date { get; set; }
    }

    public class Bank_transaction_time
    {
        public string Start_time_millis { get; set; }
        public string End_time_millis { get; set; }
    }


    public class Instalment_info
    {
        public int Number_of_instalments { get; set; }
        public int First_payment { get; set; }
        public int Each_payment { get; set; }
        public int MyProperty { get; set; }
        public Extra_info Extra_info { get; set; }
    }

    public class Extra_info
    {
        public string Install_order_no { get; set; }
        public string Install_period { get; set; }
        public string Install_pay_fee { get; set; }
        public string Install_pay { get; set; }
        public string Install_down_pay { get; set; }
        public string Install_down_pay_fee { get; set; }
        public string Installment { get; set; }
        public string Installment_type { get; set; }
        public string First_amt { get; set; }
        public string Each_amt { get; set; }
        public string Fee { get; set; }
        public string Inst { get; set; }
        public string Inst_first { get; set; }
        public string Inst_each { get; set; }
        public string Period_number { get; set; }
        public string ITA { get; set; }
        public string IP { get; set; }
        public string IPA { get; set; }
        public string IFPA { get; set; }
        public string Period { get; set; }
        public string Rate { get; set; }
        public string DownPaymentAmt { get; set; }
        public string EachPaymentAmt { get; set; }
        public string InterestAmt { get; set; }
    }

    public class Redeem_info
    {
        public string Used_point { get; set; }
        public string Balance { get; set; }
        public string Offset_amount { get; set; }
        public string Due_amount { get; set; }
        public Extra_info Extra_info { get; set; }
    }


    public class Merchant_reference_info
    {
        public List<string> Affiliate_codes { get; set; }
    }
    
    public class Transaction_method_details
    {
        public string Transaction_method { get; set; }
        public string Transaction_method_reference { get; set; }
    }

}