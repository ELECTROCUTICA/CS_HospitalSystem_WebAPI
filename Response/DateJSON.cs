namespace HospitalSystem_WebAPI_dotnet6.Response {
    
    public class DateJSON {

        public string? DateText { get; set; }
        public string? DateParam { get; set; }

        public DateJSON() { }

        public DateJSON(string dateText, string dateParam) {
            DateText = dateText;
            DateParam = dateParam;
        }

    }
}
