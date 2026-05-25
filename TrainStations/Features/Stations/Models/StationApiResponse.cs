namespace Features.Stations.Models;

public class StationApiResponse
{
    public StationResponse? RESPONSE { get; set; }
}

public class StationResponse
{
    public StationResult[] RESULT { get; set; } = [];
}

public class StationResult
{
    public StationDto[] TrainStation { get; set; } = [];
}

public class StationDto
{
    public bool Advertised { get; set; }
    public string? AdvertisedLocationName { get; set; }
    public string? AdvertisedShortLocationName { get; set; }
    public string? PrimaryLocationCode { get; set; }
    public string? CountryCode { get; set; }
    public int[] CountyNo { get; set; } = [];
    public bool Deleted { get; set; }
    public GeometryDto? Geometry { get; set; }
    public string? LocationInformationText { get; set; }
    public string? LocationSignature { get; set; }
    public string[] PlatformLine { get; set; } = [];
    public bool Prognosticated { get; set; }
    public string? OfficialLocationName { get; set; }
    public DateTime ModifiedTime { get; set; }
}

public class GeometryDto
{
    public string? SWEREF99TM { get; set; }
    public string? WGS84 { get; set; }
}