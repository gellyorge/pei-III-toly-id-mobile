﻿using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Diagnostics;
using TolyID.MVVM.Models;

namespace TolyID.Services;

public static class BancoDeDadosService
{
    static SQLiteConnection _bancoDeDados;

    private static async Task Init()
    {
        if (_bancoDeDados != null) { return; }
       
        var caminhoDoBanco = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tatu.db3");
        _bancoDeDados = new SQLiteConnection(caminhoDoBanco);

        _bancoDeDados.CreateTable<TatuModel>();
        _bancoDeDados.CreateTable<CapturaModel>();
        _bancoDeDados.CreateTable<DadosGeraisModel>();
        _bancoDeDados.CreateTable<FichaAnestesicaModel>();
        _bancoDeDados.CreateTable<ParametroFisiologicoModel>();
        _bancoDeDados.CreateTable<BiometriaModel>();
        _bancoDeDados.CreateTable<AmostrasModel>();
    }

    // ========================  CRUD TATUS ======================== 

    public static async Task SalvaTatu(TatuModel tatu)
    {
        await Init();
        _bancoDeDados.InsertWithChildren(tatu);
    }

    public static async Task<IEnumerable<TatuModel>> GetTatus()
    {
        await Init();
        var tatus = _bancoDeDados.GetAllWithChildren<TatuModel>().ToList();
        return tatus;
    }

    public static async Task<TatuModel> GetTatu(int tatuId)
    {
        await Init();
        var tatu = _bancoDeDados.GetWithChildren<TatuModel>(tatuId);

        foreach (var captura in tatu.Capturas)
        {
            _bancoDeDados.GetChildren(captura);
            _bancoDeDados.GetChildren(captura.DadosGerais);
            _bancoDeDados.GetChildren(captura.Biometria);
            _bancoDeDados.GetChildren(captura.Amostras);
            _bancoDeDados.GetChildren(captura.FichaAnestesica);
        }

        return tatu;
    }

    public static async Task AtualizaTatu(TatuModel tatuAtualizado)
    {
        await Init();

        var tatu = await GetTatu(tatuAtualizado.Id);
        
        if (tatu != null)
        {
            tatu.IdentificacaoAnimal = tatuAtualizado.IdentificacaoAnimal;
            tatu.NumeroMicrochip = tatuAtualizado.NumeroMicrochip;
            tatu.Capturas = tatuAtualizado.Capturas;
        }

        _bancoDeDados.UpdateWithChildren(tatu);
    }

    public static async Task DeletaTatu(TatuModel tatu)
    {
        await Init();
        var tatuSelecionado = _bancoDeDados.GetWithChildren<TatuModel>(tatu.Id);

        foreach (var captura in tatuSelecionado.Capturas) 
        { 
            await DeletaCaptura(captura);  
        }

        _bancoDeDados.Delete<TatuModel>(tatu.Id);
    }

    // ======================== CRUD CAPTURAS ======================== 
    public static async Task SalvaCaptura(CapturaModel novaCaptura, TatuModel tatu)
    {
        await Init();

        FichaAnestesicaModel ficha = novaCaptura.FichaAnestesica;

        // Verificar se a lista de parâmetros fisiológicos não é nula e contém itens
        if (ficha.ParametrosFisiologicos != null && ficha.ParametrosFisiologicos.Count > 0)
        {
            // Inserir a FichaAnestesica antes para garantir que temos um ID para os parâmetros
            _bancoDeDados.Insert(ficha);

            foreach (var parametro in ficha.ParametrosFisiologicos)
            {
                parametro.FichaAnestesicaId = ficha.Id;
                _bancoDeDados.Insert(parametro);
            }
        }

        // Inserir a Captura com a ficha e outros modelos associados
        novaCaptura.FichaAnestesicaId = ficha.Id;
        _bancoDeDados.InsertWithChildren(novaCaptura, true);

        // Atualizar o Tatu com a nova Captura
        if (tatu.Capturas == null)
        {
            tatu.Capturas = new List<CapturaModel>();
        }
        tatu.Capturas.Add(novaCaptura);
        _bancoDeDados.UpdateWithChildren(tatu);
    }

    public static async Task<CapturaModel> GetCaptura(int capturaId)
    {
        await Init();
        var captura = _bancoDeDados.GetWithChildren<CapturaModel>(capturaId);

        _bancoDeDados.GetChildren(captura.DadosGerais);
        _bancoDeDados.GetChildren(captura.Biometria);
        _bancoDeDados.GetChildren(captura.FichaAnestesica);
        _bancoDeDados.GetChildren(captura.Amostras);

        return captura;
    }

    public static async Task AtualizaCaptura(CapturaModel capturaAtualizada)
    {
        await Init();

        var capturaAntiga = await GetCaptura(capturaAtualizada.Id);

        _bancoDeDados.Update(capturaAtualizada);
        _bancoDeDados.Update(capturaAtualizada.DadosGerais);
        _bancoDeDados.Update(capturaAtualizada.Biometria);
        _bancoDeDados.Update(capturaAtualizada.Amostras);
        _bancoDeDados.Update(capturaAtualizada.FichaAnestesica);

        // CUIDADO! CRIME GRAVÍSSIMO A SEGUIR!
        // Gambiarra master para atualizar os parâmetros fisiológicos
        // TODO: exclusão de parâmetros
        for (int i = 0; i <= capturaAtualizada.FichaAnestesica.ParametrosFisiologicos.Count - 1; i++)
        {
            // Garante que parâmetros presentes anteriormente sejam só atualizados, e não duplicados
            if (i <= capturaAntiga.FichaAnestesica.ParametrosFisiologicos.Count - 1)
            {
                _bancoDeDados.Update(capturaAtualizada.FichaAnestesica.ParametrosFisiologicos[i]);
                continue;
            }

            // Adiciona novos parâmetros
            capturaAtualizada.FichaAnestesica.ParametrosFisiologicos[i].FichaAnestesicaId = capturaAtualizada.FichaAnestesica.Id;
            _bancoDeDados.Insert(capturaAtualizada.FichaAnestesica.ParametrosFisiologicos[i]);
        }
    }

    public static async Task DeletaCaptura(CapturaModel captura)
    { 
        await Init();

        _bancoDeDados.Delete<DadosGeraisModel>(captura.DadosGeraisId);
        _bancoDeDados.Delete<BiometriaModel>(captura.BiometriaId);
        _bancoDeDados.Delete<AmostrasModel>(captura.AmostrasId);

        foreach (var parametroFisiologico in captura.FichaAnestesica.ParametrosFisiologicos)
        {
            _bancoDeDados.Delete<ParametroFisiologicoModel>(parametroFisiologico.Id);
        }

        _bancoDeDados.Delete<FichaAnestesicaModel>(captura.FichaAnestesicaId);
        _bancoDeDados.Delete<CapturaModel>(captura.Id);   
    }
}
