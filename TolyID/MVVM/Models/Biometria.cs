﻿using Newtonsoft.Json;
using SQLite;
using System.ComponentModel;

namespace TolyID.MVVM.Models;

[Table("Biometrias")]
public class Biometria
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [JsonProperty("comprimentoTotal")]
    public double? ComprimentoTotal { get; set; }

    [JsonProperty("comprimentoDaCabeca")]
    public double? ComprimentoDaCabeca { get; set; }

    [JsonProperty("larguraDaCabeca")]
    public double? LarguraDaCabeca { get; set; }

    [JsonProperty("padraoEscudoCefalico")]
    public string? PadraoEscudoCefalico { get; set; }

    [JsonProperty("comprimentoEscudoCefalico")]
    public double? ComprimentoEscudoCefalico { get; set; }

    [JsonProperty("larguraEscudoCefalico")]
    public double? LarguraEscudoCefalico { get; set; }

    [JsonProperty("larguraInterOrbital")]
    public double? LarguraInterOrbital { get; set; }

    [JsonProperty("larguraInterLacrimal")]
    public double? LarguraInterLacrimal { get; set; }

    [JsonProperty("comprimentoDaOrelha")]
    public double? ComprimentoDaOrelha { get; set; }

    [JsonProperty("comprimentoDaCauda")]
    public double? ComprimentoDaCauda { get; set; }

    [JsonProperty("larguraDaCauda")]
    public double? LarguraDaCauda { get; set; }

    [JsonProperty("comprimentoEscudoEscapular")]
    public double? ComprimentoEscudoEscapular { get; set; }

    [JsonProperty("semicircunferenciaEscudoEscapular")]
    public double? SemicircunferenciaEscudoEscapular { get; set; }

    [JsonProperty("comprimentoEscudoPelvico")]
    public double? ComprimentoEscudoPelvico { get; set; }

    [JsonProperty("semicircunferenciaEscudoPelvico")]
    public double? SemicircunferenciaEscudoPelvico { get; set; }

    [JsonProperty("larguraNaSegundaCinta")]
    public double? LarguraNaSegundaCinta { get; set; }

    [JsonProperty("numeroDeCintas")]
    public int? NumeroDeCintas { get; set; }

    [JsonProperty("comprimentoMaoSemUnha")]
    public double? ComprimentoMaoSemUnha { get; set; }

    [JsonProperty("comprimentoUnhaDaMao")]
    public double? ComprimentoUnhaDaMao { get; set; }

    [JsonProperty("comprimentoPeSemUnha")]
    public double? ComprimentoPeSemUnha { get; set; }

    [JsonProperty("comprimentoUnhaDoPe")]
    public double? ComprimentoUnhaDoPe { get; set; }

    [JsonProperty("comprimentoDoPenis")]
    public double? ComprimentoDoPenis { get; set; }

    [JsonProperty("larguraBasePenis")]
    public double? LarguraBasePenis { get; set; }

    [JsonProperty("comprimentoDoClitoris")]
    public double? ComprimentoDoClitoris { get; set; }
}
