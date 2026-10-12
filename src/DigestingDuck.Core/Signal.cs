using System.Globalization;
using IqOptionApiDotNet.Models;
using IqOptionApiDotNet.Models.DigitalOptions;

namespace DigestingDuck.Core;

public sealed record Signal(Guid Id, DateTimeOffset At, int AssetId, OrderDirection Direction, DigitalOptionsExpiryDuration Duration);
public static class SignalCsv
{
    // ISO-8601 timestamp; asset name or numeric ID; CALL/PUT; 1/5/15 minutes
    public static IReadOnlyList<Signal> Parse(string csv)
    {
        var result = new List<Signal>();
        var lines = csv.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;
            var cells = line.Split(';', StringSplitOptions.TrimEntries);
            if (i == 0 && cells[0].Equals("data", StringComparison.OrdinalIgnoreCase)) continue;
            if (cells.Length != 4) throw new FormatException($"Linha {i + 1}: use data;ativo;direção;minutos.");
            if (!DateTimeOffset.TryParse(cells[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out var at) ||
                !(cells[0].EndsWith('Z') || System.Text.RegularExpressions.Regex.IsMatch(cells[0], @"[+-]\d{2}:\d{2}$")))
                throw new FormatException($"Linha {i + 1}: a data deve incluir o fuso horário.");
            if (!Enum.TryParse<ActivePair>(cells[1].Replace('-', '_'), true, out var pair) || (int)pair <= 0)
                throw new FormatException($"Linha {i + 1}: ativo inválido.");
            if (!Enum.TryParse<OrderDirection>(cells[2], true, out var direction) || !Enum.IsDefined(direction))
                throw new FormatException($"Linha {i + 1}: use CALL ou PUT.");
            if (!int.TryParse(cells[3].TrimStart('M','m'), out var minutes) || minutes is not (1 or 5 or 15))
                throw new FormatException($"Linha {i + 1}: duração deve ser 1, 5 ou 15.");
            result.Add(new(Guid.NewGuid(), at, (int)pair, direction, (DigitalOptionsExpiryDuration)minutes));
        }
        return result;
    }
}
