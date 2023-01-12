using AdverseActionsLettersFileCreator.Integrations.Models;
using AdverseActionsLettersFileCreator.Integrations.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace AdverseActionsLettersFileCreator.Integrations.QueryHandlers
{
    public class GetDndbLettersQueryHandler : IRequestHandler<GetDndbLettersQuery, List<AdverseActionResponse>>
    {
        private readonly IOptions<ApplicationSettings> _appSettings;

        public GetDndbLettersQueryHandler(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<List<AdverseActionResponse>> Handle(GetDndbLettersQuery request, CancellationToken cancellationToken)
        {
            var dndbLetterType = _appSettings.Value.Attributes.DndbLetterType;
            int dndbLookbackDays = _appSettings.Value.Attributes.DndbLookbackDays;
            string inputFilePath = _appSettings.Value.LettersInputFilePath;

            // TODO: Perform this validation in a Validator class
            if (string.IsNullOrEmpty(dndbLetterType))
                throw new Exception("DNDB letter type is not available.");

            var adverseActionList = new List<AdverseActionResponse>();

            var dndbRecords = await GetDataTableFromCSVFile(inputFilePath);

            foreach (var dndbRecord in dndbRecords)
            {
                // Protect
                var cityStateZip = string.Concat(dndbRecord.ApplicantCity, ", ", dndbRecord.ApplicantState, " ",
                    string.IsNullOrEmpty(dndbRecord.ApplicantPostalCode) ? "" : dndbRecord.ApplicantPostalCode);

                if (dndbRecord.ApplicantFormattedName.Length > 35)
                    dndbRecord.ApplicantFormattedName = dndbRecord.ApplicantFormattedName.Substring(0, 35);
                if (dndbRecord.AddressLine1.Length > 35)
                    dndbRecord.AddressLine1 = dndbRecord.AddressLine1.Substring(0, 35);
                if (dndbRecord.ApplicantAddressExtra.Length > 35)
                    dndbRecord.ApplicantAddressExtra = dndbRecord.ApplicantAddressExtra.Substring(0, 35);
                if (cityStateZip.Length > 40)
                    cityStateZip = cityStateZip.Substring(0, 40);

                var adverseAction = new AdverseActionResponse
                {
                    LetterType = dndbLetterType,
                    BureauDate = dndbRecord.StartDate,
                    CustomerName = dndbRecord.ApplicantFormattedName,
                    CustomerAddress = dndbRecord.AddressLine1,
                    CustomerAddress2 = dndbRecord.ApplicantAddressExtra,
                    CustomerCityStateZip = cityStateZip,
                    DealerName = dndbRecord.StoreName,
                    ApplicantName = dndbRecord.ApplicantFormattedName
                };
                adverseActionList.Add(adverseAction);
            }
            return adverseActionList;
        }

        private async Task<List<CarsDndbResponse>> GetDataTableFromCSVFile(string csv_file_path)
        {

            DataTable csvData = new DataTable();
            bool headerRead = true;
            List<CarsDndbResponse> response = new List<CarsDndbResponse>();

            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });

                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)
                    {
                        DataColumn datacolumn = new DataColumn(column);
                        datacolumn.AllowDBNull = true;
                        csvData.Columns.Add(datacolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        string[] fieldData = csvReader.ReadFields();
                        //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                        if (headerRead)
                        {
                            //Making empty value as null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (fieldData[i] == "")
                                {
                                    fieldData[i] = null;
                                }
                            }

                            csvData.Rows.Add(fieldData);
                        }
                        else
                        {
                            headerRead = true;
                        }
                    }
                }

                foreach (DataRow row in csvData.Rows)
                {

                    var DndbLetter = new CarsDndbResponse
                    {

                        StartDate = DateTime.Parse(row["StartDate"].ToString()),
                        EndDate = DateTime.Parse(row["EndDate"].ToString()),
                        AppId = int.Parse(row["AppId"].ToString()),
                        ApplicantFormattedName = row["ApplicantFormattedName"].ToString(),
                        AddressLine1 = row["AddressLine1"].ToString(),
                        ApplicantAddressExtra = row["ApplicantAddressExtra"].ToString(),
                        ApplicantCity = row["ApplicantCity"].ToString(),
                        ApplicantState = row["ApplicantState"].ToString(),
                        ApplicantPostalCode = row["ApplicantPostalCode"].ToString(),
                        Source = row["Source"].ToString(),
                        RoutingSource = row["RoutingSource"].ToString(),
                        StartDateFormatted = row["StartDateFormatted"].ToString(),
                        EndDateFormatted = row["EndDateFormatted"].ToString(),
                        DealerNum = int.Parse(row["Source"].ToString()),
                        StoreName = row["StoreName"].ToString()

                    };

                    response.Add(DndbLetter);
                }

            }
            catch (Exception ex)
            {
            }



            return response;
        }

    }
}
