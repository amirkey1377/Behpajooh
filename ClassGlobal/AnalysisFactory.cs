using Newtonsoft.Json;
using Skydat;
using System;
using System.Collections.Generic;
using System.Linq;



public static class AnalysisFactory
{
    public static List<ITechniques> DeserializeAnalyses(
        string json,
        IReadOnlyDictionary<string, TechniqueSpecification> specifications)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<ITechniques>();

        var items = JsonConvert.DeserializeObject<List<AnalysisJsonDto>>(json);

        if (items == null)
            return new List<ITechniques>();

        List<ITechniques> result = new List<ITechniques>();

        foreach (var item in items)
        {
            string techniqueName = GetTechniqueName(item);

            if (!specifications.TryGetValue(
                techniqueName, out var specific))
            {
                throw new NotSupportedException(
                    $"Specification for {techniqueName} was not found.");
            }

            ITechniques analysis = CreateAnalysis(item, techniqueName, specific);

            result.Add(analysis);
        }

        return result;
    }

    private static string GetTechniqueName(AnalysisJsonDto item)
    {
        if (!string.IsNullOrWhiteSpace(item.Type))
        {
            string typeName = item.Type.Split(',')[0].Trim();

            int lastDot = typeName.LastIndexOf('.');

            return lastDot >= 0
                ? typeName.Substring(lastDot + 1)
                : typeName;
        }

        return item.Name?.Trim()
            ?? throw new JsonException("Technique type is missing.");
    }

    private static ITechniques CreateAnalysis(
        AnalysisJsonDto item,
        string techniqueName,
        TechniqueSpecification specific)
    {
        switch (techniqueName.ToUpperInvariant())
        {
            case "CV":
                return new CV(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                    Required(item.HStep, nameof(item.HStep)),
                    item.Cycles ?? 1,
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );

            case "NPV":
                return new NPV(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                    Required(item.HStep, nameof(item.HStep)),
                    Required(item.PulseWidth, nameof(item.PulseWidth)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );
            case "LSV":
                return new LSV(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                    Required(item.HStep, nameof(item.HStep)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );

            case "CA":
                return new CA(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.T1, nameof(item.T1)),
                    Required(item.T2, nameof(item.T2)),
                    item.Cycles ?? 0,
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );
            case "DPV":
                return new DPV(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                    Required(item.HStep, nameof(item.HStep)),
                    Required(item.PulseWidth, nameof(item.PulseWidth)),
                    Required(item.PulseHeight, nameof(item.PulseHeight)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );

            case "OCP":
                return new OCP(
                    specific,
                    Required(item.time, nameof(item.time)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );
            case "SWV":
                return new SWV(
                     specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                    Required(item.HStep, nameof(item.HStep)),
                    Required(item.PulseHeight, nameof(item.PulseHeight)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );

            case "Tafel":
                return new Tafel(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.E2, nameof(item.E2)),
                    Required(item.ScanRate, nameof(item.ScanRate)),
                     Required(item.HStep, nameof(item.HStep)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );
            case "PRE":
                return new PRE(
                    specific,
                    Required(item.E1, nameof(item.E1)),
                    Required(item.T1, nameof(item.T1)),
                    item.EquilibriumTime ?? 0,
                    item.Comment ?? ""
                );
            default:
                throw new NotSupportedException(
                    $"Technique '{techniqueName}' is not supported.");
        }
    }

    private static double Required(double? value, string parameterName)
    {
        return value ?? throw new JsonException(
            $"Required parameter '{parameterName}' is missing.");
    }

    private static int Required(int? value, string parameterName)
    {
        return value ?? throw new JsonException(
            $"Required parameter '{parameterName}' is missing.");
    }
}

public class AnalysisJsonDto
{
    [JsonProperty("$type")]
    public string Type { get; set; }

    public string Name { get; set; }
    public string Comment { get; set; }

    public int? ColorCode { get; set; }

    public double? E1 { get; set; }
    public double? E2 { get; set; }
    public double? E3 { get; set; }

    public double? ScanRate { get; set; }
    public double? HStep { get; set; }
    public int? T1 { get; set; }
    public int? T2 { get; set; }

    public double? PulseWidth { get; set; }
    public double? PulseHeight { get; set; }
    public int? time { get; set; }

    public int? Cycles { get; set; }
    public int? EquilibriumTime { get; set; }
}