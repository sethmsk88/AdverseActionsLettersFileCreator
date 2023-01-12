namespace AdverseActionsLettersFileCreator.Integrations.Models
{
    public class CarsDndbResponse
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AppId { get; set; }
        public string ApplicantFormattedName { get; set; }
        public string AddressLine1 { get; set; }
        public string ApplicantAddressExtra { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantPostalCode { get; set; }
        public string Source { get; set; }
        public string RoutingSource { get; set; }
        public string StartDateFormatted { get; set; }
        public string EndDateFormatted { get; set; }
        public int DealerNum { get; set; }

        public string StoreName { get; set; }
    }
}
