
namespace HospitalSystem_WebAPI_dotnet6.Entity {

    public class Page {
        public int Start { get; set; }

        public int Size { get; set; }

        public int Current_Page { get; set; }

        public Page() { }

        public Page(int start, int size, int current_page) {
            Start = start;
            Size = size;
            Current_Page = current_page;
        }
    }
}
