using System;
using System.ComponentModel.DataAnnotations;

namespace Acesoft.Web.Pay.Models
{
    public class WepayMicroPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "auth_code")]
        public string AuthCode { get; set; }
    }

    public class WepayPubPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }

        [Required]
        [Display(Name = "openid")]
        public string OpenId { get; set; }
    }

    public class WepayQrCodePayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WepayAppPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WepayH5PayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }
    }

    public class WepayLiteAppPayViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "body")]
        public string Body { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }

        [Required]
        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }

        [Required]
        [Display(Name = "trade_type")]
        public string TradeType { get; set; }

        [Required]
        [Display(Name = "openid")]
        public string OpenId { get; set; }
    }

    public class WepayOrderQueryViewModel
    {
        [Display(Name = "transaction_id")]
        public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
    }

    public class WepayReverseViewModel
    {
        [Display(Name = "transaction_id")]
        public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
    }

    public class WepayCloseOrderViewModel
    {
        [Required]
        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
    }

    public class WepayRefundViewModel
    {
        [Required]
        [Display(Name = "out_refund_no")]
        public string OutRefundNo { get; set; }

        [Display(Name = "transaction_id")]
        public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }

        [Required]
        [Display(Name = "total_fee")]
        public int TotalFee { get; set; }

        [Required]
        [Display(Name = "refund_fee")]
        public int RefundFee { get; set; }

        [Display(Name = "refund_desc")]
        public string RefundDesc { get; set; }

        [Display(Name = "notify_url")]
        public string NotifyUrl { get; set; }
    }

    public class WepayRefundQueryViewModel
    {
        [Display(Name = "refund_id")]
        public string RefundId { get; set; }

        [Display(Name = "out_refund_no")]
        public string OutRefundNo { get; set; }

        [Display(Name = "transaction_id")]
        public string TransactionId { get; set; }

        [Display(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
    }

    public class WepayDownloadBillViewModel
    {
        [Required]
        [Display(Name = "bill_date")]
        public string BillDate { get; set; }

        [Required]
        [Display(Name = "bill_type")]
        public string BillType { get; set; }

        [Display(Name = "tar_type")]
        public string TarType { get; set; }
    }

    public class WepayDownloadFundFlowViewModel
    {
        [Required]
        [Display(Name = "bill_date")]
        public string BillDate { get; set; }

        [Required]
        [Display(Name = "account_type")]
        public string AccountType { get; set; }

        [Display(Name = "tar_type")]
        public string TarType { get; set; }
    }

    public class WepayTransfersViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }

        [Required]
        [Display(Name = "openid")]
        public string OpenId { get; set; }

        [Required]
        [Display(Name = "check_name")]
        public string CheckName { get; set; }

        [Display(Name = "re_user_name")]
        public string ReUserName { get; set; }

        [Required]
        [Display(Name = "amount")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "desc")]
        public string Desc { get; set; }

        [Required]
        [Display(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }
    }

    public class WepayGetTransferInfoViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }
    }

    public class WepayPayBankViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }

        [Required]
        [Display(Name = "enc_bank_no")]
        public string EncBankNo { get; set; }

        [Required]
        [Display(Name = "enc_true_name")]
        public string EncTrueName { get; set; }

        [Required]
        [Display(Name = "bank_code")]
        public string BankCode { get; set; }

        [Required]
        [Display(Name = "amount")]
        public int Amount { get; set; }

        [Display(Name = "desc")]
        public string Desc { get; set; }
    }

    public class WepayQueryBankViewModel
    {
        [Required]
        [Display(Name = "partner_trade_no")]
        public string PartnerTradeNo { get; set; }
    }
}