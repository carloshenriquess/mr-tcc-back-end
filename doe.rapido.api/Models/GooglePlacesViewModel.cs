namespace doe.rapido.api.Models
{
    public class GooglePlacesViewModel
    {
        public List<Candidate> candidates { get; set; }
        public string status { get; set; }

    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Candidate
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string name { get; set; }
    }
}
