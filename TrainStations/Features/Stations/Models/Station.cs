namespace Features.Stations.Models;

public class Station
{
    public int Id { get; set; }

    public bool Advertised { get; set; }

    public string AdvertisedLocationName { get; set; } = string.Empty;

    public string AdvertisedShortLocationName { get; set; } = string.Empty;

    public string PrimaryLocationCode { get; set; } = string.Empty;

    public string CountryCode { get; set; } = string.Empty;

    public bool Deleted { get; set; }

    public string? LocationInformationText { get; set; }

    public string LocationSignature { get; set; } = string.Empty;

    public bool Prognosticated { get; set; }

    public string OfficialLocationName { get; set; } = string.Empty;

    public DateTime ModifiedTime { get; set; }

    public string? Sweref99Tm { get; set; }

    public string? Wgs84 { get; set; }

    public string CountyNumbers { get; set; } = string.Empty;

    public string PlatformLines { get; set; } = string.Empty;
}