namespace AdverseActionsLettersFileCreator.Integrations.Models
{
    public class AdverseActionResponse
    {
        public string? WorkId { get; set; }
        public int ConsumerId { get; set; }
        public string? PayType { get; set; }
        public string? CurrentStatusId { get; set; }
        public int DecisionId { get; set; }
        public string? CounterOfferIndicator { get; set; }
        public string? RiskScore { get; set; }
        public string? LetterType { get; set; } //LTRTYP
        public string? CustomerFirstName { get; set; }
        public string? CustomerMiddleName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerSuffix { get; set; }
        public string? CustomerName { get; set; } //CUST
        public string? CustomerAddress { get; set; } //ADDR1
        public string? CustomerAddress2 { get; set; } //ADDR2
        public string? CustomerCity { get; set; }
        public string? CustomerState { get; set; }
        public string? CustomerZipCode { get; set; }
        public string? CustomerCityStateZip { get; set; } //CITYSTZIP
        public string? ApplicantName { get; set; } //NAME
        public string? DealerNumber { get; set; }
        public string? DealerName { get; set; } //KMXLOC
        public string? DealerState { get; set; }
        public string? DeclineReason1 { get; set; } //DCLRSN1
        public string? DeclineReason2 { get; set; } //DCLRSN2
        public string? DeclineReason3 { get; set; } //DCLRSN3
        public string? DeclineReason4 { get; set; } //DCLRSN4
        public string? CreditReportingAgencyName { get; set; } //BURNAME
        public string? CreditReportingAgencyAddress { get; set; } //BURADDR1
        public string? CreditReportingAgencyAddress2 { get; set; } //BURADDR2
        public string? CreditReportingAgencyPhone { get; set; } //BURPHN
        public string? DecisionOfficer { get; set; }
        public string? BuyerName { get; set; } //BUYER
        public DateTime BureauDate { get; set; } //DATE
        public string? BureauScore { get; set; } //FICOSCORE
        public string? BureauScoreReason1 { get; set; } //FICOREASON1
        public string? BureauScoreReason2 { get; set; } //FICOREASON2
        public string? BureauScoreReason3 { get; set; } //FICOREASON3
        public string? BureauScoreReason4 { get; set; } //FICOREASON4
        public string? BureauScoreReason5 { get; set; } //FICOREASON5
        public int BureauMinimumOfRange { get; set; } //MINRANGE
        public int BureauMaximumOfRange { get; set; } //MAXRANGE
        public int BureauPercentile { get; set; } //PERCENTILE
        public string? LxnxName { get; set; } //LXNXNAME
        public string? LxnxAddress { get; set; } //LXNXADDR1
        public string? LxnxPhone { get; set; } //LXNXPHN
        public string? LxnxScore { get; set; } //LXNXSCORE
        public string? LxnxScoreReason1 { get; set; } //LXNXREASON1
        public string? LxnxScoreReason2 { get; set; } //LXNXREASON2
        public string? LxnxScoreReason3 { get; set; } //LXNXREASON3
        public string? LxnxScoreReason4 { get; set; } //LXNXREASON4
        public string? LxnxScoreReason5 { get; set; } //LXNXREASON5
        public string? LxnxMinimumOfRange { get; set; } //LXNXMIN
        public string? LxnxMaximumOfRange { get; set; } //LXNXMAX

        /// <summary>
        /// Application ID field
        /// </summary>
        /// <remarks>Per compliance request, requirement to include the applicationId at the end of the AdverseAction file</remarks>
        public string? ApplicationId { get; set; }   //APPID (Loan_id from the DB)
    }
}
