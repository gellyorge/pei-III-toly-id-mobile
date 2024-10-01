﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TolyID.MVVM.Models;
using TolyID.Services;

namespace TolyID.MVVM.ViewModels;

public partial class MicrochipViewModel : ObservableObject
{
    private readonly TatuService _tatuService;

    private Tatu _tatu;
    public Tatu Tatu
    {
        get => _tatu;
        set => SetProperty(ref _tatu, value);
    }

    public MicrochipViewModel(Tatu tatu, TatuService tatuService)
    {
        Tatu = tatu;
        _tatuService = tatuService;
    }

    private async Task AtualizaMicrochip()
    {
       await _tatuService.AtualizaTatu(Tatu);
    }

    [RelayCommand]
    private async Task Atualiza()
    {
        await AtualizaMicrochip();
    }
}
