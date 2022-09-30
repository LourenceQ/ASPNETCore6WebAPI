namespace CityInfo.API.Models;

/// <summary>
/// DTO par acidade sem pontos de interesse
/// </summary>
public class CityWithoutPointsOfInterestDTO
{
    /// <summary>
    /// Id da cidade
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Nome da cidade
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Descrição da cidade
    /// </summary>
    public string? Description { get; set; }
}
