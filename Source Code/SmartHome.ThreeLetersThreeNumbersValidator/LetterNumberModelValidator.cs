using System.Text.RegularExpressions;
using ModeloValidador.Abstracciones;

namespace SmartHome.ThreeLetersThreeNumbersValidator;

public sealed class LetterNumberModelValidator : IModeloValidador
{
    public required string ValidatorName = "LetterNumberValidator";
    public bool EsValido(Modelo modelo)
    {
        var threeNumberthreeLettersRegex = "^[A-Za-z]{3}[0-9]{3}$";
        return Regex.IsMatch(modelo.Value, threeNumberthreeLettersRegex);
    }
}
